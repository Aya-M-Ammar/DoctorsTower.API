using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.DoctorsFeature.DeleteDoctors
{
    public class DeleteDoctorCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteDoctorCommand(int id)
        {
            Id = id;
        }
    }
}
