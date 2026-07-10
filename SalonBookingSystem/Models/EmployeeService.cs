namespace SalonBookingSystem.Models
{
    public class EmployeeService
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public int BeautyServiceId { get; set; }

        public BeautyService BeautyService { get; set; } = null!;
    }
}
