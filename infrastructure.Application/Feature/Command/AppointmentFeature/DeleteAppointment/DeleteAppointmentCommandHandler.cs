
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Appointmentt = DoctorsTower.Domain.Entities.Appointment;
namespace DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler
         : IRequestHandler<DeleteAppointmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppointmentCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork
                .GetRepository<Appointmentt>()
                .GetByIdAsync(request.Id);

            if (appointment == null)
                return false;

            _unitOfWork
                .GetRepository<Appointmentt>()
                .Delete(appointment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
