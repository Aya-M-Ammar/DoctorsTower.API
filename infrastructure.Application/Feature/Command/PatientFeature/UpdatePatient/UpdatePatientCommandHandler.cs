using AutoMapper;
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient
{
    public class UpdatePatientCommandHandler
         : IRequestHandler<UpdatePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePatientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdatePatientCommand request,
            CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(request.Patient.Id);

            if (patient == null)
                return false;

            _mapper.Map(request.Patient, patient);

            _unitOfWork
                .GetRepository<Patient>()
                .Update(patient);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
