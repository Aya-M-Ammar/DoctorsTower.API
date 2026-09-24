
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}
