using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Exception(string Message) => throw new InvalidOperationException(Message ?? "Ошибка в контроллере!");

        public IActionResult ContactUs() => View();

        public IActionResult Status(string id)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            switch (id)
            {
                default: return Content($"Status ---{id}");
                case "404": return View("Error404");
            }
        }
    }
}
