
using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.PatientQuery.GetAll
{
    public class GetAllPatientQueryHandler
        : IRequestHandler<GetAllPatientQuery, IEnumerable<PatientDTO>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPatientQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDTO>> Handle(
            GetAllPatientQuery request,
            CancellationToken cancellationToken)
        {
            var patients = await _unitOfWork
                .GetRepository<Patient>()
                .GetAllAsync();
            if (!patients.Any()) return [];
            var result = _mapper.Map<IEnumerable<PatientDTO>>(patients);

            return result;
        }
    }
}
