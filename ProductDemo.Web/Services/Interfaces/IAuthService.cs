using ProductDemo.Web.Models;

namespace ProductDemo.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(AdminUser user);
    }
}
