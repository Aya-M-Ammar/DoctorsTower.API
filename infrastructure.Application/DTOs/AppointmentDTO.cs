using DoctorsTower.Domain.Entities.Enums;

using System.ComponentModel.DataAnnotations;

namespace DoctorsTower.Application.DTOs
{
    public class AppointmentDTO
    {
       // public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }
    }
}