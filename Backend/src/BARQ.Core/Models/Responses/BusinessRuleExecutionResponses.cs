using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class BusinessRuleExecutionResponse : BaseResponse
{
    public List<RuleExecutionResultDto> ExecutionResults { get; set; } = new();
    public bool AllRulesPassed { get; set; }
    public TimeSpan TotalExecutionTime { get; set; }
    public Dictionary<string, object> ExecutionSummary { get; set; } = new();
}

public class BusinessRuleValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
    public Dictionary<string, object> ValidationMetrics { get; set; } = new();
    public TimeSpan ValidationTime { get; set; }
}
