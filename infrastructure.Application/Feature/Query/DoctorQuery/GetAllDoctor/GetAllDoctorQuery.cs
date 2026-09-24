using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.DoctorQuery.GetAllDoctor
{
    public class GetAllDoctorQuery : IRequest<IEnumerable<DoctorDTO>>
    {
    }
}
