namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class BusinessOperationValidationRequest
{
    /// <summary>
    /// </summary>
    public string OperationType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> OperationData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> ValidationRules { get; set; } = new();
}

/// <summary>
/// </summary>
public class CreateValidationContextRequest
{
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string ContextType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ContextData { get; set; } = new();
}

/// <summary>
/// </summary>
public class ValidationPipelineExecutionRequest
{
    /// <summary>
    /// </summary>
    public Guid ValidationContextId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationSteps { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool StopOnFirstFailure { get; set; } = false;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> ExecutionParameters { get; set; } = new();
}

/// <summary>
/// </summary>
public class RegisterCustomValidatorRequest
{
    /// <summary>
    /// </summary>
    public string ValidatorName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ValidatorType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public int Priority { get; set; } = 100;
}

/// <summary>
/// </summary>
public class AddValidationRuleRequest
{
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
    public int Priority { get; set; } = 100;
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
}
