using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Services;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IReservationService _reservationService;

        public EmployeeController(IEmployeeService employeeService, IReservationService reservationService)
        {
            _employeeService = employeeService;
            _reservationService = reservationService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int userId)
        {
            var model = await _employeeService.GetCreateModelAsync(userId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormViewModel model)
        {
            ModelState.Remove(nameof(model.User));

            if (!ModelState.IsValid)
            {
                var reload = await _employeeService.GetCreateModelAsync(model.UserId);

                if (reload == null)
                    return NotFound();

                reload.Specialty = model.Specialty;
                reload.Description = model.Description;
                reload.WorkStart = model.WorkStart;
                reload.WorkEnd = model.WorkEnd;

                foreach (var service in reload.Services)
                {
                    service.Selected = model.Services
                        .Any(x => x.BeautyServiceId == service.BeautyServiceId && x.Selected);
                }

                return View(reload);
            }

            await _employeeService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _employeeService.GetEditModelAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeFormViewModel model)
        {
            ModelState.Remove(nameof(model.User));

            if (!ModelState.IsValid)
            {
                var reload = await _employeeService.GetEditModelAsync(model.Id);

                if (reload == null)
                    return NotFound();

                reload.Specialty = model.Specialty;
                reload.Description = model.Description;
                reload.WorkStart = model.WorkStart;
                reload.WorkEnd = model.WorkEnd;

                foreach (var service in reload.Services)
                {
                    service.Selected = model.Services
                        .Any(x => x.BeautyServiceId == service.BeautyServiceId && x.Selected);
                }

                return View(reload);
            }

            await _employeeService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyReservations()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var employee = await _employeeService.GetByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var reservations =
                await _reservationService.GetEmployeeReservationsAsync(employee.Id);

            return View(reservations);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ReservationDetails(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var employee = await _employeeService.GetByUserIdAsync(userId);

            if (employee == null || reservation.EmployeeId != employee.Id)
            {
                return Forbid();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CompleteReservation(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var employee = await _employeeService.GetByUserIdAsync(userId);

            if (employee == null || reservation.EmployeeId != employee.Id)
            {
                return Forbid();
            }

            await _reservationService.ChangeStatusAsync(
                id,
                ReservationStatus.Completed);

            return RedirectToAction(nameof(MyReservations));
        }
    }
}