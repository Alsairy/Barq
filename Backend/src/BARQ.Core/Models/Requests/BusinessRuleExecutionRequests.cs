namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class BusinessRuleExecutionRequest
{
    /// <summary>
    /// </summary>
    public List<Guid> RuleIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ExecutionContext { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool StopOnFirstFailure { get; set; } = false;
    
    /// <summary>
    /// </summary>
    public string? ExecutionMode { get; set; }
}

/// <summary>
/// </summary>
public class BusinessRuleValidationRequest
{
    /// <summary>
    /// </summary>
    public Guid RuleId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ValidationContext { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? ValidationMode { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IncludePerformanceMetrics { get; set; } = false;
}
