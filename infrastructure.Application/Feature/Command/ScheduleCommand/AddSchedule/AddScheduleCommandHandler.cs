
using AutoMapper;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule
{
    public class AddScheduleCommandHandler
        : IRequestHandler<AddScheduleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddScheduleCommandHandler> _logger;

        public AddScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddScheduleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(
            AddScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var scheduleRepository =
                _unitOfWork.GetRepository<Schedule>();

            // Rule 1: Doctor must exist
            var doctor = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Doctor>()
                .GetByIdAsync(request.Schedule.DoctorId);

            if (doctor == null)
            {
                _logger.LogError(
                    $"Doctor with ID {request.Schedule.DoctorId} not found.",
                    request.Schedule.DoctorId);

                return 0;
            }

            // Rule 2: Start time must be before end time
            if (request.Schedule.StartTime >= request.Schedule.EndTime)
            {
                _logger.LogError(
                    $"Start time {request.Schedule.StartTime} must be before end time {request.Schedule.EndTime}.",
                    request.Schedule.StartTime,
                    request.Schedule.EndTime);

                return 0;
            }

            // Rule 3: Prevent overlapping schedules
            // for the same doctor and same day
            var existingSchedules =
                await scheduleRepository.GetAllAsync(
                    x =>
                        x.DoctorId == request.Schedule.DoctorId &&
                        x.Day == request.Schedule.Day &&
                        x.StartTime < request.Schedule.EndTime &&
                        x.EndTime > request.Schedule.StartTime);

            if (existingSchedules.Any())
            {
                _logger.LogError(
                    "The schedule overlaps with an existing schedule.");

                return 0;
            }

            // Map DTO to Entity
            var schedule =
                _mapper.Map<Schedule>(request.Schedule);

            await scheduleRepository.AddAsync(schedule);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
