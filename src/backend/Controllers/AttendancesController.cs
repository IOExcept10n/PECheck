using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly AttendanceService _attendanceService;
        private readonly UserService _userService;

        public AttendancesController(AttendanceService attendanceService, UserService userService)
        {
            _attendanceService = attendanceService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAttendances(
            [FromQuery] string? studentId = null,
            [FromQuery] int? sectionId = null,
            [FromQuery] int? semesterId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            // Check permissions - only allow teachers to see their section attendances or moderators to see all
            if (!User.IsInRole("Moderator"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (User.IsInRole("Teacher"))
                {
                    // Teachers can only access their sections
                    if (!sectionId.HasValue)
                        return Forbid();
                    
                    // Would need to check if the teacher teaches this section
                    // For simplicity, this is omitted here
                }
                else if (User.IsInRole("Student"))
                {
                    // Students can only access their own attendances
                    if (studentId != userId)
                        return Forbid();
                }
            }

            var attendances = await _attendanceService.GetAttendancesAsync(studentId, sectionId, semesterId, startDate, endDate);
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDto>> GetAttendanceById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
                return NotFound();

            // Check permissions
            if (!User.IsInRole("Moderator"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (User.IsInRole("Student") && attendance.StudentId != userId)
                    return Forbid();
                
                // For teachers, would need to check if they teach the section
                // Omitted for simplicity
            }

            return Ok(attendance);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult<AttendanceDto>> CreateAttendance(CreateAttendanceDto createAttendanceDto)
        {
            // Get current user ID for recording who added this attendance
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            // For teachers, check if they teach this section
            if (User.IsInRole("Teacher"))
            {
                // This check is omitted for simplicity
                // Would check if the teacher teaches this section
            }

            var attendance = await _attendanceService.CreateAttendanceAsync(createAttendanceDto, userId);
            if (attendance == null)
                return BadRequest("Failed to create attendance record");

            return CreatedAtAction(nameof(GetAttendanceById), new { id = attendance.Id }, attendance);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> CreateBulkAttendance(BulkAttendanceDto bulkAttendanceDto)
        {
            // Get current user ID for recording who added these attendances
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            // For teachers, check if they teach this section
            if (User.IsInRole("Teacher"))
            {
                // This check is omitted for simplicity
                // Would check if the teacher teaches this section
            }

            var attendances = await _attendanceService.CreateBulkAttendanceAsync(bulkAttendanceDto, userId);
            if (!attendances.Any())
                return BadRequest("Failed to create attendance records");

            return Ok(attendances);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult<AttendanceDto>> UpdateAttendance(int id, UpdateAttendanceDto updateAttendanceDto)
        {
            // Get current user ID for recording who updated this attendance
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var attendance = await _attendanceService.UpdateAttendanceAsync(id, updateAttendanceDto, userId);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteAttendance(int id)
        {
            var result = await _attendanceService.DeleteAttendanceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}