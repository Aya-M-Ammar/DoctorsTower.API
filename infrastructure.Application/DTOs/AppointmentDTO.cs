
using DoctorsTower.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
