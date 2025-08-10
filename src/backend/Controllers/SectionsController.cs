using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly SectionService _sectionService;

        public SectionsController(SectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetAllSections([FromQuery] bool activeOnly = false)
        {
            var sections = await _sectionService.GetAllSectionsAsync(activeOnly);
            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetSectionById(int id)
        {
            var section = await _sectionService.GetSectionByIdAsync(id);
            if (section == null)
                return NotFound();

            return Ok(section);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetSectionsByTeacherId(string teacherId)
        {
            var sections = await _sectionService.GetSectionsByTeacherIdAsync(teacherId);
            return Ok(sections);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<SectionDto>> CreateSection(CreateSectionDto createSectionDto)
        {
            var section = await _sectionService.CreateSectionAsync(createSectionDto);
            if (section == null)
                return BadRequest("Failed to create section");

            return CreatedAtAction(nameof(GetSectionById), new { id = section.Id }, section);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<SectionDto>> UpdateSection(int id, UpdateSectionDto updateSectionDto)
        {
            var section = await _sectionService.UpdateSectionAsync(id, updateSectionDto);
            if (section == null)
                return NotFound();

            return Ok(section);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteSection(int id)
        {
            var result = await _sectionService.DeleteSectionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}