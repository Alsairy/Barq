using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class ValidationPipelineResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public List<ValidationResultDto> ValidationResults { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ValidationContext { get; set; } = new();
}

/// <summary>
/// </summary>
public class ValidationContextResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Guid ValidationContextId { get; set; }
    
    /// <summary>
    /// </summary>
    public string ContextType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ContextData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// </summary>
public class ValidationResultResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public List<ValidationResultDto> Results { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool AllValidationsPassed { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
}

/// <summary>
/// </summary>
public class CustomValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string ValidatorName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsRegistered { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
}

/// <summary>
/// </summary>
public class ValidationRuleResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ValidationRuleDto? Rule { get; set; }
}

/// <summary>
/// </summary>
public class ValidationResultDto
{
    /// <summary>
    /// </summary>
    public string RuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public object? AttemptedValue { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// </summary>
public class ValidationRuleDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string RuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RuleExpression { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// </summary>
public class ValidationPerformanceDto
{
    /// <summary>
    /// </summary>
    public string RuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public int ExecutionCount { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageExecutionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastExecuted { get; set; }
}
