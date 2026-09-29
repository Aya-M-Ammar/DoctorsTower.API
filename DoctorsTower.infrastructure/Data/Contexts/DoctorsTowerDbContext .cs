using DoctorsTower.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorsTower.Infrastructure.Persistence
{
    public class DoctorsTowerDbContext : IdentityDbContext<ApplicationUser>
    {
        public DoctorsTowerDbContext(
            DbContextOptions<DoctorsTowerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DoctorsTowerDbContext).Assembly);
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}