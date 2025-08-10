using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Manages physical education sections operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Sections")]
    public class SectionsController : ControllerBase
    {
        private readonly SectionService _sectionService;

        /// <summary>
        /// Initializes a new instance of the SectionsController
        /// </summary>
        /// <param name="sectionService">The section management service</param>
        public SectionsController(SectionService sectionService)
        {
            _sectionService = sectionService;
        }

        /// <summary>
        /// Retrieves all sections
        /// </summary>
        /// <remarks>
        /// This endpoint returns information about all physical education sections.
        /// Anyone can access this endpoint.
        /// </remarks>
        /// <param name="activeOnly">If true, returns only active sections</param>
        /// <returns>A list of all sections</returns>
        /// <response code="200">Returns the list of sections</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SectionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetAllSections([FromQuery] bool activeOnly = false)
        {
            var sections = await _sectionService.GetAllSectionsAsync(activeOnly);
            return Ok(sections);
        }

        /// <summary>
        /// Retrieves a specific section by ID
        /// </summary>
        /// <remarks>
        /// This endpoint returns detailed information about a specific physical education section.
        /// Anyone can access this endpoint.
        /// </remarks>
        /// <param name="id">The unique identifier of the section</param>
        /// <returns>The requested section information</returns>
        /// <response code="200">Returns the requested section information</response>
        /// <response code="404">If the section is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SectionDto>> GetSectionById(int id)
        {
            var section = await _sectionService.GetSectionByIdAsync(id);
            if (section == null)
                return NotFound();

            return Ok(section);
        }

        /// <summary>
        /// Retrieves all sections taught by a specific teacher
        /// </summary>
        /// <remarks>
        /// This endpoint returns all sections that are assigned to a specific teacher.
        /// Anyone can access this endpoint.
        /// </remarks>
        /// <param name="teacherId">The unique identifier of the teacher</param>
        /// <returns>A list of sections taught by the specified teacher</returns>
        /// <response code="200">Returns the list of sections</response>
        [HttpGet("teacher/{teacherId}")]
        [ProducesResponseType(typeof(IEnumerable<SectionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetSectionsByTeacherId(string teacherId)
        {
            var sections = await _sectionService.GetSectionsByTeacherIdAsync(teacherId);
            return Ok(sections);
        }

        /// <summary>
        /// Creates a new section
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new physical education section.
        /// Only moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     POST /api/sections
        ///     {
        ///        "name": "Swimming",
        ///        "description": "Swimming training for beginners",
        ///        "coverImageUrl": "https://example.com/swimming.jpg",
        ///        "teacherId": "teacher-id-here",
        ///        "maxStudents": 15,
        ///        "minAttendanceForGrade": 10,
        ///        "maxAttendance": 16,
        ///        "isActive": true
        ///     }
        ///
        /// </remarks>
        /// <param name="createSectionDto">The section creation details</param>
        /// <returns>The newly created section information</returns>
        /// <response code="201">Returns the newly created section</response>
        /// <response code="400">If the section creation fails</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<SectionDto>> CreateSection(CreateSectionDto createSectionDto)
        {
            var section = await _sectionService.CreateSectionAsync(createSectionDto);
            if (section == null)
                return BadRequest("Failed to create section");

            return CreatedAtAction(nameof(GetSectionById), new { id = section.Id }, section);
        }

        /// <summary>
        /// Updates an existing section
        /// </summary>
        /// <remarks>
        /// This endpoint updates the details of an existing physical education section.
        /// Only moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/sections/{id}
        ///     {
        ///        "name": "Advanced Swimming",
        ///        "description": "Swimming training for advanced students",
        ///        "maxStudents": 12,
        ///        "isActive": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The unique identifier of the section to update</param>
        /// <param name="updateSectionDto">The section update details</param>
        /// <returns>The updated section information</returns>
        /// <response code="200">Returns the updated section information</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        /// <response code="404">If the section is not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Moderator")]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SectionDto>> UpdateSection(int id, UpdateSectionDto updateSectionDto)
        {
            var section = await _sectionService.UpdateSectionAsync(id, updateSectionDto);
            if (section == null)
                return NotFound();

            return Ok(section);
        }

        /// <summary>
        /// Deletes a section
        /// </summary>
        /// <remarks>
        /// This endpoint permanently deletes a physical education section.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <param name="id">The unique identifier of the section to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the section was successfully deleted</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        /// <response code="404">If the section is not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Moderator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSection(int id)
        {
            var result = await _sectionService.DeleteSectionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}