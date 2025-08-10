namespace backend.Models
{
    public class NormativeResult
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public int NormativeId { get; set; }
        public string Result { get; set; } = string.Empty; // The actual result (could be time, repetitions, etc.)
        public double Grade { get; set; } // Calculated grade based on normative criteria
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty; // Teacher who recorded the result
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual StudentSection? StudentSection { get; set; }
        public virtual Normative? Normative { get; set; }
    }
}