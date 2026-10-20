using DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient
{
    public class DeletePatientCommandHandler
        : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePatientCommandHandler> _logger;

        public DeletePatientCommandHandler(
            IUnitOfWork unitOfWork, ILogger<DeletePatientCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(
            DeletePatientCommand request,
            CancellationToken cancellationToken)
        {
            var patientRepository =
                _unitOfWork.GetRepository<Patient>();

            // Rule 1:
            // Patient must exist
            var patient = await patientRepository
                .GetByIdAsync(request.Id);

            if (patient == null)
                return false;

            // Rule 2:
            // Patient cannot be deleted
            // if he has appointments
            var appointments = await _unitOfWork
                .GetRepository<DoctorsTower.Domain.Entities.Appointment>()
                .GetAllAsync(
                    x => x.PatientId == request.Id);

            if (appointments.Any())
            {
                _logger.LogError($"Cannot delete patient {patient.Id} because he has appointments.");
                return false;
            } 

            patientRepository.Delete(patient);

        return     await _unitOfWork.SaveChangesAsync()>0;

           
        }
    }
}