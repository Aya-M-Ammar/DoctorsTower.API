using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;

namespace DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor
{
    public class UpdateDoctorCommandHandler
        : IRequestHandler<UpdateDoctorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDoctorCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdateDoctorCommand request,
            CancellationToken cancellationToken)
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
                throw new Exception(
                    "A doctor with this phone number already exists.");
            }

            _mapper.Map(request.Doctor, doctor);

            doctorRepository.Update(doctor);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}