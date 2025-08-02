using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserSearchResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public IEnumerable<UserProfileDto> Users { get; set; } = new List<UserProfileDto>();
    
    /// <summary>
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// </summary>
    public int PageNumber { get; set; }
    
    /// <summary>
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// </summary>
    public bool HasNextPage { get; set; }
    
    /// <summary>
    /// </summary>
    public bool HasPreviousPage { get; set; }
}
