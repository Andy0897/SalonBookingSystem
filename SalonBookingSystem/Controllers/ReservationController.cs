using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IUserRepository _userRepository;

        public ReservationController(
            IReservationService reservationService,
            IUserRepository userRepository)
        {
            _reservationService = reservationService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService
                .GetAllAsync();

            return View(reservations);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _reservationService.GetCreateModelAsync();

            return View(model);
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var reload = await _reservationService.GetCreateModelAsync();

                return View(reload);
            }

            var userIdString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);


            if (userIdString == null)
                return Unauthorized();


            int userId = int.Parse(userIdString);


            var user = await _userRepository
                .GetByIdAsync(userId);


            if (user == null)
                return Unauthorized();


            try
            {
                await _reservationService
                    .CreateAsync(user.Id, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }


            return RedirectToAction(
                nameof(MyReservations));
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MyReservations()
        {
            var userIdString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);


            if (userIdString == null)
                return Unauthorized();


            int userId = int.Parse(userIdString);


            var reservations =
                await _reservationService
                .GetClientReservationsAsync(userId);


            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);


            if (reservation == null)
                return NotFound();


            reservation.Status = ReservationStatus.Cancelled;


            await _reservationService.UpdateAsync(reservation);


            return RedirectToAction(nameof(MyReservations));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int id, ReservationStatus status)
        {
            await _reservationService
                .ChangeStatusAsync(id, status);


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(int serviceId)
        {
            var employees = await _reservationService
                .GetEmployeesByServiceAsync(serviceId);

            return Json(employees);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int employeeId, int beautyServiceId, DateOnly date)
        {
            Console.WriteLine("Emoloyee id; " + employeeId);

            var times = await _reservationService
                .GetAvailableTimesAsync(
                    employeeId,
                    beautyServiceId,
                    date);


            return Json(times);
        }
    }
}