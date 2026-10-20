using DoctorsTower.Application.Feature.Command.DoctorsFeature.DeleteDoctors;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;
using Appointmentt = DoctorsTower.Domain.Entities.Appointment;
using Microsoft.Extensions.Logging;
namespace DoctorsTower.Application.Feature.Command.DoctorsFeature.DeleteDoctors
{

    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteDoctorCommandHandler> _logger;

        public DeleteDoctorCommandHandler(IUnitOfWork unitOfWork,ILogger<DeleteDoctorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle( DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctorRepository =
                _unitOfWork.GetRepository<Doctoree>();

            // Rule 1:
            // Doctor must exist
            var doctor = await doctorRepository
                .GetByIdAsync(request.Id);

            if (doctor == null)
                return false;

            // Rule 2:
            // Doctor cannot be deleted if he has schedules
            var schedules = await _unitOfWork
                .GetRepository<Schedule>()
                .GetAllAsync(x => x.DoctorId == request.Id);
           

            if (schedules.Any())
            {
               _logger.LogError($"Cannot delete doctor with id {request.Id} because he has schedules.");
                return false;
            }

            // Rule 3:
            // Doctor cannot be deleted if he has appointments
            var appointments = await _unitOfWork
                .GetRepository<Appointmentt>()
                .GetAllAsync(x => x.DoctorId == request.Id);

            if (appointments.Any())
            {
               _logger.LogError($"Cannot delete doctor with id {request.Id} because he has appointments.");
                return false;
            }

          doctorRepository.Delete(doctor);

           return  await _unitOfWork.SaveChangesAsync()>0;

           
        }
    }
}