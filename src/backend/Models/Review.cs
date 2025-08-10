namespace backend.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public int Rating { get; set; } // Rating from 1-5
        public string Comment { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual StudentSection? StudentSection { get; set; }
    }
}