using Microsoft.AspNetCore.Http;

namespace BARQ.Infrastructure.Security;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SecurityHeadersConfiguration _config;

    public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        AddSecurityHeaders(context);
        await _next(context);
    }

    private void AddSecurityHeaders(HttpContext context)
    {
        var response = context.Response;

        if (_config.EnableHsts)
        {
            response.Headers.Add("Strict-Transport-Security", 
                $"max-age={_config.HstsMaxAge}; includeSubDomains; preload");
        }

        if (_config.EnableContentSecurityPolicy)
        {
            response.Headers.Add("Content-Security-Policy", _config.ContentSecurityPolicy);
        }

        if (_config.EnableXFrameOptions)
        {
            response.Headers.Add("X-Frame-Options", _config.XFrameOptions);
        }

        if (_config.EnableXContentTypeOptions)
        {
            response.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        if (_config.EnableXssProtection)
        {
            response.Headers.Add("X-XSS-Protection", "1; mode=block");
        }

        if (_config.EnableReferrerPolicy)
        {
            response.Headers.Add("Referrer-Policy", _config.ReferrerPolicy);
        }

        if (_config.EnablePermissionsPolicy)
        {
            response.Headers.Add("Permissions-Policy", _config.PermissionsPolicy);
        }

        if (_config.EnableCrossOriginEmbedderPolicy)
        {
            response.Headers.Add("Cross-Origin-Embedder-Policy", "require-corp");
        }

        if (_config.EnableCrossOriginOpenerPolicy)
        {
            response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
        }

        if (_config.EnableCrossOriginResourcePolicy)
        {
            response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
        }

        if (_config.RemoveServerHeader)
        {
            response.Headers.Remove("Server");
        }

        if (_config.RemoveXPoweredByHeader)
        {
            response.Headers.Remove("X-Powered-By");
        }

        if (_config.EnableExpectCertificateTransparency)
        {
            response.Headers.Add("Expect-CT", 
                $"max-age={_config.ExpectCtMaxAge}, enforce, report-uri=\"{_config.ExpectCtReportUri}\"");
        }

        if (_config.EnablePublicKeyPinning && !string.IsNullOrEmpty(_config.PublicKeyPins))
        {
            response.Headers.Add("Public-Key-Pins", _config.PublicKeyPins);
        }
    }
}

public class SecurityHeadersConfiguration
{
    public bool EnableHsts { get; set; } = true;
    public int HstsMaxAge { get; set; } = 31536000; // 1 year

    public bool EnableContentSecurityPolicy { get; set; } = true;
    public string ContentSecurityPolicy { get; set; } = 
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: https:; " +
        "font-src 'self' https:; " +
        "connect-src 'self' https:; " +
        "media-src 'self'; " +
        "object-src 'none'; " +
        "child-src 'self'; " +
        "frame-ancestors 'none'; " +
        "form-action 'self'; " +
        "base-uri 'self'; " +
        "manifest-src 'self'";

    public bool EnableXFrameOptions { get; set; } = true;
    public string XFrameOptions { get; set; } = "DENY";

    public bool EnableXContentTypeOptions { get; set; } = true;
    public bool EnableXssProtection { get; set; } = true;

    public bool EnableReferrerPolicy { get; set; } = true;
    public string ReferrerPolicy { get; set; } = "strict-origin-when-cross-origin";

    public bool EnablePermissionsPolicy { get; set; } = true;
    public string PermissionsPolicy { get; set; } = 
        "accelerometer=(), " +
        "camera=(), " +
        "geolocation=(), " +
        "gyroscope=(), " +
        "magnetometer=(), " +
        "microphone=(), " +
        "payment=(), " +
        "usb=()";

    public bool EnableCrossOriginEmbedderPolicy { get; set; } = true;
    public bool EnableCrossOriginOpenerPolicy { get; set; } = true;
    public bool EnableCrossOriginResourcePolicy { get; set; } = true;

    public bool RemoveServerHeader { get; set; } = true;
    public bool RemoveXPoweredByHeader { get; set; } = true;

    public bool EnableExpectCertificateTransparency { get; set; } = true;
    public int ExpectCtMaxAge { get; set; } = 86400; // 24 hours
    public string ExpectCtReportUri { get; set; } = "/api/security/ct-report";

    public bool EnablePublicKeyPinning { get; set; } = false;
    public string PublicKeyPins { get; set; } = string.Empty;
}
