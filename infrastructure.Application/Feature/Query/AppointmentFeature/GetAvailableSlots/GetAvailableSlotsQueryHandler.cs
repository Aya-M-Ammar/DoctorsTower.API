using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.Domain.Entities.Enums;
using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Query.AppointmentQuery.GetAvailableSlots
{
    public class GetAvailableSlotsQueryHandler
        : IRequestHandler<
            GetAvailableSlotsQuery,
            IEnumerable<AvailableSlotDTO>?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableSlotsQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AvailableSlotDTO>> Handle(
            GetAvailableSlotsQuery request,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // Rule 1:
            // Doctor must exist
            // ==========================================

            var doctor = await _unitOfWork
                .GetRepository<Doctor>()
                .GetByIdAsync(request.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            // ==========================================
            // Get doctor's schedule for this day
            // ==========================================

            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync(
                    x =>
                        x.DoctorId == request.DoctorId &&
                        x.Day == request.Date.DayOfWeek);

            if (!schedules.Any())
            {
                return [];
            }

            // ==========================================
            // Get appointments for this doctor/date
            // ==========================================

            var dayStart = request.Date.Date;

            var dayEnd = dayStart.AddDays(1);

            var appointments = await _unitOfWork
                .GetRepository<Appointment>()
                .GetAllAsync(
                    x =>
                        x.DoctorId == request.DoctorId &&
                        x.AppointmentDate >= dayStart &&
                        x.AppointmentDate < dayEnd &&
                        x.Status != AppointmentStatus.Cancelled);

            var result = new List<AvailableSlotDTO>();

            // ==========================================
            // Generate slots
            // ==========================================

            foreach (var schedule in schedules)
            {
                var slotDuration =
                    TimeSpan.FromMinutes(
                        60.0 / schedule.AppointmentsPerHour);

                var currentTime = schedule.StartTime;

                while (currentTime + slotDuration <= schedule.EndTime)
                {
                    var slotStart =
                        request.Date.Date.Add(currentTime);

                    var slotEnd =
                        slotStart.Add(slotDuration);

                    var bookedCount = appointments.Count(
                        x =>
                            x.AppointmentDate >= slotStart &&
                            x.AppointmentDate < slotEnd);

                    if (bookedCount == 0)
                    {
                        result.Add(new AvailableSlotDTO
                        {
                            StartTime = slotStart,
                            EndTime = slotEnd
                        });
                    }

                    currentTime += slotDuration;
                }
            }

            // ==========================================
            // Return available slots
            // ==========================================

            return result;
        }
    }
}