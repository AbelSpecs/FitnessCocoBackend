# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. Copiar archivos de proyecto para restaurar (Capa por capa)
# Importante: Mantener la estructura de carpetas exacta de tu solución
COPY ["Service/InsightCore.WebApi/InsightCore.WebApi.csproj", "Service/InsightCore.WebApi/"]
COPY ["Application/InsightCore.Application.DTO/InsightCore.Application.DTO.csproj", "Application/InsightCore.Application.DTO/"]
COPY ["Application/InsightCore.Application.Interface/InsightCore.Application.Interface.csproj", "Application/InsightCore.Application.Interface/"]
COPY ["Application/InsightCore.Application.UseCases/InsightCore.Application.UseCases.csproj", "Application/InsightCore.Application.UseCases/"]
COPY ["Domain/InsightCore.Domain/InsightCore.Domain.csproj", "Domain/InsightCore.Domain/"]
COPY ["Infrastructure/InsightCore.Infrastructure/InsightCore.Infrastructure.csproj", "Infrastructure/InsightCore.Infrastructure/"]
COPY ["Transversal/InsightCore.Transversal/InsightCore.Transversal.csproj", "Transversal/InsightCore.Transversal/"]

# 3. Restaurar paquetes NuGet
RUN dotnet restore "Service/InsightCore.WebApi/InsightCore.WebApi.csproj"

# 4. Copiar el resto del código y publicar
COPY . .
WORKDIR "/src/Service/InsightCore.WebApi"
RUN dotnet publish "InsightCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 8080
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsightCore.WebApi.dll"]