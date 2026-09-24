using DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment;
using DoctorsTower.Domain.Entities;
using DoctorsTower.Domain.Entities.Enums;

using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment
{
    public class CancelAppointmentCommandHandler
        : IRequestHandler<CancelAppointmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelAppointmentCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            CancelAppointmentCommand request,
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
            // Cannot cancel an already cancelled appointment
            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new Exception(
                    "Appointment is already cancelled.");
            }

            // Rule 3:
            // Cannot cancel a completed appointment
            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new Exception(
                    "Completed appointment cannot be cancelled.");
            }

            // Change status
            appointment.Status =
                AppointmentStatus.Cancelled;

            appointmentRepository.Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}