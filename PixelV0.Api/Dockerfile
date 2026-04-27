# 1. ��Z�M: .NET 9.0 imajlar�n� kullan�yoruz
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 2. ��Z�M: Eksik olan Shared projesini de kopyal�yoruz
COPY ["PixelV0.Shared/PixelV0.Shared.csproj", "PixelV0.Shared/"]

# Di�er projeler
COPY ["PixelV0.Api/PixelV0.Api.csproj", "PixelV0.Api/"]
COPY ["PixelV0.Infrastructure/PixelV0.Infrastructure.csproj", "PixelV0.Infrastructure/"]
COPY ["PixelV0.Modules.Identity/PixelV0.Modules.Identity.csproj", "PixelV0.Modules.Identity/"]
COPY ["PixelV0.Modules.Catalog/PixelV0.Modules.Catalog.csproj", "PixelV0.Modules.Catalog/"]
COPY ["PixelV0.Modules.Social/PixelV0.Modules.Social.csproj", "PixelV0.Modules.Social/"]

# Restore i�lemi
RUN dotnet restore "PixelV0.Api/PixelV0.Api.csproj"

# T�m kodlar� kopyala ve build al
COPY . .
WORKDIR "/src/PixelV0.Api"
RUN dotnet build "PixelV0.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PixelV0.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final imaj� (Run-time)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["PixelV0.Api/appsettings.json", "."]
ENTRYPOINT ["dotnet", "PixelV0.Api.dll"]