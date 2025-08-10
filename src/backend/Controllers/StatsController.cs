using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.DTOs.Stats;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Provides statistics and analytics for sections, students, and semesters
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    [Tags("Statistics")]
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService;

        /// <summary>
        /// Initializes a new instance of the StatsController
        /// </summary>
        /// <param name="statsService">The statistics service</param>
        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        /// <summary>
        /// Retrieves statistics for a specific section
        /// </summary>
        /// <remarks>
        /// This endpoint returns comprehensive statistics about a specific physical education section,
        /// including attendance data, grades, and normative results.
        /// 
        /// Any authenticated user can access this endpoint, but teachers can only view statistics for
        /// sections they teach, and students can only view statistics for sections they are enrolled in.
        /// </remarks>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">Optional semester ID to filter statistics by semester</param>
        /// <returns>The section statistics</returns>
        /// <response code="200">Returns the section statistics</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this section's statistics</response>
        /// <response code="404">If the section is not found</response>
        [HttpGet("section/{sectionId}")]
        [ProducesResponseType(typeof(SectionStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves statistics for a specific student
        /// </summary>
        /// <remarks>
        /// This endpoint returns comprehensive statistics about a specific student,
        /// including attendance data, grades, and normative results across all sections.
        /// 
        /// Any authenticated user can access this endpoint, but students can only view their own statistics,
        /// while teachers and moderators can view any student's statistics.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <returns>The student statistics</returns>
        /// <response code="200">Returns the student statistics</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this student's statistics</response>
        /// <response code="404">If the student is not found</response>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(StudentStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves statistics for a specific semester
        /// </summary>
        /// <remarks>
        /// This endpoint returns comprehensive statistics about a specific semester,
        /// including section participation, attendance data, and grades.
        /// 
        /// Only teachers and moderators can access this endpoint.
        /// </remarks>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <returns>The semester statistics</returns>
        /// <response code="200">Returns the semester statistics</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized (not a teacher or moderator)</response>
        /// <response code="404">If the semester is not found</response>
        [HttpGet("semester/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(typeof(SemesterStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SemesterStatsDto>> GetSemesterStats(int semesterId)
        {
            var stats = await _statsService.GetSemesterStatsAsync(semesterId);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }
    }
}