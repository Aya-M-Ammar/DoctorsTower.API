using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.DTOs
{
    namespace DoctorsTower.Application.DTOs
    {
        public class ScheduleDTO
        {
            public int Id { get; set; }
            public int DoctorId { get; set; }
            public DayOfWeek Day { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public int AppointmentsPerHour { get; set; }
        }
    }
}
