using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using Microsoft.Extensions.Logging;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;
namespace DoctorsTower.Application.Feature.Command.Doctor.AddDoctor
{
    public class AddDoctorCommandHandler
        : IRequestHandler<AddDoctorCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddDoctorCommandHandler> _logger;

        public AddDoctorCommandHandler( IUnitOfWork unitOfWork, IMapper mapper,ILogger<AddDoctorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle( AddDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctorRepository =   _unitOfWork.GetRepository<Doctoree>();

            // Business Rule:
            // Phone must be unique
            var existingDoctor = await doctorRepository.GetAllAsync(x => x.Phone == request.Doctor.Phone);

            if (existingDoctor.Any())
            {
                _logger.LogError($"Doctor with phone {request.Doctor.Phone} already exists.");
                return 0; }

            var doctor = _mapper.Map<Doctoree>(request.Doctor);

            await doctorRepository.AddAsync(doctor);

            return await _unitOfWork.SaveChangesAsync();

          
        }
    }
}