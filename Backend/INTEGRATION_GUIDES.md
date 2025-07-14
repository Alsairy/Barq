# External System Integration Guides

## Overview
This guide provides step-by-step instructions for integrating external systems with the BARQ platform using the new Integration Gateway framework.

## Integration Gateway Setup

### 1. Register External Endpoint

First, register your external system endpoint with the Integration Gateway:

```http
POST /api/integration/endpoints
Content-Type: application/json
Authorization: Bearer {your_jwt_token}
X-Tenant-ID: {your_tenant_id}

{
  "id": "external-crm-system",
  "name": "Customer CRM Integration",
  "baseUrl": "https://api.yourcrm.com",
  "protocol": "REST",
  "authentication": {
    "type": "ApiKey",
    "apiKey": "your-api-key",
    "headerName": "X-API-Key"
  },
  "healthCheckEndpoint": "/health",
  "timeout": 30000,
  "retryPolicy": {
    "maxRetries": 3,
    "backoffStrategy": "exponential"
  }
}
```

### 2. Test Endpoint Health

Verify your endpoint is properly configured:

```http
GET /api/integration/health/external-crm-system
Authorization: Bearer {your_jwt_token}
X-Tenant-ID: {your_tenant_id}
```

Expected response:
```json
{
  "endpointId": "external-crm-system",
  "status": "Healthy",
  "responseTime": 150,
  "lastChecked": "2025-07-14T15:25:00Z"
}
```

## Protocol-Specific Integration

### REST API Integration

#### Basic REST Call
```http
POST /api/integration/gateway/route
Content-Type: application/json
Authorization: Bearer {your_jwt_token}
X-Tenant-ID: {your_tenant_id}

{
  "endpointId": "external-crm-system",
  "method": "POST",
  "path": "/customers",
  "headers": {
    "Content-Type": "application/json"
  },
  "body": {
    "name": "John Doe",
    "email": "john@example.com"
  }
}
```

#### Advanced REST Configuration
```json
{
  "endpointId": "external-api",
  "protocol": "REST",
  "baseUrl": "https://api.external.com",
  "authentication": {
    "type": "OAuth2",
    "clientId": "your-client-id",
    "clientSecret": "your-client-secret",
    "tokenEndpoint": "https://auth.external.com/token",
    "scope": "read write"
  },
  "defaultHeaders": {
    "Accept": "application/json",
    "User-Agent": "BARQ-Integration/1.0"
  }
}
```

### SOAP Integration

#### SOAP Endpoint Registration
```json
{
  "id": "legacy-soap-service",
  "name": "Legacy SOAP Service",
  "baseUrl": "https://legacy.company.com/soap",
  "protocol": "SOAP",
  "wsdlUrl": "https://legacy.company.com/soap?wsdl",
  "authentication": {
    "type": "Basic",
    "username": "service-user",
    "password": "service-password"
  },
  "soapAction": "http://legacy.company.com/GetCustomer"
}
```

#### SOAP Request Example
```http
POST /api/integration/gateway/route
Content-Type: application/json

{
  "endpointId": "legacy-soap-service",
  "method": "POST",
  "soapAction": "GetCustomer",
  "body": {
    "customerId": "12345"
  }
}
```

### GraphQL Integration

#### GraphQL Endpoint Setup
```json
{
  "id": "graphql-api",
  "name": "GraphQL API",
  "baseUrl": "https://api.graphql.com/graphql",
  "protocol": "GraphQL",
  "authentication": {
    "type": "Bearer",
    "token": "your-bearer-token"
  },
  "introspectionEnabled": true
}
```

#### GraphQL Query Example
```http
POST /api/integration/gateway/route
Content-Type: application/json

{
  "endpointId": "graphql-api",
  "method": "POST",
  "body": {
    "query": "query GetUser($id: ID!) { user(id: $id) { name email } }",
    "variables": {
      "id": "user-123"
    }
  }
}
```

## Message Orchestration

### Asynchronous Message Processing

#### Enqueue Message
```http
POST /api/integration/messages/enqueue
Content-Type: application/json

{
  "queueName": "customer-updates",
  "message": {
    "type": "CustomerCreated",
    "data": {
      "customerId": "12345",
      "name": "John Doe",
      "email": "john@example.com"
    }
  },
  "priority": "Normal",
  "delaySeconds": 0,
  "retryPolicy": {
    "maxRetries": 3,
    "backoffStrategy": "exponential"
  }
}
```

#### Process Messages
```http
GET /api/integration/messages/dequeue/customer-updates
Authorization: Bearer {your_jwt_token}
```

### Message Transformation

#### Transform Message Format
```http
POST /api/integration/messages/transform
Content-Type: application/json

{
  "message": {
    "customerId": "12345",
    "customerName": "John Doe"
  },
  "sourceFormat": "internal",
  "targetFormat": "salesforce",
  "transformationRules": [
    {
      "source": "customerId",
      "target": "Id",
      "type": "rename"
    },
    {
      "source": "customerName",
      "target": "Name",
      "type": "rename"
    }
  ]
}
```

## Authentication Integration

### SSO Integration Setup

#### SAML 2.0 Integration

1. **Configure SAML Provider**
```http
PUT /api/auth/sso/configuration
Content-Type: application/json

{
  "tenantId": "your-tenant-id",
  "provider": "SAML",
  "configuration": {
    "entityId": "https://your-idp.com/metadata",
    "ssoUrl": "https://your-idp.com/sso",
    "x509Certificate": "-----BEGIN CERTIFICATE-----\n...\n-----END CERTIFICATE-----",
    "nameIdFormat": "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent",
    "attributeMapping": {
      "email": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
      "firstName": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
      "lastName": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"
    }
  }
}
```

