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
    public class StudentSectionsController : ControllerBase
    {
        private readonly StudentSectionService _studentSectionService;

        public StudentSectionsController(StudentSectionService studentSectionService)
        {
            _studentSectionService = studentSectionService;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<IEnumerable<StudentSectionDto>>> GetAllStudentSections()
        {
            var studentSections = await _studentSectionService.GetAllStudentSectionsAsync();
            return Ok(studentSections);
        }

        [HttpGet("student/{studentId}")]
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

        [HttpGet("section/{sectionId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult<IEnumerable<StudentSectionDto>>> GetStudentSectionsBySectionId(
            int sectionId, 
            [FromQuery] int? semesterId = null)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var studentSections = await _studentSectionService.GetStudentSectionsBySectionIdAsync(sectionId, semesterId);
            return Ok(studentSections);
        }

        [HttpGet("{studentId}/{sectionId}/{semesterId}")]
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

        [HttpPost]
        [Authorize(Roles = "Teacher,Moderator")]
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

        [HttpPut("{studentId}/{sectionId}/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
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

        [HttpDelete("{studentId}/{sectionId}/{semesterId}")]
        [Authorize(Roles = "Teacher,Moderator")]
        public async Task<ActionResult> DisenrollStudent(string studentId, int sectionId, int semesterId)
        {
            // For teachers, would check if they teach this section
            // Omitted for simplicity

            var result = await _studentSectionService.DisenrollStudentAsync(studentId, sectionId, semesterId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{studentId}/{sectionId}/{semesterId}/grade")]
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

        [HttpPost("{studentId}/{sectionId}/{semesterId}/update-grade")]
        [Authorize(Roles = "Teacher,Moderator")]
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