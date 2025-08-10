namespace backend.DTOs.Stats
{
    public class SemesterStatsDto
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int TotalSections { get; set; }
        public int TotalStudents { get; set; }
        public double AverageGrade { get; set; }
        public double AverageAttendance { get; set; }
        public int TotalPayments { get; set; }
        public int UnpaidStudents { get; set; }
        public List<SectionShortStatsDto> SectionStats { get; set; } = new List<SectionShortStatsDto>();
    }

    public class SectionShortStatsDto
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public double AverageGrade { get; set; }
    }
}