using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public IRedisCacheService CacheService;

        public ProductsController(AppDbContext _appDbContext, IRedisCacheService cacheService
            )
        {
            appDbContext = _appDbContext;
            CacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = appDbContext.Products.ToList();            

            return products != null ? Ok(products) : NotFound($"No Product exists in database");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carsByIdFromCache = CacheService.GetData<Products>($"ProductById - {id}");
            if (carsByIdFromCache != null)
            {
                Console.WriteLine($"Got from cache - {carsByIdFromCache.Name}");
                return Ok(carsByIdFromCache);
            }

            var product = appDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product!=null)
            {

                CacheService.SetData($"ProductById - {id}", product);
                Console.WriteLine($"Added to cache - {product.Name}");

            }

            return product != null ? Ok(product) : NotFound($"No Product exists in database with Id {id}");
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Products product)
        {
            var products = new Products
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                InventoryCount = product.InventoryCount
            };

            appDbContext.Products.Add(products);
            appDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, product);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Products product)
        {

            var products = new Products
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                InventoryCount = product.InventoryCount
            };

            appDbContext.Products.Update(products);
            appDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = appDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                appDbContext.Products.Remove(product);
                appDbContext.SaveChanges();
                CacheService.RemoveData<Products>($"ProductById - {id}");
                Console.WriteLine($"Removed from cache - {product.Name}");

                return Ok($"The Product with Id {id} deleted");
            }
            return NotFound($"No Product exists in database with Id {id} to delete.");
        }

    }
}
