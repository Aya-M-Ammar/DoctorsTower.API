using DoctorsTower.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorsTower.Infrastructure.Persistence
{
    public class DoctorsTowerDbContext : DbContext
    {
        public DoctorsTowerDbContext(DbContextOptions<DoctorsTowerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DoctorsTowerDbContext).Assembly);
        }
    }
}