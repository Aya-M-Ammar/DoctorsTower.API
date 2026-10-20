using AutoMapper;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using Hangfire.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule
{
    public class UpdateScheduleCommandHandler
        : IRequestHandler<UpdateScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateScheduleCommandHandler> _logger;

        public UpdateScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,ILogger<UpdateScheduleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(
            UpdateScheduleCommand request,
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
            // Doctor must exist
            var doctor = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Doctor>()
                .GetByIdAsync(request.Schedule.DoctorId);

            if (doctor == null)
            {
                _logger.LogWarning(
                    $"Doctor with ID {request.Schedule.DoctorId} not found."  );
                return false;
            }

            // Rule 3:
            // Start time must be before end time
            if (request.Schedule.StartTime >=
                request.Schedule.EndTime)
            {
                _logger.LogError(
                    $"Invalid schedule times: StartTime {request.Schedule.StartTime} is not before EndTime {request.Schedule.EndTime}.");
                return false;   
            }

            // Rule 4:
            // No overlapping schedule
            // Exclude the current schedule
            var existingSchedules =
                await scheduleRepository.GetAllAsync(
                    x =>
                        x.Id != request.Id &&
                        x.DoctorId == request.Schedule.DoctorId &&
                        x.Day == request.Schedule.Day &&
                        x.StartTime < request.Schedule.EndTime &&
                        x.EndTime > request.Schedule.StartTime);

            if (existingSchedules.Any())
            {
                _logger.LogError(
                    "This schedule overlaps with an existing schedule.");
                return false;
            }

            _mapper.Map(request.Schedule, schedule);

            scheduleRepository.Update(schedule);

            return await _unitOfWork.SaveChangesAsync()>0;

            
        }
    }
}