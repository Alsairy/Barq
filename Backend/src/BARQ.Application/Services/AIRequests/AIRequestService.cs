using AutoMapper;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Interfaces;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Repositories;
using BARQ.Core.Services;
using Microsoft.Extensions.Logging;

namespace BARQ.Application.Services.AIRequests;

public class AIRequestService : IAIRequestService
{
    private readonly IRepository<AIRequest> _aiRequestRepository;
    private readonly IRepository<AIRequestApproval> _approvalRepository;
    private readonly IWorkflowService _workflowService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AIRequestService> _logger;
    private readonly ITenantProvider _tenantProvider;

    public AIRequestService(
        IRepository<AIRequest> aiRequestRepository,
        IRepository<AIRequestApproval> approvalRepository,
        IWorkflowService workflowService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AIRequestService> logger,
        ITenantProvider tenantProvider)
    {
        _aiRequestRepository = aiRequestRepository ?? throw new ArgumentNullException(nameof(aiRequestRepository));
        _approvalRepository = approvalRepository ?? throw new ArgumentNullException(nameof(approvalRepository));
        _workflowService = workflowService ?? throw new ArgumentNullException(nameof(workflowService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
    }

    public async Task<AIRequest> CreateRequestAsync(CreateAIRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating AI request: {Title}", request.Title);

            var aiRequest = new AIRequest
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                RequestType = request.RequestType,
                Priority = request.Priority,
                RequestData = request.RequestData,
                RequesterId = request.RequesterId,
                DueDate = request.DueDate,
                Status = AIRequestStatus.Draft,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow
            };

            await _aiRequestRepository.AddAsync(aiRequest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request created successfully: {RequestId}", aiRequest.Id);
            return aiRequest;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating AI request: {Title}", request.Title);
            throw;
        }
    }

    public async Task<AIRequest?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            return request;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting AI request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<IEnumerable<AIRequest>> GetUserRequestsAsync(Guid userId, AIRequestStatus? status = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _aiRequestRepository.GetAllAsync(cancellationToken);
            var userRequests = requests.Where(r => r.RequesterId == userId);

            if (status.HasValue)
            {
                userRequests = userRequests.Where(r => r.Status == status.Value);
            }

            return userRequests.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user requests: {UserId}", userId);
            throw;
        }
    }

