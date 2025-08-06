# BARQ Platform - Setup Guide

## Quick Start for Local Development

### Backend (.NET 8)

1. **Prerequisites**
   - Install .NET 8 SDK
   - Install SQL Server (or use Docker)

2. **Database Setup**
   ```bash
   # Create database (SQL Server)
   # Using SQL Server Management Studio or sqlcmd
   sqlcmd -S localhost -E -Q "CREATE DATABASE BarqDb"
   ```

3. **Run Backend**
   ```bash
   cd Backend
   dotnet restore BARQ.sln
   dotnet build BARQ.sln --configuration Release
   cd src/BARQ.API
   dotnet run
   ```

4. **Access Swagger Documentation**
   - Development: http://localhost:7062/swagger
   - The WAF has been configured to allow swagger access in development

### Frontend (Node.js 18)

1. **Prerequisites**
   - Install Node.js 18+
   - Install npm or yarn

2. **Run Frontend**
   ```bash
   cd Frontend/barq-frontend
   npm ci
   npm run dev
   ```

### Configuration Notes

- **Database**: Uses simple localhost connection without authentication for development
- **Security**: WAF protection is enabled but excludes swagger endpoints for development
- **JWT**: Uses development-only secret key (change for production)
- **Redis**: Optional for development, defaults to localhost:6379

### Production Deployment

- Uses environment variables for secure configuration
- Database requires proper authentication
- JWT uses secure secret keys
- Full WAF protection enabled
- HTTPS required

## Architecture Overview

BARQ is a comprehensive multi-tenant SaaS platform combining:
- AI orchestration and workflow automation
- Project management with agile development features  
- Compliance frameworks (GDPR, HIPAA, SOX)
- External system integration capabilities

### Key Components

- **Backend**: .NET 8 Web API with Entity Framework Core
- **Frontend**: React/TypeScript with Redux Toolkit
- **Database**: SQL Server with multi-tenant data isolation
- **Security**: Comprehensive WAF, rate limiting, and compliance monitoring
- **Integration**: REST/SOAP/GraphQL adapters with circuit breakers
