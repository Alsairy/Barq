namespace BARQ.Core.Models.Requests;

public class BusinessRuleExecutionRequest
{
    public List<Guid> RuleIds { get; set; } = new();
    public Dictionary<string, object> ExecutionContext { get; set; } = new();
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public bool StopOnFirstFailure { get; set; } = false;
    public string? ExecutionMode { get; set; }
}

public class BusinessRuleValidationRequest
{
    public Guid RuleId { get; set; }
    public Dictionary<string, object> ValidationContext { get; set; } = new();
    public string? ValidationMode { get; set; }
    public bool IncludePerformanceMetrics { get; set; } = false;
}
