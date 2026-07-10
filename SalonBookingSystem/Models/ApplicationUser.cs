using System.ComponentModel.DataAnnotations;
using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; } = UserRole.Client;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
