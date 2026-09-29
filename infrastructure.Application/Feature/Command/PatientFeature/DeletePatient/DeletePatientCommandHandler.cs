using DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient
{
    public class DeletePatientCommandHandler
        : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePatientCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                throw new Exception(
                    "Cannot delete patient because he has appointments.");
            }

            patientRepository.Delete(patient);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}