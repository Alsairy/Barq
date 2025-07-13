using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class BusinessRuleResponse : BaseResponse
{
    public BusinessRuleDto? Rule { get; set; }
}

public class BusinessRuleTestResponse : BaseResponse
{
    public bool TestPassed { get; set; }
    public List<string> TestResults { get; set; } = new();
    public Dictionary<string, object> TestOutput { get; set; } = new();
    public TimeSpan ExecutionTime { get; set; }
}

public class RuleExecutionResultDto
{
    public Guid RuleId { get; set; }
    public string RuleName { get; set; } = string.Empty;
    public bool ExecutionSuccessful { get; set; }
    public bool RulePassed { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object> ExecutionOutput { get; set; } = new();
    public TimeSpan ExecutionTime { get; set; }
    public DateTime ExecutedAt { get; set; }
}
