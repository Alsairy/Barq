using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BARQ.Infrastructure.Security;

public class InputValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<InputValidationMiddleware> _logger;
    private readonly InputValidationConfiguration _config;

    public InputValidationMiddleware(
        RequestDelegate next, 
        ILogger<InputValidationMiddleware> logger,
        InputValidationConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_config.EnableInputValidation)
        {
            await _next(context);
            return;
        }

        if (await ValidateRequestAsync(context))
        {
            await _next(context);
        }
        else
        {
            await HandleInvalidInputAsync(context);
        }
    }

    private async Task<bool> ValidateRequestAsync(HttpContext context)
    {
        var request = context.Request;

        if (!ValidateHeaders(request))
        {
            _logger.LogWarning("Invalid headers detected from {IP}", GetClientIpAddress(context));
            return false;
        }

        if (!ValidateQueryParameters(request))
        {
            _logger.LogWarning("Invalid query parameters detected from {IP}", GetClientIpAddress(context));
            return false;
        }

        if (!await ValidateRequestBodyAsync(request))
        {
            _logger.LogWarning("Invalid request body detected from {IP}", GetClientIpAddress(context));
            return false;
        }

        if (!ValidateContentLength(request))
        {
            _logger.LogWarning("Request size exceeds limit from {IP}", GetClientIpAddress(context));
            return false;
        }

        return true;
    }

    private bool ValidateHeaders(HttpRequest request)
    {
        foreach (var header in request.Headers)
        {
            if (!IsValidHeaderName(header.Key))
                return false;

            foreach (var value in header.Value)
            {
                if (!IsValidHeaderValue(value))
                    return false;
            }
        }

        return true;
    }

    private bool ValidateQueryParameters(HttpRequest request)
    {
        foreach (var param in request.Query)
        {
            if (!IsValidParameterName(param.Key))
                return false;

            foreach (var value in param.Value)
            {
                if (!IsValidParameterValue(value))
                    return false;
            }
        }

        return true;
    }

    private async Task<bool> ValidateRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == 0)
            return true;

        if (request.HasFormContentType)
        {
            return ValidateFormData(request);
        }

        if (request.ContentType?.Contains("application/json") == true)
        {
            return await ValidateJsonBodyAsync(request);
        }

        return true;
    }

    private bool ValidateFormData(HttpRequest request)
    {
        if (request.Form == null)
            return true;

        foreach (var field in request.Form)
        {
            if (!IsValidParameterName(field.Key))
                return false;

            foreach (var value in field.Value)
            {
                if (!IsValidParameterValue(value))
                    return false;
            }
        }

        return true;
    }

    private async Task<bool> ValidateJsonBodyAsync(HttpRequest request)
    {
        if (request.ContentLength > _config.MaxJsonBodySize)
            return false;

        try
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            if (string.IsNullOrEmpty(body))
                return true;

            return IsValidJsonContent(body);
        }
        catch
        {
            return false;
        }
    }

    private bool ValidateContentLength(HttpRequest request)
    {
        if (request.ContentLength > _config.MaxRequestSize)
            return false;

        return true;
    }

    private bool IsValidHeaderName(string headerName)
    {
        if (string.IsNullOrEmpty(headerName))
            return false;

        if (headerName.Length > _config.MaxHeaderNameLength)
            return false;

        return Regex.IsMatch(headerName, @"^[a-zA-Z0-9\-_]+$");
    }

    private bool IsValidHeaderValue(string? headerValue)
    {
        if (headerValue == null)
            return true;

        if (headerValue.Length > _config.MaxHeaderValueLength)
            return false;

        return !ContainsMaliciousPatterns(headerValue);
    }

    private bool IsValidParameterName(string paramName)
    {
        if (string.IsNullOrEmpty(paramName))
            return false;

        if (paramName.Length > _config.MaxParameterNameLength)
            return false;

        return Regex.IsMatch(paramName, @"^[a-zA-Z0-9\-_\.]+$");
    }

    private bool IsValidParameterValue(string? paramValue)
    {
        if (paramValue == null)
            return true;

        if (paramValue.Length > _config.MaxParameterValueLength)
            return false;

        return !ContainsMaliciousPatterns(paramValue);
    }

    private bool IsValidJsonContent(string jsonContent)
    {
        if (jsonContent.Length > _config.MaxJsonBodySize)
            return false;

        return !ContainsMaliciousPatterns(jsonContent);
    }

    private bool ContainsMaliciousPatterns(string input)
    {
        var maliciousPatterns = new[]
        {
            @"<script[^>]*>.*?</script>",
            @"javascript:",
            @"vbscript:",
            @"data:text/html",
            @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|CREATE|ALTER|EXEC|EXECUTE)\b)",
            @"(\||&|;|\$\(|\`)",
            @"(\.\./|\.\.\\")",
            @"(%2e%2e%2f|%2e%2e%5c)",
            @"(eval\s*\(|setTimeout\s*\(|setInterval\s*\()",
            @"(document\.|window\.|location\.)",
            @"(\bon\w+\s*=)",
            @"(expression\s*\()",
            @"(@import|url\s*\()"
        };

        foreach (var pattern in maliciousPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }

    private async Task HandleInvalidInputAsync(HttpContext context)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = "Invalid input detected",
            message = "The request contains invalid or potentially malicious input.",
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }

    private string GetClientIpAddress(HttpContext context)
    {
        var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Connection.RemoteIpAddress?.ToString();
        }
        return ipAddress ?? "Unknown";
    }
}

public class InputValidationConfiguration
{
    public bool EnableInputValidation { get; set; } = true;
    public int MaxRequestSize { get; set; } = 10 * 1024 * 1024; // 10MB
    public int MaxJsonBodySize { get; set; } = 1024 * 1024; // 1MB
    public int MaxHeaderNameLength { get; set; } = 100;
    public int MaxHeaderValueLength { get; set; } = 4096;
    public int MaxParameterNameLength { get; set; } = 100;
    public int MaxParameterValueLength { get; set; } = 4096;
    public bool EnableStrictValidation { get; set; } = true;
    public bool LogInvalidRequests { get; set; } = true;
}
