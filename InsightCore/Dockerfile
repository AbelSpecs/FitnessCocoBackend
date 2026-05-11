# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. Copiar archivos usando la ruta completa desde la raíz del repo
COPY ["InsightCore/InsightCore.sln", "InsightCore/"]
COPY ["InsightCore/InsightCore.WebApi/InsightCore.WebApi.csproj", "InsightCore/InsightCore.WebApi/"]
COPY ["InsightCore/InsightCore.Application.DTO/InsightCore.Application.DTO.csproj", "InsightCore/InsightCore.Application.DTO/"]
COPY ["InsightCore/InsightCore.Application.Interface/InsightCore.Application.Interface.csproj", "InsightCore/InsightCore.Application.Interface/"]
COPY ["InsightCore/InsightCore.Application.UseCases/InsightCore.Application.UseCases.csproj", "InsightCore/InsightCore.Application.UseCases/"]
COPY ["InsightCore/InsightCore.Domain/InsightCore.Domain.csproj", "InsightCore/InsightCore.Domain/"]
COPY ["InsightCore/InsightCore.Infrastructure/InsightCore.Infrastructure.csproj", "InsightCore/InsightCore.Infrastructure/"]
COPY ["InsightCore/InsightCore.Persistence/InsightCore.Persistence.csproj", "InsightCore/InsightCore.Persistence/"]
COPY ["InsightCore/InsightCore.Transversal.Common/InsightCore.Transversal.Common.csproj", "InsightCore/InsightCore.Transversal.Common/"]

# 3. Restaurar paquetes usando la ruta de la solución
RUN dotnet restore "InsightCore/InsightCore.sln"

# 4. Copiar todo el código y publicar
COPY . .
WORKDIR "/src/InsightCore/InsightCore.WebApi"
RUN dotnet publish "InsightCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsightCore.WebApi.dll"]