using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public DateTime? SubscriptionExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserCount { get; set; }
    public int ProjectCount { get; set; }
}
