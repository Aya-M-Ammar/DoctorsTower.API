
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.DTOs.DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.ScheduleQuery.GetById
{
    public class GetScheduleByIdQuery : IRequest<ScheduleDTO>
    {
        public int Id { get; set; }

        public GetScheduleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
