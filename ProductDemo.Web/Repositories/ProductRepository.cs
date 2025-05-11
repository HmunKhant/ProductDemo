using Dapper;
using ProductDemo.Web.Models;
using ProductDemo.Web.Repositories.Interfaces;
using System.Data;
using System.Text;

namespace ProductDemo.Web.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT id,name, price, description, created_at as createdat, updated_at as updatedat FROM product ORDER BY created_at DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var query = "SELECT id,name, price, description, created_at as createdat, updated_at as updatedat FROM product WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });
        }

        public async Task AddAsync(Product product)
        {
            var query = @"
                INSERT INTO product (name, price, description, created_at, updated_at, created_by)
                VALUES (@Name, @Price, @Description, @CreatedAt, @UpdatedAt, @CreatedBy)";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, product);
        }

        public async Task UpdateAsync(Product product)
        {
            var query = @"
                UPDATE product
                SET name = @Name,
                    price = @Price,
                    description = @Description,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy
                WHERE id = @Id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, product);
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM product WHERE id = @Id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var query = new StringBuilder("SELECT id,name, price, description, created_at as createdat, updated_at as updatedat" +
                " FROM product WHERE 1=1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                query.Append(" AND name LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }
            if (minPrice.HasValue)
            {
                query.Append(" AND price >= @MinPrice");
                parameters.Add("MinPrice", minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query.Append(" AND price <= @MaxPrice");
                parameters.Add("MaxPrice", maxPrice.Value);
            }

            query.Append(" ORDER BY created_at DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
            parameters.Add("Offset", (page - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query.ToString(), parameters);
        }

        public async Task<int> GetFilteredProductsCount(string? name, decimal? minPrice, decimal? maxPrice)
        {
            var query = new StringBuilder("SELECT COUNT(*) FROM product WHERE 1=1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                query.Append(" AND name LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }
            if (minPrice.HasValue)
            {
                query.Append(" AND price >= @MinPrice");
                parameters.Add("MinPrice", minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query.Append(" AND price <= @MaxPrice");
                parameters.Add("MaxPrice", maxPrice.Value);
            }

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query.ToString(), parameters);
        }

    }
}
