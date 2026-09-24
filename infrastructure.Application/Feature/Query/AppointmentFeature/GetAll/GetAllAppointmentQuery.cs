
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.AppointmentFeature.GetAll
{
    public class GetAllAppointmentQuery
        : IRequest<IEnumerable<AppointmentDTO>?>
    {
    }
}
