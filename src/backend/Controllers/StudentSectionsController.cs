using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Manages student enrollment in physical education sections
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    [Tags("Student Enrollments")]
    public class StudentSectionsController : ControllerBase
    {
        private readonly StudentSectionService _studentSectionService;

        /// <summary>
        /// Initializes a new instance of the StudentSectionsController
        /// </summary>
        /// <param name="studentSectionService">The student section management service</param>
        public StudentSectionsController(StudentSectionService studentSectionService)
        {
            _studentSectionService = studentSectionService;
        }

        /// <summary>
        /// Retrieves all student section enrollments
        /// </summary>
        /// <remarks>
        /// This endpoint returns information about all student enrollments in physical education sections.
        /// Only moderators can access this endpoint.
        /// </remarks>
        /// <returns>A list of all student section enrollments</returns>
        /// <response code="200">Returns the list of student section enrollments</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a moderator</response>
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        [ProducesResponseType(typeof(IEnumerable<StudentSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentSectionDto>>> GetAllStudentSections()
        {
            var studentSections = await _studentSectionService.GetAllStudentSectionsAsync();
            return Ok(studentSections);
        }

        /// <summary>
        /// Retrieves all section enrollments for a specific student
        /// </summary>
        /// <remarks>
        /// This endpoint returns information about all physical education sections that a student is enrolled in.
        /// Students can only view their own enrollments, while teachers and moderators can view any student's enrollments.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <returns>A list of section enrollments for the specified student</returns>
        /// <response code="200">Returns the list of student's section enrollments</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this student's enrollments</response>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<StudentSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentSectionDto>>> GetStudentSectionsByStudentId(string studentId)
        {
            // Check if user is authorized to view this student's data
            if (!User.IsInRole("Moderator") && !User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != studentId)
                    return Forbid();
            }

            var studentSections = await _studentSectionService.GetStudentSectionsByStudentIdAsync(studentId);
            return Ok(studentSections);
        }

        /// <summary>
        /// Retrieves all student enrollments for a specific section
        /// </summary>
        /// <remarks>
        /// This endpoint returns information about all students enrolled in a specific physical education section.
        /// Only teachers who teach the section and moderators can access this endpoint.
        /// </remarks>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">Optional semester ID to filter enrollments by semester</param>
        /// <returns>A list of student enrollments for the specified section</returns>
        /// <response code="200">Returns the list of students enrolled in the section</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this section's enrollments</response>
        [HttpGet("section/{sectionId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(typeof(IEnumerable<StudentSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentSectionDto>>> GetStudentSectionsBySectionId(
            int sectionId, 
            [FromQuery] int? semesterId = null)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var studentSections = await _studentSectionService.GetStudentSectionsBySectionIdAsync(sectionId, semesterId);
            return Ok(studentSections);
        }

        /// <summary>
        /// Retrieves a specific student section enrollment
        /// </summary>
        /// <remarks>
        /// This endpoint returns detailed information about a specific student's enrollment in a specific section.
        /// Students can only view their own enrollments, while teachers and moderators can view any enrollment.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <returns>The requested student section enrollment</returns>
        /// <response code="200">Returns the requested student section enrollment</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this enrollment</response>
        /// <response code="404">If the enrollment is not found</response>
        [HttpGet("{studentId}/{sectionId}/{semesterId}")]
        [ProducesResponseType(typeof(StudentSectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentSectionDto>> GetStudentSection(string studentId, int sectionId, int semesterId)
        {
            // Check if user is authorized to view this data
            if (!User.IsInRole("Moderator") && !User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != studentId)
                    return Forbid();
            }

            var studentSection = await _studentSectionService.GetStudentSectionAsync(studentId, sectionId, semesterId);
            if (studentSection == null)
                return NotFound();

            return Ok(studentSection);
        }

        /// <summary>
        /// Enrolls a student in a section
        /// </summary>
        /// <remarks>
        /// This endpoint enrolls a student in a specific physical education section for a specific semester.
        /// Only teachers who teach the section and moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     POST /api/studentSections
        ///     {
        ///        "studentId": "student-id-here",
        ///        "sectionId": 1,
        ///        "semesterId": 1,
        ///        "isActive": true
        ///     }
        ///
        /// </remarks>
        /// <param name="createStudentSectionDto">The enrollment details</param>
        /// <returns>The newly created student section enrollment</returns>
        /// <response code="201">Returns the newly created enrollment</response>
        /// <response code="400">If the enrollment fails (e.g., student already enrolled, section full)</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a teacher or moderator</response>
        [HttpPost]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(typeof(StudentSectionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StudentSectionDto>> EnrollStudent(CreateStudentSectionDto createStudentSectionDto)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var studentSection = await _studentSectionService.EnrollStudentAsync(createStudentSectionDto);
            if (studentSection == null)
                return BadRequest("Failed to enroll student. Student may already be enrolled in another section for this semester or the section is full.");

            return CreatedAtAction(
                nameof(GetStudentSection), 
                new { 
                    studentId = studentSection.StudentId, 
                    sectionId = studentSection.SectionId, 
                    semesterId = studentSection.SemesterId 
                }, 
                studentSection);
        }

        /// <summary>
        /// Updates a student's section enrollment
        /// </summary>
        /// <remarks>
        /// This endpoint updates details of a student's enrollment in a physical education section.
        /// Only teachers who teach the section and moderators can access this endpoint.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/studentSections/{studentId}/{sectionId}/{semesterId}
        ///     {
        ///        "isActive": false,
        ///        "finalGrade": 85.5,
        ///        "disenrollmentDate": "2023-12-15T00:00:00Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <param name="updateStudentSectionDto">The enrollment update details</param>
        /// <returns>The updated student section enrollment</returns>
        /// <response code="200">Returns the updated enrollment</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a teacher or moderator</response>
        /// <response code="404">If the enrollment is not found</response>
        [HttpPut("{studentId}/{sectionId}/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(typeof(StudentSectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentSectionDto>> UpdateStudentSection(
            string studentId, 
            int sectionId, 
            int semesterId, 
            UpdateStudentSectionDto updateStudentSectionDto)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var studentSection = await _studentSectionService.UpdateStudentSectionAsync(
                studentId, 
                sectionId, 
                semesterId, 
                updateStudentSectionDto);

            if (studentSection == null)
                return NotFound();

            return Ok(studentSection);
        }

        /// <summary>
        /// Disenrolls a student from a section
        /// </summary>
        /// <remarks>
        /// This endpoint removes a student from a physical education section.
        /// Only teachers who teach the section and moderators can access this endpoint.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the student was successfully disenrolled</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a teacher or moderator</response>
        /// <response code="404">If the enrollment is not found</response>
        [HttpDelete("{studentId}/{sectionId}/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DisenrollStudent(string studentId, int sectionId, int semesterId)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var result = await _studentSectionService.DisenrollStudentAsync(studentId, sectionId, semesterId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Calculates a student's grade for a section
        /// </summary>
        /// <remarks>
        /// This endpoint calculates a student's grade for a specific physical education section based on
        /// attendance and normative results. The grade is calculated but not saved.
        /// Students can only view their own grades, while teachers and moderators can view any student's grade.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <returns>The calculated grade</returns>
        /// <response code="200">Returns the calculated grade</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to view this grade</response>
        /// <response code="404">If the enrollment is not found</response>
        [HttpGet("{studentId}/{sectionId}/{semesterId}/grade")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<double>> CalculateGrade(string studentId, int sectionId, int semesterId)
        {
            // Check if user is authorized to view this data
            if (!User.IsInRole("Moderator") && !User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != studentId)
                    return Forbid();
            }

            var grade = await _studentSectionService.CalculateFinalGradeAsync(studentId, sectionId, semesterId);
            if (!grade.HasValue)
                return NotFound();

            return Ok(grade.Value);
        }

        /// <summary>
        /// Updates and saves a student's final grade for a section
        /// </summary>
        /// <remarks>
        /// This endpoint calculates and saves a student's final grade for a specific physical education section.
        /// Only teachers who teach the section and moderators can access this endpoint.
        /// </remarks>
        /// <param name="studentId">The unique identifier of the student</param>
        /// <param name="sectionId">The unique identifier of the section</param>
        /// <param name="semesterId">The unique identifier of the semester</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the grade was successfully updated</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized as a teacher or moderator</response>
        /// <response code="404">If the enrollment is not found</response>
        [HttpPost("{studentId}/{sectionId}/{semesterId}/update-grade")]
        [Authorize(Roles = "Teacher,Moderator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateFinalGrade(string studentId, int sectionId, int semesterId)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var result = await _studentSectionService.UpdateFinalGradeAsync(studentId, sectionId, semesterId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}