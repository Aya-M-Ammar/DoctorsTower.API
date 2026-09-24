using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.Appointment.AddAppointment;

using DoctorsTower.Application.Feature.Command.Appointment.UpdateAppointment;
using DoctorsTower.Application.Feature.Command.AppointmentFeature.DeleteAppointment;
using DoctorsTower.Application.Feature.Query.AppointmentFeature.GetById;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Appointment
        // GET: api/Appointment?doctorId=2
        // GET: api/Appointment?patientId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments(
            int? doctorId,
            int? patientId)
        {
            var result = await _mediator.Send(
                new GetAppointmentsQuery
                {
                    DoctorId = doctorId,
                    PatientId = patientId
                });

            return Ok(result);
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<ActionResult<int>> AddAppointment(
            AppointmentDTO appointment)
        {
            var result = await _mediator.Send(
                new AddAppointmentCommand(appointment));

            return Ok(result);
        }

        // PUT: api/Appointment/1
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateAppointment(
            int id,
            AppointmentDTO appointment)
        {
            appointment.Id = id;

            var result = await _mediator.Send(
                new UpdateAppointmentCommand(appointment));

            if (!result)
                return NotFound();

            return Ok(result);
        }

        // PUT: api/Appointment/1/cancel
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<bool>> CancelAppointment(int id)
        {
            var result = await _mediator.Send(
                new CancelAppointmentCommand(id));

            if (!result)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Appointment/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAppointment(int id)
        {
            var result = await _mediator.Send(
                new DeleteAppointmentCommand(id));

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}