using BARQ.Core.Models.Responses;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class MfaBackupCodesResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public IEnumerable<string> BackupCodes { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public int RemainingCodes { get; set; }
}
