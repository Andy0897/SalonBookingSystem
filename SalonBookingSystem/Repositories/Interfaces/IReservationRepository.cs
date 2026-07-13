using SalonBookingSystem.Models;

namespace SalonBookingSystem.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();

        Task<List<Reservation>> GetByClientIdAsync(int clientId);

        Task<List<Reservation>> GetByEmployeeIdAsync(int employeeId);

        Task<Reservation?> GetByIdAsync(int id);

        Task CreateAsync(Reservation reservation);

        Task UpdateAsync(Reservation reservation);

        Task DeleteAsync(Reservation reservation);

        Task<bool> HasConflictAsync(int employeeId, DateOnly date, TimeOnly startTime, TimeOnly endTime);

        Task<int> CountUpcomingAsync();

        Task<List<Reservation>> GetLastAsync(int count);
    }
}