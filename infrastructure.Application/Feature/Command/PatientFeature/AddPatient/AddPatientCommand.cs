
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient
{
    public class AddPatientCommand : IRequest<int>
    {
        public CreatePatientDTO Patient { get; set; }

        public AddPatientCommand(CreatePatientDTO patient)
        {
            Patient = patient;
        }
    }
}
