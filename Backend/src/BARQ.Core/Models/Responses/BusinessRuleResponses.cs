using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class BusinessRuleResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public BusinessRuleDto? Rule { get; set; }
}

/// <summary>
/// </summary>
public class BusinessRuleTestResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool TestPassed { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> TestResults { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> TestOutput { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
}

/// <summary>
/// </summary>
public class RuleExecutionResultDto
{
    /// <summary>
    /// </summary>
    public Guid RuleId { get; set; }
    
    /// <summary>
    /// </summary>
    public string RuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool ExecutionSuccessful { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RulePassed { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ExecutionOutput { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ExecutedAt { get; set; }
}
