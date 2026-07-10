using SalonBookingSystem.Models;

namespace SalonBookingSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task AddAsync(ApplicationUser user);

        Task<bool> EmailExistsAsync(string email);

        Task<List<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser?> GetByIdAsync(int id);

        Task UpdateAsync(ApplicationUser user);
    }
}