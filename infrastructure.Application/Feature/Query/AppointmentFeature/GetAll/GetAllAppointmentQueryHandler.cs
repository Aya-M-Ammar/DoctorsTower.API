using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.AppointmentFeature.GetAll
{
    public class GetAllAppointmentQueryHandler
        : IRequestHandler<
            GetAllAppointmentQuery,
            IEnumerable<AppointmentDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAppointmentQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDTO>> Handle(
            GetAllAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _unitOfWork
                .GetRepository<Appointment>()
                .GetAllAsync();

            var result =
                _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);

            return result;
        }
    }
}
