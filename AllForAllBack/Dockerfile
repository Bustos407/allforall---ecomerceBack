# Etapa base para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80  # Cambiado de 8080 y 8081 a 80

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AllForAllBack/AllForAllBack.csproj", "AllForAllBack/"]
RUN dotnet restore "./AllForAllBack/AllForAllBack.csproj"
COPY . .
WORKDIR "/src/AllForAllBack"
RUN dotnet build "./AllForAllBack.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AllForAllBack.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AllForAllBack.dll"]
