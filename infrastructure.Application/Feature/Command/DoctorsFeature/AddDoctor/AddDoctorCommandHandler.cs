
using AutoMapper;
using DoctorsTower.Application.DTOs;
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
            var doctor = _mapper.Map<Doctoree>(request.Doctor);

            await _unitOfWork
                .GetRepository<Doctoree>()
                .AddAsync(doctor);

            await _unitOfWork.SaveChangesAsync();

            return doctor.Id;
        }
    }

}
