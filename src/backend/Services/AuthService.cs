using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using backend.Auth;
using backend.DTOs.Auth;
using backend.Models;

namespace backend.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(UserManager<ApplicationUser> userManager, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            // Update last login time
            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                UserId = user.Id,
                Roles = roles.ToList()
            };
        }
    }
}