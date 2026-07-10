using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterViewModel model);

        Task<ApplicationUser?> LoginAsync(LoginViewModel model);

        Task<List<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser?> GetByIdAsync(int id);
    }
}