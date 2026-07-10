using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Models;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;

        public EmployeeController(
            IEmployeeService employeeService,
            IUserService userService)
        {
            _employeeService = employeeService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Create(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            var employee = new Employee
            {
                UserId = user.Id,
                User = user,
                WorkStart = new TimeOnly(9, 0),
                WorkEnd = new TimeOnly(17, 0)
            };

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            ModelState.Remove(nameof(Employee.User));

            if (!ModelState.IsValid)
            {
                employee.User = await _userService.GetByIdAsync(employee.UserId);
                return View(employee);
            }

            await _employeeService.CreateAsync(employee);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            ModelState.Remove(nameof(Employee.User));

            if (!ModelState.IsValid)
            {
                employee.User = await _userService.GetByIdAsync(employee.UserId);

                return View(employee);
            }

            await _employeeService.UpdateAsync(employee);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}