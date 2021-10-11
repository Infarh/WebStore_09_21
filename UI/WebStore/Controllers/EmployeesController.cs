using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        //[Route("~/employees/all")]
        public IActionResult Index() => View(_EmployeesData.GetAll());

        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.GetById(id);

            //if (ReferenceEquals(employee, null))
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        #region Edit

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.LastName == "Асама" && model.Name == "Бин" && model.Patronymic == "Ладан")
                ModelState.AddModelError("", "Террористов не берём!");

            if (!ModelState.IsValid) return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age,
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
