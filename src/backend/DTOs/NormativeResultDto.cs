namespace backend.DTOs
{
    public class NormativeResultDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int NormativeId { get; set; }
        public string NormativeName { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public double Grade { get; set; }
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty;
        public string RecordedByName { get; set; } = string.Empty;
        public DateTime RecordedAt { get; set; }
    }

    public class CreateNormativeResultDto
    {
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public int NormativeId { get; set; }
        public string Result { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class UpdateNormativeResultDto
    {
        public string? Result { get; set; }
        public string? Notes { get; set; }
    }

    public class BulkNormativeResultDto
    {
        public int NormativeId { get; set; }
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public List<StudentNormativeResultDto> StudentResults { get; set; } = new List<StudentNormativeResultDto>();
    }

    public class StudentNormativeResultDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}