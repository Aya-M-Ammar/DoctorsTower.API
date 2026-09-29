using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.PatientQuery.GetById
{
    public class GetScheduleByIdQueryHandler
       : IRequestHandler<GetPatientByIdQuery, PatientDTO?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetScheduleByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientDTO> Handle(
            GetPatientByIdQuery request,
            CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(request.Id);

            if (patient == null)
                return null;

            var result = _mapper.Map<PatientDTO>(patient);

            return result;
        }
    }
}
