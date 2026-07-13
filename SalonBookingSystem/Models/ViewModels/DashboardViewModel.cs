using SalonBookingSystem.Models;

namespace SalonBookingSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int UsersCount { get; set; }

        public int EmployeesCount { get; set; }

        public int ServicesCount { get; set; }

        public int ReservationsCount { get; set; }

        public List<Reservation> LastReservations { get; set; } = new();
    }
}