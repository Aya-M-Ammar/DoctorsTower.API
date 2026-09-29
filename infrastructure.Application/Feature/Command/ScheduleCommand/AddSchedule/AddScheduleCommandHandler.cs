using AutoMapper;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;

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
            var scheduleRepository =
                _unitOfWork.GetRepository<Schedule>();

            // Rule 1:
            // Doctor must exist
            var doctor = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Doctor>()
                .GetByIdAsync(request.Schedule.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            // Rule 2:
            // Start time must be before end time
            if (request.Schedule.StartTime >=
                request.Schedule.EndTime)
            {
                throw new Exception(
                    "Start time must be before end time.");
            }

            // Rule 3:
            // Schedule must not overlap with another schedule
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
                throw new Exception(
                    "This schedule overlaps with an existing schedule.");
            }

            var schedule =
                _mapper.Map<Schedule>(request.Schedule);

            await scheduleRepository.AddAsync(schedule);

          return   await _unitOfWork.SaveChangesAsync();

            
        }
    }
}