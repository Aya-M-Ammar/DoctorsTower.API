using DoctorsTower.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DoctorsTower.Application.DTOs
{
    public class CreateDoctorDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [Required]
       
        public DoctorSpecialization Specialization { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
    }
}