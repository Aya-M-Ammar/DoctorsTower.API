using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule
{
    public class DeleteScheduleCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteScheduleCommand(int id)
        {
            Id = id;
        }
    }
}
