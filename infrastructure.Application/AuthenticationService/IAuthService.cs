using DoctorsTower.Application.DTOs.Authentication;

namespace DoctorsTower.Application.Services.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Register(RegisterDTO registerDTO);

        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    }
}