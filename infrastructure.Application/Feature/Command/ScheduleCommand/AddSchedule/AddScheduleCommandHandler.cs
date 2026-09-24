
using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule
{
    public class AddScheduleCommandHandler
        : IRequestHandler<AddScheduleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            AddScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request.Schedule);

            await _unitOfWork
                .GetRepository<Schedule>()
                .AddAsync(schedule);

            await _unitOfWork.SaveChangesAsync();

            return schedule.Id;
        }
    }
}
