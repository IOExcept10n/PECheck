namespace backend.DTOs
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public int MinAttendanceForGrade { get; set; }
        public int MaxAttendance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
        public int EnrolledStudentsCount { get; set; }
    }

    public class CreateSectionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public int MinAttendanceForGrade { get; set; }
        public int MaxAttendance { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateSectionDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? TeacherId { get; set; }
        public int? MaxStudents { get; set; }
        public int? MinAttendanceForGrade { get; set; }
        public int? MaxAttendance { get; set; }
        public bool? IsActive { get; set; }
    }
}