    public async Task<AIRequest> UpdateRequestAsync(Guid requestId, UpdateAIRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating AI request: {RequestId}", requestId);

            var existingRequest = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (existingRequest == null)
            {
                throw new InvalidOperationException($"AI request not found: {requestId}");
            }

            if (!string.IsNullOrEmpty(request.Title))
                existingRequest.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                existingRequest.Description = request.Description;

            if (request.RequestType.HasValue)
                existingRequest.RequestType = request.RequestType.Value;

            if (request.Priority.HasValue)
                existingRequest.Priority = request.Priority.Value;

            if (!string.IsNullOrEmpty(request.RequestData))
                existingRequest.RequestData = request.RequestData;

            if (request.DueDate.HasValue)
                existingRequest.DueDate = request.DueDate;

            existingRequest.UpdatedAt = DateTime.UtcNow;

            await _aiRequestRepository.UpdateAsync(existingRequest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request updated successfully: {RequestId}", requestId);
            return existingRequest;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating AI request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<bool> DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting AI request: {RequestId}", requestId);

            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
            {
                return false;
            }

            await _aiRequestRepository.DeleteAsync(request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request deleted successfully: {RequestId}", requestId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting AI request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<AIRequest> SubmitRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Submitting AI request: {RequestId}", requestId);

            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
            {
                throw new InvalidOperationException($"AI request not found: {requestId}");
            }

            if (request.Status != AIRequestStatus.Draft)
            {
                throw new InvalidOperationException($"AI request cannot be submitted in current status: {request.Status}");
            }

            request.Status = AIRequestStatus.Submitted;
            request.UpdatedAt = DateTime.UtcNow;

            await _aiRequestRepository.UpdateAsync(request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request submitted successfully: {RequestId}", requestId);
            return request;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting AI request: {RequestId}", requestId);
            throw;
        }
    }

    public async Task<WorkflowExecutionResult> StartWorkflowAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting workflow for AI request: {RequestId}", requestId);

            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
            {
                return new WorkflowExecutionResult { IsSuccess = false, Message = "AI request not found" };
            }

            if (request.Status != AIRequestStatus.Submitted)
            {
                return new WorkflowExecutionResult { IsSuccess = false, Message = "AI request must be submitted before starting workflow" };
            }

            var workflowResult = await _workflowService.StartWorkflowAsync(requestId, cancellationToken);

            if (workflowResult.IsSuccess)
            {
                request.Status = AIRequestStatus.UnderReview;
                request.UpdatedAt = DateTime.UtcNow;
                await _aiRequestRepository.UpdateAsync(request, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Workflow started for AI request: {RequestId}, Success: {IsSuccess}", requestId, workflowResult.IsSuccess);
            return workflowResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting workflow for AI request: {RequestId}", requestId);
            return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<IEnumerable<AIRequest>> GetPendingApprovalsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var approvals = await _approvalRepository.GetAllAsync(cancellationToken);
            var pendingApprovals = approvals.Where(a => 
                (a.ApproverId == userId || a.DelegatedToId == userId) && 
                a.Status == ApprovalStatus.Pending);

            var requestIds = pendingApprovals.Select(a => a.AIRequestId).Distinct();
            var requests = await _aiRequestRepository.GetAllAsync(cancellationToken);
            
            return requests.Where(r => requestIds.Contains(r.Id)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending approvals for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<AIRequestApproval> ApproveRequestAsync(Guid requestId, Guid approverId, string? comments = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Approving AI request: {RequestId} by {ApproverId}", requestId, approverId);

            var approval = await GetPendingApprovalAsync(requestId, approverId, cancellationToken);
            if (approval == null)
            {
                throw new InvalidOperationException($"No pending approval found for request {requestId} and approver {approverId}");
            }

            approval.Status = ApprovalStatus.Approved;
            approval.ApprovedAt = DateTime.UtcNow;
            approval.Comments = comments;
            approval.UpdatedAt = DateTime.UtcNow;

            await _approvalRepository.UpdateAsync(approval, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request approved: {RequestId} by {ApproverId}", requestId, approverId);
            return approval;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving AI request: {RequestId} by {ApproverId}", requestId, approverId);
            throw;
        }
    }

    public async Task<AIRequestApproval> RejectRequestAsync(Guid requestId, Guid approverId, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Rejecting AI request: {RequestId} by {ApproverId}", requestId, approverId);

            var approval = await GetPendingApprovalAsync(requestId, approverId, cancellationToken);
            if (approval == null)
            {
                throw new InvalidOperationException($"No pending approval found for request {requestId} and approver {approverId}");
            }

            approval.Status = ApprovalStatus.Rejected;
            approval.RejectionReason = reason;
            approval.UpdatedAt = DateTime.UtcNow;

            await _approvalRepository.UpdateAsync(approval, cancellationToken);

            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request != null)
            {
                request.Status = AIRequestStatus.Rejected;
                request.UpdatedAt = DateTime.UtcNow;
                await _aiRequestRepository.UpdateAsync(request, cancellationToken);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("AI request rejected: {RequestId} by {ApproverId}", requestId, approverId);
            return approval;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting AI request: {RequestId} by {ApproverId}", requestId, approverId);
            throw;
        }
    }

    public async Task<AIRequestApproval> RequestChangesAsync(Guid requestId, Guid reviewerId, string changeRequests, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Requesting changes for AI request: {RequestId} by {ReviewerId}", requestId, reviewerId);

            var approval = await GetPendingApprovalAsync(requestId, reviewerId, cancellationToken);
            if (approval == null)
            {
                throw new InvalidOperationException($"No pending approval found for request {requestId} and reviewer {reviewerId}");
            }

            approval.Status = ApprovalStatus.RequiresChanges;
            approval.Comments = changeRequests;
            approval.UpdatedAt = DateTime.UtcNow;

            await _approvalRepository.UpdateAsync(approval, cancellationToken);

            var request = await _aiRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request != null)
            {
                request.Status = AIRequestStatus.RequiresChanges;
                request.UpdatedAt = DateTime.UtcNow;
                await _aiRequestRepository.UpdateAsync(request, cancellationToken);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Changes requested for AI request: {RequestId} by {ReviewerId}", requestId, reviewerId);
            return approval;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting changes for AI request: {RequestId} by {ReviewerId}", requestId, reviewerId);
            throw;
        }
    }

    public async Task<bool> DelegateApprovalAsync(Guid approvalId, Guid delegateToId, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Delegating approval: {ApprovalId} to {DelegateToId}", approvalId, delegateToId);

            var approval = await _approvalRepository.GetByIdAsync(approvalId, cancellationToken);
            if (approval == null)
            {
                return false;
            }

            approval.DelegatedToId = delegateToId;
            approval.DelegatedAt = DateTime.UtcNow;
            approval.DelegationReason = reason;
            approval.Status = ApprovalStatus.Delegated;
            approval.UpdatedAt = DateTime.UtcNow;

            await _approvalRepository.UpdateAsync(approval, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Approval delegated: {ApprovalId} to {DelegateToId}", approvalId, delegateToId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error delegating approval: {ApprovalId} to {DelegateToId}", approvalId, delegateToId);
            throw;
        }
    }

    public async Task<IEnumerable<AIRequest>> GetRequestsByStatusAsync(AIRequestStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _aiRequestRepository.GetAllAsync(cancellationToken);
            return requests.Where(r => r.Status == status).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting requests by status: {Status}", status);
            throw;
        }
    }

    public async Task<object> GetRequestAnalyticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _aiRequestRepository.GetAllAsync(cancellationToken);
            
            return new
            {
                TotalRequests = requests.Count(),
                PendingRequests = requests.Count(r => r.Status == AIRequestStatus.Submitted || r.Status == AIRequestStatus.UnderReview),
                ApprovedRequests = requests.Count(r => r.Status == AIRequestStatus.Approved),
                CompletedRequests = requests.Count(r => r.Status == AIRequestStatus.Completed),
                RejectedRequests = requests.Count(r => r.Status == AIRequestStatus.Rejected),
                RequestsByType = requests.GroupBy(r => r.RequestType).ToDictionary(g => g.Key.ToString(), g => g.Count()),
                RequestsByPriority = requests.GroupBy(r => r.Priority).ToDictionary(g => g.Key.ToString(), g => g.Count())
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting request analytics");
            throw;
        }
    }

    private async Task<AIRequestApproval?> GetPendingApprovalAsync(Guid requestId, Guid approverId, CancellationToken cancellationToken)
    {
        var approvals = await _approvalRepository.GetAllAsync(cancellationToken);
        return approvals.FirstOrDefault(a => 
            a.AIRequestId == requestId && 
            (a.ApproverId == approverId || a.DelegatedToId == approverId) && 
            a.Status == ApprovalStatus.Pending);
    }
}
