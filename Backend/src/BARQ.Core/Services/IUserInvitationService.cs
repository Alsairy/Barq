using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IUserInvitationService
{
    Task<UserInvitationResponse> SendInvitationAsync(SendUserInvitationRequest request);
    Task<BulkInvitationResponse> SendBulkInvitationsAsync(BulkUserInvitationRequest request);
    Task<InvitationAcceptanceResponse> AcceptInvitationAsync(AcceptInvitationRequest request);
    Task<UserInvitationResponse> ResendInvitationAsync(Guid invitationId);
    Task<UserInvitationResponse> CancelInvitationAsync(Guid invitationId);
    Task<IEnumerable<UserInvitationDto>> GetPendingInvitationsAsync(Guid organizationId);
    Task<UserOnboardingResponse> CompleteUserOnboardingAsync(CompleteOnboardingRequest request);
    Task<bool> IsInvitationValidAsync(string invitationToken);
}
