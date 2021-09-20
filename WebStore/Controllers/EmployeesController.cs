using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEnumerable<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        //[Route("~/employees/all")]
        public IActionResult Index() => View(_Employees);

        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            //var employee = _Employees.FirstOrDefault(e => e.Id == id);
            var employee = _Employees.SingleOrDefault(e => e.Id == id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult TestAction(string Parameter1, int Param2)
        {
            return Content($"P1:{Parameter1} - P2:{Param2}");
        }
    }
}
