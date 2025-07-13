using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class UserInvitationResponse : BaseResponse
{
    public UserInvitationDto? Invitation { get; set; }
    public string? InvitationToken { get; set; }
}

public class BulkInvitationResponse : BaseResponse
{
    public List<UserInvitationDto> SuccessfulInvitations { get; set; } = new();
    public List<string> FailedEmails { get; set; } = new();
    public int TotalSent { get; set; }
    public int TotalFailed { get; set; }
}

public class InvitationAcceptanceResponse : BaseResponse
{
    public Guid? UserId { get; set; }
    public bool RequiresOnboarding { get; set; }
    public string? OnboardingToken { get; set; }
}

public class UserOnboardingResponse : BaseResponse
{
    public bool IsComplete { get; set; }
    public List<string> CompletedSteps { get; set; } = new();
    public List<string> RemainingSteps { get; set; } = new();
}
