namespace backend.Models
{
    public class Normative
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SectionId { get; set; } // The section this normative belongs to
        public string GradingCriteria { get; set; } = string.Empty; // JSON structure with grading rules
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Section? Section { get; set; }
        public virtual ICollection<NormativeResult>? NormativeResults { get; set; }
    }
}