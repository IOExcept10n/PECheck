using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs.Stats;
using backend.Models;

namespace backend.Services
{
    public class StatsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StudentSectionService _studentSectionService;

        public StatsService(ApplicationDbContext dbContext, StudentSectionService studentSectionService)
        {
            _dbContext = dbContext;
            _studentSectionService = studentSectionService;
        }

        public async Task<SectionStatsDto?> GetSectionStatsAsync(int sectionId, int? semesterId = null)
        {
            var section = await _dbContext.Sections
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.Attendances)
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.NormativeResults)
                    .ThenInclude(nr => nr.Normative)
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.Payments)
                .FirstOrDefaultAsync(s => s.Id == sectionId);

            if (section == null)
                return null;

            var studentSections = section.StudentSections;
            if (semesterId.HasValue)
                studentSections = studentSections?.Where(ss => ss.SemesterId == semesterId.Value).ToList();

            if (studentSections == null || !studentSections.Any())
                return new SectionStatsDto
                {
                    SectionId = sectionId,
                    SectionName = section.Name,
                    TotalStudents = 0,
                    ActiveStudents = 0,
                    AverageAttendance = 0,
                    AverageGrade = 0,
                    TotalPayments = 0,
                    UnpaidStudents = 0,
                    NormativeStats = new List<NormativeStatsDto>()
                };

            var totalStudents = studentSections.Count();
            var activeStudents = studentSections.Count(ss => ss.IsActive);
            
            // Calculate average attendance
            double averageAttendance = 0;
            if (studentSections.Any(ss => ss.Attendances != null && ss.Attendances.Any()))
            {
                var attendanceCounts = studentSections
                    .Select(ss => ss.Attendances?.Count(a => a.IsPresent) ?? 0)
                    .ToList();
                
                averageAttendance = attendanceCounts.Any() ? attendanceCounts.Average() : 0;
            }

            // Calculate average grade
            var gradesCount = studentSections.Count(ss => ss.FinalGrade.HasValue);
            var averageGrade = gradesCount > 0 
                ? studentSections.Where(ss => ss.FinalGrade.HasValue).Average(ss => ss.FinalGrade!.Value) 
                : 0;

            // Calculate payments stats
            var totalPayments = studentSections.Count(ss => ss.Payments != null && ss.Payments.Any(p => p.IsPaid));
            var unpaidStudents = totalStudents - totalPayments;

            // Get normative stats
            var normativeStats = new List<NormativeStatsDto>();
            var normativeResults = studentSections
                .SelectMany(ss => ss.NormativeResults ?? Enumerable.Empty<NormativeResult>())
                .ToList();

            if (normativeResults.Any())
            {
                var normativeGroups = normativeResults
                    .GroupBy(nr => new { nr.NormativeId, NormativeName = nr.Normative?.Name ?? string.Empty })
                    .Select(g => new NormativeStatsDto
                    {
                        NormativeId = g.Key.NormativeId,
                        NormativeName = g.Key.NormativeName,
                        AverageGrade = g.Average(nr => nr.Grade),
                        TotalResults = g.Count()
                    })
                    .ToList();

                normativeStats.AddRange(normativeGroups);
            }

            return new SectionStatsDto
            {
                SectionId = sectionId,
                SectionName = section.Name,
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                AverageAttendance = Math.Round(averageAttendance, 2),
                AverageGrade = Math.Round(averageGrade, 2),
                TotalPayments = totalPayments,
                UnpaidStudents = unpaidStudents,
                NormativeStats = normativeStats
            };
        }

        public async Task<StudentStatsDto?> GetStudentStatsAsync(string studentId)
        {
            var student = await _dbContext.Users
                .Include(u => u.StudentSections)
                    .ThenInclude(ss => ss.Section)
                .Include(u => u.StudentSections)
                    .ThenInclude(ss => ss.Semester)
                .Include(u => u.StudentSections)
                    .ThenInclude(ss => ss.Attendances)
                .Include(u => u.StudentSections)
                    .ThenInclude(ss => ss.NormativeResults)
                    .ThenInclude(nr => nr.Normative)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (student == null || student.StudentSections == null || !student.StudentSections.Any())
                return null;

            var totalSections = student.StudentSections.Count;
            
            // Calculate overall average grade
            var sectionGrades = new List<SectionGradeDto>();
            double totalGrade = 0;
            int gradesCount = 0;

            foreach (var ss in student.StudentSections)
            {
                var section = ss.Section;
                var semester = ss.Semester;
                
                if (section == null || semester == null)
                    continue;

                // Calculate attendance percentage
                var totalPossibleAttendances = section.MaxAttendance;
                var actualAttendances = ss.Attendances?.Count(a => a.IsPresent) ?? 0;
                var attendancePercentage = totalPossibleAttendances > 0 
                    ? (actualAttendances * 100.0) / totalPossibleAttendances 
                    : 0;

                // Get normative results
                var normativeResults = ss.NormativeResults?
                    .Select(nr => new NormativeResultShortDto
                    {
                        NormativeId = nr.NormativeId,
                        NormativeName = nr.Normative?.Name ?? string.Empty,
                        Result = nr.Result,
                        Grade = nr.Grade
                    })
                    .ToList() ?? new List<NormativeResultShortDto>();

                var sectionGrade = new SectionGradeDto
                {
                    SectionId = section.Id,
                    SectionName = section.Name,
                    SemesterId = semester.Id,
                    SemesterName = semester.Name,
                    FinalGrade = ss.FinalGrade,
                    AttendanceCount = actualAttendances,
                    AttendancePercentage = Math.Round(attendancePercentage, 2),
                    NormativeResults = normativeResults
                };

                sectionGrades.Add(sectionGrade);

                if (ss.FinalGrade.HasValue)
                {
                    totalGrade += ss.FinalGrade.Value;
                    gradesCount++;
                }
            }

            var averageGrade = gradesCount > 0 ? totalGrade / gradesCount : 0;

            // Calculate total attendances and attendance percentage
            var allAttendances = student.StudentSections
                .SelectMany(ss => ss.Attendances ?? Enumerable.Empty<Attendance>())
                .Count(a => a.IsPresent);

            var allPossibleAttendances = student.StudentSections
                .Sum(ss => ss.Section?.MaxAttendance ?? 0);

            var overallAttendancePercentage = allPossibleAttendances > 0 
                ? (allAttendances * 100.0) / allPossibleAttendances 
                : 0;

            return new StudentStatsDto
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                TotalSections = totalSections,
                AverageGrade = Math.Round(averageGrade, 2),
                TotalAttendances = allAttendances,
                AttendancePercentage = Math.Round(overallAttendancePercentage, 2),
                SectionGrades = sectionGrades
            };
        }

        public async Task<SemesterStatsDto?> GetSemesterStatsAsync(int semesterId)
        {
            var semester = await _dbContext.Semesters
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.Section)
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.Attendances)
                .Include(s => s.StudentSections)
                    .ThenInclude(ss => ss.Payments)
                .FirstOrDefaultAsync(s => s.Id == semesterId);

            if (semester == null || semester.StudentSections == null || !semester.StudentSections.Any())
                return new SemesterStatsDto
                {
                    SemesterId = semesterId,
                    SemesterName = semester?.Name ?? string.Empty,
                    TotalSections = 0,
                    TotalStudents = 0,
                    AverageGrade = 0,
                    AverageAttendance = 0,
                    TotalPayments = 0,
                    UnpaidStudents = 0,
                    SectionStats = new List<SectionShortStatsDto>()
                };

            var studentSections = semester.StudentSections;
            var totalStudents = studentSections.Count();
            
            // Get unique sections
            var sections = studentSections
                .Select(ss => ss.Section)
                .Where(s => s != null)
                .DistinctBy(s => s!.Id)
                .ToList();
            
            var totalSections = sections.Count;

            // Calculate average grade
            var gradesCount = studentSections.Count(ss => ss.FinalGrade.HasValue);
            var averageGrade = gradesCount > 0 
                ? studentSections.Where(ss => ss.FinalGrade.HasValue).Average(ss => ss.FinalGrade!.Value) 
                : 0;

            // Calculate average attendance
            double averageAttendance = 0;
            if (studentSections.Any(ss => ss.Attendances != null && ss.Attendances.Any()))
            {
                var attendanceCounts = studentSections
                    .Select(ss => ss.Attendances?.Count(a => a.IsPresent) ?? 0)
                    .ToList();
                
                averageAttendance = attendanceCounts.Any() ? attendanceCounts.Average() : 0;
            }

            // Calculate payments stats
            var totalPayments = studentSections.Count(ss => ss.Payments != null && ss.Payments.Any(p => p.IsPaid));
            var unpaidStudents = totalStudents - totalPayments;

            // Get section stats
            var sectionStats = new List<SectionShortStatsDto>();
            foreach (var section in sections)
            {
                if (section == null) continue;

                var sectionStudentSections = studentSections
                    .Where(ss => ss.SectionId == section.Id)
                    .ToList();

                var studentCount = sectionStudentSections.Count;
                
                var sectionGradesCount = sectionStudentSections.Count(ss => ss.FinalGrade.HasValue);
                var sectionAverageGrade = sectionGradesCount > 0 
                    ? sectionStudentSections.Where(ss => ss.FinalGrade.HasValue).Average(ss => ss.FinalGrade!.Value) 
                    : 0;

                sectionStats.Add(new SectionShortStatsDto
                {
                    SectionId = section.Id,
                    SectionName = section.Name,
                    StudentCount = studentCount,
                    AverageGrade = Math.Round(sectionAverageGrade, 2)
                });
            }

            return new SemesterStatsDto
            {
                SemesterId = semesterId,
                SemesterName = semester.Name,
                TotalSections = totalSections,
                TotalStudents = totalStudents,
                AverageGrade = Math.Round(averageGrade, 2),
                AverageAttendance = Math.Round(averageAttendance, 2),
                TotalPayments = totalPayments,
                UnpaidStudents = unpaidStudents,
                SectionStats = sectionStats
            };
        }
    }
}