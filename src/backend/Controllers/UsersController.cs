using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Manages user-related operations such as creating, updating, and retrieving users
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Moderator")]
    [Produces("application/json")]
    [Tags("User Management")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the UsersController
        /// </summary>
        /// <param name="userService">The user management service</param>
        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users in the system
        /// </summary>
        /// <remarks>
        /// This endpoint returns all users registered in the system, including their roles.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <returns>A list of all users</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by ID
        /// </summary>
        /// <remarks>
        /// This endpoint returns detailed information about a specific user.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <param name="id">The unique identifier of the user</param>
        /// <returns>The requested user information</returns>
        /// <response code="200">Returns the requested user information</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Retrieves all users with a specific role
        /// </summary>
        /// <remarks>
        /// This endpoint returns all users who have been assigned a specific role.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <param name="role">The role to filter users by (e.g., "Student", "Teacher", "Moderator")</param>
        /// <returns>A list of users with the specified role</returns>
        /// <response code="200">Returns the list of users with the specified role</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet("role/{role}")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new user with the specified details and roles.
        /// Only moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     POST /api/users
        ///     {
        ///        "userName": "newuser@example.com",
        ///        "email": "newuser@example.com",
        ///        "password": "Password123!",
        ///        "firstName": "New",
        ///        "lastName": "User",
        ///        "profilePictureUrl": null,
        ///        "roles": ["Student"]
        ///     }
        ///
        /// </remarks>
        /// <param name="createUserDto">The user creation details</param>
        /// <returns>The newly created user information</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user creation fails</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            if (user == null)
                return BadRequest("Failed to create user");

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <remarks>
        /// This endpoint updates the details of an existing user.
        /// Only moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/users/{id}
        ///     {
        ///        "email": "updated@example.com",
        ///        "firstName": "Updated",
        ///        "lastName": "User",
        ///        "profilePictureUrl": "https://example.com/profile.jpg",
        ///        "roles": ["Teacher"]
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The unique identifier of the user to update</param>
        /// <param name="updateUserDto">The user update details</param>
        /// <returns>The updated user information</returns>
        /// <response code="200">Returns the updated user information</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        /// <response code="404">If the user is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <remarks>
        /// This endpoint permanently deletes a user from the system.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <param name="id">The unique identifier of the user to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the user was successfully deleted</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        /// <response code="404">If the user is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Retrieves all students
        /// </summary>
        /// <remarks>
        /// This endpoint returns all users with the "Student" role.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <returns>A list of all students</returns>
        /// <response code="200">Returns the list of students</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet("students")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllStudents()
        {
            var students = await _userService.GetUsersByRoleAsync("Student");
            return Ok(students);
        }

        /// <summary>
        /// Retrieves all teachers
        /// </summary>
        /// <remarks>
        /// This endpoint returns all users with the "Teacher" role.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <returns>A list of all teachers</returns>
        /// <response code="200">Returns the list of teachers</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet("teachers")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllTeachers()
        {
            var teachers = await _userService.GetUsersByRoleAsync("Teacher");
            return Ok(teachers);
        }

        /// <summary>
        /// Retrieves all moderators
        /// </summary>
        /// <remarks>
        /// This endpoint returns all users with the "Moderator" role.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <returns>A list of all moderators</returns>
        /// <response code="200">Returns the list of moderators</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet("moderators")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllModerators()
        {
            var moderators = await _userService.GetUsersByRoleAsync("Moderator");
            return Ok(moderators);
        }
    }
}