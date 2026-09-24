using DoctorsTower.Application.DTOs;

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule
{
    public class UpdateScheduleCommand : IRequest<bool>
    {
        public CreateScheduleDTO Schedule { get; set; }
        public int Id { get; set; }

        public UpdateScheduleCommand(CreateScheduleDTO schedule,int id)
        {
            Id = id;
            Schedule = schedule;
        }
    }
}
