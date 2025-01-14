using CartService.Data;
using CartService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;

        public CartController(AppDbContext _appDbContext, IConfiguration _configuration)
        {
            appDbContext = _appDbContext;
            configuration = _configuration;
        }


        [HttpPost]
        [Route("AddItemToCart/{userId}")]
        public IActionResult AddItemToCart(string userId, [FromBody] CartItem item)
        {
            var cart = appDbContext.Carts.FirstOrDefault(x => x.CartId == item.CartId);
            if (cart != null)
            {
                if (!string.Equals(cart.UserId, userId, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("The userid doesnot match");
                }
            }
            var itemExistsInCart = appDbContext.CartItems.FirstOrDefault(y => y.ProductId == item.ProductId);
            if (itemExistsInCart != null)
            {
                itemExistsInCart.Quantity += item.Quantity;
                appDbContext.CartItems.Update(itemExistsInCart);
                appDbContext.SaveChanges();

            }
            else
            {
                appDbContext.CartItems.Add(item);
                appDbContext.SaveChanges();
            }

            var cartItems = appDbContext.CartItems.Where(x => x.CartId == item.CartId).ToList();
            var totalAmount = cartItems.Sum(x => x.TotalPrice);
            if(cart==null)
                cart = appDbContext.Carts.FirstOrDefault(x => x.CartId == item.CartId);
            cart.TotalAmount = totalAmount;
            appDbContext.Carts.Update(cart);
            appDbContext.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [Route("RemoveItemFromCart/{userId}")]
        public IActionResult RemoveItemFromCart(string userId, [FromBody] CartItem item)
        {
            var cart = appDbContext.Carts.FirstOrDefault(x => x.CartId == item.CartId);
            if (cart!=null && !string.Equals(cart.UserId, userId, StringComparison.OrdinalIgnoreCase)) 
            {
                return BadRequest("The userid doesnot match");
            }
            var itemExistsInCart = appDbContext.CartItems.FirstOrDefault(y => y.ProductId == item.ProductId);

            if (itemExistsInCart != null)
            {
                itemExistsInCart.Quantity -= item.Quantity;
                appDbContext.CartItems.Update(itemExistsInCart);
                appDbContext.SaveChanges();
            }
            else
            {
                return NotFound($"The item with Product id {item.ProductId} not found in cart to remove");
            }

            var cartItems = appDbContext.CartItems.Where(x => x.CartId == item.CartId).ToList();
            
            var totalAmount = cartItems.Sum(x => x.TotalPrice);

            cart.TotalAmount = totalAmount;
            appDbContext.Carts.Update(cart);
            appDbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("CheckOut/{userId}")]
        public async Task<IActionResult> CheckOut(string userId, [FromBody] Cart cart)
        {          
            var orderDetails = new OrderDetails
            {
                UserId = userId,
                ShippingCity = "Faridabad",
                TotalAmount = cart.Items.Sum(x => x.TotalPrice)
            };
            appDbContext.OrderDetails.Add(orderDetails);
            appDbContext.SaveChanges();
            var lstOrderItems = new List<OrderItem>();
            foreach (var item in cart.Items) 
            {
                var orderItems = new OrderItem
                {
                    OrderId=orderDetails.OrderId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                lstOrderItems.Add(orderItems);
            }
            appDbContext.OrderItems.AddRange(lstOrderItems);
            appDbContext.SaveChanges();


            var cartToDelete = appDbContext.Carts.Where(x => x.CartId == cart.CartId).Include(c=>c.Items).FirstOrDefault();
            appDbContext.Carts.Remove(cartToDelete);
            appDbContext.SaveChanges();

            //docker run -d --hostname rmq --name rabbit-server -p 8080:15672 -p 5672:5672 rabbitmq:3-management

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetValue<string>("RabbitMQUrl")),//Pull it from secret file
                ClientProvidedName = "Cart Service - Order placed"
            };
            IConnection cnn = await factory.CreateConnectionAsync();

            IChannel channel = await cnn.CreateChannelAsync();
            

            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
            await channel.QueueDeclareAsync(queueName, false, false, false, null);

            await channel.QueueBindAsync(queueName,exchangeName, routingKey,null);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes("OrderPlaced");
            await channel.BasicPublishAsync(exchangeName,routingKey,messageBodyBytes);

            await channel.CloseAsync();
            await cnn.CloseAsync();
            return Ok();   
             

        }
    }
}
