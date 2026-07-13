using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBookingSystem.Models
{
    public class BeautyService
    {
        public int Id { get; set; }



        [Required(ErrorMessage = "Името на услугата е задължително.")]
        [StringLength(100, ErrorMessage = "Името не може да бъде повече от 100 символа.")]
        [Display(Name = "Име на услуга")]
        public string Name { get; set; } = string.Empty;



        [Required(ErrorMessage = "Продължителността е задължителна.")]
        [Range(5, 600, ErrorMessage = "Продължителността трябва да бъде между 5 и 600 минути.")]
        [Display(Name = "Продължителност (минути)")]
        public int DurationMinutes { get; set; }



        [Required(ErrorMessage = "Цената е задължителна.")]
        [Range(typeof(decimal), "0.01", "10000",
            ErrorMessage = "Цената трябва да бъде между 0.01 и 10000 лв.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена (лв.)")]
        public decimal Price { get; set; }



        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}