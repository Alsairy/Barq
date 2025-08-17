FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BARQ.API/BARQ.API.csproj", "src/BARQ.API/"]
COPY ["src/BARQ.Application/BARQ.Application.csproj", "src/BARQ.Application/"]
COPY ["src/BARQ.Core/BARQ.Core.csproj", "src/BARQ.Core/"]
COPY ["src/BARQ.Infrastructure/BARQ.Infrastructure.csproj", "src/BARQ.Infrastructure/"]
COPY ["src/BARQ.Shared/BARQ.Shared.csproj", "src/BARQ.Shared/"]
RUN dotnet restore "./src/BARQ.API/BARQ.API.csproj"
COPY . .
WORKDIR "/src/src/BARQ.API"
RUN dotnet build "./BARQ.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BARQ.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser
COPY --from=publish /app/publish .
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 CMD wget -qO- http://127.0.0.1:8080/health || exit 1
ENTRYPOINT ["dotnet", "BARQ.API.dll"]
