using Microsoft.EntityFrameworkCore;
using BARQ.Core.Entities;
using BARQ.Infrastructure.Data;

namespace BARQ.Infrastructure.Data.SeedData;

public static class OAuthProviderSeeder
{
    public static async Task SeedOAuthProvidersAsync(BarqDbContext context)
    {
        if (await context.SsoConfigurations.AnyAsync())
        {
            return;
        }

        // Use the existing Test Organization
        var defaultTenantId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        var oauthProviders = new List<SsoConfiguration>
        {
            new SsoConfiguration
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Provider = "AzureAD",
                ProviderName = "Azure Active Directory",
                IsEnabled = true,
                IsRequired = false,
                ClientId = "your-azure-ad-client-id",
                ClientSecret = "your-azure-ad-client-secret",
                Authority = "https://login.microsoftonline.com/common/v2.0",
                CallbackUrl = "https://barq-application-plu4nmbz.devinapps.com/auth/callback/azuread",
                Scopes = "openid profile email",
                AttributeMappings = "{\"email\":\"email\",\"firstName\":\"given_name\",\"lastName\":\"family_name\",\"displayName\":\"name\"}",
                DefaultRole = "User",
                AutoProvisionUsers = true,
                IsValid = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfigurationJson = "{}"
            },
            new SsoConfiguration
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Provider = "Google",
                ProviderName = "Google",
                IsEnabled = true,
                IsRequired = false,
                ClientId = "your-google-client-id.apps.googleusercontent.com",
                ClientSecret = "your-google-client-secret",
                Authority = "https://accounts.google.com",
                CallbackUrl = "https://barq-application-plu4nmbz.devinapps.com/auth/callback/google",
                Scopes = "openid profile email",
                AttributeMappings = "{\"email\":\"email\",\"firstName\":\"given_name\",\"lastName\":\"family_name\",\"displayName\":\"name\",\"picture\":\"picture\"}",
                DefaultRole = "User",
                AutoProvisionUsers = true,
                IsValid = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfigurationJson = "{}"
            },
            new SsoConfiguration
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Provider = "Microsoft",
                ProviderName = "Microsoft",
                IsEnabled = true,
                IsRequired = false,
                ClientId = "your-microsoft-client-id",
                ClientSecret = "your-microsoft-client-secret",
                Authority = "https://login.microsoftonline.com/common/v2.0",
                CallbackUrl = "https://barq-application-plu4nmbz.devinapps.com/auth/callback/microsoft",
                Scopes = "openid profile email User.Read",
                AttributeMappings = "{\"email\":\"mail\",\"firstName\":\"givenName\",\"lastName\":\"surname\",\"displayName\":\"displayName\"}",
                DefaultRole = "User",
                AutoProvisionUsers = true,
                IsValid = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfigurationJson = "{}"
            },
            new SsoConfiguration
            {
                Id = Guid.NewGuid(),
                TenantId = defaultTenantId,
                Provider = "GitHub",
                ProviderName = "GitHub",
                IsEnabled = true,
                IsRequired = false,
                ClientId = "your-github-client-id",
                ClientSecret = "your-github-client-secret",
                Authority = "https://github.com/login/oauth/authorize",
                SsoUrl = "https://github.com/login/oauth/authorize",
                CallbackUrl = "https://barq-application-plu4nmbz.devinapps.com/auth/callback/github",
                Scopes = "user:email read:user",
                AttributeMappings = "{\"email\":\"email\",\"firstName\":\"name\",\"lastName\":\"\",\"displayName\":\"login\",\"avatar\":\"avatar_url\"}",
                DefaultRole = "User",
                AutoProvisionUsers = true,
                IsValid = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfigurationJson = "{}"
            }
        };

        await context.SsoConfigurations.AddRangeAsync(oauthProviders);
        await context.SaveChangesAsync();
    }
}
