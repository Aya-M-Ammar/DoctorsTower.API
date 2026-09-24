using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.DoctorQuery.GetAllDoctor
{
    public class GetAllDoctorQueryHandler
       : IRequestHandler<GetAllDoctorQuery, IEnumerable<DoctorDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDoctorQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDTO>> Handle(
            GetAllDoctorQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = await _unitOfWork
                .GetRepository<Doctor>()
                .GetAllAsync();

            var result = _mapper.Map<IEnumerable<DoctorDTO>>(doctors);

            return result;
        }
    }
}
