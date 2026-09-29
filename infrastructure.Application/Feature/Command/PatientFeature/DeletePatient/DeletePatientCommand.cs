using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient
{
    public class DeletePatientCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeletePatientCommand(int id)
        {
            Id = id;
        }
    }
}
