using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Doctor.AddDoctor
{
    public class AddDoctorCommand : IRequest<int>
    {
        public CreateDoctorDTO Doctor { get; set; }

        public AddDoctorCommand(CreateDoctorDTO doctor)
        {
            Doctor = doctor;
        }
    }
}
