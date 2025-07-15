using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents cost tracking for AI services and platform usage
/// </summary>
public class CostTracking : TenantEntity
{
    /// <summary>
    /// Cost entry name/description
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Cost category
    /// </summary>
    public CostCategory Category { get; set; }

    /// <summary>
    /// Cost type
    /// </summary>
    public CostType CostType { get; set; }

    /// <summary>
    /// Cost amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency code (ISO 4217)
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Cost date
    /// </summary>
    public DateTime CostDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Billing period start
    /// </summary>
    public DateTime? BillingPeriodStart { get; set; }

    /// <summary>
    /// Billing period end
    /// </summary>
    public DateTime? BillingPeriodEnd { get; set; }

    /// <summary>
    /// AI provider (if applicable)
    /// </summary>
    public AIProvider? AIProvider { get; set; }

    /// <summary>
    /// AI model used (if applicable)
    /// </summary>
    [MaxLength(100)]
    public string? AIModel { get; set; }

    /// <summary>
    /// Tokens used (if applicable)
    /// </summary>
    public int? TokensUsed { get; set; }

    /// <summary>
    /// Cost per token
    /// </summary>
    public decimal? CostPerToken { get; set; }

    /// <summary>
    /// Processing time in milliseconds
    /// </summary>
    public long? ProcessingTimeMs { get; set; }

    /// <summary>
    /// Resource usage details as JSON
    /// </summary>
    public string? ResourceUsage { get; set; }

    /// <summary>
    /// Cost allocation method
    /// </summary>
    public CostAllocationMethod AllocationMethod { get; set; } = CostAllocationMethod.Direct;

    /// <summary>
    /// Allocation percentage (for shared costs)
    /// </summary>
    public decimal? AllocationPercentage { get; set; }

    /// <summary>
    /// External invoice ID
    /// </summary>
    [MaxLength(100)]
    public string? ExternalInvoiceId { get; set; }

    /// <summary>
    /// External transaction ID
    /// </summary>
    [MaxLength(100)]
    public string? ExternalTransactionId { get; set; }

    /// <summary>
    /// Cost is approved
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// Approved by user
    /// </summary>
    public Guid? ApprovedById { get; set; }
    public virtual User? ApprovedBy { get; set; }

    /// <summary>
    /// Approval date
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Cost is billable to client
    /// </summary>
    public bool IsBillable { get; set; } = false;

    /// <summary>
    /// Client billing rate
    /// </summary>
    public decimal? BillingRate { get; set; }

    /// <summary>
    /// Billable amount
    /// </summary>
    public decimal? BillableAmount { get; set; }

    /// <summary>
    /// Cost tags as JSON array
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Additional metadata as JSON
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Project this cost belongs to
    /// </summary>
    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Sprint this cost belongs to
    /// </summary>
    public Guid? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// User story this cost is related to
    /// </summary>
    public Guid? UserStoryId { get; set; }
    /// <summary>
    /// User story this cost is related to
    /// </summary>
    public virtual UserStory? UserStory { get; set; }

    /// <summary>
    /// AI task this cost is related to
    /// </summary>
    public Guid? AITaskId { get; set; }
    /// <summary>
    /// AI task this cost is related to
    /// </summary>
    public virtual AITask? AITask { get; set; }

    /// <summary>
    /// AI provider configuration used
    /// </summary>
    public Guid? AIProviderConfigurationId { get; set; }
    /// <summary>
    /// AI provider configuration used
    /// </summary>
    public virtual AIProviderConfiguration? AIProviderConfiguration { get; set; }

    /// <summary>
    /// User who incurred the cost
    /// </summary>
    public Guid? UserId { get; set; }
    /// <summary>
    /// User who incurred the cost
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Team that incurred the cost
    /// </summary>
    [MaxLength(200)]
    public string? Team { get; set; }

    /// <summary>
    /// Department that incurred the cost
    /// </summary>
    [MaxLength(200)]
    public string? Department { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(100)]
    public string? CostCenter { get; set; }
}

/// <summary>
/// Represents aggregated cost summary for reporting
/// </summary>
public class CostSummary : TenantEntity
{
    /// <summary>
    /// Summary name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Summary period start
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Summary period end
    /// </summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>
    /// Summary type
    /// </summary>
    public CostSummaryType SummaryType { get; set; }

    /// <summary>
    /// Total cost
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Cost breakdown by category as JSON
    /// </summary>
    public string? CategoryBreakdown { get; set; }

    /// <summary>
    /// Cost breakdown by provider as JSON
    /// </summary>
    public string? ProviderBreakdown { get; set; }

    /// <summary>
    /// Cost breakdown by project as JSON
    /// </summary>
    public string? ProjectBreakdown { get; set; }

    /// <summary>
    /// Cost breakdown by team as JSON
    /// </summary>
    public string? TeamBreakdown { get; set; }

    /// <summary>
    /// Number of transactions
    /// </summary>
    public int TransactionCount { get; set; }

    /// <summary>
    /// Average cost per transaction
    /// </summary>
    public decimal AverageCostPerTransaction { get; set; }

    /// <summary>
    /// Total tokens used
    /// </summary>
    public long? TotalTokensUsed { get; set; }

    /// <summary>
    /// Average cost per token
    /// </summary>
    public decimal? AverageCostPerToken { get; set; }

    /// <summary>
    /// Total processing time in milliseconds
    /// </summary>
    public long? TotalProcessingTimeMs { get; set; }

    /// <summary>
    /// Cost variance from budget
    /// </summary>
    public decimal? BudgetVariance { get; set; }

    /// <summary>
    /// Cost variance percentage
    /// </summary>
    public decimal? BudgetVariancePercentage { get; set; }

    /// <summary>
    /// Previous period cost for comparison
    /// </summary>
    public decimal? PreviousPeriodCost { get; set; }

    /// <summary>
    /// Cost change from previous period
    /// </summary>
    public decimal? PeriodChange { get; set; }

    /// <summary>
    /// Cost change percentage
    /// </summary>
    public decimal? PeriodChangePercentage { get; set; }

    /// <summary>
    /// Summary metadata as JSON
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Project this summary belongs to (if applicable)
    /// </summary>
    public Guid? ProjectId { get; set; }
    /// <summary>
    /// Project this summary belongs to (if applicable)
    /// </summary>
    public virtual Project? Project { get; set; }
}

