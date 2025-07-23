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
    /// Gets or sets the unique identifier of the tenant in which the rules should be executed.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user requesting the rule execution.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether execution should stop on the first rule failure.
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
    /// Gets or sets the unique identifier of the business rule to validate.
    /// </summary>
    public Guid RuleId { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, object> ValidationContext { get; set; } = new();

    /// <summary>
    /// </summary>
    public string? ValidationMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include performance metrics in the validation result.
    /// </summary>
    public bool IncludePerformanceMetrics { get; set; } = false;
}
