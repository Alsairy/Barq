# Traefik CORS Configuration Fix

This directory contains the Traefik configuration files needed to resolve the CORS authentication blocking issue with the BARQ platform.

## Problem
The Traefik proxy is challenging all requests (including OPTIONS preflight requests) with HTTP Basic authentication, preventing browsers from completing the CORS handshake. This blocks the frontend from authenticating with the backend API.

## Solution
The `dynamic-config.yml` file provides:

1. **CORS Middleware**: Adds proper CORS headers including `Access-Control-Allow-Origin`
2. **High-Priority OPTIONS Router**: Handles preflight requests without authentication
3. **Middleware Ordering**: Applies CORS headers before basic authentication

## How to Apply

### Method 1: Direct Configuration Update
If you have direct access to your Traefik configuration:

1. Copy the contents of `dynamic-config.yml` to your Traefik dynamic configuration file
2. Update the `barq-basic-auth` users section with your actual credentials
3. Ensure the `barq-api` service URL points to your actual backend service
4. Reload Traefik configuration:
   ```bash
   # For Docker Compose
   docker-compose restart traefik
   
   # For Docker
   docker restart traefik
   
   # For systemd service
   sudo systemctl reload traefik
   ```

### Method 2: File Provider
If using Traefik's file provider:

1. Place `dynamic-config.yml` in your Traefik configuration directory
2. Ensure your `traefik.yml` includes:
   ```yaml
   providers:
     file:
       directory: /path/to/config/directory
       watch: true
   ```
3. Traefik will automatically reload the configuration

### Method 3: Docker Compose Integration
If using Docker Compose, mount the configuration file:

```yaml
services:
  traefik:
    image: traefik:v3.0
    volumes:
      - ./Infrastructure/traefik/dynamic-config.yml:/etc/traefik/dynamic/dynamic-config.yml:ro
```

## Verification
After applying the configuration, test with:

```bash
# Test OPTIONS preflight request
curl -X OPTIONS \
  -H "Origin: https://barq-application-plu4nmbz.devinapps.com" \
  -H "Access-Control-Request-Method: POST" \
  -H "Access-Control-Request-Headers: Content-Type,Authorization" \
  -v https://technology-stack-app-tunnel-phn9uram.devinapps.com/api/auth/login

# Should return 200 OK with CORS headers, no authentication challenge
```

## Expected Result
- OPTIONS requests return 200 OK with proper CORS headers
- No authentication challenge on preflight requests
- Frontend authentication flow works end-to-end
- Login at https://barq-application-plu4nmbz.devinapps.com succeeds

## Configuration Details

### CORS Headers Applied
- `Access-Control-Allow-Origin`: https://barq-application-plu4nmbz.devinapps.com
- `Access-Control-Allow-Methods`: GET, POST, PUT, PATCH, DELETE, OPTIONS
- `Access-Control-Allow-Headers`: Authorization, Content-Type, X-Tenant-ID, X-Correlation-ID, X-Requested-With
- `Access-Control-Allow-Credentials`: true

### Router Priority
- `barq-api-preflight` (priority 100): Handles OPTIONS without auth
- `barq-api` (default priority): Handles all other requests with auth

### Middleware Order
1. `barq-cors`: Adds CORS headers first
2. `barq-basic-auth`: Applies authentication second

This ensures CORS headers are present even on 401 responses.
