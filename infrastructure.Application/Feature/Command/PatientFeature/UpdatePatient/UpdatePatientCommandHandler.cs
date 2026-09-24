using AutoMapper;
using DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient
{
    public class UpdatePatientCommandHandler
        : IRequestHandler<UpdatePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePatientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdatePatientCommand request,
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
            // Phone must be unique
            var existingPatient =
                await patientRepository.GetAllAsync(
                    x => x.Phone == request.Patient.Phone
                         && x.Id != request.Id);

            if (existingPatient.Any())
            {
                throw new Exception(
                    "A patient with this phone number already exists.");
            }

            // Rule 3:
            // Date of birth cannot be in the future
            if (request.Patient.DateOfBirth.Date >
                DateTime.UtcNow.Date)
            {
                throw new Exception(
                    "Date of birth cannot be in the future.");
            }

            _mapper.Map(request.Patient, patient);

            patientRepository.Update(patient);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}