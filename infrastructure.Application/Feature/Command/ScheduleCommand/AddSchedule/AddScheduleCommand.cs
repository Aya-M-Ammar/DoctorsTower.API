
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule
{
    public class AddScheduleCommand : IRequest<int>
    {
        public CreateScheduleDTO Schedule { get; set; }

        public AddScheduleCommand(CreateScheduleDTO schedule)
        {
            Schedule = schedule;
        }
    }
}
