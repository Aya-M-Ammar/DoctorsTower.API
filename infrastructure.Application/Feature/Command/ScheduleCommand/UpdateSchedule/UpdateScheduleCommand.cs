using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.DTOs.DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule
{
    public class UpdateScheduleCommand : IRequest<bool>
    {
        public ScheduleDTO Schedule { get; set; }

        public UpdateScheduleCommand(ScheduleDTO schedule)
        {
            Schedule = schedule;
        }
    }
}
