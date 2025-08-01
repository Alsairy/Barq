namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class CreateBusinessRuleRequest
{
    /// <summary>
    /// </summary>
    public string RuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RuleExpression { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RuleType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Priority { get; set; } = 100;
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? Configuration { get; set; }
}

/// <summary>
/// </summary>
public class UpdateBusinessRuleRequest
{
    /// <summary>
    /// </summary>
    public Guid RuleId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RuleName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RuleExpression { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RuleType { get; set; }
    
    /// <summary>
    /// </summary>
    public int? Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? Configuration { get; set; }
}

/// <summary>
/// </summary>
public class TestBusinessRuleRequest
{
    /// <summary>
    /// </summary>
    public Guid RuleId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> TestData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? TestScenario { get; set; }
}

/// <summary>
/// </summary>
public class ExecuteBusinessRuleRequest
{
    /// <summary>
    /// </summary>
    public List<Guid> RuleIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ExecutionContext { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool StopOnFirstFailure { get; set; } = false;
}
