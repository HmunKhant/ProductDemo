using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProductDemo.Web.Services.Interfaces;
using ProductDemo.Web.Models;
using ProductDemo.Web.Repositories.Interfaces;
using ProductDemo.Web.Helpers;

namespace ProductDemo.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdminUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IAdminUserRepository userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> Authenticate(AdminUser user)
        {
            var dbUser = await _userRepository.GetUserAsync(user.Username);
            if (dbUser == null || !PasswordHasher.VerifyPassword(user.Password, dbUser.Password))
                return null;

            var token = await _jwtHelper.GenerateJwtTokenAsync(dbUser.Id);
            return token;
        }
    }
}
