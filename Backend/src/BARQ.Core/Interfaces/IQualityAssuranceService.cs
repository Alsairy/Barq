using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;

namespace BARQ.Core.Interfaces;

public interface IQualityAssuranceService
{
    Task<QualityAssessment> CreateAssessmentAsync(CreateQualityAssessmentRequest request, CancellationToken cancellationToken = default);
    Task<QualityAssessment?> GetAssessmentAsync(Guid assessmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<QualityAssessment>> GetRequestAssessmentsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<QualityAssessment> UpdateAssessmentAsync(Guid assessmentId, UpdateQualityAssessmentRequest request, CancellationToken cancellationToken = default);
    Task<QualityAssessment> CompleteAssessmentAsync(Guid assessmentId, CompleteQualityAssessmentRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<QualityAssessment>> GetPendingAssessmentsAsync(Guid assessorId, CancellationToken cancellationToken = default);
    Task<bool> ValidateQualityStandardsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<object> GetQualityMetricsAsync(Guid? requestId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<QualityAssessment>> GetAssessmentsByTypeAsync(QualityAssessmentType type, CancellationToken cancellationToken = default);
    Task<bool> EscalateQualityIssueAsync(Guid assessmentId, string reason, CancellationToken cancellationToken = default);
}
