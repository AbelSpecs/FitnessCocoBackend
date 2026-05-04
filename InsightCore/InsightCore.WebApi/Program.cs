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

builder.Services.AddFeature(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
//builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddInjection(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
//builder.Services.AddVersioning();
//builder.Services.AddSwagger();
//builder.Services.AddHealthCheck(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddRatelimiting(builder.Configuration);
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InsightCore.WebApi",
        Version = "v1",
        Description = "Web API que permitirá la interconexión entre front y el back"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

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

app.UseHttpsRedirection();
app.UseCors("policyApiInsight");
app.AddMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
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