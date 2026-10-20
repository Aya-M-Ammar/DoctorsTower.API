
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
            IEnumerable<AvailableSlotDTO>>
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
            // Rule 1: Doctor must exist
            var doctor = await _unitOfWork
                .GetRepository<Doctor>()
                .GetByIdAsync(request.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            // Rule 2: Get doctor's schedule for the selected day
            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync(x =>
                    x.DoctorId == request.DoctorId &&
                    x.Day == request.Date.DayOfWeek);

            if (!schedules.Any())
                return Enumerable.Empty<AvailableSlotDTO>();

            // Rule 3: Get appointments for this doctor and date
            var appointments = await _unitOfWork
                .GetRepository<Appointment>()
                .GetAllAsync(x =>
                    x.DoctorId == request.DoctorId &&
                    x.AppointmentDay == request.Date &&
                    x.Status != AppointmentStatus.Cancelled);

            var bookedTimes = appointments
                .Select(x => x.AppointmentTime)
                .ToHashSet();

            var result = new List<AvailableSlotDTO>();

            // Rule 4: Generate available slots
            foreach (var schedule in schedules.OrderBy(x => x.StartTime))
            {
                if (schedule.AppointmentsPerHour <= 0)
                    continue;

                var slotDuration = TimeSpan.FromMinutes(
                    60.0 / schedule.AppointmentsPerHour);

                var currentTime = schedule.StartTime;

                while (currentTime.Add(slotDuration) <= schedule.EndTime)
                {
                    // Skip past slots when the selected day is today
                    if (request.Date == DateOnly.FromDateTime(DateTime.Today) &&
                        currentTime <= TimeOnly.FromDateTime(DateTime.Now))
                    {
                        currentTime = currentTime.Add(slotDuration);
                        continue;
                    }

                    // A slot is available if its start time is not booked
                    if (!bookedTimes.Contains(currentTime))
                    {
                        result.Add(new AvailableSlotDTO
                        {
                            StartTime = currentTime,
                            EndTime = TimeOnly.FromTimeSpan(
                                currentTime.ToTimeSpan().Add(slotDuration))
                        });
                    }

                    currentTime = currentTime.Add(slotDuration);
                }
            }

            return result;
        }
    }
}
