using Microsoft.EntityFrameworkCore;
using BARQ.Core.Entities;
using BARQ.Core.Enums;

namespace BARQ.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(BarqDbContext context)
    {
        if (await context.Organizations.AnyAsync())
        {
            return; // Database already seeded
        }

        var tenant1Id = Guid.Parse("476da891-1280-468e-a785-391243584bd4");
        var tenant2Id = Guid.Parse("91b85b71-f5ec-4f68-9a6e-29cd0e80f679");

        var org1 = new Organization
        {
            Id = tenant1Id,
            Name = "Acme Corporation",
            DisplayName = "Acme Corporation",
            Description = "Leading technology company",
            Website = "https://acme.com",
            SubscriptionPlan = SubscriptionPlan.Enterprise,
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Settings = "{\"theme\":\"dark\",\"notifications\":true}",
            MaxUsers = 100,
            MaxProjects = 50
        };

        var org2 = new Organization
        {
            Id = tenant2Id,
            Name = "Beta Industries",
            DisplayName = "Beta Industries",
            Description = "Innovative manufacturing company",
            Website = "https://beta.com",
            SubscriptionPlan = SubscriptionPlan.Professional,
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Settings = "{\"theme\":\"light\",\"notifications\":false}",
            MaxUsers = 25,
            MaxProjects = 10
        };

        context.Organizations.AddRange(org1, org2);
        await context.SaveChangesAsync(); // Save organizations first

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@acme.com",
            FirstName = "John",
            LastName = "Doe",
            TenantId = tenant1Id,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = "hashed_password_123",
            EmailConfirmed = true,
            JobTitle = "Administrator",
            Department = "IT"
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@acme.com",
            FirstName = "Jane",
            LastName = "Smith",
            TenantId = tenant1Id,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = "hashed_password_456",
            EmailConfirmed = true,
            JobTitle = "Project Manager",
            Department = "Operations"
        };

        var user3 = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@beta.com",
            FirstName = "Bob",
            LastName = "Johnson",
            TenantId = tenant2Id,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = "hashed_password_789",
            EmailConfirmed = true,
            JobTitle = "Administrator",
            Department = "IT"
        };

        var user4 = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@beta.com",
            FirstName = "Alice",
            LastName = "Wilson",
            TenantId = tenant2Id,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = "hashed_password_abc",
            EmailConfirmed = true,
            JobTitle = "Developer",
            Department = "Engineering"
        };

        context.Users.AddRange(user1, user2, user3, user4);
        await context.SaveChangesAsync(); // Save users before creating projects

        var project1 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Acme Project Alpha",
            Description = "First project for Acme Corp",
            TenantId = tenant1Id,
            ProjectOwnerId = user1.Id,
            Status = ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Budget = 100000m,
            ProgressPercentage = 25.5m
        };

        var project2 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Beta Project One",
            Description = "First project for Beta Industries",
            TenantId = tenant2Id,
            ProjectOwnerId = user3.Id,
            Status = ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Budget = 75000m,
            ProgressPercentage = 40.0m
        };

        context.Projects.AddRange(project1, project2);
        await context.SaveChangesAsync(); // Save projects and users
    }
}
