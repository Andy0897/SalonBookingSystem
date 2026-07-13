using System.ComponentModel.DataAnnotations;
using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа.")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = string.Empty;



        [Required(ErrorMessage = "Фамилията е задължителна.")]
        [StringLength(50, ErrorMessage = "Фамилията не може да бъде повече от 50 символа.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;



        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;



        [Required]
        public string PasswordHash { get; set; } = string.Empty;



        [Phone(ErrorMessage = "Невалиден телефонен номер.")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }



        [Display(Name = "Роля")]
        public UserRole Role { get; set; } = UserRole.Client;



        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}