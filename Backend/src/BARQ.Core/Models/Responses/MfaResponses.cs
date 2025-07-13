using BARQ.Core.Models.Responses;

namespace BARQ.Core.Models.Responses;

public class MfaBackupCodesResponse : BaseResponse
{
    public IEnumerable<string> BackupCodes { get; set; } = new List<string>();
    public DateTime GeneratedAt { get; set; }
    public int RemainingCodes { get; set; }
}
