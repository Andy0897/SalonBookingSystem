using SalonBookingSystem.Models;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task CreateAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int id);
    }
}