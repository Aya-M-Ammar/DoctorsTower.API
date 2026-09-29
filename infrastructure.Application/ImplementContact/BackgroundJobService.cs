
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.ImplementContact
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BackgroundJobService> _logger;

        public BackgroundJobService(
            IUnitOfWork unitOfWork,
            ILogger<BackgroundJobService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // ==========================================
        // 1. Fire-and-Forget
        // Notify Doctor after booking
        // ==========================================

        public void EnqueueDoctorNotification(int appointmentId)
        {
            BackgroundJob.Enqueue(
                () => NotifyDoctor(appointmentId)
            );
        }

        public void NotifyDoctor(int appointmentId)
        {
            var appointment = _unitOfWork
                .GetRepository<Appointment>()
                .GetByIdAsync(appointmentId)
                .GetAwaiter()
                .GetResult();

            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} not found.",
                    appointmentId);

                return;
            }

            var doctor = _unitOfWork
                .GetRepository<Doctor>()
                .GetByIdAsync(appointment.DoctorId)
                .GetAwaiter()
                .GetResult();

            if (doctor == null)
            {
                _logger.LogWarning(
                    "Doctor {DoctorId} not found.",
                    appointment.DoctorId);

                return;
            }

            _logger.LogInformation(
                "Doctor {DoctorName} has a new appointment on {AppointmentDate}.",
                doctor.FullName,
                appointment.AppointmentDate);
        }

        // ==========================================
        // 2. Delayed Job
        // Send Reminder to Patient one day before
        // ==========================================

        public void ScheduleAppointmentReminder(
            int appointmentId,
            DateTime appointmentDate)
        {
            var reminderTime = appointmentDate.AddDays(-1);

            if (reminderTime <= DateTime.Now)
            {
                return;
            }

            BackgroundJob.Schedule(
                () => SendAppointmentReminder(appointmentId),
                reminderTime - DateTime.Now
            );
        }

        public void SendAppointmentReminder(int appointmentId)
        {
            var appointment = _unitOfWork
                .GetRepository<Appointment>()
                .GetByIdAsync(appointmentId)
                .GetAwaiter()
                .GetResult();

            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} not found.",
                    appointmentId);

                return;
            }

            var patient = _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(appointment.PatientId)
                .GetAwaiter()
                .GetResult();

            if (patient == null)
            {
                _logger.LogWarning(
                    "Patient {PatientId} not found.",
                    appointment.PatientId);

                return;
            }

            _logger.LogInformation(
                "Reminder: Patient {PatientId} has an appointment on {AppointmentDate}.",
                patient.Id,
                appointment.AppointmentDate);
        }

        // ==========================================
        // 3. Recurring Job
        // Delete expired appointments
        // ==========================================

        public void DeleteExpiredAppointments()
        {
            var appointmentRepository =
                _unitOfWork.GetRepository<Appointment>();

            var expiredAppointments = appointmentRepository
                .GetAllAsync(
                    x => x.AppointmentDate < DateTime.Now)
                .GetAwaiter()
                .GetResult();

            foreach (var appointment in expiredAppointments)
            {
                appointmentRepository.Delete(appointment);
            }

            _unitOfWork
                .SaveChangesAsync()
                .GetAwaiter()
                .GetResult();

            _logger.LogInformation(
                "Expired appointments deleted successfully.");
        }
    }
}
