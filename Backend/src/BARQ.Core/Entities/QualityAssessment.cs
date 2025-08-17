using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class QualityAssessment : BaseEntity
{
    public Guid AIRequestId { get; set; }
    public virtual AIRequest AIRequest { get; set; } = null!;

    public Guid AssessorId { get; set; }
    public virtual User Assessor { get; set; } = null!;

    public QualityAssessmentType Type { get; set; }

    public QualityAssessmentStatus Status { get; set; } = QualityAssessmentStatus.Pending;

    public int QualityScore { get; set; }

    [MaxLength(2000)]
    public string? Comments { get; set; }

    [MaxLength(2000)]
    public string? Recommendations { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string QualityCriteria { get; set; } = string.Empty;

    public string AssessmentResults { get; set; } = string.Empty;

    public bool RequiresReview { get; set; }

    public virtual ICollection<QualityMetric> Metrics { get; set; } = new List<QualityMetric>();
}
