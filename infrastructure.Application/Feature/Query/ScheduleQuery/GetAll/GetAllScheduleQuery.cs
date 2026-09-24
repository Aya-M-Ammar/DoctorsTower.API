using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.DTOs.DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.ScheduleQuery.GetAll
{

    public class GetAllScheduleQuery : IRequest<IEnumerable<ScheduleDTO>>
    {
    }
}
