using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class OrganizationResponse : BaseResponse
{
    public OrganizationDto? Organization { get; set; }
}

public class OrganizationSettingsResponse : BaseResponse
{
    public Dictionary<string, object> Settings { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}

public class OrganizationBrandingResponse : BaseResponse
{
    public OrganizationBrandingDto? Branding { get; set; }
}

public class OrganizationValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
    public Dictionary<string, object> ValidationContext { get; set; } = new();
}

public class OrganizationBrandingDto
{
    public Guid OrganizationId { get; set; }
    public string? LogoUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? FontFamily { get; set; }
    public Dictionary<string, object> CustomBranding { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}
