using ProductService.Models;
using ProductService.Service.IService;
using System.Text;
using System.Text.Json;

namespace AccountManagementService.Service
{
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProductDetailsService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<List<ProductDetails>> GetAllProductDetails()
        {
            var client = httpClientFactory.CreateClient("ProductDetails");
            var response = await client.GetAsync($"/api/ProductDetails/AllProductDetails");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductDetails>>(responseContent, options);
            }
            return null;
        }

        public async Task<ProductDetails> GetProductDetails(int productId)
        {
            var client = httpClientFactory.CreateClient("ProductDetails");
            var response = await client.GetAsync($"/api/ProductDetails/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDetails>(responseContent, options);
               
            }
            return null;
        }

        public async Task<ProductDetails> UpdateProductDetails(ProductDetails productDetails)
        {
            string jsonContent = JsonSerializer.Serialize(productDetails);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient("ProductDetails");
            var response = await client.PutAsync($"/api/ProductDetails/UpdateProduct", content);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDetails>(responseContent, options);

            }
            return null;
        }

        public async Task<ProductDetails> DeleteProductDetails(int id)
        {
            var client = httpClientFactory.CreateClient("ProductDetails");
            var response = await client.DeleteAsync($"/api/ProductDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDetails>(responseContent, options);

            }
            return null;
        }

        public async Task<ProductDetails> AddProductDetails(ProductDetails productDetails)
        {
            string jsonContent = JsonSerializer.Serialize(productDetails);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient("ProductDetails");
            var response = await client.PostAsync($"/api/ProductDetails/AddProduct",content);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDetails>(responseContent, options);

            }
            return null;
        }

    }
}
