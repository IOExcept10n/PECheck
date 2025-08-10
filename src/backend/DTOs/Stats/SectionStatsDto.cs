namespace backend.DTOs.Stats
{
    public class SectionStatsDto
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public double AverageAttendance { get; set; }
        public double AverageGrade { get; set; }
        public int TotalPayments { get; set; }
        public int UnpaidStudents { get; set; }
        public List<NormativeStatsDto> NormativeStats { get; set; } = new List<NormativeStatsDto>();
    }

    public class NormativeStatsDto
    {
        public int NormativeId { get; set; }
        public string NormativeName { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
        public int TotalResults { get; set; }
    }
}