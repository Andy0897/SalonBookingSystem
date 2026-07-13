using System.ComponentModel.DataAnnotations;
using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }



        public int ClientId { get; set; }


        public ApplicationUser Client { get; set; } = null!;



        public int EmployeeId { get; set; }


        public Employee Employee { get; set; } = null!;



        public int BeautyServiceId { get; set; }


        public BeautyService BeautyService { get; set; } = null!;



        [Required(ErrorMessage = "Изберете дата.")]
        [Display(Name = "Дата")]
        public DateOnly ReservationDate { get; set; }



        [Required(ErrorMessage = "Изберете начален час.")]
        [Display(Name = "Начален час")]
        public TimeOnly StartTime { get; set; }



        [Display(Name = "Краен час")]
        public TimeOnly EndTime { get; set; }



        [Display(Name = "Статус")]
        public ReservationStatus Status { get; set; }
    }
}