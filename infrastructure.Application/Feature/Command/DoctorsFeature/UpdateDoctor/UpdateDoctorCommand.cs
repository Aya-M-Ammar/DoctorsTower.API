
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor
{
    public class UpdateDoctorCommand : IRequest<bool>
    {
        public DoctorDTO Doctor { get; set; }
        public int Id { get; set; }

        public UpdateDoctorCommand(DoctorDTO doctor,int id)
        {
            Doctor = doctor;
            Id = id;
        }
    }
}
