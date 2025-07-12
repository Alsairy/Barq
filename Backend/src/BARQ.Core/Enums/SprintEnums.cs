namespace BARQ.Core.Enums;

/// <summary>
/// Sprint status enumeration
/// </summary>
public enum SprintStatus
{
    /// <summary>
    /// Sprint is in planning phase
    /// </summary>
    Planning = 0,

    /// <summary>
    /// Sprint is active and in progress
    /// </summary>
    Active = 1,

    /// <summary>
    /// Sprint is completed
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Sprint is cancelled
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// Sprint is on hold
    /// </summary>
    OnHold = 4
}

/// <summary>
/// User story status enumeration
/// </summary>
public enum UserStoryStatus
{
    /// <summary>
    /// New user story
    /// </summary>
    New = 0,

    /// <summary>
    /// User story is in backlog
    /// </summary>
    Backlog = 1,

    /// <summary>
    /// User story is ready for development
    /// </summary>
    Ready = 2,

    /// <summary>
    /// User story is in progress
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// User story is in review
    /// </summary>
    InReview = 4,

    /// <summary>
    /// User story is in testing
    /// </summary>
    Testing = 5,

    /// <summary>
    /// User story is done
    /// </summary>
    Done = 6,

    /// <summary>
    /// User story is blocked
    /// </summary>
    Blocked = 7,

    /// <summary>
    /// User story is cancelled
    /// </summary>
    Cancelled = 8
}

