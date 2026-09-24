
using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Query.ScheduleQuery.GetAll;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.ScheduleQuery.GetAll
{
    public class GetAllScheduleQueryHandler
       : IRequestHandler<GetAllScheduleQuery, IEnumerable<ScheduleDTO>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllScheduleQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleDTO>> Handle(
            GetAllScheduleQuery request,
            CancellationToken cancellationToken)
        {
            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync();

            if (!schedules.Any()) return [];
            var result = _mapper.Map<IEnumerable<ScheduleDTO>>(schedules);

            return result;
        }
    }
}
