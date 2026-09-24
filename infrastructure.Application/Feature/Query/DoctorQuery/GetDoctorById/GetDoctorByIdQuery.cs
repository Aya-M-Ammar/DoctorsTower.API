using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.DoctorQuery.GetDoctorById
{
    public class GetDoctorByIdQuery : IRequest<DoctorDTO>
    {
        public int Id { get; set; }

        public GetDoctorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
