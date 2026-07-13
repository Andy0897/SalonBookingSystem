using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models.ViewModels
{
    public class ReservationFormViewModel
    {

        [Required(ErrorMessage = "Изберете услуга.")]
        [Display(Name = "Услуга")]
        public int BeautyServiceId { get; set; }



        [Required(ErrorMessage = "Изберете служител.")]
        [Display(Name = "Служител")]
        public int EmployeeId { get; set; }



        [Required(ErrorMessage = "Изберете дата.")]
        [Display(Name = "Дата")]
        public DateOnly ReservationDate { get; set; }



        [Required(ErrorMessage = "Изберете час.")]
        [Display(Name = "Час")]
        public TimeOnly StartTime { get; set; }



        public List<SelectListItem> Services { get; set; } = new();



        public List<SelectListItem> Employees { get; set; } = new();



        public List<SelectListItem> AvailableTimes { get; set; } = new();

    }
}