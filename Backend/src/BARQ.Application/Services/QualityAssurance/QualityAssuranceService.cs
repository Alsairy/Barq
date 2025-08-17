using AutoMapper;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Interfaces;
using BARQ.Core.Models.Requests;
using BARQ.Core.Repositories;
using BARQ.Core.Services;
using Microsoft.Extensions.Logging;

namespace BARQ.Application.Services.QualityAssurance;

public class QualityAssuranceService : IQualityAssuranceService
{
    private readonly IRepository<QualityAssessment> _assessmentRepository;
    private readonly IRepository<QualityMetric> _metricRepository;
    private readonly IRepository<AIRequest> _aiRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<QualityAssuranceService> _logger;
    private readonly ITenantProvider _tenantProvider;

    public QualityAssuranceService(
        IRepository<QualityAssessment> assessmentRepository,
        IRepository<QualityMetric> metricRepository,
        IRepository<AIRequest> aiRequestRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<QualityAssuranceService> logger,
        ITenantProvider tenantProvider)
    {
        _assessmentRepository = assessmentRepository ?? throw new ArgumentNullException(nameof(assessmentRepository));
        _metricRepository = metricRepository ?? throw new ArgumentNullException(nameof(metricRepository));
        _aiRequestRepository = aiRequestRepository ?? throw new ArgumentNullException(nameof(aiRequestRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
    }

    public async Task<QualityAssessment> CreateAssessmentAsync(CreateQualityAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating quality assessment for AI request: {AIRequestId}", request.AIRequestId);

            var aiRequest = await _aiRequestRepository.GetByIdAsync(request.AIRequestId, cancellationToken);
            if (aiRequest == null)
            {
                throw new InvalidOperationException($"AI request not found: {request.AIRequestId}");
            }

            var assessment = new QualityAssessment
            {
                Id = Guid.NewGuid(),
                AIRequestId = request.AIRequestId,
                AssessorId = request.AssessorId,
                Type = request.Type,
                QualityCriteria = request.QualityCriteria,
                Status = QualityAssessmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _assessmentRepository.AddAsync(assessment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Quality assessment created successfully: {AssessmentId}", assessment.Id);
            return assessment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quality assessment for AI request: {AIRequestId}", request.AIRequestId);
            throw;
        }
    }

    public async Task<QualityAssessment?> GetAssessmentAsync(Guid assessmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _assessmentRepository.GetByIdAsync(assessmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting quality assessment: {AssessmentId}", assessmentId);
            throw;
        }
    }

    public async Task<IEnumerable<QualityAssessment>> GetRequestAssessmentsAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync(cancellationToken);
            return assessments.Where(a => a.AIRequestId == requestId).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assessments for AI request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<QualityAssessment> UpdateAssessmentAsync(Guid assessmentId, UpdateQualityAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating quality assessment: {AssessmentId}", assessmentId);

            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId, cancellationToken);
            if (assessment == null)
            {
                throw new InvalidOperationException($"Quality assessment not found: {assessmentId}");
            }

            if (request.QualityScore.HasValue)
                assessment.QualityScore = request.QualityScore.Value;

            if (!string.IsNullOrEmpty(request.Comments))
                assessment.Comments = request.Comments;

            if (!string.IsNullOrEmpty(request.Recommendations))
                assessment.Recommendations = request.Recommendations;

            if (!string.IsNullOrEmpty(request.AssessmentResults))
                assessment.AssessmentResults = request.AssessmentResults;

            if (request.RequiresReview.HasValue)
                assessment.RequiresReview = request.RequiresReview.Value;

            assessment.UpdatedAt = DateTime.UtcNow;

            await _assessmentRepository.UpdateAsync(assessment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Quality assessment updated successfully: {AssessmentId}", assessmentId);
            return assessment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quality assessment: {AssessmentId}", assessmentId);
            throw;
        }
    }

    public async Task<QualityAssessment> CompleteAssessmentAsync(Guid assessmentId, CompleteQualityAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Completing quality assessment: {AssessmentId}", assessmentId);

            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId, cancellationToken);
            if (assessment == null)
            {
                throw new InvalidOperationException($"Quality assessment not found: {assessmentId}");
            }

            assessment.QualityScore = request.QualityScore;
            assessment.Comments = request.Comments;
            assessment.Recommendations = request.Recommendations;
            assessment.AssessmentResults = request.AssessmentResults;
            assessment.RequiresReview = request.RequiresReview;
            assessment.Status = request.Status;
            assessment.CompletedAt = DateTime.UtcNow;
            assessment.UpdatedAt = DateTime.UtcNow;

            await _assessmentRepository.UpdateAsync(assessment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var aiRequest = await _aiRequestRepository.GetByIdAsync(assessment.AIRequestId, cancellationToken);
            if (aiRequest != null)
            {
                if (request.Status == QualityAssessmentStatus.Approved)
                {
                    aiRequest.Status = AIRequestStatus.Completed;
                }
                else if (request.Status == QualityAssessmentStatus.Rejected || request.RequiresReview)
                {
                    aiRequest.Status = AIRequestStatus.QualityReview;
                }

                aiRequest.UpdatedAt = DateTime.UtcNow;
                await _aiRequestRepository.UpdateAsync(aiRequest, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Quality assessment completed successfully: {AssessmentId}", assessmentId);
            return assessment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing quality assessment: {AssessmentId}", assessmentId);
            throw;
        }
    }

    public async Task<IEnumerable<QualityAssessment>> GetPendingAssessmentsAsync(Guid assessorId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync(cancellationToken);
            return assessments.Where(a => 
                a.AssessorId == assessorId && 
                (a.Status == QualityAssessmentStatus.Pending || a.Status == QualityAssessmentStatus.InProgress))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending assessments for assessor: {AssessorId}", assessorId);
            throw;
        }
    }

    public async Task<bool> ValidateQualityStandardsAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating quality standards for AI request: {RequestId}", requestId);

            var assessments = await GetRequestAssessmentsAsync(requestId, cancellationToken);
            var completedAssessments = assessments.Where(a => a.Status == QualityAssessmentStatus.Completed || a.Status == QualityAssessmentStatus.Approved);

            if (!completedAssessments.Any())
            {
                _logger.LogWarning("No completed quality assessments found for request: {RequestId}", requestId);
                return false;
            }

            var averageScore = completedAssessments.Average(a => a.QualityScore);
            var minimumQualityThreshold = 75;

            var meetsStandards = averageScore >= minimumQualityThreshold && 
                                completedAssessments.All(a => !a.RequiresReview);

            _logger.LogInformation("Quality validation for request {RequestId}: Score={AverageScore}, Meets Standards={MeetsStandards}", 
                requestId, averageScore, meetsStandards);

            return meetsStandards;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating quality standards for request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<object> GetQualityMetricsAsync(Guid? requestId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync(cancellationToken);

            if (requestId.HasValue)
            {
                assessments = assessments.Where(a => a.AIRequestId == requestId.Value);
            }

            if (fromDate.HasValue)
            {
                assessments = assessments.Where(a => a.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                assessments = assessments.Where(a => a.CreatedAt <= toDate.Value);
            }

            var completedAssessments = assessments.Where(a => a.Status == QualityAssessmentStatus.Completed || a.Status == QualityAssessmentStatus.Approved);

            return new
            {
                TotalAssessments = assessments.Count(),
                CompletedAssessments = completedAssessments.Count(),
                PendingAssessments = assessments.Count(a => a.Status == QualityAssessmentStatus.Pending),
                InProgressAssessments = assessments.Count(a => a.Status == QualityAssessmentStatus.InProgress),
                AverageQualityScore = completedAssessments.Any() ? completedAssessments.Average(a => a.QualityScore) : 0,
                AssessmentsByType = assessments.GroupBy(a => a.Type).ToDictionary(g => g.Key.ToString(), g => g.Count()),
                AssessmentsByStatus = assessments.GroupBy(a => a.Status).ToDictionary(g => g.Key.ToString(), g => g.Count()),
                QualityTrend = completedAssessments
                    .GroupBy(a => a.CreatedAt.Date)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Average(a => a.QualityScore))
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting quality metrics");
            throw;
        }
    }

    public async Task<IEnumerable<QualityAssessment>> GetAssessmentsByTypeAsync(QualityAssessmentType type, CancellationToken cancellationToken = default)
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync(cancellationToken);
            return assessments.Where(a => a.Type == type).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assessments by type: {Type}", type);
            throw;
        }
    }

    public async Task<bool> EscalateQualityIssueAsync(Guid assessmentId, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Escalating quality issue for assessment: {AssessmentId}", assessmentId);

            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId, cancellationToken);
            if (assessment == null)
            {
                return false;
            }

            assessment.Status = QualityAssessmentStatus.RequiresReview;
            assessment.RequiresReview = true;
            assessment.Comments = $"ESCALATED: {reason}. Previous comments: {assessment.Comments}";
            assessment.UpdatedAt = DateTime.UtcNow;

            await _assessmentRepository.UpdateAsync(assessment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var aiRequest = await _aiRequestRepository.GetByIdAsync(assessment.AIRequestId, cancellationToken);
            if (aiRequest != null)
            {
                aiRequest.Status = AIRequestStatus.QualityReview;
                aiRequest.UpdatedAt = DateTime.UtcNow;
                await _aiRequestRepository.UpdateAsync(aiRequest, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Quality issue escalated successfully for assessment: {AssessmentId}", assessmentId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escalating quality issue for assessment: {AssessmentId}", assessmentId);
            throw;
        }
    }
}
