using DoctorsTower.infrastructure.Contract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Doctoree = DoctorsTower.Domain.Entities.Doctor;
namespace DoctorsTower.Application.Feature.Command.DoctorsFeature.DeleteDoctors
{
    public class DeleteDoctorCommandHandler
        : IRequestHandler<DeleteDoctorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork
                .GetRepository<Doctoree>()
                .GetByIdAsync(request.Id);

            if (doctor == null)
                return false;

            _unitOfWork
                .GetRepository<Doctoree>()
                .Delete(doctor);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
