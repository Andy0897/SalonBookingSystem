using SalonBookingSystem.Models.ViewModels;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}