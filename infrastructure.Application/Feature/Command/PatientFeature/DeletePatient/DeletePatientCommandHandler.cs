
using DoctorsTower.Domain.Entities;
using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient
{
    public class DeletePatientCommandHandler
          : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePatientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeletePatientCommand request,
            CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork
                .GetRepository<Patient>()
                .GetByIdAsync(request.Id);

            if (patient == null)
                return false;

            _unitOfWork
                .GetRepository<Patient>()
                .Delete(patient);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
