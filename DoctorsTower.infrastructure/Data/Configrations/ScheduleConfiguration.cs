using DoctorsTower.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsTower.Infrastructure.Persistence.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Day)
                   .IsRequired();

            builder.Property(s => s.StartTime)
                   .IsRequired();

            builder.Property(s => s.EndTime)
                   .IsRequired();

            builder.Property(s => s.AppointmentsPerHour)
                   .IsRequired();

            builder.HasOne(s => s.Doctor)
                   .WithMany(d => d.Schedules)
                   .HasForeignKey(s => s.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}