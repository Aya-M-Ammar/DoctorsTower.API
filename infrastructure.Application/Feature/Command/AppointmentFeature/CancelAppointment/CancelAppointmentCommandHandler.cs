using DoctorsTower.Domain.Entities.Enums;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Appointmentt = DoctorsTower.Domain.Entities.Appointment;
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
            var appointment = await _unitOfWork
                .GetRepository<Appointmentt>()
                .GetByIdAsync(request.Id);

            if (appointment == null)
                return false;

            appointment.Status = AppointmentStatus.Cancelled;

            _unitOfWork
                .GetRepository<Appointmentt>()
                .Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
