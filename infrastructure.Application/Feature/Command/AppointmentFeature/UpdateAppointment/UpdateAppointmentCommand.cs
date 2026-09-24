
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
        public  int Id { get; set; }

        public UpdateAppointmentCommand(AppointmentDTO appointment,int id)
        {
            Appointment = appointment;
            Id = id;
        }
    }
}
