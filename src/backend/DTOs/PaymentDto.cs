namespace backend.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        public string? Notes { get; set; }
        public string RecordedById { get; set; } = string.Empty;
        public string RecordedByName { get; set; } = string.Empty;
    }

    public class CreatePaymentDto
    {
        public string StudentId { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int SemesterId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdatePaymentDto
    {
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? IsPaid { get; set; }
        public string? Notes { get; set; }
    }
}