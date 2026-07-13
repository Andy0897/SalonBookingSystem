using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Filters;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AnonymousOnly]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AnonymousOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _userService.RegisterAsync(model);

                TempData["Success"] = "Регистрацията беше успешна.";

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AnonymousOnly]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AnonymousOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.LoginAsync(model);

            if (user == null)
            {
                ModelState.AddModelError("", "Невалиден имейл или парола.");
                return View(model);
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            ClaimsIdentity identity = new(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            if (user.Role == UserRole.Admin)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (user.Role == UserRole.Employee)
            {
                return RedirectToAction("MyReservations", "Employee");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var model = await _userService.GetProfileAsync(userId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.UpdateProfileAsync(model);

            TempData["Success"] = "Профилът беше обновен успешно.";

            return RedirectToAction(nameof(Profile));
        }
    }
}