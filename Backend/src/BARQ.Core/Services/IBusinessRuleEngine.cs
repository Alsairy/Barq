using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IBusinessRuleEngine
{
    Task<BusinessRuleExecutionResponse> ExecuteRulesAsync(BusinessRuleExecutionRequest request);
    Task<BusinessRuleValidationResponse> ValidateRuleAsync(BusinessRuleValidationRequest request);
    Task<BusinessRuleDto> CreateRuleAsync(CreateBusinessRuleRequest request);
    Task<BusinessRuleDto> UpdateRuleAsync(UpdateBusinessRuleRequest request);
    Task<BusinessRuleResponse> DeleteRuleAsync(Guid ruleId);
    Task<IEnumerable<BusinessRuleDto>> GetRulesAsync(string context);
    Task<BusinessRuleTestResponse> TestRuleAsync(TestBusinessRuleRequest request);
    Task<RuleExecutionResultDto> GetRuleExecutionHistoryAsync(Guid ruleId, int page = 1, int pageSize = 20);
}
