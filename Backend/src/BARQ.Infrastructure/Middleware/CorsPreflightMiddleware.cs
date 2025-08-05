using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BARQ.Infrastructure.Middleware
{
    public class CorsPreflightMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsPreflightMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"].FirstOrDefault();
            if (!string.IsNullOrEmpty(origin))
            {
                if (origin == "https://barq-application-plu4nmbz.devinapps.com")
                {
                    context.Response.Headers["Access-Control-Allow-Origin"] = origin;
                    context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                    context.Response.Headers["Vary"] = "Origin";
                }
            }

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, PATCH, OPTIONS";
                context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, X-Requested-With, X-Tenant-ID, X-Correlation-ID, DNT, User-Agent, If-Modified-Since, Cache-Control, Range";
                context.Response.Headers["Access-Control-Max-Age"] = "86400";
                
                context.Response.StatusCode = 200;
                return;
            }

            await _next(context);
        }
    }
}
