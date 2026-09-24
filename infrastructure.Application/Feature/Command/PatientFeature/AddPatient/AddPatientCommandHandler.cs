
using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
            var patient = _mapper.Map<Patient>(request.Patient);

            await _unitOfWork
                .GetRepository<Patient>()
                .AddAsync(patient);

            await _unitOfWork.SaveChangesAsync();

            return patient.Id;
        }
    }
}
