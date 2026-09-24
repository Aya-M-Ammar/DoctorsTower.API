using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient
{
    public class UpdatePatientCommand : IRequest<bool>
    {
        public CreatePatientDTO Patient { get; set; }
        public int Id { get; set; }

        public UpdatePatientCommand(CreatePatientDTO patient,int id)
        {
            Patient = patient;
            Id = id;
        }
    }
}
