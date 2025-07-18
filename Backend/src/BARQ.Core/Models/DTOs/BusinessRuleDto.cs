namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class BusinessRuleDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// </summary>
    public string RuleExpression { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Context { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastExecutedAt { get; set; }

    /// <summary>
    /// </summary>
    public int ExecutionCount { get; set; }
}
