
using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.AppointmentFeature.GetById
{
    public class GetAppointmentsQuery : IRequest<IEnumerable<AppointmentDTO>?>
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
    }
}
