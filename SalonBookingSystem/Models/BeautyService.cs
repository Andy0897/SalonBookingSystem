using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBookingSystem.Models
{
    public class BeautyService
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(100)]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(5, 600)]
        [Display(Name = "Продължителност (минути)")]
        public int DurationMinutes { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "10000")]
        [Display(Name = "Цена (лв.)")]
        public decimal Price { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}