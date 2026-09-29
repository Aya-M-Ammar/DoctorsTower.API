
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment
{
    public class DeleteAppointmentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteAppointmentCommand(int id)
        {
            Id = id;
        }
    }
}
