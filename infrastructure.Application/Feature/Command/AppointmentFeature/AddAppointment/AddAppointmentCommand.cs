using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Appointment.AddAppointment
{
    public class AddAppointmentCommand : IRequest<int>
    {
        public AppointmentDTO Appointment { get; set; }

        public AddAppointmentCommand(AppointmentDTO appointment)
        {
            Appointment = appointment;
        }
    }
}
