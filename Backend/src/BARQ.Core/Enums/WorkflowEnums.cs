namespace BARQ.Core.Enums;

/// <summary>
/// Workflow step type enumeration
/// </summary>
public enum WorkflowStepType
{
    /// <summary>
    /// </summary>
    Task = 0,

    /// <summary>
    /// </summary>
    Approval = 1,

    /// <summary>
    /// </summary>
    Condition = 2,

    /// <summary>
    /// </summary>
    Parallel = 3,

    /// <summary>
    /// </summary>
    Sequential = 4,

    /// <summary>
    /// </summary>
    Loop = 5,

    /// <summary>
    /// </summary>
    AITask = 6,

    /// <summary>
    /// </summary>
    Integration = 7,

    /// <summary>
    /// </summary>
    DataTransformation = 8,

    /// <summary>
    /// </summary>
    Notification = 9,

    /// <summary>
    /// </summary>
    Delay = 10,

    /// <summary>
    /// Custom step type
    /// </summary>
    Custom = 99
}

/// <summary>
/// Workflow step status enumeration
/// </summary>
public enum WorkflowStepStatus
{
    /// <summary>
    /// </summary>
    Pending = 0,

    /// <summary>
    /// </summary>
    Running = 1,

    /// <summary>
    /// </summary>
    Completed = 2,

    /// <summary>
    /// </summary>
    Failed = 3,

    /// <summary>
    /// </summary>
    Skipped = 4,

    /// <summary>
    /// Step is waiting for approval
    /// </summary>
    WaitingForApproval = 5,

    /// <summary>
    /// Step was cancelled
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// </summary>
    TimedOut = 7,

    /// <summary>
    /// </summary>
    Retrying = 8
}

/// <summary>
/// Workflow type enumeration
/// </summary>
public enum WorkflowType
{
    /// <summary>
    /// Business Requirements Document approval
    /// </summary>
    BRDApproval = 0,

    /// <summary>
    /// Code review and approval
    /// </summary>
    CodeReview = 1,

    /// <summary>
    /// Testing and QA approval
    /// </summary>
    TestingApproval = 2,

    /// <summary>
    /// Security review and approval
    /// </summary>
    SecurityReview = 3,

    /// <summary>
    /// Deployment approval
    /// </summary>
    DeploymentApproval = 4,

    /// <summary>
    /// UX/UI design approval
    /// </summary>
    DesignApproval = 5,

    /// <summary>
    /// Product naming approval
    /// </summary>
    NamingApproval = 6,

    /// <summary>
    /// Logo design approval
    /// </summary>
    LogoApproval = 7,

    /// <summary>
    /// AI task approval
    /// </summary>
    AITaskApproval = 8,

    /// <summary>
    /// Project creation approval
    /// </summary>
    ProjectApproval = 9,

    /// <summary>
    /// Sprint planning approval
    /// </summary>
    SprintApproval = 10,

    /// <summary>
    /// Custom workflow type
    /// </summary>
    Custom = 99
}

/// <summary>
/// Workflow status enumeration
/// </summary>
public enum WorkflowStatus
{
    /// <summary>
    /// Workflow is pending start
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Workflow is in progress
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Workflow is waiting for approval
    /// </summary>
    WaitingForApproval = 2,

    /// <summary>
    /// Workflow is pending approval (alias for WaitingForApproval)
    /// </summary>
    PendingApproval = 2,

    /// <summary>
    /// Workflow is approved and completed
    /// </summary>
    Approved = 3,

    /// <summary>
    /// Workflow is rejected
    /// </summary>
    Rejected = 4,

    /// <summary>
    /// Workflow is cancelled
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Workflow is on hold
    /// </summary>
    OnHold = 6,

    /// <summary>
    /// Workflow has expired (SLA breach)
    /// </summary>
    Expired = 7,

    /// <summary>
    /// Workflow is escalated
    /// </summary>
    Escalated = 8,

    /// <summary>
    /// Workflow status is unknown
    /// </summary>
    Unknown = 9,

    /// <summary>
    /// Workflow is created but not started
    /// </summary>
    Created = 10,

    /// <summary>
    /// Workflow is completed
    /// </summary>
    Completed = 11
}

