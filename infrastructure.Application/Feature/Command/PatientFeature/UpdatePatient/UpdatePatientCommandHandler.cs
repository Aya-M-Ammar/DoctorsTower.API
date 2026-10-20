using AutoMapper;
using DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient
{
    public class UpdatePatientCommandHandler
        : IRequestHandler<UpdatePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly    ILogger<UpdatePatientCommandHandler> _logger;


        public UpdatePatientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,ILogger<UpdatePatientCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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
                _logger.LogError($"A patient with phone number {request.Patient.Phone} already exists.");
                return false;
            }

            // Rule 3:
            // Date of birth cannot be in the future
            if (request.Patient.DateOfBirth.Date >
                DateTime.UtcNow.Date)
            {
               _logger.LogError($"Date of birth {request.Patient.DateOfBirth} cannot be in the future.");
                return false;
            }

            _mapper.Map(request.Patient, patient);

            patientRepository.Update(patient);

           return  await _unitOfWork.SaveChangesAsync()>0;

           
        }
    }
}