namespace backend.DTOs.Stats
{
    public class StudentStatsDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int TotalSections { get; set; }
        public double AverageGrade { get; set; }
        public int TotalAttendances { get; set; }
        public double AttendancePercentage { get; set; }
        public List<SectionGradeDto> SectionGrades { get; set; } = new List<SectionGradeDto>();
    }

    public class SectionGradeDto
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public double? FinalGrade { get; set; }
        public int AttendanceCount { get; set; }
        public double AttendancePercentage { get; set; }
        public List<NormativeResultShortDto> NormativeResults { get; set; } = new List<NormativeResultShortDto>();
    }

    public class NormativeResultShortDto
    {
        public int NormativeId { get; set; }
        public string NormativeName { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public double Grade { get; set; }
    }
}