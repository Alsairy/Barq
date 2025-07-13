using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class ValidationPipelineResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public List<ValidationResultDto> ValidationResults { get; set; } = new();
    public Dictionary<string, object> ValidationContext { get; set; } = new();
}

public class ValidationContextResponse : BaseResponse
{
    public Guid ValidationContextId { get; set; }
    public string ContextType { get; set; } = string.Empty;
    public Dictionary<string, object> ContextData { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class ValidationResultResponse : BaseResponse
{
    public List<ValidationResultDto> Results { get; set; } = new();
    public bool AllValidationsPassed { get; set; }
    public TimeSpan ExecutionTime { get; set; }
}

public class CustomValidationResponse : BaseResponse
{
    public string ValidatorName { get; set; } = string.Empty;
    public bool IsRegistered { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}

public class ValidationRuleResponse : BaseResponse
{
    public ValidationRuleDto? Rule { get; set; }
}

public class ValidationResultDto
{
    public string RuleName { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public object? AttemptedValue { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

public class ValidationRuleDto
{
    public Guid Id { get; set; }
    public string RuleName { get; set; } = string.Empty;
    public string RuleExpression { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ValidationPerformanceDto
{
    public string RuleName { get; set; } = string.Empty;
    public TimeSpan ExecutionTime { get; set; }
    public int ExecutionCount { get; set; }
    public TimeSpan AverageExecutionTime { get; set; }
    public DateTime LastExecuted { get; set; }
}
