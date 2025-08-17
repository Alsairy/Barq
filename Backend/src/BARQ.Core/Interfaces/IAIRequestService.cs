using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Interfaces;

public interface IAIRequestService
{
    Task<AIRequest> CreateRequestAsync(CreateAIRequestRequest request, CancellationToken cancellationToken = default);
    Task<AIRequest?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIRequest>> GetUserRequestsAsync(Guid userId, AIRequestStatus? status = null, CancellationToken cancellationToken = default);
    Task<AIRequest> UpdateRequestAsync(Guid requestId, UpdateAIRequestRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<AIRequest> SubmitRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<WorkflowExecutionResult> StartWorkflowAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIRequest>> GetPendingApprovalsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AIRequestApproval> ApproveRequestAsync(Guid requestId, Guid approverId, string? comments = null, CancellationToken cancellationToken = default);
    Task<AIRequestApproval> RejectRequestAsync(Guid requestId, Guid approverId, string reason, CancellationToken cancellationToken = default);
    Task<AIRequestApproval> RequestChangesAsync(Guid requestId, Guid reviewerId, string changeRequests, CancellationToken cancellationToken = default);
    Task<bool> DelegateApprovalAsync(Guid approvalId, Guid delegateToId, string reason, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIRequest>> GetRequestsByStatusAsync(AIRequestStatus status, CancellationToken cancellationToken = default);
    Task<object> GetRequestAnalyticsAsync(CancellationToken cancellationToken = default);
}
