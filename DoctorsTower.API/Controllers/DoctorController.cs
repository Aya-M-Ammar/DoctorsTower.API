using DoctorsTower.API.Filters;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Application.Feature.Command.Doctor.AddDoctor;
using DoctorsTower.Application.Feature.Command.Doctors.UpdateDoctor;
using DoctorsTower.Application.Feature.Command.DoctorsFeature.DeleteDoctors;
using DoctorsTower.Application.Feature.Query.DoctorQuery.GetAllDoctor;
using DoctorsTower.Application.Feature.Query.DoctorQuery.GetDoctorById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class DoctorController : ControllerBase
        {
            private readonly IMediator _mediator;

            public DoctorController(IMediator mediator)
            {
                _mediator = mediator;
            }

            // GET: api/Doctor
            [HttpGet]
            [Cached(60)]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctors()
            {
                var result = await _mediator.Send(
                    new GetAllDoctorQuery());

                return Ok(result);
            }

            // GET: api/Doctor/5
            [HttpGet("{id}")]
            public async Task<ActionResult<DoctorDTO>> GetDoctorById(int id)
            {
                var result = await _mediator.Send(
                    new GetDoctorByIdQuery(id));

                if (result == null)
                    return NotFound();

                return Ok(result);
            }

            // POST: api/Doctor
            [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<int>> AddDoctor(
                CreateDoctorDTO doctor)
            {
                var result = await _mediator.Send(
                    new AddDoctorCommand(doctor));

                return Ok(result);
            }

            // PUT: api/Doctor/5
            [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<bool>> UpdateDoctor(int id,

                CreateDoctorDTO doctor)
            {
          
            var result =await _mediator.Send(new UpdateDoctorCommand(doctor,id));

                if (!result)
                    return NotFound();

                return Ok(result);
            }

            // DELETE: api/Doctor/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<bool>> DeleteDoctor(int id)
            {
                var result = await _mediator.Send(
                    new DeleteDoctorCommand(id));

                if (!result)
                    return NotFound();

                return Ok(result);
            }
        }
    }

