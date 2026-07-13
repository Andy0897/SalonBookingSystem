using Microsoft.AspNetCore.Mvc.Rendering;
using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;

namespace SalonBookingSystem.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllAsync();

        Task<List<Reservation>> GetClientReservationsAsync(int clientId);

        Task<ReservationFormViewModel> GetCreateModelAsync();

        Task CreateAsync(int clientId, ReservationFormViewModel model);

        Task<List<SelectListItem>> GetEmployeesByServiceAsync(int serviceId);

        Task<Reservation?> GetByIdAsync(int id);

        Task UpdateAsync(Reservation reservation);

        Task ChangeStatusAsync(int id, ReservationStatus status);

        Task<List<SelectListItem>> GetAvailableTimesAsync(int employeeId, int beautyServiceId, DateOnly date);

        Task<List<Reservation>> GetEmployeeReservationsAsync(int employeeId);
    }
}