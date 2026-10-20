using AutoMapper;
using DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;
using Patientt = DoctorsTower.Domain.Entities.Patient;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient
{
    public class AddPatientCommandHandler: IRequestHandler<AddPatientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPatientCommandHandler> _logger;


        public AddPatientCommandHandler( IUnitOfWork unitOfWork,IMapper mapper, ILogger<AddPatientCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(AddPatientCommand request,CancellationToken cancellationToken)
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
                _logger.LogError($"A patient with phone number {request.Patient.Phone} already exists.");
                return 0;
            }


            // Rule 2:
            // Date of birth cannot be in the future
            if (request.Patient.DateOfBirth.Date > DateTime.UtcNow.Date)
            {
              _logger.LogError($"Date of birth {request.Patient.DateOfBirth} cannot be in the future.");
                return 0;
            }

            var patient =
                _mapper.Map<Patientt>(request.Patient);

            await patientRepository.AddAsync(patient);

            return await _unitOfWork.SaveChangesAsync();

            
        }
    }
}