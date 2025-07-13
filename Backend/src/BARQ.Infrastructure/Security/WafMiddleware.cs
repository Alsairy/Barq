using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BARQ.Infrastructure.Security;

public class WafMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<WafMiddleware> _logger;
    private readonly WafConfiguration _config;

    public WafMiddleware(RequestDelegate next, ILogger<WafMiddleware> logger, WafConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (await IsBlockedRequestAsync(context))
            {
                await HandleBlockedRequestAsync(context);
                return;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WAF middleware error for request {Path}", context.Request.Path);
            throw;
        }
    }

    private async Task<bool> IsBlockedRequestAsync(HttpContext context)
    {
        var request = context.Request;

        if (await CheckSqlInjectionAsync(request))
        {
            _logger.LogWarning("SQL injection attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckXssAttackAsync(request))
        {
            _logger.LogWarning("XSS attack attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckCommandInjectionAsync(request))
        {
            _logger.LogWarning("Command injection attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckPathTraversalAsync(request))
        {
            _logger.LogWarning("Path traversal attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckLdapInjectionAsync(request))
        {
            _logger.LogWarning("LDAP injection attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckXmlInjectionAsync(request))
        {
            _logger.LogWarning("XML injection attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        if (await CheckSsrfAttackAsync(request))
        {
            _logger.LogWarning("SSRF attack attempt detected from {IP} on {Path}", 
                GetClientIpAddress(context), request.Path);
            return true;
        }

        return false;
    }

    private async Task<bool> CheckSqlInjectionAsync(HttpRequest request)
    {
        var sqlPatterns = new[]
        {
            @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|CREATE|ALTER|EXEC|EXECUTE)\b)",
            @"(\b(UNION|OR|AND)\b.*\b(SELECT|INSERT|UPDATE|DELETE)\b)",
            @"('|(\\x27)|(\\x2D\\x2D)|(%27)|(%2D%2D))",
            @"(\b(WAITFOR|DELAY)\b)",
            @"(\b(CAST|CONVERT|CHAR|NCHAR)\b.*\()",
            @"(@@\w+)",
            @"(\bxp_\w+)",
            @"(\bsp_\w+)"
        };

        return await CheckPatternsAsync(request, sqlPatterns);
    }

    private async Task<bool> CheckXssAttackAsync(HttpRequest request)
    {
        var xssPatterns = new[]
        {
            @"<script[^>]*>.*?</script>",
            @"javascript:",
            @"vbscript:",
            @"onload\s*=",
            @"onerror\s*=",
            @"onclick\s*=",
            @"onmouseover\s*=",
            @"<iframe[^>]*>",
            @"<object[^>]*>",
            @"<embed[^>]*>",
            @"<link[^>]*>",
            @"<meta[^>]*>",
            @"expression\s*\(",
            @"url\s*\(",
            @"@import"
        };

        return await CheckPatternsAsync(request, xssPatterns);
    }

    private async Task<bool> CheckCommandInjectionAsync(HttpRequest request)
    {
        var commandPatterns = new[]
        {
            @"(\||&|;|\$\(|\`)",
            @"(\b(cat|ls|pwd|whoami|id|uname|ps|netstat|ifconfig|ping|nslookup|dig|wget|curl)\b)",
            @"(\b(cmd|powershell|bash|sh|zsh|csh|tcsh)\b)",
            @"(\.\./|\.\.\\)",
            @"(/etc/passwd|/etc/shadow|/etc/hosts)",
            @"(\b(rm|del|format|fdisk|mkfs)\b)",
            @"(\b(nc|netcat|telnet|ssh|ftp|tftp)\b)"
        };

        return await CheckPatternsAsync(request, commandPatterns);
    }

    private async Task<bool> CheckPathTraversalAsync(HttpRequest request)
    {
        var pathPatterns = new[]
        {
            @"(\.\./|\.\.\\)",
            @"(%2e%2e%2f|%2e%2e%5c)",
            @"(\.\.%2f|\.\.%5c)",
            @"(%252e%252e%252f|%252e%252e%255c)",
            @"(\\\.\\\.\\|/\.\./)",
            @"(\.\.\\\.\.\\)",
            @"(/etc/|/proc/|/sys/|/dev/)",
            @"(\\windows\\|\\system32\\)"
        };

        return await CheckPatternsAsync(request, pathPatterns);
    }

    private async Task<bool> CheckLdapInjectionAsync(HttpRequest request)
    {
        var ldapPatterns = new[]
        {
            @"(\*|\(|\)|&|\||!)",
            @"(\b(objectClass|cn|uid|mail|memberOf)\b.*[=<>])",
            @"(\(\w+=[^)]*\*[^)]*\))",
            @"(\(\|.*\))",
            @"(\(&.*\))",
            @"(\(!.*\))"
        };

        return await CheckPatternsAsync(request, ldapPatterns);
    }

    private async Task<bool> CheckXmlInjectionAsync(HttpRequest request)
    {
        var xmlPatterns = new[]
        {
            @"<!ENTITY",
            @"<!DOCTYPE",
            @"SYSTEM\s+[""'][^""']*[""']",
            @"PUBLIC\s+[""'][^""']*[""']",
            @"&\w+;",
            @"<!\[CDATA\[",
            @"]]>",
            @"<?xml.*encoding\s*=\s*[""'][^""']*[""']"
        };

        return await CheckPatternsAsync(request, xmlPatterns);
    }

    private async Task<bool> CheckSsrfAttackAsync(HttpRequest request)
    {
        var ssrfPatterns = new[]
        {
            @"(localhost|127\.0\.0\.1|0\.0\.0\.0)",
            @"(192\.168\.|10\.|172\.(1[6-9]|2[0-9]|3[01])\.)",
            @"(file://|ftp://|gopher://|dict://|ldap://)",
            @"(@.*:.*@)",
            @"(169\.254\.)",
            @"(::1|::ffff:)"
        };

        return await CheckPatternsAsync(request, ssrfPatterns);
    }

    private async Task<bool> CheckPatternsAsync(HttpRequest request, string[] patterns)
    {
        var content = await GetRequestContentAsync(request);
        
        foreach (var pattern in patterns)
        {
            if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<string> GetRequestContentAsync(HttpRequest request)
    {
        var content = string.Empty;

        content += request.Path.ToString();
        content += request.QueryString.ToString();

        foreach (var header in request.Headers)
        {
            content += $"{header.Key}:{string.Join(",", header.Value)}";
        }

        if (request.HasFormContentType && request.Form != null)
        {
            foreach (var form in request.Form)
            {
                content += $"{form.Key}:{string.Join(",", form.Value)}";
            }
        }

        if (request.ContentLength > 0 && request.ContentLength < 1024 * 1024) // Max 1MB
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            content += body;
        }

        return content;
    }

    private async Task HandleBlockedRequestAsync(HttpContext context)
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = "Request blocked by Web Application Firewall",
            message = "Your request contains potentially malicious content and has been blocked for security reasons.",
            timestamp = DateTime.UtcNow,
            requestId = context.TraceIdentifier
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

public class WafConfiguration
{
    public bool EnableSqlInjectionProtection { get; set; } = true;
    public bool EnableXssProtection { get; set; } = true;
    public bool EnableCommandInjectionProtection { get; set; } = true;
    public bool EnablePathTraversalProtection { get; set; } = true;
    public bool EnableLdapInjectionProtection { get; set; } = true;
    public bool EnableXmlInjectionProtection { get; set; } = true;
    public bool EnableSsrfProtection { get; set; } = true;
    public int MaxRequestSizeBytes { get; set; } = 1024 * 1024; // 1MB
    public bool LogBlockedRequests { get; set; } = true;
}
