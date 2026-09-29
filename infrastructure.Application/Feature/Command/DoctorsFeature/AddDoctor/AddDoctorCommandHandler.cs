using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;
namespace DoctorsTower.Application.Feature.Command.Doctor.AddDoctor
{
    public class AddDoctorCommandHandler
        : IRequestHandler<AddDoctorCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDoctorCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            AddDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var doctorRepository =
                _unitOfWork.GetRepository<Doctoree>();

            // Business Rule:
            // Phone must be unique
            var existingDoctor = await doctorRepository.GetAllAsync(
                x => x.Phone == request.Doctor.Phone);

            if (existingDoctor.Any())
            {
                throw new Exception("A doctor with this phone number already exists.");
            }

            var doctor = _mapper.Map<Doctoree>(request.Doctor);

            await doctorRepository.AddAsync(doctor);

            return await _unitOfWork.SaveChangesAsync();

          
        }
    }
}