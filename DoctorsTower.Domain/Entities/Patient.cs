using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
