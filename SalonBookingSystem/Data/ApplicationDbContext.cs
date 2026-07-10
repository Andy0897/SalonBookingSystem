using Microsoft.EntityFrameworkCore;
using SalonBookingSystem.Models;

namespace SalonBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<BeautyService> BeautyServices { get; set; }

        public DbSet<EmployeeService> EmployeeServices { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeService>()
                .HasKey(es => new { es.EmployeeId, es.BeautyServiceId });

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.BeautyService)
                .WithMany(bs => bs.EmployeeServices)
                .HasForeignKey(es => es.BeautyServiceId);

            modelBuilder.Entity<BeautyService>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.Reservations)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.BeautyService)
                .WithMany(b => b.Reservations)
                .HasForeignKey(r => r.BeautyServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
