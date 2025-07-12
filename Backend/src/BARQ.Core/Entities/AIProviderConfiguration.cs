using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents AI provider configuration for an organization
/// </summary>
public class AIProviderConfiguration : TenantEntity
{
    /// <summary>
    /// Configuration name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Configuration description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// AI provider type
    /// </summary>
    public AIProvider Provider { get; set; }

    /// <summary>
    /// Provider endpoint URL
    /// </summary>
    [MaxLength(500)]
    public string? EndpointUrl { get; set; }

    /// <summary>
    /// API key (encrypted)
    /// </summary>
    [MaxLength(1000)]
    public string? ApiKey { get; set; }

    /// <summary>
    /// Additional configuration settings as JSON
    /// </summary>
    public string? Settings { get; set; }

    /// <summary>
    /// Default model to use for this provider
    /// </summary>
    [MaxLength(100)]
    public string? DefaultModel { get; set; }

    /// <summary>
    /// Maximum tokens per request
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Rate limit per minute
    /// </summary>
    public int? RateLimitPerMinute { get; set; }

    /// <summary>
    /// Cost per token (in cents)
    /// </summary>
    public decimal? CostPerToken { get; set; }

    /// <summary>
    /// Configuration is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Configuration priority (higher number = higher priority)
    /// </summary>
    public int Priority { get; set; } = 1;

    /// <summary>
    /// Supported task types as JSON array
    /// </summary>
    public string? SupportedTaskTypes { get; set; }

    /// <summary>
    /// Quality score for this provider (0-100)
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// Average response time in milliseconds
    /// </summary>
    public long? AverageResponseTimeMs { get; set; }

    /// <summary>
    /// Success rate percentage
    /// </summary>
    public decimal? SuccessRate { get; set; }

    /// <summary>
    /// Last health check date
    /// </summary>
    public DateTime? LastHealthCheck { get; set; }

    /// <summary>
    /// Health check status
    /// </summary>
    public bool IsHealthy { get; set; } = true;

    /// <summary>
    /// Organization this configuration belongs to
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// AI tasks that used this provider configuration
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();

    /// <summary>
    /// Cost tracking records for this provider
    /// </summary>
    public virtual ICollection<CostTracking> CostTrackings { get; set; } = new List<CostTracking>();
}

