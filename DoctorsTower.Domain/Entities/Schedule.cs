
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int AppointmentsPerHour { get; set; }

        public Doctor Doctor { get; set; } = null!;
    }
}
