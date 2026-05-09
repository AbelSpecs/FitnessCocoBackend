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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// CORS: 
var frontendUrl = "http://localhost:3000"; 
builder.Services.AddCors(options =>
{
    options.AddPolicy("MainPolicy", policy =>
    {
        policy.WithOrigins(frontendUrl)
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

// Swagger configuration is handled in AddSwagger extension which registers SwaggerGen and ConfigureSwaggerOptions

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
        //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger cargue en la raíz
    });


    //app.UseReDoc(options =>
    //        {
    //            foreach (var description in provider.ApiVersionDescriptions)
    //            {
    //                options.DocumentTitle = "Insight Services API Market";
    //                options.SpecUrl = $"/swagger/{description.GroupName}/swagger.json";
    //            }
    //        });
}

// Por esto (para que funcione en Easypanel):
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PyrosFit API V1");
    c.RoutePrefix = "swagger"; // Esto hace que cargue en /swagger
});

app.UseHttpsRedirection();

// IMPORTANT: Ensure CORS middleware runs after UseRouting() and before
// authentication/authorization to allow preflight (OPTIONS) requests
// to be handled without being blocked by authentication middleware.
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