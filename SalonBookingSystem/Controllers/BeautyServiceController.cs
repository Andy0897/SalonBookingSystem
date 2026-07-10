using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBookingSystem.Models;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BeautyServiceController : Controller
    {
        private readonly IBeautyServiceService _beautyServiceService;

        public BeautyServiceController(IBeautyServiceService beautyServiceService)
        {
            _beautyServiceService = beautyServiceService;
        }

        // GET: BeautyService
        public async Task<IActionResult> Index()
        {
            var services = await _beautyServiceService.GetAllAsync();
            return View(services);
        }

        // GET: BeautyService/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var service = await _beautyServiceService.GetByIdAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // GET: BeautyService/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BeautyService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BeautyService beautyService)
        {
            if (!ModelState.IsValid)
                return View(beautyService);

            await _beautyServiceService.CreateAsync(beautyService);

            return RedirectToAction(nameof(Index));
        }

        // GET: BeautyService/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _beautyServiceService.GetByIdAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: BeautyService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BeautyService beautyService)
        {
            if (id != beautyService.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(beautyService);

            await _beautyServiceService.UpdateAsync(beautyService);

            return RedirectToAction(nameof(Index));
        }

        // GET: BeautyService/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _beautyServiceService.GetByIdAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: BeautyService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _beautyServiceService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}