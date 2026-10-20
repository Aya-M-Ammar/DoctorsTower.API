using DoctorsTower.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
       
        public DoctorSpecialization Specialization { get; set; } 
        public string Phone { get; set; } = null!;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
