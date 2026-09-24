using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient
{
    public class UpdatePatientCommand : IRequest<bool>
    {
        public PatientDTO Patient { get; set; }

        public UpdatePatientCommand(PatientDTO patient)
        {
            Patient = patient;
        }
    }
}
