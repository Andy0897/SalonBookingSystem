using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBeautyServiceRepository _beautyServiceRepository;
        private readonly IReservationRepository _reservationRepository;

        public DashboardService(
            IUserRepository userRepository,
            IEmployeeRepository employeeRepository,
            IBeautyServiceRepository beautyServiceRepository,
            IReservationRepository reservationRepository)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _beautyServiceRepository = beautyServiceRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            return new DashboardViewModel
            {
                UsersCount = await _userRepository.CountAsync(),

                EmployeesCount = await _employeeRepository.CountAsync(),

                ServicesCount = await _beautyServiceRepository.CountAsync(),

                ReservationsCount = await _reservationRepository.CountUpcomingAsync(),

                LastReservations = await _reservationRepository.GetLastAsync(5)
            };
        }
    }
}