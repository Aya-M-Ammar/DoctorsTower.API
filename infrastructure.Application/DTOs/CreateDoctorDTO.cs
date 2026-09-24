using System.ComponentModel.DataAnnotations;

namespace DoctorsTower.Application.DTOs
{
    public class CreateDoctorDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Specialization { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
    }
}