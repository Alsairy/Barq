using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class UserSearchResponse : BaseResponse
{
    public IEnumerable<UserProfileDto> Users { get; set; } = new List<UserProfileDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
