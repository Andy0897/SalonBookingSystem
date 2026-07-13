using System.ComponentModel.DataAnnotations;

namespace SalonBookingSystem.Models.ViewModels
{
    public class ServiceSelectionViewModel
    {

        public int BeautyServiceId { get; set; }



        [Display(Name = "Услуга")]
        public string BeautyServiceName { get; set; } = string.Empty;



        [Display(Name = "Избрана")]
        public bool Selected { get; set; }

    }
}