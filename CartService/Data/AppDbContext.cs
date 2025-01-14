using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                        .HasMany(c => c.Items)
                        .WithOne(ci => ci.Cart)
                        .HasForeignKey(ci => ci.CartId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetails>()
                       .HasMany(c => c.Items)
                       .WithOne(ci => ci.OrderDetails)
                       .HasForeignKey(ci => ci.OrderId);


            modelBuilder.Entity<Cart>().HasData(new Cart
            {
                CartId = 1,
                UserId = "User1",
            });


            modelBuilder.Entity<CartItem>().HasData(new CartItem
            {
                Id=1,
                CartId=1,
                Price=100,
                Quantity=1,
                ProductId=1,
                ProductName="TShirts"
            });
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Cart>().HasData(
            //    new[] {
            //        new Cart
            //        {
            //           CartId=1,
            //           Items=new List<CartItem>
            //           {
            //               new CartItem
            //               {
            //                   Id=1,
            //                   ProductId=1,
            //                   ProductName="Shirts",
            //                   Price=100,
            //                   Quantity=5
            //               }
            //           },
            //           UserId="NG22"
            //        } 
            //    });
        }
    }
}
