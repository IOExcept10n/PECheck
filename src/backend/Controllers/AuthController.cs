using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Auth;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Handles authentication related operations such as user login
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the AuthController
        /// </summary>
        /// <param name="authService">The authentication service</param>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth/login
        ///     {
        ///        "email": "admin@example.com",
        ///        "password": "Admin123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="loginDto">The login credentials</param>
        /// <returns>Authentication result with JWT token if successful</returns>
        /// <response code="200">Returns the authentication result with JWT token</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}