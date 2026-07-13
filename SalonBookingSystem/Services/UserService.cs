using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public UserService(
            IUserRepository userRepository,
            PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task RegisterAsync(RegisterViewModel model)
        {
            if (await _userRepository.EmailExistsAsync(model.Email))
                throw new Exception("Потребител с този имейл вече съществува.");

            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = _passwordService.Hash(model.Password),
                Role = Models.Enums.UserRole.Client
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<ApplicationUser?> LoginAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
                return null;

            bool valid = _passwordService.Verify(
                user.PasswordHash,
                model.Password);

            if (!valid)
                return null;

            return user;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ProfileViewModel?> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

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
                throw new Exception("Потребителят не съществува.");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            await _userRepository.UpdateAsync(user);
        }
    }
}