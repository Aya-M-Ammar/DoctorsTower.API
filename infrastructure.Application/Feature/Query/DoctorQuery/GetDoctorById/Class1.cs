
using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.DoctorQuery.GetDoctorById
{
    public class GetDoctorByIdQueryHandler
          : IRequestHandler<GetDoctorByIdQuery, DoctorDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDoctorByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DoctorDTO> Handle(
            GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork
                .GetRepository<Doctor>()
                .GetByIdAsync(request.Id);

            var result = _mapper.Map<DoctorDTO>(doctor);

            return result;
        }
    }
}
