using Microsoft.EntityFrameworkCore;
using ProductDetailsService.Models;

namespace ProductDetailsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ProductDetails> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductDetails>().HasData(
                new[] {
                    new ProductDetails
                    {
                        Id = 1,
                        Design="Normal",
                        Size=1,
                        Price=100
                    } 
                });
        }
    }
}
