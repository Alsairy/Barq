using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;
using BARQ.Core.Services;
using MediatR;
using FluentValidation;
using AutoMapper;
using BARQ.Application.Services.Authentication;
using BARQ.Application.Services.Users;
using BARQ.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BARQ.Testing.Framework;

public class TestStartup
{
    public TestStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(BARQ.API.Controllers.AuthController).Assembly);
        
        services.AddDbContext<BarqDbContext>(options =>
        {
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddScoped<ITenantProvider, TestTenantProvider>();
        services.AddScoped<ITestDataSeeder, TestDataSeeder>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BARQ.Application.Commands.Users.RegisterUserCommand).Assembly));
        
        services.AddAutoMapper(typeof(BARQ.Application.Profiles.UserProfile).Assembly);
        
        services.AddValidatorsFromAssembly(typeof(BARQ.Application.Validators.RegisterUserCommandValidator).Assembly);
        
        services.AddScoped<BARQ.Core.Repositories.IUnitOfWork, BARQ.Infrastructure.Repositories.UnitOfWork>();
        services.AddScoped(typeof(BARQ.Core.Repositories.IRepository<>), typeof(BARQ.Infrastructure.Repositories.GenericRepository<>));
        
        services.AddScoped<BARQ.Core.Services.IAuthenticationService, BARQ.Application.Services.Authentication.AuthenticationService>();
        services.AddScoped<BARQ.Core.Services.IPasswordService, BARQ.Application.Services.Authentication.PasswordService>();
        services.AddScoped<BARQ.Core.Services.IMultiFactorAuthService, BARQ.Application.Services.Authentication.MultiFactorAuthService>();
        services.AddScoped<BARQ.Core.Services.IUserRoleService, BARQ.Application.Services.Users.UserRoleService>();
        services.AddScoped<BARQ.Core.Services.IUserRegistrationService, BARQ.Application.Services.Users.UserRegistrationService>();
        services.AddScoped<BARQ.Core.Services.IUserProfileService, BARQ.Application.Services.Users.UserProfileService>();
        services.AddScoped<BARQ.Core.Services.INotificationService, BARQ.Application.Services.BusinessLogic.NotificationService>();

        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_TEST_SECRET") ?? "default-test-key-that-should-be-overridden-in-environment");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/api/health");
        });
    }
}
