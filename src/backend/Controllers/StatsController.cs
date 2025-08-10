using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.DTOs.Stats;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("section/{sectionId}")]
        public async Task<ActionResult<SectionStatsDto>> GetSectionStats(
            int sectionId, 
            [FromQuery] int? semesterId = null)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var stats = await _statsService.GetSectionStatsAsync(sectionId, semesterId);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<StudentStatsDto>> GetStudentStats(string studentId)
        {
            // Check if user is authorized to view this student's stats
            if (!User.IsInRole("Moderator") && !User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != studentId)
                    return Forbid();
            }

            var stats = await _statsService.GetStudentStatsAsync(studentId);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }

        [HttpGet("semester/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult<SemesterStatsDto>> GetSemesterStats(int semesterId)
        {
            var stats = await _statsService.GetSemesterStatsAsync(semesterId);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }
    }
}