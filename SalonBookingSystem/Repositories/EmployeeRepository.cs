using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Data;
using SalonBookingSystem.Models;
using SalonBookingSystem.Repositories.Interfaces;

namespace SalonBookingSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.BeautyService)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetByUserIdAsync(int userId)
        {
            return await _context.Employees
                .Include(e => e.EmployeeServices)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.IsActive);
        }

        public async Task<int> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return employee.Id;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetFullEmployeeAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.BeautyService)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task SaveEmployeeServicesAsync(int employeeId, List<int> serviceIds)
        {
            var oldServices = _context.EmployeeServices
                .Where(x => x.EmployeeId == employeeId);

            _context.EmployeeServices.RemoveRange(oldServices);

            foreach (var serviceId in serviceIds)
            {
                _context.EmployeeServices.Add(new EmployeeService
                {
                    EmployeeId = employeeId,
                    BeautyServiceId = serviceId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetByServiceIdAsync(int beautyServiceId)
        {
            return await _context.Employees
                .Where(e => e.IsActive &&
                            e.EmployeeServices
                             .Any(es => es.BeautyServiceId == beautyServiceId))
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Employees
                .CountAsync(e => e.IsActive);
        }
    }
}