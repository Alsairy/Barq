using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IValidationPipelineService
{
    Task<ValidationPipelineResponse> ValidateBusinessOperationAsync(BusinessOperationValidationRequest request);
    Task<ValidationContextResponse> CreateValidationContextAsync(CreateValidationContextRequest request);
    Task<ValidationResultResponse> ExecuteValidationPipelineAsync(ValidationPipelineExecutionRequest request);
    Task<CustomValidationResponse> RegisterCustomValidatorAsync(RegisterCustomValidatorRequest request);
    Task<ValidationRuleResponse> AddValidationRuleAsync(AddValidationRuleRequest request);
    Task<IEnumerable<ValidationRuleDto>> GetValidationRulesAsync(string operationType);
    Task<ValidationPerformanceDto> GetValidationPerformanceMetricsAsync();
}
