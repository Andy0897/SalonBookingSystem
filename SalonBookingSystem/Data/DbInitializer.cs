using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Data
{
    public static class DbInitializer
    {
        private static readonly PasswordHasher<ApplicationUser> _hasher = new();


        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();


            bool exists = await context.ApplicationUsers
                .AnyAsync(u => u.Email == "admin@salon.com");


            if (exists)
                return;



            var admin = new ApplicationUser
            {
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@salon.com",
                PhoneNumber = "0000000000",
                Role = UserRole.Admin
            };


            admin.PasswordHash =
                _hasher.HashPassword(admin, "Admin1234");



            context.ApplicationUsers.Add(admin);


            await context.SaveChangesAsync();
        }
    }
}