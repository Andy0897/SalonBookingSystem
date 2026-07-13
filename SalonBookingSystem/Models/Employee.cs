using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models
{
    public class Employee
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



        [Display(Name = "Начало на работа")]
        public TimeOnly WorkStart { get; set; }



        [Display(Name = "Край на работа")]
        public TimeOnly WorkEnd { get; set; }



        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();



        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;
    }
}