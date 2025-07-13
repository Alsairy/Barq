namespace BARQ.Core.Models.Requests;

public class CreateBusinessRuleRequest
{
    public string RuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RuleExpression { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public int Priority { get; set; } = 100;
    public bool IsActive { get; set; } = true;
    public Guid TenantId { get; set; }
    public Dictionary<string, object>? Configuration { get; set; }
}

public class UpdateBusinessRuleRequest
{
    public Guid RuleId { get; set; }
    public string? RuleName { get; set; }
    public string? Description { get; set; }
    public string? RuleExpression { get; set; }
    public string? RuleType { get; set; }
    public int? Priority { get; set; }
    public bool? IsActive { get; set; }
    public Dictionary<string, object>? Configuration { get; set; }
}

public class TestBusinessRuleRequest
{
    public Guid RuleId { get; set; }
    public Dictionary<string, object> TestData { get; set; } = new();
    public string? TestScenario { get; set; }
}

public class ExecuteBusinessRuleRequest
{
    public List<Guid> RuleIds { get; set; } = new();
    public Dictionary<string, object> ExecutionContext { get; set; } = new();
    public bool StopOnFirstFailure { get; set; } = false;
}
