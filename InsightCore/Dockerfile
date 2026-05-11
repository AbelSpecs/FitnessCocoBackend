# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. Copiar la solución y proyectos (Rutas relativas a InsightCore)
COPY ["InsightCore.sln", "./"]
COPY ["InsightCore.WebApi/InsightCore.WebApi.csproj", "InsightCore.WebApi/"]
COPY ["InsightCore.Application.DTO/InsightCore.Application.DTO.csproj", "InsightCore.Application.DTO/"]
COPY ["InsightCore.Application.Interface/InsightCore.Application.Interface.csproj", "InsightCore.Application.Interface/"]
COPY ["InsightCore.Application.UseCases/InsightCore.Application.UseCases.csproj", "InsightCore.Application.UseCases/"]
COPY ["InsightCore.Domain/InsightCore.Domain.csproj", "InsightCore.Domain/"]
COPY ["InsightCore.Infrastructure/InsightCore.Infrastructure.csproj", "InsightCore.Infrastructure/"]
COPY ["InsightCore.Persistence/InsightCore.Persistence.csproj", "InsightCore.Persistence/"]
COPY ["InsightCore.Transversal.Common/InsightCore.Transversal.Common.csproj", "InsightCore.Transversal.Common/"]

# 3. Restaurar paquetes NuGet
RUN dotnet restore "InsightCore.sln"

# 4. Copiar el resto y publicar
COPY . .
WORKDIR "/src/InsightCore.WebApi"
RUN dotnet publish "InsightCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Runtime (Imagen final ligera)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsightCore.WebApi.dll"]