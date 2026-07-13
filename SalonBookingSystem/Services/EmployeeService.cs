using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBeautyServiceRepository _beautyServiceRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository,
            IBeautyServiceRepository beautyServiceRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _beautyServiceRepository = beautyServiceRepository;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<EmployeeFormViewModel?> GetCreateModelAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            var services = await _beautyServiceRepository.GetAllAsync();

            return new EmployeeFormViewModel
            {
                UserId = user.Id,
                User = user,

                WorkStart = new TimeOnly(9, 0),
                WorkEnd = new TimeOnly(17, 0),

                Services = services.Select(s => new ServiceSelectionViewModel
                {
                    BeautyServiceId = s.Id,
                    BeautyServiceName = s.Name,
                    Selected = false
                }).ToList()
            };
        }

        public async Task<EmployeeFormViewModel?> GetEditModelAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetFullEmployeeAsync(employeeId);

            if (employee == null)
                return null;

            var allServices = await _beautyServiceRepository.GetAllAsync();

            return new EmployeeFormViewModel
            {
                Id = employee.Id,

                UserId = employee.UserId,

                User = employee.User,

                Specialty = employee.Specialty,

                Description = employee.Description,

                WorkStart = employee.WorkStart,

                WorkEnd = employee.WorkEnd,

                Services = allServices.Select(s => new ServiceSelectionViewModel
                {
                    BeautyServiceId = s.Id,

                    BeautyServiceName = s.Name,

                    Selected = employee.EmployeeServices
                        .Any(es => es.BeautyServiceId == s.Id)

                }).ToList()
            };
        }

        public async Task CreateAsync(EmployeeFormViewModel model)
        {
            var existing = await _employeeRepository.GetByUserIdAsync(model.UserId);

            // Ако вече има служител
            if (existing != null)
            {
                // Ако е активен - не може да се добави втори път
                if (existing.IsActive)
                    throw new Exception("Този потребител вече е служител.");

                // Ако е неактивен - активираме стария запис
                existing.IsActive = true;
                existing.Specialty = model.Specialty;
                existing.Description = model.Description;
                existing.WorkStart = model.WorkStart;
                existing.WorkEnd = model.WorkEnd;

                await _employeeRepository.UpdateAsync(existing);

                var selectedServices = model.Services
                    .Where(s => s.Selected)
                    .Select(s => s.BeautyServiceId)
                    .ToList();

                await _employeeRepository.SaveEmployeeServicesAsync(
                    existing.Id,
                    selectedServices);

                var existingUser = await _userRepository.GetByIdAsync(model.UserId);

                if (existingUser != null)
                {
                    existingUser.Role = UserRole.Employee;
                    await _userRepository.UpdateAsync(existingUser);
                }

                return;
            }

            // Няма запис -> създаваме нов
            var user = await _userRepository.GetByIdAsync(model.UserId);

            if (user == null)
                throw new Exception("Потребителят не съществува.");

            var employee = new Employee
            {
                UserId = model.UserId,
                Specialty = model.Specialty,
                Description = model.Description,
                WorkStart = model.WorkStart,
                WorkEnd = model.WorkEnd,
                IsActive = true
            };

            int employeeId = await _employeeRepository.CreateAsync(employee);

            var services = model.Services
                .Where(s => s.Selected)
                .Select(s => s.BeautyServiceId)
                .ToList();

            await _employeeRepository.SaveEmployeeServicesAsync(employeeId, services);

            user.Role = UserRole.Employee;

            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateAsync(EmployeeFormViewModel model)
        {
            var employee = await _employeeRepository.GetByIdAsync(model.Id);

            if (employee == null)
                throw new Exception("Служителят не съществува.");

            employee.Specialty = model.Specialty;

            employee.Description = model.Description;

            employee.WorkStart = model.WorkStart;

            employee.WorkEnd = model.WorkEnd;

            await _employeeRepository.UpdateAsync(employee);

            var selectedServices = model.Services
                .Where(s => s.Selected)
                .Select(s => s.BeautyServiceId)
                .ToList();

            await _employeeRepository.SaveEmployeeServicesAsync(
                employee.Id,
                selectedServices);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return;

            var user = await _userRepository.GetByIdAsync(employee.UserId);
            
            user.Role = UserRole.Client;
            await _userRepository.UpdateAsync(user);

            employee.IsActive = false;
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<Employee?> GetByUserIdAsync(int userId)
        {
            return await _employeeRepository.GetByUserIdAsync(userId);
        }
    }
}