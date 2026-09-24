using AutoMapper;
using DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Patientt = DoctorsTower.Domain.Entities.Patient;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient
{
    public class AddPatientCommandHandler
        : IRequestHandler<AddPatientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddPatientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            AddPatientCommand request,
            CancellationToken cancellationToken)
        {
            var patientRepository =
                _unitOfWork.GetRepository<Patientt>();

            // Rule 1:
            // Phone must be unique
            var existingPatient =
                await patientRepository.GetAllAsync(
                    x => x.Phone == request.Patient.Phone);

            if (existingPatient.Any())
            {
                throw new Exception(
                    "A patient with this phone number already exists.");
            }

            // Rule 2:
            // Date of birth cannot be in the future
            if (request.Patient.DateOfBirth.Date > DateTime.UtcNow.Date)
            {
                throw new Exception(
                    "Date of birth cannot be in the future.");
            }

            var patient =
                _mapper.Map<Patientt>(request.Patient);

            await patientRepository.AddAsync(patient);

            return await _unitOfWork.SaveChangesAsync();

            
        }
    }
}