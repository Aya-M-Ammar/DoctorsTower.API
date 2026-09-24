using DoctorsTower.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Feature.Query.PatientQuery.GetAll
{

    public class GetAllPatientQuery : IRequest<IEnumerable<PatientDTO>?>
    {
    }
}
