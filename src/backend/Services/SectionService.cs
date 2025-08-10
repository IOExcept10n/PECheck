using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class SectionService
    {
        private readonly ApplicationDbContext _dbContext;

        public SectionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SectionDto>> GetAllSectionsAsync(bool activeOnly = false)
        {
            var query = _dbContext.Sections
                .Include(s => s.Teacher)
                .Include(s => s.Schedules)
                .Include(s => s.StudentSections)
                .AsQueryable();

            if (activeOnly)
                query = query.Where(s => s.IsActive);

            var sections = await query.ToListAsync();
            
            return sections.Select(s => new SectionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CoverImageUrl = s.CoverImageUrl,
                TeacherId = s.TeacherId,
                TeacherName = $"{s.Teacher?.FirstName} {s.Teacher?.LastName}",
                MaxStudents = s.MaxStudents,
                MinAttendanceForGrade = s.MinAttendanceForGrade,
                MaxAttendance = s.MaxAttendance,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                Schedules = s.Schedules?.Select(sc => new ScheduleDto
                {
                    Id = sc.Id,
                    SectionId = sc.SectionId,
                    SectionName = s.Name,
                    DayOfWeek = sc.DayOfWeek,
                    StartTime = sc.StartTime,
                    EndTime = sc.EndTime,
                    Location = sc.Location,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    IsActive = sc.IsActive
                }).ToList() ?? new List<ScheduleDto>(),
                EnrolledStudentsCount = s.StudentSections?.Count(ss => ss.IsActive) ?? 0
            }).ToList();
        }

        public async Task<SectionDto?> GetSectionByIdAsync(int id)
        {
            var section = await _dbContext.Sections
                .Include(s => s.Teacher)
                .Include(s => s.Schedules)
                .Include(s => s.StudentSections)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null)
                return null;

            return new SectionDto
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                CoverImageUrl = section.CoverImageUrl,
                TeacherId = section.TeacherId,
                TeacherName = $"{section.Teacher?.FirstName} {section.Teacher?.LastName}",
                MaxStudents = section.MaxStudents,
                MinAttendanceForGrade = section.MinAttendanceForGrade,
                MaxAttendance = section.MaxAttendance,
                IsActive = section.IsActive,
                CreatedAt = section.CreatedAt,
                Schedules = section.Schedules?.Select(sc => new ScheduleDto
                {
                    Id = sc.Id,
                    SectionId = sc.SectionId,
                    SectionName = section.Name,
                    DayOfWeek = sc.DayOfWeek,
                    StartTime = sc.StartTime,
                    EndTime = sc.EndTime,
                    Location = sc.Location,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    IsActive = sc.IsActive
                }).ToList() ?? new List<ScheduleDto>(),
                EnrolledStudentsCount = section.StudentSections?.Count(ss => ss.IsActive) ?? 0
            };
        }

        public async Task<SectionDto?> CreateSectionAsync(CreateSectionDto createSectionDto)
        {
            var section = new Section
            {
                Name = createSectionDto.Name,
                Description = createSectionDto.Description,
                CoverImageUrl = createSectionDto.CoverImageUrl,
                TeacherId = createSectionDto.TeacherId,
                MaxStudents = createSectionDto.MaxStudents,
                MinAttendanceForGrade = createSectionDto.MinAttendanceForGrade,
                MaxAttendance = createSectionDto.MaxAttendance,
                IsActive = createSectionDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Sections.Add(section);
            await _dbContext.SaveChangesAsync();

            // Reload the section with teacher details
            section = await _dbContext.Sections
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(s => s.Id == section.Id);

            if (section == null)
                return null;

            return new SectionDto
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                CoverImageUrl = section.CoverImageUrl,
                TeacherId = section.TeacherId,
                TeacherName = $"{section.Teacher?.FirstName} {section.Teacher?.LastName}",
                MaxStudents = section.MaxStudents,
                MinAttendanceForGrade = section.MinAttendanceForGrade,
                MaxAttendance = section.MaxAttendance,
                IsActive = section.IsActive,
                CreatedAt = section.CreatedAt,
                Schedules = new List<ScheduleDto>(),
                EnrolledStudentsCount = 0
            };
        }

        public async Task<SectionDto?> UpdateSectionAsync(int id, UpdateSectionDto updateSectionDto)
        {
            var section = await _dbContext.Sections
                .Include(s => s.Teacher)
                .Include(s => s.Schedules)
                .Include(s => s.StudentSections)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null)
                return null;

            if (!string.IsNullOrEmpty(updateSectionDto.Name))
                section.Name = updateSectionDto.Name;

            if (!string.IsNullOrEmpty(updateSectionDto.Description))
                section.Description = updateSectionDto.Description;

            if (updateSectionDto.CoverImageUrl != null)
                section.CoverImageUrl = updateSectionDto.CoverImageUrl;

            if (!string.IsNullOrEmpty(updateSectionDto.TeacherId))
                section.TeacherId = updateSectionDto.TeacherId;

            if (updateSectionDto.MaxStudents.HasValue)
                section.MaxStudents = updateSectionDto.MaxStudents.Value;

            if (updateSectionDto.MinAttendanceForGrade.HasValue)
                section.MinAttendanceForGrade = updateSectionDto.MinAttendanceForGrade.Value;

            if (updateSectionDto.MaxAttendance.HasValue)
                section.MaxAttendance = updateSectionDto.MaxAttendance.Value;

            if (updateSectionDto.IsActive.HasValue)
                section.IsActive = updateSectionDto.IsActive.Value;

            section.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new SectionDto
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                CoverImageUrl = section.CoverImageUrl,
                TeacherId = section.TeacherId,
                TeacherName = $"{section.Teacher?.FirstName} {section.Teacher?.LastName}",
                MaxStudents = section.MaxStudents,
                MinAttendanceForGrade = section.MinAttendanceForGrade,
                MaxAttendance = section.MaxAttendance,
                IsActive = section.IsActive,
                CreatedAt = section.CreatedAt,
                Schedules = section.Schedules?.Select(sc => new ScheduleDto
                {
                    Id = sc.Id,
                    SectionId = sc.SectionId,
                    SectionName = section.Name,
                    DayOfWeek = sc.DayOfWeek,
                    StartTime = sc.StartTime,
                    EndTime = sc.EndTime,
                    Location = sc.Location,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    IsActive = sc.IsActive
                }).ToList() ?? new List<ScheduleDto>(),
                EnrolledStudentsCount = section.StudentSections?.Count(ss => ss.IsActive) ?? 0
            };
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            var section = await _dbContext.Sections.FindAsync(id);
            if (section == null)
                return false;

            _dbContext.Sections.Remove(section);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SectionDto>> GetSectionsByTeacherIdAsync(string teacherId)
        {
            var sections = await _dbContext.Sections
                .Include(s => s.Teacher)
                .Include(s => s.Schedules)
                .Include(s => s.StudentSections)
                .Where(s => s.TeacherId == teacherId)
                .ToListAsync();

            return sections.Select(s => new SectionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CoverImageUrl = s.CoverImageUrl,
                TeacherId = s.TeacherId,
                TeacherName = $"{s.Teacher?.FirstName} {s.Teacher?.LastName}",
                MaxStudents = s.MaxStudents,
                MinAttendanceForGrade = s.MinAttendanceForGrade,
                MaxAttendance = s.MaxAttendance,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                Schedules = s.Schedules?.Select(sc => new ScheduleDto
                {
                    Id = sc.Id,
                    SectionId = sc.SectionId,
                    SectionName = s.Name,
                    DayOfWeek = sc.DayOfWeek,
                    StartTime = sc.StartTime,
                    EndTime = sc.EndTime,
                    Location = sc.Location,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    IsActive = sc.IsActive
                }).ToList() ?? new List<ScheduleDto>(),
                EnrolledStudentsCount = s.StudentSections?.Count(ss => ss.IsActive) ?? 0
            }).ToList();
        }

        public async Task<bool> IsSectionFull(int sectionId)
        {
            var section = await _dbContext.Sections
                .Include(s => s.StudentSections)
                .FirstOrDefaultAsync(s => s.Id == sectionId);

            if (section == null)
                return true; // If section doesn't exist, consider it "full"

            var activeStudentCount = section.StudentSections?.Count(ss => ss.IsActive) ?? 0;
            return activeStudentCount >= section.MaxStudents;
        }
    }
}