using AutoMapper;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;

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
                throw new Exception("Doctor not found.");

            // Rule 3:
            // Start time must be before end time
            if (request.Schedule.StartTime >=
                request.Schedule.EndTime)
            {
                throw new Exception(
                    "Start time must be before end time.");
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
                throw new Exception(
                    "This schedule overlaps with an existing schedule.");
            }

            _mapper.Map(request.Schedule, schedule);

            scheduleRepository.Update(schedule);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}