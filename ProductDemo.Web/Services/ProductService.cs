using Azure.Core;
using ProductDemo.Web.Helpers;
using ProductDemo.Web.Models;
using ProductDemo.Web.Repositories.Interfaces;
using ProductDemo.Web.Services.Interfaces;

namespace ProductDemo.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly JwtHelper _jwtHelper;

        public ProductService(IProductRepository repository, JwtHelper jwtHelper)
        {
            _repository = repository;
            _jwtHelper = jwtHelper;
        }

        public Task<IEnumerable<Product>> GetAllProducts() => _repository.GetAllAsync();

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(string? name, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            return await _repository.GetFilteredProducts(name, minPrice, maxPrice, page, pageSize);
        }
        public async Task<int> GetFilteredProductsCountAsync(string? name, decimal? minPrice, decimal? maxPrice)
        {
            return await _repository.GetFilteredProductsCount(name, minPrice, maxPrice);
        }


        public Task<Product> GetProductById(int id) => _repository.GetByIdAsync(id);
        public async Task AddProduct(HttpRequest request, Product product)
        {
            product.CreatedBy = int.Parse(_jwtHelper.GetUserIdFromJwt(request));
            product.CreatedAt = DateTime.Now;
            await _repository.AddAsync(product);
        }

        public async Task UpdateProduct(HttpRequest request,Product product)
        {
            product.UpdatedBy = int.Parse(_jwtHelper.GetUserIdFromJwt(request));
            product.UpdatedAt = DateTime.Now;
            await _repository.UpdateAsync(product);
        }
        public Task DeleteProduct(int id) => _repository.DeleteAsync(id);
    }
}
