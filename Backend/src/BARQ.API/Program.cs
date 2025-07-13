using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using StackExchange.Redis;
using MediatR;
using FluentValidation;
using AutoMapper;
using BARQ.Infrastructure.Data;
using BARQ.Infrastructure.MultiTenancy;
using BARQ.Infrastructure.Repositories;
using BARQ.Core.Services;
using BARQ.Application.Services.Users;
using BARQ.Application.Services.Authentication;
using BARQ.Application.Services.Organizations;
using BARQ.Application.Services.BusinessLogic;
using BARQ.Application.Services.Security;
using BARQ.Infrastructure.Security;

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
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BarqDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

// Configure security middleware pipeline in proper order
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<WafMiddleware>();
app.UseMiddleware<InputValidationMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("SecurePolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
