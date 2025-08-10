namespace backend.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty;
        public string RecordedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAttendanceDto
    {
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateAttendanceDto
    {
        public bool? IsPresent { get; set; }
        public string? Notes { get; set; }
    }

    public class BulkAttendanceDto
    {
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceDto> StudentAttendances { get; set; } = new List<StudentAttendanceDto>();
    }

    public class StudentAttendanceDto
    {
        public string StudentId { get; set; } = string.Empty;
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
    }
}