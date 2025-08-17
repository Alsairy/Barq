using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

public class CreateAIRequestRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public AIRequestType RequestType { get; set; }

    public AIRequestPriority Priority { get; set; } = AIRequestPriority.Normal;

    public string RequestData { get; set; } = string.Empty;

    public Guid RequesterId { get; set; }

    public DateTime? DueDate { get; set; }
}

public class UpdateAIRequestRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public AIRequestType? RequestType { get; set; }

    public AIRequestPriority? Priority { get; set; }

    public string? RequestData { get; set; }

    public DateTime? DueDate { get; set; }
}

public class CreateQualityAssessmentRequest
{
    public Guid AIRequestId { get; set; }

    public Guid AssessorId { get; set; }

    public QualityAssessmentType Type { get; set; }

    public string QualityCriteria { get; set; } = string.Empty;
}

public class UpdateQualityAssessmentRequest
{
    public int? QualityScore { get; set; }

    [MaxLength(2000)]
    public string? Comments { get; set; }

    [MaxLength(2000)]
    public string? Recommendations { get; set; }

    public string? AssessmentResults { get; set; }

    public bool? RequiresReview { get; set; }
}

public class CompleteQualityAssessmentRequest
{
    public int QualityScore { get; set; }

    [MaxLength(2000)]
    public string? Comments { get; set; }

    [MaxLength(2000)]
    public string? Recommendations { get; set; }

    public string AssessmentResults { get; set; } = string.Empty;

    public bool RequiresReview { get; set; }

    public QualityAssessmentStatus Status { get; set; }
}
