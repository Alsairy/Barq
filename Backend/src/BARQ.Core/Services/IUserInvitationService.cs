using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
public interface IUserInvitationService
{
    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<UserInvitationResponse> SendInvitationAsync(SendUserInvitationRequest request);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<BulkInvitationResponse> SendBulkInvitationsAsync(BulkUserInvitationRequest request);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<InvitationAcceptanceResponse> AcceptInvitationAsync(AcceptInvitationRequest request);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<UserInvitationResponse> ResendInvitationAsync(Guid invitationId);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<UserInvitationResponse> CancelInvitationAsync(Guid invitationId);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<IEnumerable<UserInvitationDto>> GetPendingInvitationsAsync(Guid organizationId);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<UserOnboardingResponse> CompleteUserOnboardingAsync(CompleteOnboardingRequest request);

    /// <summary>
    /// Service for managing user invitations and onboarding processes within organizations.
/// </summary>
    Task<bool> IsInvitationValidAsync(string invitationToken);
}
