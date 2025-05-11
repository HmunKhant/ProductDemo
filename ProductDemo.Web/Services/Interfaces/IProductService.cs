using ProductDemo.Web.Models;

namespace ProductDemo.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetFilteredProductsAsync(string? name, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
        Task<int> GetFilteredProductsCountAsync(string? name, decimal? minPrice, decimal? maxPrice);
        Task<Product> GetProductById(int id);
        Task AddProduct(HttpRequest request, Product product);
        Task UpdateProduct(HttpRequest request, Product product);
        Task DeleteProduct(int id);
    }
}
