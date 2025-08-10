namespace backend.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public int MinAttendanceForGrade { get; set; } // Minimum attendances required for passing grade
        public int MaxAttendance { get; set; } // Maximum possible attendances for the section
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser? Teacher { get; set; }
        public virtual ICollection<StudentSection>? StudentSections { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}