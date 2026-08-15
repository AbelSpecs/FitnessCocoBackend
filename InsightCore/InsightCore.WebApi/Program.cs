using InsightCore.Application;
using InsightCore.Application.Interface.UseCases;
using InsightCore.Application.UseCases;
using InsightCore.Persistence;
using InsightCore.Persistence.Contexts;
using InsightCore.Persistence.Repositories;
using InsightCore.WebApi.Modules.Authentication;
using InsightCore.WebApi.Modules.Feature;
using InsightCore.WebApi.Modules.Injection;
using InsightCore.WebApi.Modules.Middleware;
using InsightCore.WebApi.Modules.RateLimiter;
using InsightCore.WebApi.Modules.Redis;
using InsightCore.WebApi.Modules.Swagger;
using InsightCore.WebApi.Modules.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using InsightCore.WebApi.JsonConverters;
using InsightCore.Application.Interface.Integration;
using InsightCore.Infrastructure.Integration.OpenFoodFacts;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores y configurar serialización para DateOnly
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();

// Registrar OpenFoodFacts Typed HttpClient
builder.Services.AddHttpClient<IOpenFoodFactsService, OpenFoodFactsService>(client =>
{
    client.BaseAddress = new Uri("https://es.openfoodfacts.org/api/v2/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("PyrosFitApp/1.0 (contact@pyrosfit.com)");
    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("es-ES,es;q=0.9");
});

// CORS: 
var frontendUrl = "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddPolicy("MainPolicy", policy =>
    {
        // Agregamos localhost y tus dominios de producción
        policy.WithOrigins("http://localhost:3000", "https://pyrosfit.com", "https://www.pyrosfit.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddFeature(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
//builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddInjection(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddVersioning();
builder.Services.AddSwagger();
//builder.Services.AddHealthCheck(builder.Configuration);
//builder.Services.AddRedisCache(builder.Configuration);
//builder.Services.AddRatelimiting(builder.Configuration);
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// Registrar el servicio de Forwarded Headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                               Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


// Console logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Opcional: añadir filtro específico para Npgsql
builder.Logging.AddFilter("Npgsql", LogLevel.Debug);

// Swagger configuration is handled in AddSwagger extension which registers SwaggerGen and ConfigureSwaggerOptions

var app = builder.Build();

// 1. PRIMERO SIEMPRE: Configurar headers para que la app entienda el HTTPS del Proxy
app.UseForwardedHeaders();

// 2. Swagger configurado para no fallar en Proxy
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        // Esto le dice a Swagger que use el esquema que viene del proxy (HTTPS)
        swaggerDoc.Servers = new List<OpenApiServer> {
            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
        };
    });
});

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PyrosFit API V1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseCors("MainPolicy");

app.AddMiddleware();
app.UseAuthentication();
app.UseAuthorization();
//app.UseRateLimiter();
app.UseRequestTimeouts();
app.MapControllers();
//app.MapHealthChecksUI();
//app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
//{
//    Predicate = _ => true,
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});



app.Run();

public partial class Program { };