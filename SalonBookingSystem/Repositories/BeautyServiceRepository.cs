using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Data;
using SalonBookingSystem.Models;
using SalonBookingSystem.Repositories.Interfaces;

namespace SalonBookingSystem.Repositories
{
    public class BeautyServiceRepository : IBeautyServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public BeautyServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BeautyService>> GetAllAsync()
        {
            return await _context.BeautyServices.ToListAsync();
        }

        public async Task<BeautyService?> GetByIdAsync(int id)
        {
            return await _context.BeautyServices.FindAsync(id);
        }

        public async Task AddAsync(BeautyService beautyService)
        {
            _context.BeautyServices.Add(beautyService);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BeautyService beautyService)
        {
            _context.BeautyServices.Update(beautyService);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BeautyService beautyService)
        {
            _context.BeautyServices.Remove(beautyService);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.BeautyServices.CountAsync();
        }
    }
}