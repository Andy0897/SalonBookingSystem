using SalonBookingSystem.Models;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class BeautyServiceService : IBeautyServiceService
    {
        private readonly IBeautyServiceRepository _repository;

        public BeautyServiceService(IBeautyServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BeautyService>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BeautyService?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(BeautyService beautyService)
        {
            await _repository.AddAsync(beautyService);
        }

        public async Task UpdateAsync(BeautyService beautyService)
        {
            await _repository.UpdateAsync(beautyService);
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _repository.GetByIdAsync(id);

            if (service != null)
            {
                await _repository.DeleteAsync(service);
            }
        }
    }
}