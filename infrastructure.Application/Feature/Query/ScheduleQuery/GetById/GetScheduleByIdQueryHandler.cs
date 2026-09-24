using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.DTOs.DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.ScheduleQuery.GetById
{
    public class GetScheduleByIdQueryHandler
          : IRequestHandler<GetScheduleByIdQuery, ScheduleDTO>
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

        public async Task<ScheduleDTO> Handle(
            GetScheduleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork
                .GetRepository<Schedule>()
                .GetByIdAsync(request.Id);

            if (schedule == null)
                return null;

            var result = _mapper.Map<ScheduleDTO>(schedule);

            return result;
        }
    }
}
