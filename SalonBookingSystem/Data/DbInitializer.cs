using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;

namespace SalonBookingSystem.Data
{
    public static class DbInitializer
    {
        private static readonly PasswordHasher<object> _hasher = new();

        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.ApplicationUsers.AnyAsync(u => u.Role == UserRole.Admin))
                return;

            var admin = new ApplicationUser
            {
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@salon.com",
                PhoneNumber = "0000000000",

                PasswordHash = _hasher.HashPassword(null!, "Admin1234"),

                Role = UserRole.Admin
            };

            context.ApplicationUsers.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
