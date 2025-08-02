using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserInvitationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public UserInvitationDto? Invitation { get; set; }
    
    /// <summary>
    /// </summary>
    public string? InvitationToken { get; set; }
}

/// <summary>
/// </summary>
public class BulkInvitationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public List<UserInvitationDto> SuccessfulInvitations { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> FailedEmails { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public int TotalSent { get; set; }
    
    /// <summary>
    /// </summary>
    public int TotalFailed { get; set; }
}

/// <summary>
/// </summary>
public class InvitationAcceptanceResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresOnboarding { get; set; }
    
    /// <summary>
    /// </summary>
    public string? OnboardingToken { get; set; }
}

/// <summary>
/// </summary>
public class UserOnboardingResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsComplete { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> CompletedSteps { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> RemainingSteps { get; set; } = new();
}
