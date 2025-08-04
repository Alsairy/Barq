using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IAdminConfigurationService
{
    Task<AdminConfigurationResponse> CreateCodeGenerationConfigAsync(CreateCodeGenerationConfigRequest request);
    Task<AdminConfigurationResponse> UpdateCodeGenerationConfigAsync(UpdateCodeGenerationConfigRequest request);
    Task<AdminConfigurationResponse> GetCodeGenerationConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetCodeGenerationConfigsAsync();
    Task<AdminConfigurationResponse> DeleteCodeGenerationConfigAsync(Guid id);

    Task<AdminConfigurationResponse> CreateBRDTemplateConfigAsync(CreateBRDTemplateConfigRequest request);
    Task<AdminConfigurationResponse> UpdateBRDTemplateConfigAsync(UpdateBRDTemplateConfigRequest request);
    Task<AdminConfigurationResponse> GetBRDTemplateConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetBRDTemplateConfigsAsync();
    Task<AdminConfigurationResponse> DeleteBRDTemplateConfigAsync(Guid id);

    Task<AdminConfigurationResponse> CreateProposalConfigAsync(CreateProposalConfigRequest request);
    Task<AdminConfigurationResponse> UpdateProposalConfigAsync(UpdateProposalConfigRequest request);
    Task<AdminConfigurationResponse> GetProposalConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetProposalConfigsAsync();
    Task<AdminConfigurationResponse> DeleteProposalConfigAsync(Guid id);

    Task<AdminConfigurationResponse> CreatePresentationConfigAsync(CreatePresentationConfigRequest request);
    Task<AdminConfigurationResponse> UpdatePresentationConfigAsync(UpdatePresentationConfigRequest request);
    Task<AdminConfigurationResponse> GetPresentationConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetPresentationConfigsAsync();
    Task<AdminConfigurationResponse> DeletePresentationConfigAsync(Guid id);

    Task<AdminConfigurationResponse> CreateDesignConfigAsync(CreateDesignConfigRequest request);
    Task<AdminConfigurationResponse> UpdateDesignConfigAsync(UpdateDesignConfigRequest request);
    Task<AdminConfigurationResponse> GetDesignConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetDesignConfigsAsync();
    Task<AdminConfigurationResponse> DeleteDesignConfigAsync(Guid id);

    Task<AdminConfigurationResponse> CreateTestingConfigAsync(CreateTestingConfigRequest request);
    Task<AdminConfigurationResponse> UpdateTestingConfigAsync(UpdateTestingConfigRequest request);
    Task<AdminConfigurationResponse> GetTestingConfigAsync(Guid id);
    Task<AdminConfigurationListResponse> GetTestingConfigsAsync();
    Task<AdminConfigurationResponse> DeleteTestingConfigAsync(Guid id);

    Task<AdminConfigurationResponse> UpdateConstraintRulesAsync(UpdateConstraintRulesRequest request);
    Task<TemplateManagementResponse> CreateTemplateAsync(CreateTemplateRequest request);
    Task<TemplateManagementResponse> UpdateTemplateAsync(UpdateTemplateRequest request);
    Task<TemplateManagementListResponse> GetTemplatesAsync(string category);
    Task<AdminConfigurationResponse> DeleteTemplateAsync(Guid id);
}
