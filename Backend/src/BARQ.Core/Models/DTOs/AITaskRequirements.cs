using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class AITaskRequirements
{
    /// <summary>
    /// </summary>
    public long? MaxResponseTime { get; set; }

    /// <summary>
    /// </summary>
    public List<string> RequiredCapabilities { get; set; } = new();

    /// <summary>
    /// </summary>
    public decimal? MaxCost { get; set; }

    /// <summary>
    /// Minimum quality score required
    /// </summary>
    public int? MinQualityScore { get; set; }

    /// <summary>
    /// </summary>
    public AIProvider? PreferredProvider { get; set; }

    /// <summary>
    /// </summary>
    public string? ModelRequirements { get; set; }
}
