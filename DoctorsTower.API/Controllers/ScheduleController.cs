using DoctorsTower.API.Filters;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.AddSchedule;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.DeleteSchedule;
using DoctorsTower.Application.Feature.Command.ScheduleFeature.UpdateSchedule;
using DoctorsTower.Application.Feature.Query.ScheduleQuery.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Schedule
        [HttpGet]
        [Cached(60)]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetAllSchedules()
        {
            var result = await _mediator.Send(
                new GetAllScheduleQuery());

            return Ok(result);
        }

        // POST: api/Schedule
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<int>> AddSchedule(
            CreateScheduleDTO schedule)
        {
            var result = await _mediator.Send(
                new AddScheduleCommand(schedule));

            return Ok(result);
        }

        // PUT: api/Schedule/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<bool>> UpdateSchedule(
            int id,
            CreateScheduleDTO schedule)
        {
            

            var result = await _mediator.Send(
                new UpdateScheduleCommand(schedule,id));

            if (!result)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Schedule/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<bool>> DeleteSchedule(int id)
        {
            var result = await _mediator.Send(
                new DeleteScheduleCommand(id));

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}