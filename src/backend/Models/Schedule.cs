namespace backend.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } // Optional start date of this schedule
        public DateTime? EndDate { get; set; } // Optional end date of this schedule
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Section? Section { get; set; }
    }
}