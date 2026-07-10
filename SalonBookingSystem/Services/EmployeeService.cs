using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Employee employee)
        {
            var existing = await _employeeRepository.GetByUserIdAsync(employee.UserId);

            if (existing != null)
                throw new Exception("Този потребител вече е служител.");

            var user = await _userRepository.GetByIdAsync(employee.UserId);

            if (user == null)
                throw new Exception("Потребителят не съществува.");

            user.Role = UserRole.Employee;

            await _userRepository.UpdateAsync(user);

            await _employeeRepository.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            ApplicationUser user = employee.User;
            user.Role = UserRole.Client;
            await _userRepository.UpdateAsync(user);

            if (employee == null)
                return;

            await _employeeRepository.DeleteAsync(employee);
        }
    }
}