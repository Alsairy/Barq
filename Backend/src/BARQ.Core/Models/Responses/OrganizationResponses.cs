using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class OrganizationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}

/// <summary>
/// </summary>
public class OrganizationSettingsResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// </summary>
public class OrganizationBrandingResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public OrganizationBrandingDto? Branding { get; set; }
}

/// <summary>
/// </summary>
public class OrganizationValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationMessages { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ValidationContext { get; set; } = new();
}

/// <summary>
/// </summary>
public class OrganizationBrandingDto
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? LogoUrl { get; set; }
    
    /// <summary>
    /// </summary>
    public string? PrimaryColor { get; set; }
    
    /// <summary>
    /// </summary>
    public string? SecondaryColor { get; set; }
    
    /// <summary>
    /// </summary>
    public string? FontFamily { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> CustomBranding { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
