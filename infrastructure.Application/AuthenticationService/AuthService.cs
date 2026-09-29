using DoctorsTower.Application.DTOs.Authentication;
using DoctorsTower.Application.Services.ServiceAbstraction;
using DoctorsTower.Domain.Entities;
using DoctorsTower.Infrastructure.CreateToken;
using Microsoft.AspNetCore.Identity;

namespace DoctorsTower.Application.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(
                registerDTO.Email
            );

            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var roleExists = await _roleManager.RoleExistsAsync(
                registerDTO.Role
            );

            if (!roleExists)
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(registerDTO.Role)
                );
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName
            };

            var result = await _userManager.CreateAsync(
                user,
                registerDTO.Password
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description)
                );

                throw new Exception(errors);
            }

            await _userManager.AddToRoleAsync(
                user,
                registerDTO.Role
            );

            var token = _tokenService.CreateToken(
                user,
                registerDTO.Role
            );

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email!,
                Role = registerDTO.Role,
                UserId = user.Id
            };
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(
                loginDTO.Email
            );

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                loginDTO.Password
            );

            if (!passwordValid)
            {
                throw new Exception("Invalid email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            if (role == null)
            {
                throw new Exception("User has no role");
            }

            var token = _tokenService.CreateToken(
                user,
                role
            );

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email!,
                Role = role,
                UserId = user.Id
            };
        }
    }
}