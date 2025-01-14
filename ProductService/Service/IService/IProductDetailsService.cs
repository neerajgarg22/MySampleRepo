using ProductService.Models;

namespace ProductService.Service.IService
{
    public interface IProductDetailsService
    {
        Task<ProductDetails> GetProductDetails(int productId);
        Task<List<ProductDetails>> GetAllProductDetails();
        Task<ProductDetails> AddProductDetails(ProductDetails productDetails);
        Task<ProductDetails> DeleteProductDetails(int id);
        Task<ProductDetails> UpdateProductDetails(ProductDetails productDetails);
    }
}
