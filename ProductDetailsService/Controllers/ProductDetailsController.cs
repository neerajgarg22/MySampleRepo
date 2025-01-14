using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductDetailsService.Data;
using ProductDetailsService.Models;

namespace ProductDetailsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public ProductDetailsController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProductDetails(int id)
        {
            var productById = appDbContext.ProductDetails.FirstOrDefault(x => x.Id == id);
            return productById != null ? Ok(productById) : NotFound($"The Product with Product Id {id} not found");
        }

        [HttpGet]
        [Route("AllProductDetails")]
        public IActionResult GetProductDetails()
        {
            var allProductDetails = appDbContext.ProductDetails.ToList();
            return allProductDetails != null ? Ok(allProductDetails) : NotFound($"No Product exists in database");
        }

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProductDetails([FromBody] ProductDetails productDetails)
        {
            appDbContext.ProductDetails.Add(productDetails);
            appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, productDetails);

        }

        [HttpPut]
        [Route("UpdateProduct")]
        public IActionResult UpdateProductDetails([FromBody] ProductDetails productDetails)
        {
            appDbContext.ProductDetails.Update(productDetails);
            appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK, productDetails);

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProductDetails(int id)
        {
            var productDetailsToDelete = appDbContext.ProductDetails.AsNoTracking().FirstOrDefault(y => y.Id==id);
            if (productDetailsToDelete != null)
            {
                appDbContext.ProductDetails.Remove(productDetailsToDelete);
                appDbContext.SaveChanges();
                return Ok(productDetailsToDelete);
            }
            return NotFound($"The Product details with Product Id {id} not found.");

        }
    }
}
