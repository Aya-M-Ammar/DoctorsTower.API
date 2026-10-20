
using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.Domain.Entities.Enums;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.Appointment.AddAppointment
{
    public class AddAppointmentCommandHandler
        : IRequestHandler<AddAppointmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAppointmentCommandHandler> _logger;

        public AddAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddAppointmentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(
            AddAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointmentRepository =
                _unitOfWork.GetRepository<DoctorsTower.Domain.Entities.Appointment>();

            var doctorId = request.Appointment.DoctorId;
            var patientId = request.Appointment.PatientId;
            var appointmentDay = request.Appointment.AppointmentDay;

            // Rule 1: Doctor must exist
            var doctor = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Doctor>()
                .GetByIdAsync(doctorId);

            if (doctor == null)
            {
                _logger.LogWarning(
                    "Doctor with ID {DoctorId} was not found.",
                    doctorId);

                return 0;
            }

            // Rule 2: Patient must exist
            var patient = await _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(patientId);

            if (patient == null)
            {
                _logger.LogWarning(
                    "Patient with ID {PatientId} was not found.",
                    patientId);

                return 0;
            }

            // Rule 3: Appointment day cannot be in the past
            if (appointmentDay < DateOnly.FromDateTime(DateTime.Today))
            {
                _logger.LogWarning(
                    "Appointment day {AppointmentDay} is in the past.",
                    appointmentDay);

                return 0;
            }

            // Rule 4: Get doctor's schedules for the selected day
            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync(x =>
                    x.DoctorId == doctorId &&
                    x.Day == appointmentDay.DayOfWeek);

            var orderedSchedules = schedules
                .OrderBy(x => x.StartTime)
                .ToList();

            if (!orderedSchedules.Any())
            {
                _logger.LogWarning(
                    "No schedule found for Doctor {DoctorId} on {Day}.",
                    doctorId,
                    appointmentDay.DayOfWeek);

                return 0;
            }

            // Rule 5: Get existing non-cancelled appointments
            var existingAppointments =
                await appointmentRepository.GetAllAsync(x =>
                    x.DoctorId == doctorId &&
                    x.AppointmentDay == appointmentDay &&
                    x.Status != AppointmentStatus.Cancelled);

            var bookedTimes = existingAppointments
                .Select(x => x.AppointmentTime)
                .ToHashSet();

            // Rule 6: Find the first available time slot
            TimeOnly? availableTime = null;

            foreach (var schedule in orderedSchedules)
            {
                if (schedule.AppointmentsPerHour <= 0)
                {
                    _logger.LogWarning(
                        "Invalid appointment capacity for Schedule {ScheduleId}.",
                        schedule.Id);

                    continue;
                }

                double slotMinutes =
                    60.0 / schedule.AppointmentsPerHour;

                var currentTime = schedule.StartTime;

                while (currentTime.AddMinutes(slotMinutes)
                    <= schedule.EndTime)
                {
                    // Skip past times when booking for today
                    if (appointmentDay ==
                            DateOnly.FromDateTime(DateTime.Today) &&
                        currentTime <= TimeOnly.FromDateTime(DateTime.Now))
                    {
                        currentTime = currentTime.AddMinutes(slotMinutes);
                        continue;
                    }

                    if (!bookedTimes.Contains(currentTime))
                    {
                        availableTime = currentTime;
                        break;
                    }

                    currentTime = currentTime.AddMinutes(slotMinutes);
                }

                if (availableTime.HasValue)
                    break;
            }

            if (!availableTime.HasValue)
            {
                _logger.LogWarning(
                    "No available appointment slots for Doctor {DoctorId} on {Day}.",
                    doctorId,
                    appointmentDay);

                return 0;
            }

            // Rule 7: Create the appointment
            var appointment =
                _mapper.Map<DoctorsTower.Domain.Entities.Appointment>(request.Appointment);

            appointment.AppointmentTime = availableTime.Value;
            appointment.Status = AppointmentStatus.Booked;
            appointment.CreatedAt = DateTime.UtcNow;

            await appointmentRepository.AddAsync(appointment);

           return  await _unitOfWork.SaveChangesAsync();

        
        }
    }
}
