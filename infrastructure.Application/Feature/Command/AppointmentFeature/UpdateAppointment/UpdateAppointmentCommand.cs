
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<bool>
    {
        public AppointmentDTO Appointment { get; set; }

        public UpdateAppointmentCommand(AppointmentDTO appointment)
        {
            Appointment = appointment;
        }
    }
}
