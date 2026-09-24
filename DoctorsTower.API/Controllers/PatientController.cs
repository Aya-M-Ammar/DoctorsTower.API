
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.PatientFeature.AddPatient;
using DoctorsTower.Application.Feature.Command.PatientFeature.DeletePatient;
using DoctorsTower.Application.Feature.Command.PatientFeature.UpdatePatient;
using DoctorsTower.Application.Feature.Query.PatientQuery.GetAll;
using DoctorsTower.Application.Feature.Query.PatientQuery.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllPatients()
        {
            var result = await _mediator.Send(
                new GetAllPatientQuery());

            return Ok(result);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatientById(int id)
        {
            var result = await _mediator.Send(
                new GetPatientByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Patient
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<int>> AddPatient(
            CreatePatientDTO patient)
        {
            var result = await _mediator.Send(
                new AddPatientCommand(patient));

            return Ok(result);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<bool>> UpdatePatient(
            int id,
            CreatePatientDTO patient)
        {
           

            var result = await _mediator.Send(
                new UpdatePatientCommand(patient,id));

            if (!result)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeletePatient(int id)
        {
            var result = await _mediator.Send(
                new DeletePatientCommand(id));

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}
