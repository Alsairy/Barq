using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace BARQ.Infrastructure.Middleware
{
    public class CorsPreflightMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _allowedOrigins = new[] { "https://barq-application-plu4nmbz.devinapps.com" };

        public CorsPreflightMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"].FirstOrDefault();
            
            if (!string.IsNullOrEmpty(origin) && _allowedOrigins.Contains(origin))
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = origin;
                context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                context.Response.Headers["Vary"] = "Origin";
                
                context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, PATCH, OPTIONS";
                context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, X-Requested-With, X-Tenant-ID, X-Correlation-ID, X-JWT-Token, DNT, User-Agent, If-Modified-Since, Cache-Control, Range";
                context.Response.Headers["Access-Control-Max-Age"] = "86400";
            }

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }

            await _next(context);
        }
    }
}
