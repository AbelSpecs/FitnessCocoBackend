# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. Copiar archivos de proyecto para restaurar (Capa por capa)
# Importante: Mantener la estructura de carpetas exacta de tu solución
COPY ["InsightCore.WebApi/InsightCore.WebApi.csproj", "InsightCore.WebApi/"]
COPY ["InsightCore.Application.DTO/InsightCore.Application.DTO.csproj", "InsightCore.Application.DTO/"]
COPY ["InsightCore.Application.Interface/InsightCore.Application.Interface.csproj", "InsightCore.Application.Interface/"]
COPY ["InsightCore.Application.UseCases/InsightCore.Application.UseCases.csproj", "InsightCore.Application.UseCases/"]
COPY ["InsightCore.Domain/InsightCore.Domain.csproj", "InsightCore.Domain/"]
COPY ["InsightCore.Infrastructure/InsightCore.Infrastructure.csproj", "InsightCore.Infrastructure/"]
# Si tienes Transversal, asegúrate de que el nombre de la carpeta sea exacto
COPY ["InsightCore.Transversal.Common/InsightCore.Transversal.Common.csproj", "InsightCore.Transversal.Common/"]

# 3. Restaurar paquetes NuGet
RUN dotnet restore "InsightCore.WebApi/InsightCore.WebApi.csproj"

# 4. Copiar el resto del código y publicar
COPY . .
WORKDIR "/src/InsightCore.WebApi"
RUN dotnet publish "InsightCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 8080
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsightCore.WebApi.dll"]