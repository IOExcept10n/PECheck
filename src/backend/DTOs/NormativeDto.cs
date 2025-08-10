namespace backend.DTOs
{
    public class NormativeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string GradingCriteria { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNormativeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string GradingCriteria { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateNormativeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? GradingCriteria { get; set; }
        public bool? IsActive { get; set; }
    }
}