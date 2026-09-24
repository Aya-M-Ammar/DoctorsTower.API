using DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule
{ 
    public class DeleteScheduleCommandHandler
        : IRequestHandler<DeleteScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteScheduleCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                         x.AppointmentDate.DayOfWeek == schedule.Day &&
                         x.AppointmentDate.TimeOfDay >= schedule.StartTime &&
                         x.AppointmentDate.TimeOfDay < schedule.EndTime);

            if (appointments.Any())
            {
                throw new Exception(
                    "Cannot delete schedule because it has appointments.");
            }

            scheduleRepository.Delete(schedule);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}