using Dapper;
using ProductDemo.Web.Models;
using ProductDemo.Web.Repositories.Interfaces;

namespace ProductDemo.Web.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly DapperContext _context;

        public AdminUserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<AdminUser> GetUserAsync(string username)
        {
            var query = "SELECT * FROM admin_user WHERE username = @Username";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<AdminUser>(query, new { Username = username });
        }
    }
}
