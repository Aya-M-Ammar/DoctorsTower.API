
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor
{
    public class UpdateDoctorCommand : IRequest<bool>
    {
        public CreateDoctorDTO Doctor { get; set; }
        public int Id { get; set; }


        public UpdateDoctorCommand(CreateDoctorDTO doctor,int id)
        {
            Id = id;
            Doctor = doctor;

        }
    }
}
