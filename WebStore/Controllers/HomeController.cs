using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };

        public IActionResult Index() // http://localhost:5000/Home/Index
        {
            //return Content("Hello from first controller");
            return View();
        }

        public IActionResult SeconAction(int id)
        {
            return Content($"Second action with parameter {id}");
        }

        public IActionResult Employees() // http://localhost:5000/Home/Employees
        {
            return View(__Employees);
        }
    }
}
