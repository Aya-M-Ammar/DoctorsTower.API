using System.ComponentModel.DataAnnotations;

namespace DoctorsTower.Application.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}