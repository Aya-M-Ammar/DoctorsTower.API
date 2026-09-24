using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.AppointmentFeature.GetById
{
    public class GetAppointmentsQueryHandler
    : IRequestHandler<GetAppointmentsQuery, IEnumerable<AppointmentDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAppointmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<AppointmentDTO>> Handle(
      GetAppointmentsQuery request,
      CancellationToken cancellationToken)
        {
            IEnumerable<Appointment> appointments;

            if (request.DoctorId.HasValue)
            {
                appointments = await _unitOfWork
                    .GetRepository<Appointment>()
                    .GetAllAsync(x => x.DoctorId == request.DoctorId.Value);
            }
            else if (request.PatientId.HasValue)
            {
                appointments = await _unitOfWork
                    .GetRepository<Appointment>()
                    .GetAllAsync(x => x.PatientId == request.PatientId.Value);
            }
            else
            {
                appointments = await _unitOfWork
                    .GetRepository<Appointment>()
                    .GetAllAsync();
            }

            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }
    }
}