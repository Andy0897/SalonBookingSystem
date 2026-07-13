using SalonBookingSystem.Models;

namespace SalonBookingSystem.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<BeautyService> Services { get; set; } = new();

        public List<Employee> Employees { get; set; } = new();
    }
}