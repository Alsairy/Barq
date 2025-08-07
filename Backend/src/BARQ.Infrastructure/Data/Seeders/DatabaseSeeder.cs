using BARQ.Core.Entities;
using BARQ.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BARQ.Infrastructure.Data.Seeders;

public class DatabaseSeeder
{
    private readonly BarqDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(BarqDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            if (!await _context.Organizations.AnyAsync())
            {
                await SeedDefaultOrganizationAndUserAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedDefaultOrganizationAndUserAsync()
    {
        var organizationId = Guid.NewGuid();
        var organization = new Organization
        {
            Id = organizationId,
            Name = "Default Organization",
            Description = "Default organization for initial setup",
            Domain = "default.local",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Organizations.Add(organization);

        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Administrator",
            Description = "System administrator with full access",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            TenantId = organizationId
        };

        _context.Set<Role>().Add(adminRole);

        var defaultUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@default.local",
            FirstName = "System",
            LastName = "Administrator",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Status = UserStatus.Active,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            TenantId = organizationId
        };

        _context.Users.Add(defaultUser);

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = defaultUser.Id,
            RoleId = adminRole.Id,
            RoleName = adminRole.Name,
            Description = "Administrator role assignment",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            TenantId = organizationId
        };

        _context.UserRoles.Add(userRole);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Default organization and admin user seeded successfully");
        _logger.LogInformation("Default admin credentials: admin@default.local / Admin123!");
    }
}
