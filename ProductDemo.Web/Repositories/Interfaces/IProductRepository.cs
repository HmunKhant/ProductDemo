using ProductDemo.Web.Models;

namespace ProductDemo.Web.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
        Task<int> GetFilteredProductsCount(string? name, decimal? minPrice, decimal? maxPrice);
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
