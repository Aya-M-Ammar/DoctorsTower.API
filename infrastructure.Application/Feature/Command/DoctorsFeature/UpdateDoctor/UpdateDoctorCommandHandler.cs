
using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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
            var doctor = await _unitOfWork
                .GetRepository<Doctoree>()
                .GetByIdAsync(request.Id);

            if (doctor == null)
                return false;

            _mapper.Map(request.Doctor, doctor);

            _unitOfWork
                .GetRepository<Doctoree>()
                .Update(doctor);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
