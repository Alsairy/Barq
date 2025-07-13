namespace BARQ.Core.Models.Requests;

public class BusinessOperationValidationRequest
{
    public string OperationType { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public Dictionary<string, object> OperationData { get; set; } = new();
    public List<string> ValidationRules { get; set; } = new();
}

public class CreateValidationContextRequest
{
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public string ContextType { get; set; } = string.Empty;
    public Dictionary<string, object> ContextData { get; set; } = new();
}

public class ValidationPipelineExecutionRequest
{
    public Guid ValidationContextId { get; set; }
    public List<string> ValidationSteps { get; set; } = new();
    public bool StopOnFirstFailure { get; set; } = false;
    public Dictionary<string, object> ExecutionParameters { get; set; } = new();
}

public class RegisterCustomValidatorRequest
{
    public string ValidatorName { get; set; } = string.Empty;
    public string ValidatorType { get; set; } = string.Empty;
    public Dictionary<string, object> Configuration { get; set; } = new();
    public int Priority { get; set; } = 100;
}

public class AddValidationRuleRequest
{
    public string RuleName { get; set; } = string.Empty;
    public string RuleExpression { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int Priority { get; set; } = 100;
    public bool IsActive { get; set; } = true;
}
