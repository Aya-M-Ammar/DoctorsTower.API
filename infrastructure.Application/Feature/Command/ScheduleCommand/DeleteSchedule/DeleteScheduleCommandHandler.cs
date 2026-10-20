using DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule
{ 
    public class DeleteScheduleCommandHandler
        : IRequestHandler<DeleteScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteScheduleCommandHandler> _logger;

        public DeleteScheduleCommandHandler(
            IUnitOfWork unitOfWork,ILogger<DeleteScheduleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(
            DeleteScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var scheduleRepository =
                _unitOfWork.GetRepository<Schedule>();

            // Rule 1:
            // Schedule must exist
            var schedule = await scheduleRepository
                .GetByIdAsync(request.Id);

            if (schedule == null)
                return false;

            // Rule 2:
            // Cannot delete schedule if it has appointments
            //
            // We check the doctor's appointments
            // that fall inside this schedule.
            var appointments = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Appointment>()
                .GetAllAsync(
                    x => x.DoctorId == schedule.DoctorId &&
                         x.AppointmentDay.DayOfWeek == schedule.Day &&
                         x.AppointmentTime>= schedule.StartTime &&
                         x.AppointmentTime< schedule.EndTime);

            if (appointments.Any())
            {
              _logger.LogError(
                    $"Cannot delete schedule {request.Id} because it has appointments");
                return false;
            }

            scheduleRepository.Delete(schedule);

           return  await _unitOfWork.SaveChangesAsync()>0;

           
        }
    }
}