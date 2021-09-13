using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() // http://localhost:5000/Home/Index
        {
            //return Content("Hello from first controller");
            return View();
        }

        public IActionResult SeconAction(int id)
        {
            return Content($"Second action with parameter {id}");
        }
    }
}
