
using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Appointmentt= DoctorsTower.Domain.Entities.Appointment;

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
            var appointment =
                _mapper.Map<Appointmentt>(request.Appointment);

            await _unitOfWork
                .GetRepository<Appointmentt>()
                .AddAsync(appointment);

            await _unitOfWork.SaveChangesAsync();

            return appointment.Id;
        }
    }

}
