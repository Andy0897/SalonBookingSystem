using SalonBookingSystem.Models;
using SalonBookingSystem.Models.ViewModels;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<EmployeeFormViewModel?> GetCreateModelAsync(int userId);

        Task<EmployeeFormViewModel?> GetEditModelAsync(int employeeId);

        Task CreateAsync(EmployeeFormViewModel model);

        Task UpdateAsync(EmployeeFormViewModel model);

        Task DeleteAsync(int id);

        Task<Employee?> GetByUserIdAsync(int userId);
    }
}