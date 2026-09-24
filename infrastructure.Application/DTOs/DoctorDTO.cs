using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.DTOs
{
    public class DoctorDTO
    {
      
        public string FullName { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
