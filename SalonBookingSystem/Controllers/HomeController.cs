using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Models;
using SalonBookingSystem.Services;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IBeautyServiceService _beautyServiceService;
        private readonly IEmployeeService _employeeService;

        public HomeController(IHomeService homeService, IBeautyServiceService beautyServiceService, IEmployeeService employeeService)
        {
            _homeService = homeService;
            _beautyServiceService = beautyServiceService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _homeService.GetHomeModelAsync();

            return View(model);
        }

        public async Task<IActionResult> Services()
        {
            var services = await _beautyServiceService.GetAllAsync();

            return View(services);
        }

        public async Task<IActionResult> Employees()
        {
            var employees = await _employeeService.GetAllAsync();

            return View(employees);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
