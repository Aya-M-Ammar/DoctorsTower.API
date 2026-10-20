
using DoctorsTower.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace DoctorsTower.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public UserType UserType { get; set; }

        public int? DoctorId { get; set; }

        public int? PatientId { get; set; }

        public Doctor? Doctor { get; set; }

        public Patient? Patient { get; set; }
    }
}

