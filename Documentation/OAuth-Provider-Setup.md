# OAuth Provider Setup Guide

This guide provides step-by-step instructions for setting up OAuth applications with each supported provider in the BARQ platform.

## Overview

BARQ supports OAuth 2.0 and OpenID Connect authentication with the following providers:
- Azure Active Directory (Azure AD)
- Google Cloud Platform
- Microsoft Identity Platform
- GitHub

Each provider requires creating an OAuth application and configuring the appropriate callback URLs and scopes.

## Callback URL Pattern

All OAuth providers must be configured with the following callback URL pattern:
```
{FRONTEND_URL}/auth/callback
```

For example:
- Development: `http://localhost:3000/auth/callback`
- Production: `https://your-domain.com/auth/callback`

## Azure Active Directory Setup

### 1. Create App Registration

1. Navigate to the [Azure Portal](https://portal.azure.com)
2. Go to **Azure Active Directory** > **App registrations**
3. Click **New registration**
4. Fill in the application details:
   - **Name**: BARQ Enterprise Platform
   - **Supported account types**: Accounts in any organizational directory (Any Azure AD directory - Multitenant)
   - **Redirect URI**: Web - `{FRONTEND_URL}/auth/callback`
5. Click **Register**

### 2. Configure Authentication

1. In your app registration, go to **Authentication**
2. Under **Redirect URIs**, ensure your callback URL is listed
3. Under **Implicit grant and hybrid flows**, enable:
   - Access tokens (used for implicit flows)
   - ID tokens (used for implicit and hybrid flows)
4. Click **Save**

### 3. Create Client Secret

1. Go to **Certificates & secrets**
2. Click **New client secret**
3. Add a description and set expiration
4. Click **Add**
5. **Important**: Copy the secret value immediately (it won't be shown again)

### 4. Configure API Permissions

1. Go to **API permissions**
2. Click **Add a permission**
3. Select **Microsoft Graph**
4. Choose **Delegated permissions**
5. Add the following permissions:
   - `openid`
   - `profile`
   - `email`
   - `User.Read`
6. Click **Grant admin consent** (if you have admin privileges)

### 5. Configuration Values

After setup, you'll need these values for BARQ configuration:
- **Client ID**: Found in the app registration overview
- **Client Secret**: The secret you created
- **Authority**: `https://login.microsoftonline.com/common/v2.0`
- **Scopes**: `openid profile email User.Read`

## Google Cloud Platform Setup

### 1. Create OAuth 2.0 Client

1. Navigate to the [Google Cloud Console](https://console.cloud.google.com)
2. Select or create a project
3. Go to **APIs & Services** > **Credentials**
4. Click **Create Credentials** > **OAuth 2.0 Client IDs**
5. If prompted, configure the OAuth consent screen first
6. Select **Web application** as the application type
7. Add your callback URL to **Authorized redirect URIs**:
   - `{FRONTEND_URL}/auth/callback`
8. Click **Create**

### 2. Configure OAuth Consent Screen

1. Go to **APIs & Services** > **OAuth consent screen**
2. Choose **External** user type (unless you're using Google Workspace)
3. Fill in the required information:
   - **App name**: BARQ Enterprise Platform
   - **User support email**: Your support email
   - **Developer contact information**: Your contact email
4. Add scopes:
   - `openid`
   - `profile`
   - `email`
5. Save and continue through the remaining steps

### 3. Configuration Values

After setup, you'll need these values:
- **Client ID**: The OAuth 2.0 client ID (ends with `.apps.googleusercontent.com`)
- **Client Secret**: The OAuth 2.0 client secret
- **Authority**: `https://accounts.google.com`
- **Scopes**: `openid profile email`

## Microsoft Identity Platform Setup

### 1. Register Application

1. Navigate to the [Microsoft App Registration Portal](https://portal.azure.com/#blade/Microsoft_AAD_RegisteredApps/ApplicationsListBlade)
2. Click **New registration**
3. Fill in the details:
   - **Name**: BARQ Enterprise Platform
   - **Supported account types**: Accounts in any organizational directory and personal Microsoft accounts
   - **Redirect URI**: Web - `{FRONTEND_URL}/auth/callback`
4. Click **Register**

### 2. Configure Authentication

1. Go to **Authentication** in your app registration
2. Under **Platform configurations**, ensure your web redirect URI is configured
3. Under **Implicit grant and hybrid flows**, enable:
   - Access tokens
   - ID tokens
4. Click **Save**

### 3. Create Application Secret

1. Go to **Certificates & secrets**
2. Under **Client secrets**, click **New client secret**
3. Add a description and expiration period
4. Click **Add**
5. Copy the secret value immediately

### 4. API Permissions

1. Go to **API permissions**
2. Ensure the following Microsoft Graph permissions are granted:
   - `openid`
   - `profile`
   - `email`
   - `User.Read`

### 5. Configuration Values

- **Client ID**: Application (client) ID from the overview page
- **Client Secret**: The secret you created
- **Authority**: `https://login.microsoftonline.com/common/v2.0`
- **Scopes**: `openid profile email User.Read`

## GitHub OAuth App Setup

### 1. Create OAuth App

1. Navigate to [GitHub Developer Settings](https://github.com/settings/developers)
2. Click **OAuth Apps** > **New OAuth App**
3. Fill in the application details:
   - **Application name**: BARQ Enterprise Platform
   - **Homepage URL**: Your application's homepage
   - **Application description**: Enterprise automation platform
   - **Authorization callback URL**: `{FRONTEND_URL}/auth/callback`
4. Click **Register application**

### 2. Generate Client Secret

1. In your OAuth app settings, click **Generate a new client secret**
2. Copy the client secret immediately (it won't be shown again)

### 3. Configuration Values

- **Client ID**: Shown in the OAuth app settings
- **Client Secret**: The secret you generated
- **Authority**: `https://github.com/login/oauth/authorize`
- **Scopes**: `user:email read:user`

## BARQ Configuration

### Backend Configuration

Update your `appsettings.json` or environment variables with the OAuth provider configurations:

```json
{
  "OAuth": {
    "AzureAD": {
      "ClientId": "your-azure-ad-client-id",
      "ClientSecret": "your-azure-ad-client-secret",
      "Authority": "https://login.microsoftonline.com/common/v2.0",
      "Scopes": "openid profile email User.Read"
    },
    "Google": {
      "ClientId": "your-google-client-id.apps.googleusercontent.com",
      "ClientSecret": "your-google-client-secret",
      "Authority": "https://accounts.google.com",
      "Scopes": "openid profile email"
    },
    "Microsoft": {
      "ClientId": "your-microsoft-client-id",
      "ClientSecret": "your-microsoft-client-secret",
      "Authority": "https://login.microsoftonline.com/common/v2.0",
      "Scopes": "openid profile email User.Read"
    },
    "GitHub": {
      "ClientId": "your-github-client-id",
      "ClientSecret": "your-github-client-secret",
      "Authority": "https://github.com/login/oauth/authorize",
      "Scopes": "user:email read:user"
    }
  }
}
```

### Database Configuration

The OAuth provider configurations are stored in the `SsoConfiguration` table. You can update them through the admin interface or directly in the database:

```sql
UPDATE SsoConfigurations 
SET ClientId = 'your-actual-client-id',
    ClientSecret = 'your-actual-client-secret'
WHERE Provider = 'AzureAD';
```

## Security Considerations

### Client Secret Management

- **Never commit client secrets to version control**
- Use environment variables or secure configuration management
- Rotate client secrets regularly
- Use different credentials for development and production

### Redirect URI Validation

- Always use HTTPS in production
- Validate redirect URIs strictly
- Use exact matches for callback URLs
- Avoid wildcard redirect URIs

### Scope Minimization

- Request only the minimum required scopes
- Review and audit requested permissions regularly
- Document why each scope is necessary

## Troubleshooting

### Common Issues

1. **Invalid Redirect URI**
   - Ensure the callback URL exactly matches what's configured in the provider
   - Check for trailing slashes or protocol mismatches

2. **Invalid Client Credentials**
   - Verify client ID and secret are correct
   - Check if client secret has expired

3. **Insufficient Permissions**
   - Ensure all required scopes are granted
   - Check if admin consent is required

4. **CORS Issues**
   - Configure CORS properly in your backend
   - Ensure the frontend domain is whitelisted

### Testing OAuth Flow

1. Use the provider's OAuth playground or testing tools
2. Test with different user accounts and permission levels
3. Verify token validation and user information retrieval
4. Test error scenarios (denied permissions, expired tokens)

## Support

For additional support with OAuth provider setup:
- Azure AD: [Microsoft Identity Platform Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)
- Google: [Google Identity Platform Documentation](https://developers.google.com/identity)
- GitHub: [GitHub OAuth Documentation](https://docs.github.com/en/developers/apps/building-oauth-apps)
