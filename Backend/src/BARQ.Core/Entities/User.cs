using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a user in the BARQ platform
/// </summary>
public class User : TenantEntity
{
    /// <summary>
    /// User's email address (unique identifier)
    /// </summary>
    [Required]
    [MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's display name
    /// </summary>
    [MaxLength(200)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's phone number
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// User's profile picture URL
    /// </summary>
    [MaxLength(500)]
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// User's job title
    /// </summary>
    [MaxLength(200)]
    public string? JobTitle { get; set; }

    /// <summary>
    /// User's department
    /// </summary>
    [MaxLength(200)]
    public string? Department { get; set; }

    /// <summary>
    /// User's status
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Active;

    /// <summary>
    /// User's authentication type
    /// </summary>
    public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Local;

    /// <summary>
    /// External authentication provider ID
    /// </summary>
    [MaxLength(500)]
    public string? ExternalAuthId { get; set; }

    /// <summary>
    /// Password hash (for local authentication)
    /// </summary>
    [MaxLength(500)]
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Last login date and time
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Email confirmation status
    /// </summary>
    public bool EmailConfirmed { get; set; } = false;

    /// <summary>
    /// Phone number confirmation status
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; } = false;

    /// <summary>
    /// Two-factor authentication enabled
    /// </summary>
    public bool TwoFactorEnabled { get; set; } = false;

    /// <summary>
    /// Account lockout enabled
    /// </summary>
    public bool LockoutEnabled { get; set; } = true;

    /// <summary>
    /// Account lockout end date
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Failed login attempts count
    /// </summary>
    public int AccessFailedCount { get; set; } = 0;

    /// <summary>
    /// User preferences as JSON
    /// </summary>
    public string? Preferences { get; set; }

    /// <summary>
    /// User's timezone
    /// </summary>
    [MaxLength(100)]
    public string? TimeZone { get; set; }

    /// <summary>
    /// User's language preference
    /// </summary>
    [MaxLength(10)]
    public string? Language { get; set; } = "en";

    /// <summary>
    /// Organization this user belongs to
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// User roles
    /// </summary>
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Projects this user is assigned to
    /// </summary>
    public virtual ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();

    /// <summary>
    /// Workflow instances assigned to this user
    /// </summary>
    public virtual ICollection<WorkflowInstance> AssignedWorkflows { get; set; } = new List<WorkflowInstance>();

    /// <summary>
    /// Audit logs for actions performed by this user
    /// </summary>
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    /// <summary>
    /// Get user's full name
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Get user's display name or full name
    /// </summary>
    public string GetDisplayName() => !string.IsNullOrEmpty(DisplayName) ? DisplayName : FullName;
}

