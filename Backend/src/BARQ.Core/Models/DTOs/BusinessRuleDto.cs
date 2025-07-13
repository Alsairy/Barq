namespace BARQ.Core.Models.DTOs;

public class BusinessRuleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string RuleExpression { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastExecutedAt { get; set; }
    public int ExecutionCount { get; set; }
}
