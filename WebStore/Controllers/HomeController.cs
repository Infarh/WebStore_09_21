using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        
        public IActionResult About() => View();

        public IActionResult Catalog() => View();

        public IActionResult Status(string Code) => Content($"Status code - {Code}");
    }
}
