namespace backend.DTOs
{
    public class StudentSectionDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public DateTime? DisenrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public double? FinalGrade { get; set; }
        public int AttendanceCount { get; set; }
        public bool HasPaid { get; set; }
    }

    public class CreateStudentSectionDto
    {
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateStudentSectionDto
    {
        public bool? IsActive { get; set; }
        public double? FinalGrade { get; set; }
        public DateTime? DisenrollmentDate { get; set; }
    }
}