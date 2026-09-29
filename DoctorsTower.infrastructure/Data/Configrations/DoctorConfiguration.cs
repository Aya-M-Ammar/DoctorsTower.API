using DoctorsTower.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsTower.Infrastructure.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Specialization)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(d => d.Schedules)
                   .WithOne(s => s.Doctor)
                   .HasForeignKey(s => s.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Appointments)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}