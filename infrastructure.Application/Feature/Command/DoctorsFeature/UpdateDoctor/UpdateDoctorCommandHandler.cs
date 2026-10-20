using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;

namespace DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDoctorCommandHandler> _logger;

        public UpdateDoctorCommandHandler(  IUnitOfWork unitOfWork, IMapper mapper,ILogger<UpdateDoctorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateDoctorCommand request,CancellationToken cancellationToken)
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
            // Phone must be unique
            // Exclude the current doctor
            var existingDoctor = await doctorRepository.GetAllAsync(
                x => x.Phone == request.Doctor.Phone
                     && x.Id != request.Id);

            if (existingDoctor.Any())
            {
                _logger.LogError($"Phone number {request.Doctor.Phone} is already in use by another doctor.");
                return false;   
            }

            _mapper.Map(request.Doctor, doctor);

            doctorRepository.Update(doctor);

          return   await _unitOfWork.SaveChangesAsync()>0;

            
        }
    }
}