using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Moderator")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            if (user == null)
                return BadRequest("Failed to create user");

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllStudents()
        {
            var students = await _userService.GetUsersByRoleAsync("Student");
            return Ok(students);
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllTeachers()
        {
            var teachers = await _userService.GetUsersByRoleAsync("Teacher");
            return Ok(teachers);
        }

        [HttpGet("moderators")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllModerators()
        {
            var moderators = await _userService.GetUsersByRoleAsync("Moderator");
            return Ok(moderators);
        }
    }
}