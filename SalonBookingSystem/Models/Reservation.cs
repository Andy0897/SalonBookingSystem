using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public ApplicationUser Client { get; set; } = null!;

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public int BeautyServiceId { get; set; }

        public BeautyService BeautyService { get; set; } = null!;

        public DateOnly ReservationDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
