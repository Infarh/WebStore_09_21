using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Index()
        {
            var products = _ProductData.GetProducts();
            return View(products.Products);
        }

        public IActionResult Edit(int id) => RedirectToAction(nameof(Index));

        public IActionResult Delete(int id) => RedirectToAction(nameof(Index));
    }
}
