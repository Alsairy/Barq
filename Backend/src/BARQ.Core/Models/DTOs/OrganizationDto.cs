using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class OrganizationDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Website { get; set; }
    
    /// <summary>
    /// </summary>
    public string? LogoUrl { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// </summary>
    public SubscriptionPlan SubscriptionPlan { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? SubscriptionExpiryDate { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public int UserCount { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProjectCount { get; set; }
}
