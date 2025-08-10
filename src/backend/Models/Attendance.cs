namespace backend.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty; // Teacher or moderator who recorded attendance
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual StudentSection? StudentSection { get; set; }
    }
}