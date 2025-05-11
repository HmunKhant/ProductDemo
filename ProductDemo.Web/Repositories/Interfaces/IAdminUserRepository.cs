using ProductDemo.Web.Models;

namespace ProductDemo.Web.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<AdminUser> GetUserAsync(string username);
    }
}
