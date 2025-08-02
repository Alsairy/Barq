using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IBusinessRuleEngine
{
    /// <summary>
    /// </summary>
    Task<BusinessRuleExecutionResponse> ExecuteRulesAsync(BusinessRuleExecutionRequest request);
    
    /// <summary>
    /// </summary>
    Task<BusinessRuleValidationResponse> ValidateRuleAsync(BusinessRuleValidationRequest request);
    
    /// <summary>
    /// </summary>
    Task<BusinessRuleDto> CreateRuleAsync(CreateBusinessRuleRequest request);
    
    /// <summary>
    /// </summary>
    Task<BusinessRuleDto> UpdateRuleAsync(UpdateBusinessRuleRequest request);
    
    /// <summary>
    /// </summary>
    Task<BusinessRuleResponse> DeleteRuleAsync(Guid ruleId);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<BusinessRuleDto>> GetRulesAsync(string context);
    
    /// <summary>
    /// </summary>
    Task<BusinessRuleTestResponse> TestRuleAsync(TestBusinessRuleRequest request);
    
    /// <summary>
    /// </summary>
    Task<RuleExecutionResultDto> GetRuleExecutionHistoryAsync(Guid ruleId, int page = 1, int pageSize = 20);
}
