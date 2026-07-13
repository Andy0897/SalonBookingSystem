using SalonBookingSystem.Models;

namespace SalonBookingSystem.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> GetByUserIdAsync(int userId);

        Task<int> CreateAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Employee employee);

        Task<Employee?> GetFullEmployeeAsync(int id);

        Task SaveEmployeeServicesAsync(int employeeId, List<int> serviceIds);

        Task<List<Employee>> GetByServiceIdAsync(int beautyServiceId);

        Task<int> CountAsync();
    }
}