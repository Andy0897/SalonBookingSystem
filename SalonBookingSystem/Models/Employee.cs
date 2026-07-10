using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public string Specialty { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TimeOnly WorkStart { get; set; }

        public TimeOnly WorkEnd { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
