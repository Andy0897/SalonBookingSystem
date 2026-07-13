using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models
{
    public class EmployeeService
    {

        [Display(Name = "Служител")]
        public int EmployeeId { get; set; }


        public Employee Employee { get; set; } = null!;



        [Display(Name = "Услуга")]
        public int BeautyServiceId { get; set; }


        public BeautyService BeautyService { get; set; } = null!;
    }
}