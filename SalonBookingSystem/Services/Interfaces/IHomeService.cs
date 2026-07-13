using SalonBookingSystem.ViewModels.Home;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeModelAsync();
    }
}