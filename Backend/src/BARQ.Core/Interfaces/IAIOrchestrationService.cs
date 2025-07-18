using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;

namespace BARQ.Core.Interfaces;

/// <summary>
/// Interface for AI orchestration service
/// </summary>
public interface IAIOrchestrationService
{
    /// <summary>
    /// Execute an AI task with the best available provider
    /// </summary>
    /// <param name="task">AI task to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task execution result</returns>
    Task<AITaskResult> ExecuteTaskAsync(AITask task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute multiple AI tasks in parallel
    /// </summary>
    /// <param name="tasks">AI tasks to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task execution results</returns>
    Task<IEnumerable<AITaskResult>> ExecuteTasksAsync(IEnumerable<AITask> tasks, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the best AI provider for a specific task type
    /// </summary>
    /// <param name="taskType">Type of AI task</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="requirements">Additional requirements</param>
    /// <returns>Best AI provider configuration</returns>
    Task<AIProviderConfiguration?> GetBestProviderAsync(AITaskType taskType, Guid tenantId, AITaskRequirements? requirements = null);

    /// <summary>
    /// Validate AI provider configuration
    /// </summary>
    /// <param name="configuration">Provider configuration to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<AIProviderValidationResult> ValidateProviderAsync(AIProviderConfiguration configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get AI task status
    /// </summary>
    /// <param name="taskId">Task ID</param>
    /// <returns>Task status</returns>
    Task<AITaskStatus> GetTaskStatusAsync(Guid taskId);

    /// <summary>
    /// Cancel an AI task
    /// </summary>
    /// <param name="taskId">Task ID to cancel</param>
    /// <returns>Cancellation result</returns>
    Task<bool> CancelTaskAsync(Guid taskId);

    /// <summary>
    /// Retry a failed AI task
    /// </summary>
    /// <param name="taskId">Task ID to retry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Retry result</returns>
    Task<AITaskResult> RetryTaskAsync(Guid taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get AI provider health status
    /// </summary>
    /// <param name="providerId">Provider configuration ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Health status</returns>
    Task<AIProviderHealthStatus> GetProviderHealthAsync(Guid providerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update AI provider metrics
    /// </summary>
    /// <param name="providerId">Provider configuration ID</param>
    /// <param name="metrics">Performance metrics</param>
    /// <returns>Update result</returns>
    Task UpdateProviderMetricsAsync(Guid providerId, AIProviderMetrics metrics);

    /// <summary>
    /// </summary>
    /// <param name="request">The request containing task details</param>
    /// <returns>The result of the AI task creation</returns>
    Task<AITaskResult> CreateAITaskAsync(CreateAITaskRequest request);
    
    /// <summary>
    /// Executes an AI task by its ID
    /// </summary>
    /// <param name="taskId">The ID of the task to execute</param>
    /// <returns>The result of the AI task execution</returns>
    Task<AITaskResult> ExecuteAITaskAsync(Guid taskId);
    
    /// <summary>
    /// Gets the status of an AI task by its ID
    /// </summary>
    /// <param name="taskId">The ID of the task to check</param>
    /// <returns>The current status of the AI task</returns>
    Task<AITaskStatus> GetAITaskStatusAsync(Guid taskId);
    /// <summary>
    /// Cancels an AI task by its ID.
    /// </summary>
    /// <param name="taskId">The ID of the task to cancel.</param>
    /// <returns>True if the task was successfully cancelled; otherwise, false.</returns>
    Task<bool> CancelAITaskAsync(Guid taskId);

    /// <summary>
    /// Gets the results of an AI task by its ID.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>The result of the AI task.</returns>
    Task<AITaskResult> GetAITaskResultsAsync(Guid taskId);

    /// <summary>
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <returns>A collection of AI tasks for the project.</returns>
    Task<IEnumerable<AITask>> GetProjectAITasksAsync(Guid projectId);

    /// <summary>
    /// </summary>
    /// <returns>Analytics data for AI tasks.</returns>
    Task<object> GetAITaskAnalyticsAsync();

    /// <summary>
    /// </summary>
    /// <returns>A collection of available AI provider configurations.</returns>
    Task<IEnumerable<AIProviderConfiguration>> GetAvailableProvidersAsync();

    /// <summary>
    /// </summary>
    /// <param name="providerId">The ID of the provider to check.</param>
    /// <returns>The health status of the AI provider.</returns>
    Task<AIProviderHealthStatus> CheckProviderHealthAsync(Guid providerId);

    /// <summary>
    /// </summary>
    /// <param name="request">The configuration request for the AI provider.</param>
    /// <returns>The configured AI provider.</returns>
    Task<AIProviderConfiguration> ConfigureProviderAsync(ConfigureAIProviderRequest request);

    /// <summary>
    /// Executes a batch of AI tasks.
    /// </summary>
    /// <param name="request">The batch execution request.</param>
    /// <returns>The result of the batch execution.</returns>
    Task<object> ExecuteBatchTasksAsync(ExecuteBatchAITasksRequest request);

    /// <summary>
    /// Gets the status of the AI task queue.
    /// </summary>
    /// <returns>The status of the AI task queue.</returns>
    Task<object> GetQueueStatusAsync();

    /// <summary>
    /// </summary>
    /// <returns>Cost analysis data for AI tasks.</returns>
    Task<object> GetCostAnalysisAsync();

}

/// <summary>
/// AI task execution result
/// </summary>
public class AITaskResult
{
    /// <summary>
    /// Task ID
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// Execution success status
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result data
    /// </summary>
    public string? ResultData { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public long ExecutionTimeMs { get; set; }

    /// <summary>
    /// Tokens used
    /// </summary>
    public int? TokensUsed { get; set; }

    /// <summary>
    /// Cost incurred
    /// </summary>
    public decimal? Cost { get; set; }

    /// <summary>
    /// Quality score
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// Provider used
    /// </summary>
    public AIProvider Provider { get; set; }

    /// <summary>
    /// Model used
    /// </summary>
    public string? Model { get; set; }
}

/// <summary>
/// AI task requirements
/// </summary>
public class AITaskRequirements
{
    /// <summary>
    /// Maximum cost allowed
    /// </summary>
    public decimal? MaxCost { get; set; }

    /// <summary>
    /// Maximum execution time in milliseconds
    /// </summary>
    public long? MaxExecutionTimeMs { get; set; }

    /// <summary>
    /// Minimum quality score required
    /// </summary>
    public int? MinQualityScore { get; set; }

    /// <summary>
    /// Preferred providers
    /// </summary>
    public List<AIProvider>? PreferredProviders { get; set; }

    /// <summary>
    /// Excluded providers
    /// </summary>
    public List<AIProvider>? ExcludedProviders { get; set; }

    /// <summary>
    /// Privacy requirements
    /// </summary>
    public bool RequiresLocalProcessing { get; set; } = false;

    /// <summary>
    /// Additional requirements
    /// </summary>
    public Dictionary<string, object>? AdditionalRequirements { get; set; }
}

/// <summary>
/// AI provider validation result
/// </summary>
public class AIProviderValidationResult
{
    /// <summary>
    /// Validation success status
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
    /// Provider capabilities
    /// </summary>
    public List<AITaskType> SupportedTaskTypes { get; set; } = new();

    /// <summary>
    /// Response time in milliseconds
    /// </summary>
    public long ResponseTimeMs { get; set; }
}

/// <summary>
/// AI provider health status
/// </summary>
public class AIProviderHealthStatus
{
    /// <summary>
    /// Provider is healthy
    /// </summary>
    public bool IsHealthy { get; set; }

    /// <summary>
    /// Health check timestamp
    /// </summary>
    public DateTime CheckedAt { get; set; }

    /// <summary>
    /// Response time in milliseconds
    /// </summary>
    public long ResponseTimeMs { get; set; }

    /// <summary>
    /// Error rate percentage
    /// </summary>
    public decimal ErrorRate { get; set; }

    /// <summary>
    /// Available capacity percentage
    /// </summary>
    public decimal AvailableCapacity { get; set; }

    /// <summary>
    /// Health issues
    /// </summary>
    public List<string> Issues { get; set; } = new();
}

/// <summary>
/// AI provider performance metrics
/// </summary>
public class AIProviderMetrics
{
    /// <summary>
    /// Average response time in milliseconds
    /// </summary>
    public long AverageResponseTimeMs { get; set; }

    /// <summary>
    /// Success rate percentage
    /// </summary>
    public decimal SuccessRate { get; set; }

    /// <summary>
    /// Average quality score
    /// </summary>
    public int AverageQualityScore { get; set; }

    /// <summary>
    /// Total requests processed
    /// </summary>
    public long TotalRequests { get; set; }

    /// <summary>
    /// Total cost incurred
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Metrics period start
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Metrics period end
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}

