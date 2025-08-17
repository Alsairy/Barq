using System;
using System.Linq;
using System.Threading.Tasks;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Application.Services.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BARQ.Infrastructure.Data.SeedData
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAsync(BarqDbContext context, PasswordService passwordService)
        {
            var defaultTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            await SeedRolesAsync(context, defaultTenantId);

            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@barq.com",
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = passwordService.HashPassword("TestPassword123!"),
                    Status = UserStatus.Active,
                    EmailConfirmed = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = defaultTenantId
                };

                var regularUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "user@barq.com",
                    FirstName = "Regular",
                    LastName = "User",
                    PasswordHash = passwordService.HashPassword("TestPassword123!"),
                    Status = UserStatus.Active,
                    EmailConfirmed = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = defaultTenantId
                };

                var testUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@acme.com",
                    FirstName = "Test",
                    LastName = "User",
                    PasswordHash = passwordService.HashPassword("TestPassword123!"),
                    Status = UserStatus.Active,
                    EmailConfirmed = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = defaultTenantId
                };

                await context.Users.AddRangeAsync(adminUser, regularUser, testUser);
                await context.SaveChangesAsync();

                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                if (adminRole != null && userRole != null)
                {
                    await context.UserRoles.AddRangeAsync(
                        new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id, TenantId = defaultTenantId },
                        new UserRole { UserId = regularUser.Id, RoleId = userRole.Id, TenantId = defaultTenantId },
                        new UserRole { UserId = testUser.Id, RoleId = userRole.Id, TenantId = defaultTenantId }
                    );
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedRolesAsync(BarqDbContext context, Guid tenantId)
        {
            if (!await context.Roles.AnyAsync())
            {
                var adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Description = "Administrator role with full system access",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = tenantId
                };

                var userRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    Description = "Standard user role with basic access",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = tenantId
                };

                await context.Roles.AddRangeAsync(adminRole, userRole);
                await context.SaveChangesAsync();
            }
        }
    }
}
