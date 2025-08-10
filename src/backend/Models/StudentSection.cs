namespace backend.Models
{
    public class StudentSection
    {
        // Composite key
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public DateTime? DisenrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        public double? FinalGrade { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser? Student { get; set; }
        public virtual Section? Section { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }
        public virtual ICollection<NormativeResult>? NormativeResults { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}