using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using System.Reflection;
using Serilog;
using StackExchange.Redis;
using MediatR;
using FluentValidation;
using AutoMapper;
using BARQ.Infrastructure.Data;
using BARQ.Infrastructure.MultiTenancy;
using BARQ.Infrastructure.Repositories;
using BARQ.Core.Repositories;
using BARQ.Core.Services;
using BARQ.Core.Interfaces;
using BARQ.Application.Services.Users;
using BARQ.Application.Services.Authentication;
using BARQ.Application.Services.Organizations;
using BARQ.Application.Services.BusinessLogic;
using BARQ.Application.Services.Security;
using BARQ.Application.Services.Projects;
using BARQ.Application.Services.AI;
using BARQ.Application.Services.Workflows;
using BARQ.Infrastructure.Security;
using BARQ.Infrastructure.HealthChecks;
using BARQ.Infrastructure.Middleware;
using BARQ.Infrastructure.Analytics;
using BARQ.Infrastructure.Caching;
using BARQ.Infrastructure.Performance;
using BARQ.Infrastructure.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "BARQ")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console()
    .WriteTo.File("logs/barq-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.File("logs/security/security-.txt", 
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {CorrelationId} {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BARQ API",
        Description = "A comprehensive project management and AI orchestration platform API",
        Contact = new OpenApiContact
        {
            Name = "BARQ Support",
            Email = "support@barq.app",
            Url = new Uri("https://barq.app/support")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    // options.EnableAnnotations(); // Commented out - requires Swashbuckle.AspNetCore.Annotations package
    options.UseInlineDefinitionsForEnums();
    options.DescribeAllParametersInCamelCase();
    
    options.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    options.DocInclusionPredicate((name, api) => true);
    
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

builder.Services.AddDbContext<BarqDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<BARQ.Core.Repositories.IRepository<BARQ.Core.Entities.AuditLog>, GenericRepository<BARQ.Core.Entities.AuditLog>>();
builder.Services.AddScoped<BARQ.Core.Repositories.IRepository<BARQ.Core.Entities.User>, GenericRepository<BARQ.Core.Entities.User>>();
builder.Services.AddScoped<BARQ.Core.Repositories.IRepository<BARQ.Core.Entities.Organization>, GenericRepository<BARQ.Core.Entities.Organization>>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BARQ.Application.Commands.Users.RegisterUserCommand).Assembly));

builder.Services.AddAutoMapper(typeof(BARQ.Application.Profiles.UserProfile).Assembly);

builder.Services.AddValidatorsFromAssembly(typeof(BARQ.Application.Validators.RegisterUserCommandValidator).Assembly);

builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IMultiFactorAuthService, MultiFactorAuthService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IUserInvitationService, UserInvitationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ITenantContextService, TenantContextService>();

builder.Services.AddScoped<IBusinessRuleEngine, BusinessRuleEngine>();
builder.Services.AddScoped<IValidationPipelineService, ValidationPipelineService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAIOrchestrationService, AIOrchestrationService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IKeyManagementService, KeyManagementService>();
builder.Services.AddScoped<ISecurityMonitoringService, SecurityMonitoringService>();
builder.Services.AddScoped<IThreatDetectionService, ThreatDetectionService>();
builder.Services.AddScoped<ISiemIntegrationService, SiemIntegrationService>();
builder.Services.AddScoped<TdeConfiguration>();

builder.Services.AddScoped<IComplianceService, ComplianceService>();
builder.Services.AddScoped<IGdprComplianceService, GdprComplianceService>();
builder.Services.AddScoped<IHipaaComplianceService, HipaaComplianceService>();
builder.Services.AddScoped<ISoxComplianceService, SoxComplianceService>();

builder.Services.AddScoped<DatabaseHealthCheck>();
builder.Services.AddScoped<RedisHealthCheck>();
builder.Services.AddScoped<SecurityHealthCheck>();
builder.Services.AddScoped<AIProvidersHealthCheck>();

builder.Services.AddScoped<IApiAnalyticsService, ApiAnalyticsService>();

builder.Services.Configure<CachingOptions>(builder.Configuration.GetSection("Caching"));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICachingService, CachingService>();

builder.Services.Configure<DatabasePerformanceOptions>(builder.Configuration.GetSection("DatabasePerformance"));
builder.Services.AddScoped<IDatabasePerformanceService, DatabasePerformanceService>();

builder.Services.Configure<BackgroundJobOptions>(builder.Configuration.GetSection("BackgroundJobs"));
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

builder.Services.AddSingleton<WafConfiguration>();
builder.Services.AddSingleton<SecurityHeadersConfiguration>();
builder.Services.AddSingleton<RateLimitConfiguration>();
builder.Services.AddSingleton<InputValidationConfiguration>();

builder.Services.AddHttpClient<ISiemIntegrationService, SiemIntegrationService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "BARQ-Security-Monitor/1.0");
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "default-secret-key-for-development-only")),
            ClockSkew = TimeSpan.FromMinutes(5),
            RequireExpirationTime = true,
            ValidateActor = false,
            ValidateTokenReplay = false
        };
        
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.IncludeErrorDetails = builder.Environment.IsDevelopment();
    });

builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "application/xml",
        "text/xml",
        "text/json"
    });
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("API is running"), tags: new[] { "ready", "live" })
    .AddCheck<DatabaseHealthCheck>("database_context")
    .AddCheck<RedisHealthCheck>("redis_connection", tags: new[] { "ready" })
    .AddCheck<DatabaseHealthCheck>("database_detailed", tags: new[] { "ready", "startup" })
    .AddCheck<RedisHealthCheck>("redis_detailed", tags: new[] { "ready" })
    .AddCheck<SecurityHealthCheck>("security_monitoring", tags: new[] { "ready" })
    .AddCheck<AIProvidersHealthCheck>("ai_providers", tags: new[] { "ready" });

builder.Services.AddCors(options =>
{
    options.AddPolicy("SecurePolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:5173", "https://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()
                  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        }
        else
        {
            policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "https://barq.app" })
                  .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
                  .WithHeaders("Content-Type", "Authorization", "X-Requested-With", "X-Tenant-ID", "X-Correlation-ID")
                  .AllowCredentials()
                  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        await DatabaseSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

// Configure security middleware pipeline in proper order
app.UseMiddleware<SecurityHeadersMiddleware>();
// app.UseMiddleware<WafMiddleware>();
// app.UseMiddleware<InputValidationMiddleware>();
// app.UseMiddleware<RateLimitingMiddleware>();

app.UseApiMonitoring();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BARQ API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "BARQ API Documentation";
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
        options.EnableValidator();
    });
}
else
{
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BARQ API v1");
        options.RoutePrefix = "api-docs";
        options.DocumentTitle = "BARQ API Documentation";
    });
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseCors("SecurePolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                duration = x.Value.Duration.TotalMilliseconds,
                description = x.Value.Description
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.MapHealthChecks("/health/startup", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("startup")
});

app.Run();
