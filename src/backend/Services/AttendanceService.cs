using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class AttendanceService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StudentSectionService _studentSectionService;

        public AttendanceService(ApplicationDbContext dbContext, StudentSectionService studentSectionService)
        {
            _dbContext = dbContext;
            _studentSectionService = studentSectionService;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAttendancesAsync(
            string? studentId = null, 
            int? sectionId = null, 
            int? semesterId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _dbContext.Attendances
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Student)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Section)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Semester)
                .AsQueryable();

            if (!string.IsNullOrEmpty(studentId))
                query = query.Where(a => a.StudentId == studentId);

            if (sectionId.HasValue)
                query = query.Where(a => a.SectionId == sectionId.Value);

            if (semesterId.HasValue)
                query = query.Where(a => a.SemesterId == semesterId.Value);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            var attendances = await query.ToListAsync();

            return attendances.Select(MapToDto).ToList();
        }

        public async Task<AttendanceDto?> GetAttendanceByIdAsync(int id)
        {
            var attendance = await _dbContext.Attendances
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Student)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Section)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Semester)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return null;

            return MapToDto(attendance);
        }

        public async Task<AttendanceDto?> CreateAttendanceAsync(CreateAttendanceDto createAttendanceDto, string recordedById)
        {
            // Check if student is enrolled in the section
            var studentSection = await _dbContext.StudentSections
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == createAttendanceDto.StudentId && 
                    ss.SectionId == createAttendanceDto.SectionId && 
                    ss.SemesterId == createAttendanceDto.SemesterId && 
                    ss.IsActive);

            if (studentSection == null)
                return null; // Student is not enrolled in this section

            // Check if attendance already exists for this date
            var existingAttendance = await _dbContext.Attendances
                .FirstOrDefaultAsync(a => 
                    a.StudentId == createAttendanceDto.StudentId && 
                    a.SectionId == createAttendanceDto.SectionId && 
                    a.SemesterId == createAttendanceDto.SemesterId && 
                    a.Date.Date == createAttendanceDto.Date.Date);

            if (existingAttendance != null)
                return null; // Attendance already recorded for this date

            var attendance = new Attendance
            {
                StudentId = createAttendanceDto.StudentId,
                SectionId = createAttendanceDto.SectionId,
                SemesterId = createAttendanceDto.SemesterId,
                Date = createAttendanceDto.Date,
                IsPresent = createAttendanceDto.IsPresent,
                Notes = createAttendanceDto.Notes,
                RecordedById = recordedById,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Attendances.Add(attendance);
            await _dbContext.SaveChangesAsync();

            // Update final grade after attendance change
            await _studentSectionService.UpdateFinalGradeAsync(
                createAttendanceDto.StudentId, 
                createAttendanceDto.SectionId, 
                createAttendanceDto.SemesterId);

            // Reload the attendance with navigation properties
            attendance = await _dbContext.Attendances
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Student)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Section)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Semester)
                .FirstOrDefaultAsync(a => a.Id == attendance.Id);

            if (attendance == null)
                return null;

            return MapToDto(attendance);
        }

        public async Task<List<AttendanceDto>> CreateBulkAttendanceAsync(BulkAttendanceDto bulkAttendanceDto, string recordedById)
        {
            var createdAttendances = new List<AttendanceDto>();

            // Get all active student sections for this section and semester
            var studentSections = await _dbContext.StudentSections
                .Where(ss => 
                    ss.SectionId == bulkAttendanceDto.SectionId && 
                    ss.SemesterId == bulkAttendanceDto.SemesterId && 
                    ss.IsActive)
                .ToListAsync();

            // Get student IDs from the request
            var studentIdsInRequest = bulkAttendanceDto.StudentAttendances.Select(sa => sa.StudentId).ToList();

            // Check if all student IDs in the request are enrolled in this section
            var enrolledStudentIds = studentSections.Select(ss => ss.StudentId).ToList();
            var invalidStudentIds = studentIdsInRequest.Except(enrolledStudentIds).ToList();

            if (invalidStudentIds.Any())
                return createdAttendances; // Some students are not enrolled in this section

            // Check if any attendances already exist for this date
            var existingAttendances = await _dbContext.Attendances
                .Where(a => 
                    a.SectionId == bulkAttendanceDto.SectionId && 
                    a.SemesterId == bulkAttendanceDto.SemesterId && 
                    a.Date.Date == bulkAttendanceDto.Date.Date && 
                    studentIdsInRequest.Contains(a.StudentId))
                .ToListAsync();

            if (existingAttendances.Any())
            {
                // Remove existing attendances for this date
                _dbContext.Attendances.RemoveRange(existingAttendances);
                await _dbContext.SaveChangesAsync();
            }

            // Create new attendances
            var attendances = new List<Attendance>();
            foreach (var studentAttendance in bulkAttendanceDto.StudentAttendances)
            {
                var attendance = new Attendance
                {
                    StudentId = studentAttendance.StudentId,
                    SectionId = bulkAttendanceDto.SectionId,
                    SemesterId = bulkAttendanceDto.SemesterId,
                    Date = bulkAttendanceDto.Date,
                    IsPresent = studentAttendance.IsPresent,
                    Notes = studentAttendance.Notes,
                    RecordedById = recordedById,
                    CreatedAt = DateTime.UtcNow
                };

                attendances.Add(attendance);
            }

            _dbContext.Attendances.AddRange(attendances);
            await _dbContext.SaveChangesAsync();

            // Update final grades for all students
            foreach (var studentId in studentIdsInRequest)
            {
                await _studentSectionService.UpdateFinalGradeAsync(
                    studentId, 
                    bulkAttendanceDto.SectionId, 
                    bulkAttendanceDto.SemesterId);
            }

            // Reload the attendances with navigation properties
            var createdAttendanceIds = attendances.Select(a => a.Id).ToList();
            var loadedAttendances = await _dbContext.Attendances
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Student)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Section)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Semester)
                .Where(a => createdAttendanceIds.Contains(a.Id))
                .ToListAsync();

            return loadedAttendances.Select(MapToDto).ToList();
        }

        public async Task<AttendanceDto?> UpdateAttendanceAsync(int id, UpdateAttendanceDto updateAttendanceDto, string recordedById)
        {
            var attendance = await _dbContext.Attendances
                .Include(a => a.StudentSection)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return null;

            if (updateAttendanceDto.IsPresent.HasValue)
                attendance.IsPresent = updateAttendanceDto.IsPresent.Value;

            if (updateAttendanceDto.Notes != null)
                attendance.Notes = updateAttendanceDto.Notes;

            attendance.RecordedById = recordedById;
            attendance.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Update final grade after attendance change
            await _studentSectionService.UpdateFinalGradeAsync(
                attendance.StudentId, 
                attendance.SectionId, 
                attendance.SemesterId);

            // Reload the attendance with navigation properties
            attendance = await _dbContext.Attendances
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Student)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Section)
                .Include(a => a.StudentSection)
                    .ThenInclude(ss => ss.Semester)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return null;

            return MapToDto(attendance);
        }

        public async Task<bool> DeleteAttendanceAsync(int id)
        {
            var attendance = await _dbContext.Attendances.FindAsync(id);
            if (attendance == null)
                return false;

            string studentId = attendance.StudentId;
            int sectionId = attendance.SectionId;
            int semesterId = attendance.SemesterId;

            _dbContext.Attendances.Remove(attendance);
            await _dbContext.SaveChangesAsync();

            // Update final grade after attendance deletion
            await _studentSectionService.UpdateFinalGradeAsync(studentId, sectionId, semesterId);

            return true;
        }

        private AttendanceDto MapToDto(Attendance attendance)
        {
            var student = attendance.StudentSection?.Student;
            var section = attendance.StudentSection?.Section;
            var semester = attendance.StudentSection?.Semester;

            return new AttendanceDto
            {
                Id = attendance.Id,
                StudentId = attendance.StudentId,
                StudentName = $"{student?.FirstName} {student?.LastName}",
                SectionId = attendance.SectionId,
                SectionName = section?.Name ?? string.Empty,
                SemesterId = attendance.SemesterId,
                SemesterName = semester?.Name ?? string.Empty,
                Date = attendance.Date,
                IsPresent = attendance.IsPresent,
                Notes = attendance.Notes,
                RecordedById = attendance.RecordedById,
                RecordedByName = string.Empty, // This would need to be populated by joining with Users
                CreatedAt = attendance.CreatedAt
            };
        }
    }
}