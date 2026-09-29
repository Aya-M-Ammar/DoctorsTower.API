
using AutoMapper;
using DoctorsTower.Application.Feature.Command.Appointment.UpdateAppointment;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Appointmentt= DoctorsTower.Domain.Entities.Appointment;


namespace DoctorsTower.Application.Feature.Command.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler
        : IRequestHandler<UpdateAppointmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork
                .GetRepository<Appointmentt>()
                .GetByIdAsync(request.Id);

            if (appointment == null)
                return false;

            _mapper.Map(request.Appointment, appointment);

            _unitOfWork
                .GetRepository<Appointmentt>()
                .Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
