using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IWorkflowEngine
{
    /// <summary>
    /// Executes a workflow instance asynchronously
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Workflow execution result</returns>
    Task<WorkflowExecutionResult> ExecuteWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a specific workflow step asynchronously
    /// </summary>
    /// <param name="stepExecutionId">The ID of the step execution to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Step execution result</returns>
    Task<WorkflowStepExecutionResult> ExecuteStepAsync(Guid stepExecutionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance to pause</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the workflow was successfully paused</returns>
    Task<bool> PauseWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance to resume</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the workflow was successfully resumed</returns>
    Task<bool> ResumeWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance to stop</param>
    /// <param name="reason">Optional reason for stopping the workflow</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the workflow was successfully stopped</returns>
    Task<bool> StopWorkflowAsync(Guid workflowInstanceId, string? reason = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="stepExecutionId">The ID of the step execution to retry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Step execution result</returns>
    Task<WorkflowStepExecutionResult> RetryStepAsync(Guid stepExecutionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance</param>
    /// <returns>Workflow execution status</returns>
    Task<WorkflowExecutionStatus> GetExecutionStatusAsync(Guid workflowInstanceId);

    /// <summary>
    /// </summary>
    /// <param name="stepExecutionId">The ID of the step execution</param>
    /// <returns>Step execution status</returns>
    Task<WorkflowStepExecutionStatus> GetStepExecutionStatusAsync(Guid stepExecutionId);

    /// <summary>
    /// Validates a workflow template
    /// </summary>
    /// <param name="workflowTemplateId">The ID of the workflow template to validate</param>
    /// <returns>Validation result</returns>
    Task<WorkflowValidationResult> ValidateWorkflowAsync(Guid workflowTemplateId);

    /// <summary>
    /// </summary>
    /// <param name="workflowInstanceId">The ID of the workflow instance</param>
    /// <returns>Workflow execution metrics</returns>
    Task<WorkflowExecutionMetrics> GetExecutionMetricsAsync(Guid workflowInstanceId);
}

/// <summary>
/// </summary>
public class WorkflowStepExecutionResult
{
    /// <summary>
    /// Execution success status
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public WorkflowStepStatus Status { get; set; }

    /// <summary>
    /// </summary>
    public object? OutputData { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// </summary>
    public long? DurationMs { get; set; }

    /// <summary>
    /// </summary>
    public Guid? NextStepId { get; set; }

    /// <summary>
    /// </summary>
    public bool ShouldContinue { get; set; } = true;
}

/// <summary>
/// </summary>
public class WorkflowExecutionStatus
{
    /// <summary>
    /// Workflow instance ID
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }

    /// <summary>
    /// Current status
    /// </summary>
    public WorkflowStatus Status { get; set; }

    /// <summary>
    /// </summary>
    public Guid? CurrentStepId { get; set; }

    /// <summary>
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// </summary>
    public int TotalSteps { get; set; }

    /// <summary>
    /// Completed steps
    /// </summary>
    public int CompletedSteps { get; set; }

    /// <summary>
    /// </summary>
    public int FailedSteps { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? EstimatedCompletionAt { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowStepExecutionStatus
{
    /// <summary>
    /// </summary>
    public Guid StepExecutionId { get; set; }

    /// <summary>
    /// Current status
    /// </summary>
    public WorkflowStepStatus Status { get; set; }

    /// <summary>
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? EstimatedCompletionAt { get; set; }

    /// <summary>
    /// </summary>
    public int RetryCount { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowValidationResult
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Validation warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// </summary>
    public List<string> Suggestions { get; set; } = new();
}

/// <summary>
/// </summary>
public class WorkflowExecutionMetrics
{
    /// <summary>
    /// </summary>
    public long TotalExecutionTimeMs { get; set; }

    /// <summary>
    /// </summary>
    public long AverageStepExecutionTimeMs { get; set; }

    /// <summary>
    /// </summary>
    public int TotalRetries { get; set; }

    /// <summary>
    /// Success rate percentage
    /// </summary>
    public decimal SuccessRate { get; set; }

    /// <summary>
    /// </summary>
    public List<string> Bottlenecks { get; set; } = new();

    /// <summary>
    /// </summary>
    public Dictionary<string, object> ResourceMetrics { get; set; } = new();
}
