using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Doctor.AddDoctor
{
    public class AddDoctorCommand : IRequest<int>
    {
        public DoctorDTO Doctor { get; set; }

        public AddDoctorCommand(DoctorDTO doctor)
        {
            Doctor = doctor;
        }
    }
}
