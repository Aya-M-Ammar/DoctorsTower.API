using DoctorsTower.Application.DTOs.Authentication;
using DoctorsTower.Application.Services.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(
            RegisterDTO registerDTO)
        {
            var result = await _authService.Register(registerDTO);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(
            LoginDTO loginDTO)
        {
            var result = await _authService.Login(loginDTO);

            return Ok(result);
        }
    }
}