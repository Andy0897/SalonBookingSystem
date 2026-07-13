using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;
using SalonBookingSystem.ViewModels.Home;

namespace SalonBookingSystem.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBeautyServiceRepository _beautyServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public HomeService(IUserRepository userRepository, IBeautyServiceRepository beautyServiceRepository, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _beautyServiceRepository = beautyServiceRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<HomeViewModel> GetHomeModelAsync()
        {
            return new HomeViewModel
            {
                Services = (await _beautyServiceRepository.GetAllAsync())
                    .Take(6)
                    .ToList(),

                Employees = (await _employeeRepository.GetAllAsync())
                    .Take(4)
                    .ToList()
            };
        }

        public async Task<ProfileViewModel?> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }

        public async Task UpdateProfileAsync(ProfileViewModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);

            if (user == null)
            {
                throw new Exception("Потребителят не съществува.");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            await _userRepository.UpdateAsync(user);
        }
    }
}