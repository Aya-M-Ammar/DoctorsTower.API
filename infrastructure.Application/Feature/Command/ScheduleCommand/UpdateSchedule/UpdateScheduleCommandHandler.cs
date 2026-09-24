using AutoMapper;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule
{
    public class UpdateScheduleCommandHandler
        : IRequestHandler<UpdateScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork
                .GetRepository<Schedule>()
                .GetByIdAsync(request.Schedule.Id);

            if (schedule == null)
                return false;

            _mapper.Map(request.Schedule, schedule);

            _unitOfWork
                .GetRepository<Schedule>()
                .Update(schedule);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
