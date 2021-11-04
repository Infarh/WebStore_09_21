using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class AjaxTestController : Controller
    {
        public IActionResult Index() => View();
    }
}
