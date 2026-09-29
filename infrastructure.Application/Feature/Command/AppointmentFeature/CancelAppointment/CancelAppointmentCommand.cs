using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment
{
    public class CancelAppointmentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public CancelAppointmentCommand(int id)
        {
            Id = id;
        }
    }

}
