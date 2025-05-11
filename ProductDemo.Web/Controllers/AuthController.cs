using Microsoft.AspNetCore.Mvc;
using ProductDemo.Web.Models;
using ProductDemo.Web.Services.Interfaces;

namespace ProductDemo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminUser user)
        {
            var token = await _authService.Authenticate(user);
            if (token == null)
            {
                ViewBag.Message = "Invalid credentials.";
                return View();
            }

            Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return RedirectToAction("Login");
        }
    }
}
