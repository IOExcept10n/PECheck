using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class StudentSectionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SectionService _sectionService;

        public StudentSectionService(ApplicationDbContext dbContext, SectionService sectionService)
        {
            _dbContext = dbContext;
            _sectionService = sectionService;
        }

        public async Task<IEnumerable<StudentSectionDto>> GetAllStudentSectionsAsync()
        {
            var studentSections = await _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.Payments)
                .ToListAsync();

            return studentSections.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<StudentSectionDto>> GetStudentSectionsByStudentIdAsync(string studentId)
        {
            var studentSections = await _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.Payments)
                .Where(ss => ss.StudentId == studentId)
                .ToListAsync();

            return studentSections.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<StudentSectionDto>> GetStudentSectionsBySectionIdAsync(int sectionId, int? semesterId = null)
        {
            var query = _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.Payments)
                .Where(ss => ss.SectionId == sectionId);

            if (semesterId.HasValue)
                query = query.Where(ss => ss.SemesterId == semesterId.Value);

            var studentSections = await query.ToListAsync();

            return studentSections.Select(MapToDto).ToList();
        }

        public async Task<StudentSectionDto?> GetStudentSectionAsync(string studentId, int sectionId, int semesterId)
        {
            var studentSection = await _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.Payments)
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentId && 
                    ss.SectionId == sectionId && 
                    ss.SemesterId == semesterId);

            if (studentSection == null)
                return null;

            return MapToDto(studentSection);
        }

        public async Task<StudentSectionDto?> EnrollStudentAsync(CreateStudentSectionDto createStudentSectionDto)
        {
            // Check if student is already enrolled in any section for the current semester
            var existingEnrollment = await _dbContext.StudentSections
                .AnyAsync(ss => 
                    ss.StudentId == createStudentSectionDto.StudentId && 
                    ss.SemesterId == createStudentSectionDto.SemesterId && 
                    ss.IsActive);

            if (existingEnrollment)
                return null; // Student is already enrolled in a section for this semester

            // Check if section is full
            var isSectionFull = await _sectionService.IsSectionFull(createStudentSectionDto.SectionId);
            if (isSectionFull)
                return null; // Section is already full

            var studentSection = new StudentSection
            {
                StudentId = createStudentSectionDto.StudentId,
                SectionId = createStudentSectionDto.SectionId,
                SemesterId = createStudentSectionDto.SemesterId,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = createStudentSectionDto.IsActive
            };

            _dbContext.StudentSections.Add(studentSection);
            await _dbContext.SaveChangesAsync();

            // Reload the student section with navigation properties
            studentSection = await _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentSection.StudentId && 
                    ss.SectionId == studentSection.SectionId && 
                    ss.SemesterId == studentSection.SemesterId);

            if (studentSection == null)
                return null;

            return MapToDto(studentSection);
        }

        public async Task<StudentSectionDto?> UpdateStudentSectionAsync(
            string studentId, 
            int sectionId, 
            int semesterId, 
            UpdateStudentSectionDto updateStudentSectionDto)
        {
            var studentSection = await _dbContext.StudentSections
                .Include(ss => ss.Student)
                .Include(ss => ss.Section)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.Payments)
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentId && 
                    ss.SectionId == sectionId && 
                    ss.SemesterId == semesterId);

            if (studentSection == null)
                return null;

            if (updateStudentSectionDto.IsActive.HasValue)
                studentSection.IsActive = updateStudentSectionDto.IsActive.Value;

            if (updateStudentSectionDto.FinalGrade.HasValue)
                studentSection.FinalGrade = updateStudentSectionDto.FinalGrade.Value;

            if (updateStudentSectionDto.DisenrollmentDate.HasValue)
            {
                studentSection.DisenrollmentDate = updateStudentSectionDto.DisenrollmentDate.Value;
                studentSection.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();

            return MapToDto(studentSection);
        }

        public async Task<bool> DisenrollStudentAsync(string studentId, int sectionId, int semesterId)
        {
            var studentSection = await _dbContext.StudentSections
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentId && 
                    ss.SectionId == sectionId && 
                    ss.SemesterId == semesterId);

            if (studentSection == null)
                return false;

            studentSection.IsActive = false;
            studentSection.DisenrollmentDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<double?> CalculateFinalGradeAsync(string studentId, int sectionId, int semesterId)
        {
            var studentSection = await _dbContext.StudentSections
                .Include(ss => ss.Section)
                .Include(ss => ss.Attendances)
                .Include(ss => ss.NormativeResults)
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentId && 
                    ss.SectionId == sectionId && 
                    ss.SemesterId == semesterId);

            if (studentSection == null || studentSection.Section == null)
                return null;

            // Calculate attendance grade (60% for minimum required, 100% for max attendance)
            var totalAttendances = studentSection.Attendances?.Count(a => a.IsPresent) ?? 0;
            var minAttendance = studentSection.Section.MinAttendanceForGrade;
            var maxAttendance = studentSection.Section.MaxAttendance;
            
            double attendanceGrade;
            if (totalAttendances < minAttendance)
            {
                // Below minimum requirements - calculate proportional grade below 60%
                attendanceGrade = (totalAttendances * 60.0) / minAttendance;
            }
            else
            {
                // Above minimum requirements - scale between 60% and 100%
                var attendanceAboveMin = totalAttendances - minAttendance;
                var maxAttendanceAboveMin = maxAttendance - minAttendance;
                var additionalGrade = maxAttendanceAboveMin > 0 
                    ? (attendanceAboveMin * 40.0) / maxAttendanceAboveMin 
                    : 40.0;
                attendanceGrade = 60.0 + additionalGrade;
            }

            // Calculate normative results average grade if available
            double normativeGrade = 0;
            if (studentSection.NormativeResults != null && studentSection.NormativeResults.Any())
            {
                normativeGrade = studentSection.NormativeResults.Average(nr => nr.Grade);
            }
            else
            {
                // If no normatives yet, use just attendance grade
                return Math.Round(attendanceGrade, 1);
            }

            // Final grade is 70% attendance and 30% normatives
            var finalGrade = (attendanceGrade * 0.7) + (normativeGrade * 0.3);
            return Math.Round(finalGrade, 1);
        }

        public async Task<bool> UpdateFinalGradeAsync(string studentId, int sectionId, int semesterId)
        {
            var studentSection = await _dbContext.StudentSections
                .FirstOrDefaultAsync(ss => 
                    ss.StudentId == studentId && 
                    ss.SectionId == sectionId && 
                    ss.SemesterId == semesterId);

            if (studentSection == null)
                return false;

            var finalGrade = await CalculateFinalGradeAsync(studentId, sectionId, semesterId);
            if (!finalGrade.HasValue)
                return false;

            studentSection.FinalGrade = finalGrade.Value;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private StudentSectionDto MapToDto(StudentSection studentSection)
        {
            return new StudentSectionDto
            {
                StudentId = studentSection.StudentId,
                StudentName = $"{studentSection.Student?.FirstName} {studentSection.Student?.LastName}",
                StudentEmail = studentSection.Student?.Email ?? string.Empty,
                SectionId = studentSection.SectionId,
                SectionName = studentSection.Section?.Name ?? string.Empty,
                SemesterId = studentSection.SemesterId,
                SemesterName = studentSection.Semester?.Name ?? string.Empty,
                EnrollmentDate = studentSection.EnrollmentDate,
                DisenrollmentDate = studentSection.DisenrollmentDate,
                IsActive = studentSection.IsActive,
                FinalGrade = studentSection.FinalGrade,
                AttendanceCount = studentSection.Attendances?.Count(a => a.IsPresent) ?? 0,
                HasPaid = studentSection.Payments?.Any(p => p.IsPaid) ?? false
            };
        }
    }
}