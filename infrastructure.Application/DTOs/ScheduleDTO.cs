using System.ComponentModel.DataAnnotations;

namespace DoctorsTower.Application.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(1, 60)]
        public int AppointmentsPerHour { get; set; }
    }
}