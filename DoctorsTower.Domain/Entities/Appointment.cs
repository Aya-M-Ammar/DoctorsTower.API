using DoctorsTower.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateOnly AppointmentDay { get; set; }

        public TimeOnly AppointmentTime { get; set; }
        public AppointmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
