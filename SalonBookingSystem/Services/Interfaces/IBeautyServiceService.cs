using SalonBookingSystem.Models;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IBeautyServiceService
    {
        Task<List<BeautyService>> GetAllAsync();

        Task<BeautyService?> GetByIdAsync(int id);

        Task CreateAsync(BeautyService beautyService);

        Task UpdateAsync(BeautyService beautyService);

        Task DeleteAsync(int id);
    }
}