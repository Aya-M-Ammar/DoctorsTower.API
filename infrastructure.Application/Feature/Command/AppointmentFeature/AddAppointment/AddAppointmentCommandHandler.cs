using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.Domain.Entities.Enums;

using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.Appointment.AddAppointment
{
    public class AddAppointmentCommandHandler
        : IRequestHandler<AddAppointmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            AddAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointmentRepository =
                _unitOfWork.GetRepository<DoctorsTower.Domain.Entities.Appointment>();

            // ==========================================
            // Rule 1: Doctor must exist
            // ==========================================

            var doctor = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Doctor>()
                .GetByIdAsync(request.Appointment.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            // ==========================================
            // Rule 2: Patient must exist
            // ==========================================

            var patient = await _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(request.Appointment.PatientId);

            if (patient == null)
                throw new Exception("Patient not found.");

            // ==========================================
            // Rule 3: Appointment cannot be in the past
            // ==========================================

            if (request.Appointment.AppointmentDate <= DateTime.Now)
            {
                throw new Exception(
                    "Appointment date must be in the future.");
            }

            // ==========================================
            // Rule 4:
            // Doctor must have a schedule on this day
            // ==========================================

            var appointmentDate =
                request.Appointment.AppointmentDate;

            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync(
                    x =>
                        x.DoctorId ==
                        request.Appointment.DoctorId
                        &&
                        x.Day ==
                        appointmentDate.DayOfWeek);

            if (!schedules.Any())
            {
                throw new Exception(
                    "Doctor does not have a schedule on this day.");
            }

            // ==========================================
            // Rule 5:
            // Appointment time must be inside schedule
            // ==========================================

            var schedule = schedules.FirstOrDefault(
                x =>
                    appointmentDate.TimeOfDay >= x.StartTime
                    &&
                    appointmentDate.TimeOfDay < x.EndTime);

            if (schedule == null)
            {
                throw new Exception(
                    "Appointment time is outside doctor's working hours.");
            }

            // ==========================================
            // Rule 6:
            // Same patient cannot book same slot twice
            // ==========================================

            var patientAlreadyBooked =
                await appointmentRepository.GetAllAsync(
                    x =>
                        x.DoctorId ==
                        request.Appointment.DoctorId
                        &&
                        x.PatientId ==
                        request.Appointment.PatientId
                        &&
                        x.AppointmentDate ==
                        appointmentDate
                        &&
                        x.Status !=
                        AppointmentStatus.Cancelled);

            if (patientAlreadyBooked.Any())
            {
                throw new Exception(
                    "Patient already booked this appointment.");
            }

            // ==========================================
            // Rule 7:
            // Check appointment capacity
            // ==========================================

            var hourStart = new DateTime(
      appointmentDate.Year,
      appointmentDate.Month,
      appointmentDate.Day,
      appointmentDate.Hour,
      0,
      0);

            var hourEnd = hourStart.AddHours(1);

            var bookedAppointments =
                await appointmentRepository.GetAllAsync(
                    x =>
                        x.DoctorId ==
                        request.Appointment.DoctorId
                        &&
                        x.AppointmentDate >= hourStart
                        &&
                        x.AppointmentDate < hourEnd
                        &&
                        x.Status != AppointmentStatus.Cancelled);

            if (bookedAppointments.Count() >=
                schedule.AppointmentsPerHour)
            {
                throw new Exception(
                    "This hour is fully booked.");
            }

            // ==========================================
            // Create Appointment
            // ==========================================

            var appointment =
                _mapper.Map<DoctorsTower.Domain.Entities.Appointment>(
                    request.Appointment);

            // Always start as Booked
            appointment.Status =
                AppointmentStatus.Booked;

            appointment.CreatedAt =
                DateTime.UtcNow;

            await appointmentRepository
                .AddAsync(appointment);

            await _unitOfWork.SaveChangesAsync();

            return appointment.Id;
        }
    }
}