using SalonBookingSystem.Models;

namespace SalonBookingSystem.Repositories.Interfaces
{
    public interface IBeautyServiceRepository
    {
        Task<List<BeautyService>> GetAllAsync();

        Task<BeautyService?> GetByIdAsync(int id);

        Task AddAsync(BeautyService beautyService);

        Task UpdateAsync(BeautyService beautyService);

        Task DeleteAsync(BeautyService beautyService);

        Task<int> CountAsync();
    }
}
