using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class QualityMetric : BaseEntity
{
    public Guid QualityAssessmentId { get; set; }
    public virtual QualityAssessment QualityAssessment { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Value { get; set; }

    public decimal MinValue { get; set; }

    public decimal MaxValue { get; set; }

    public decimal TargetValue { get; set; }

    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    public bool IsPassed { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
