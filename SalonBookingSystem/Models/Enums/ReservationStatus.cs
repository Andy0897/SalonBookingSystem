using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models.Enums
{
    public enum ReservationStatus
    {
        [Display(Name = "Резервиран")]
        Reserved,

        [Display(Name = "Завършен")]
        Completed,

        [Display(Name = "Анулиран")]
        Cancelled
    }
}