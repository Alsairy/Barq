using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class BusinessRuleExecutionResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public List<RuleExecutionResultDto> ExecutionResults { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool AllRulesPassed { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan TotalExecutionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ExecutionSummary { get; set; } = new();
}

/// <summary>
/// </summary>
public class BusinessRuleValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationMessages { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ValidationMetrics { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public TimeSpan ValidationTime { get; set; }
}