2. **Initiate SAML Authentication**
```http
POST /api/auth/sso/saml/initiate
Content-Type: application/json

{
  "tenantId": "your-tenant-id",
  "relayState": "optional-state-data",
  "assertionConsumerServiceUrl": "https://your-app.com/auth/saml/callback"
}
```

#### OAuth 2.0 / OpenID Connect Integration

1. **Configure OAuth Provider**
```http
PUT /api/auth/sso/configuration
Content-Type: application/json

{
  "tenantId": "your-tenant-id",
  "provider": "OAuth",
  "configuration": {
    "clientId": "your-oauth-client-id",
    "clientSecret": "your-oauth-client-secret",
    "authorizationEndpoint": "https://oauth-provider.com/auth",
    "tokenEndpoint": "https://oauth-provider.com/token",
    "userInfoEndpoint": "https://oauth-provider.com/userinfo",
    "scope": "openid profile email",
    "redirectUri": "https://your-app.com/auth/oauth/callback"
  }
}
```

2. **Initiate OAuth Flow**
```http
POST /api/auth/sso/oauth/initiate
Content-Type: application/json

{
  "tenantId": "your-tenant-id",
  "clientId": "your-oauth-client-id",
  "redirectUri": "https://your-app.com/auth/oauth/callback",
  "scope": "openid profile email",
  "state": "random-state-value"
}
```

### LDAP Integration

#### Configure LDAP Connection
```http
PUT /api/auth/ldap/configuration
Content-Type: application/json

{
  "tenantId": "your-tenant-id",
  "serverUrl": "ldap://your-domain-controller.com:389",
  "baseDn": "DC=company,DC=com",
  "bindDn": "CN=ldap-service,OU=Service Accounts,DC=company,DC=com",
  "bindPassword": "service-account-password",
  "userSearchFilter": "(&(objectClass=user)(sAMAccountName={0}))",
  "groupSearchFilter": "(&(objectClass=group)(member={0}))",
  "attributeMapping": {
    "username": "sAMAccountName",
    "email": "mail",
    "firstName": "givenName",
    "lastName": "sn",
    "displayName": "displayName"
  },
  "enableSsl": true,
  "timeout": 30000
}
```

#### Test LDAP Connection
```http
POST /api/auth/ldap/test-connection
Content-Type: application/json

{
  "tenantId": "your-tenant-id"
}
```

## Monitoring and Logging

### View Integration Logs
```http
GET /api/integration/logs?fromDate=2025-07-14T00:00:00Z&toDate=2025-07-14T23:59:59Z
Authorization: Bearer {your_jwt_token}
```

### Monitor Queue Status
```http
GET /api/integration/queues/status
Authorization: Bearer {your_jwt_token}
```

Expected response:
```json
{
  "queues": [
    {
      "name": "customer-updates",
      "messageCount": 15,
      "processingRate": 10.5,
      "errorRate": 0.02,
      "lastProcessed": "2025-07-14T15:24:30Z"
    }
  ]
}
```

## Error Handling

### Common Error Codes
- `ENDPOINT_NOT_FOUND`: Endpoint ID not registered
- `AUTHENTICATION_FAILED`: Invalid credentials for external system
- `TIMEOUT_EXCEEDED`: Request timeout exceeded
- `RATE_LIMIT_EXCEEDED`: Too many requests
- `TRANSFORMATION_FAILED`: Message transformation error

### Retry Strategies
- **Exponential Backoff**: Delays increase exponentially (1s, 2s, 4s, 8s)
- **Linear Backoff**: Fixed delay between retries
- **Immediate**: No delay between retries (use with caution)

### Error Response Format
```json
{
  "success": false,
  "error": {
    "code": "TIMEOUT_EXCEEDED",
    "message": "Request timeout exceeded",
    "details": "External system did not respond within 30 seconds",
    "correlationId": "uuid",
    "timestamp": "2025-07-14T15:25:00Z",
    "retryable": true
  }
}
```

## Best Practices

### Security
1. Always use HTTPS for external endpoints
2. Implement proper authentication (API keys, OAuth, etc.)
3. Validate SSL certificates in production
4. Use tenant-specific configurations
5. Rotate credentials regularly

### Performance
1. Set appropriate timeouts (default: 30 seconds)
2. Implement circuit breakers for unreliable services
3. Use connection pooling for high-volume integrations
4. Monitor response times and error rates
5. Implement caching where appropriate

### Reliability
1. Configure retry policies for transient failures
2. Use dead letter queues for failed messages
3. Implement health checks for all endpoints
4. Monitor integration metrics
5. Set up alerts for failures

## Troubleshooting

### Common Issues

#### Connection Timeouts
- Check network connectivity
- Verify firewall rules
- Increase timeout values if needed
- Check external system health

#### Authentication Failures
- Verify credentials are correct
- Check token expiration
- Ensure proper authentication type
- Validate certificate chains (for SSL)

#### Message Processing Failures
- Check message format
- Verify transformation rules
- Monitor queue depths
- Review error logs

### Debug Mode
Enable debug logging by setting log level to `Debug` in configuration:
```json
{
  "Logging": {
    "LogLevel": {
      "BARQ.Application.Services.Integration": "Debug"
    }
  }
}
```
