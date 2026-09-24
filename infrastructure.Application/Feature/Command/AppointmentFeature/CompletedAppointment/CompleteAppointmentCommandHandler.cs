using DoctorsTower.Domain.Entities;
using DoctorsTower.Domain.Entities.Enums;

using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.Appointment.CompleteAppointment
{
    public class CompleteAppointmentCommandHandler
        : IRequestHandler<CompleteAppointmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompleteAppointmentCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            CompleteAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointmentRepository =
                _unitOfWork.GetRepository<DoctorsTower.Domain.Entities.Appointment>();

            // Rule 1:
            // Appointment must exist
            var appointment =
                await appointmentRepository
                    .GetByIdAsync(request.Id);

            if (appointment == null)
                return false;

            // Rule 2:
            // Only Booked appointment can be completed
            if (appointment.Status !=
                AppointmentStatus.Booked)
            {
                throw new Exception(
                    "Only booked appointments can be completed.");
            }

            // Rule 3:
            // Appointment date must have arrived
            if (appointment.AppointmentDate >
                DateTime.Now)
            {
                throw new Exception(
                    "Future appointment cannot be completed.");
            }

            appointment.Status =
                AppointmentStatus.Completed;

            appointmentRepository.Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}