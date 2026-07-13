using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }



        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа.")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = null!;



        [Required(ErrorMessage = "Фамилията е задължителна.")]
        [StringLength(50, ErrorMessage = "Фамилията не може да бъде повече от 50 символа.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;



        [Required(ErrorMessage = "Телефонът е задължителен.")]
        [Phone(ErrorMessage = "Невалиден телефонен номер.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = null!;



        [Display(Name = "Имейл")]
        public string Email { get; set; } = null!;
    }
}