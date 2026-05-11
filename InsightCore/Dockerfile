# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. Copiar la solución y proyectos (Asegúrate de que las rutas coincidan)
COPY ["InsightCore/InsightCore.sln", "InsightCore/"]
COPY ["InsightCore/InsightCore.WebApi/InsightCore.WebApi.csproj", "InsightCore.WebApi/"]
COPY ["InsightCore/InsightCore.Application.DTO/InsightCore.Application.DTO.csproj", "InsightCore.Application.DTO/"]
COPY ["InsightCore/InsightCore.Application.Interface/InsightCore.Application.Interface.csproj", "InsightCore.Application.Interface/"]
COPY ["InsightCore/InsightCore.Application.UseCases/InsightCore.Application.UseCases.csproj", "InsightCore.Application.UseCases/"]
COPY ["InsightCore/InsightCore.Domain/InsightCore.Domain.csproj", "InsightCore.Domain/"]
COPY ["InsightCore/InsightCore.Infrastructure/InsightCore.Infrastructure.csproj", "InsightCore.Infrastructure/"]
COPY ["InsightCore/InsightCore.Persistence/InsightCore.Persistence.csproj", "InsightCore.Persistence/"]
COPY ["InsightCore/InsightCore.Transversal.Common/InsightCore.Transversal.Common.csproj", "InsightCore.Transversal.Common/"]

# 3. Restaurar paquetes (Ahora la solución está en la raíz /src)
RUN dotnet restore "InsightCore.sln"

# 4. Copiar el resto y publicar
COPY . .
WORKDIR "/src/InsightCore/InsightCore.WebApi"
RUN dotnet publish "InsightCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
# Usamos el puerto que tienes configurado en Easypanel
ENV ASPNETCORE_URLS=http://+:8080 
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsightCore.WebApi.dll"]