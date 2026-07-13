using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Data;
using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Repositories.Interfaces;

namespace SalonBookingSystem.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                    .ThenInclude(e => e.User)
                .Include(r => r.BeautyService)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetByClientIdAsync(int clientId)
        {
            return await _context.Reservations
                .Where(r => r.ClientId == clientId)
                .Include(r => r.Employee)
                    .ThenInclude(e => e.User)
                .Include(r => r.BeautyService)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Reservations
                .Where(r => r.EmployeeId == employeeId)
                .Include(r => r.Client)
                .Include(r => r.BeautyService)
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                    .ThenInclude(e => e.User)
                .Include(r => r.BeautyService)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasConflictAsync(int employeeId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Reservations
                .AnyAsync(r =>
                    r.EmployeeId == employeeId &&
                    r.ReservationDate == date &&
                    r.Status != ReservationStatus.Cancelled &&
                    startTime < r.EndTime &&
                    endTime > r.StartTime);
        }

        public async Task<int> CountUpcomingAsync()
        {
            return await _context.Reservations
                .CountAsync(r =>
                    r.Status != ReservationStatus.Completed &&
                    r.Status != ReservationStatus.Cancelled);
        }

        public async Task<List<Reservation>> GetLastAsync(int count)
        {
            return await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                    .ThenInclude(e => e.User)
                .Include(r => r.BeautyService)
                .OrderByDescending(r => r.ReservationDate)
                .ThenByDescending(r => r.StartTime)
                .Take(count)
                .ToListAsync();
        }
    }
}