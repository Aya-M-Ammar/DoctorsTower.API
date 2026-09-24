
using DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule
{
    public class DeleteScheduleCommandHandler
        : IRequestHandler<DeleteScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteScheduleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork
                .GetRepository<Schedule>()
                .GetByIdAsync(request.Id);

            if (schedule == null)
                return false;

            _unitOfWork
                .GetRepository<Schedule>()
                .Delete(schedule);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
