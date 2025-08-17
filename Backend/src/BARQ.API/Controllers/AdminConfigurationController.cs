using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.API.Controllers;

[ApiController]
[Route("api/admin/configuration")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminConfigurationController : ControllerBase
{
    private readonly IAdminConfigurationService _adminConfigService;
    private readonly ILogger<AdminConfigurationController> _logger;

    public AdminConfigurationController(
        IAdminConfigurationService adminConfigService,
        ILogger<AdminConfigurationController> logger)
    {
        _adminConfigService = adminConfigService;
        _logger = logger;
    }

    [HttpPost("code-generation")]
    public async Task<IActionResult> CreateCodeGenerationConfig([FromBody] CreateCodeGenerationConfigRequest request)
    {
        try
        {
            _logger.LogInformation("Creating code generation configuration: {Name}", request.Name);
            
            var response = await _adminConfigService.CreateCodeGenerationConfigAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating code generation configuration");
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("code-generation/{id}")]
    public async Task<IActionResult> UpdateCodeGenerationConfig(Guid id, [FromBody] UpdateCodeGenerationConfigRequest request)
    {
        try
        {
            request.Id = id;
            _logger.LogInformation("Updating code generation configuration: {Id}", id);
            
            var response = await _adminConfigService.UpdateCodeGenerationConfigAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating code generation configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("code-generation/{id}")]
    public async Task<IActionResult> GetCodeGenerationConfig(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting code generation configuration: {Id}", id);
            
            var response = await _adminConfigService.GetCodeGenerationConfigAsync(id);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting code generation configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("code-generation")]
    public async Task<IActionResult> GetCodeGenerationConfigs()
    {
        try
        {
            _logger.LogInformation("Getting all code generation configurations");
            
            var response = await _adminConfigService.GetCodeGenerationConfigsAsync();
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting code generation configurations");
            return StatusCode(500, new AdminConfigurationListResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("code-generation/{id}")]
    public async Task<IActionResult> DeleteCodeGenerationConfig(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting code generation configuration: {Id}", id);
            
            var response = await _adminConfigService.DeleteCodeGenerationConfigAsync(id);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting code generation configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("brd-template")]
    public async Task<IActionResult> CreateBRDTemplateConfig([FromBody] CreateBRDTemplateConfigRequest request)
    {
        try
        {
            _logger.LogInformation("Creating BRD template configuration: {Name}", request.Name);
            
            var response = await _adminConfigService.CreateBRDTemplateConfigAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating BRD template configuration");
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("brd-template/{id}")]
    public async Task<IActionResult> UpdateBRDTemplateConfig(Guid id, [FromBody] UpdateBRDTemplateConfigRequest request)
    {
        try
        {
            request.Id = id;
            _logger.LogInformation("Updating BRD template configuration: {Id}", id);
            
            var response = await _adminConfigService.UpdateBRDTemplateConfigAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating BRD template configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("brd-template/{id}")]
    public async Task<IActionResult> GetBRDTemplateConfig(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting BRD template configuration: {Id}", id);
            
            var response = await _adminConfigService.GetBRDTemplateConfigAsync(id);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting BRD template configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("brd-template")]
    public async Task<IActionResult> GetBRDTemplateConfigs()
    {
        try
        {
            _logger.LogInformation("Getting all BRD template configurations");
            
            var response = await _adminConfigService.GetBRDTemplateConfigsAsync();
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting BRD template configurations");
            return StatusCode(500, new AdminConfigurationListResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("brd-template/{id}")]
    public async Task<IActionResult> DeleteBRDTemplateConfig(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting BRD template configuration: {Id}", id);
            
            var response = await _adminConfigService.DeleteBRDTemplateConfigAsync(id);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting BRD template configuration: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates([FromQuery] string category)
    {
        try
        {
            _logger.LogInformation("Getting templates for category: {Category}", category);
            
            var response = await _adminConfigService.GetTemplatesAsync(category);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting templates for category: {Category}", category);
            return StatusCode(500, new TemplateManagementListResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("templates")]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequest request)
    {
        try
        {
            _logger.LogInformation("Creating template: {Name}", request.Name);
            
            var response = await _adminConfigService.CreateTemplateAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating template");
            return StatusCode(500, new TemplateManagementResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("templates/{id}")]
    public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateTemplateRequest request)
    {
        try
        {
            request.Id = id;
            _logger.LogInformation("Updating template: {Id}", id);
            
            var response = await _adminConfigService.UpdateTemplateAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating template: {Id}", id);
            return StatusCode(500, new TemplateManagementResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("templates/{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting template: {Id}", id);
            
            var response = await _adminConfigService.DeleteTemplateAsync(id);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting template: {Id}", id);
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("constraints")]
    public async Task<IActionResult> UpdateConstraints([FromBody] UpdateConstraintRulesRequest request)
    {
        try
        {
            _logger.LogInformation("Updating constraint rules for configuration: {ConfigurationId}", request.ConfigurationId);
            
            var response = await _adminConfigService.UpdateConstraintRulesAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating constraint rules");
            return StatusCode(500, new AdminConfigurationResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("proposal")]
    public async Task<IActionResult> CreateProposalConfig([FromBody] CreateProposalConfigRequest request)
    {
        var response = await _adminConfigService.CreateProposalConfigAsync(request);
        return Ok(response);
    }

    [HttpPost("presentation")]
    public async Task<IActionResult> CreatePresentationConfig([FromBody] CreatePresentationConfigRequest request)
    {
        var response = await _adminConfigService.CreatePresentationConfigAsync(request);
        return Ok(response);
    }

    [HttpPost("design")]
    public async Task<IActionResult> CreateDesignConfig([FromBody] CreateDesignConfigRequest request)
    {
        var response = await _adminConfigService.CreateDesignConfigAsync(request);
        return Ok(response);
    }

    [HttpPost("testing")]
    public async Task<IActionResult> CreateTestingConfig([FromBody] CreateTestingConfigRequest request)
    {
        var response = await _adminConfigService.CreateTestingConfigAsync(request);
        return Ok(response);
    }
}
