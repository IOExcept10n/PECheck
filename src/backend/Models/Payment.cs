namespace backend.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; }
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty; // User who recorded the payment
        
        // Navigation properties
        public virtual StudentSection? StudentSection { get; set; }
    }
}