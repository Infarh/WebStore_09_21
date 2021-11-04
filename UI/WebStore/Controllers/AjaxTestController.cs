using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AjaxTestController : Controller
    {
        private readonly ILogger<AjaxTestController> _Logger;

        public AjaxTestController(ILogger<AjaxTestController> Logger) => _Logger = Logger;

        public IActionResult Index() => View();

        public async Task<IActionResult> GetJSON(int? id, string msg, int Delay = 2000)
        {
            _Logger.LogInformation("Получен запрос к GetJSON - id:{id}, msg:{msg}, Delay:{Delay}", id, msg, Delay);

            await Task.Delay(Delay);

            _Logger.LogInformation("Ответ на запрос к GetJSON - id:{id}, msg:{msg}, Delay:{Delay}", id, msg, Delay);
            return Json(new
            {
                Message = $"Response (id:{id ?? 0}): {msg ?? "--null--"}",
                ServerTime = DateTime.Now,
            });
        }

        public async Task<IActionResult> GetHTML(int? id, string msg, int Delay = 2000)
        {
            _Logger.LogInformation("Получен запрос к GetHTML - id:{id}, msg:{msg}, Delay:{Delay}", id, msg, Delay);

            await Task.Delay(Delay);

            _Logger.LogInformation("Ответ на запрос к GetHTML - id:{id}, msg:{msg}, Delay:{Delay}", id, msg, Delay);

            return PartialView("Partial/_DataView", new AjaxTestDataViewModel
            {
                Id = id ?? 0,
                Message = msg,
            });
        }

        public IActionResult Chat() => View();
    }
}
