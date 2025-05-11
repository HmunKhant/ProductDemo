using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductDemo.Web.Models;
using ProductDemo.Web.Services;
using ProductDemo.Web.Services.Interfaces;

namespace ProductDemo.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? name, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 10)
        {
            var products = await _service.GetFilteredProductsAsync(name, minPrice, maxPrice, page, pageSize);
            var totalCount = await _service.GetFilteredProductsCountAsync(name, minPrice, maxPrice);

            var model = new ProductFilterViewModel
            {
                Products = products,
                Name = name,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(model);
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            await _service.AddProduct(HttpContext.Request,product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _service.UpdateProduct(HttpContext.Request, product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteProduct(id);
            return RedirectToAction("Index");
        }

    }
}
