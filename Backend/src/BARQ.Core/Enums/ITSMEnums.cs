namespace BARQ.Core.Enums;

/// <summary>
/// ITSM ticket type enumeration
/// </summary>
public enum ITSMTicketType
{
    /// <summary>
    /// Infrastructure provisioning request
    /// </summary>
    InfrastructureRequest = 0,

    /// <summary>
    /// Network configuration request
    /// </summary>
    NetworkRequest = 1,

    /// <summary>
    /// Domain registration request
    /// </summary>
    DomainRequest = 2,

    /// <summary>
    /// SSL certificate request
    /// </summary>
    SSLCertificateRequest = 3,

    /// <summary>
    /// Database provisioning request
    /// </summary>
    DatabaseRequest = 4,

    /// <summary>
    /// Cloud resource request
    /// </summary>
    CloudResourceRequest = 5,

    /// <summary>
    /// Security review request
    /// </summary>
    SecurityReviewRequest = 6,

    /// <summary>
    /// Compliance approval request
    /// </summary>
    ComplianceRequest = 7,

    /// <summary>
    /// Third-party integration request
    /// </summary>
    IntegrationRequest = 8,

    /// <summary>
    /// Software license request
    /// </summary>
    LicenseRequest = 9,

    /// <summary>
    /// Access permission request
    /// </summary>
    AccessRequest = 10,

    /// <summary>
    /// Change management request
    /// </summary>
    ChangeRequest = 11,

    /// <summary>
    /// Incident report
    /// </summary>
    Incident = 12,

    /// <summary>
    /// Service request
    /// </summary>
    ServiceRequest = 13,

    /// <summary>
    /// General support request
    /// </summary>
    SupportRequest = 14,

    /// <summary>
    /// Custom request type
    /// </summary>
    Custom = 99
}

/// <summary>
/// ITSM ticket status enumeration
/// </summary>
public enum ITSMTicketStatus
{
    /// <summary>
    /// New ticket
    /// </summary>
    New = 0,

    /// <summary>
    /// Ticket is assigned
    /// </summary>
    Assigned = 1,

    /// <summary>
    /// Work in progress
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Pending approval
    /// </summary>
    PendingApproval = 3,

    /// <summary>
    /// Pending customer response
    /// </summary>
    PendingCustomer = 4,

    /// <summary>
    /// Pending vendor response
    /// </summary>
    PendingVendor = 5,

    /// <summary>
    /// On hold
    /// </summary>
    OnHold = 6,

    /// <summary>
    /// Resolved
    /// </summary>
    Resolved = 7,

    /// <summary>
    /// Closed
    /// </summary>
    Closed = 8,

    /// <summary>
    /// Cancelled
    /// </summary>
    Cancelled = 9,

    /// <summary>
    /// Rejected
    /// </summary>
    Rejected = 10
}

/// <summary>
/// ITSM update type enumeration
/// </summary>
public enum ITSMUpdateType
{
    /// <summary>
    /// General comment
    /// </summary>
    Comment = 0,

    /// <summary>
    /// Status change
    /// </summary>
    StatusChange = 1,

    /// <summary>
    /// Assignment change
    /// </summary>
    AssignmentChange = 2,

    /// <summary>
    /// Priority change
    /// </summary>
    PriorityChange = 3,

    /// <summary>
    /// Resolution update
    /// </summary>
    Resolution = 4,

    /// <summary>
    /// Work note
    /// </summary>
    WorkNote = 5,

    /// <summary>
    /// Approval decision
    /// </summary>
    ApprovalDecision = 6,

    /// <summary>
    /// System update
    /// </summary>
    SystemUpdate = 7
}

/// <summary>
/// ITSM update source enumeration
/// </summary>
public enum ITSMUpdateSource
{
    /// <summary>
    /// Update from BARQ platform
    /// </summary>
    Internal = 0,

    /// <summary>
    /// Update from external ITSM system
    /// </summary>
    External = 1,

    /// <summary>
    /// Automated system update
    /// </summary>
    System = 2,

    /// <summary>
    /// Email update
    /// </summary>
    Email = 3,

    /// <summary>
    /// API update
    /// </summary>
    API = 4
}

