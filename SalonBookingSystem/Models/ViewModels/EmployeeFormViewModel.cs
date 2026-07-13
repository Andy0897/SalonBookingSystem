using SalonBookingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models.ViewModels
{
    public class EmployeeFormViewModel
    {
        public int Id { get; set; }


        public int UserId { get; set; }


        public ApplicationUser User { get; set; } = null!;



        [Required(ErrorMessage = "Специалността е задължителна.")]
        [StringLength(100, ErrorMessage = "Специалността не може да бъде повече от 100 символа.")]
        [Display(Name = "Специалност")]
        public string Specialty { get; set; } = string.Empty;



        [StringLength(500, ErrorMessage = "Описанието не може да бъде повече от 500 символа.")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;



        [Required(ErrorMessage = "Началният час е задължителен.")]
        [Display(Name = "Начало на работа")]
        public TimeOnly WorkStart { get; set; }



        [Required(ErrorMessage = "Крайният час е задължителен.")]
        [Display(Name = "Край на работа")]
        public TimeOnly WorkEnd { get; set; }



        [Display(Name = "Услуги")]
        public List<ServiceSelectionViewModel> Services { get; set; } = new();
    }
}