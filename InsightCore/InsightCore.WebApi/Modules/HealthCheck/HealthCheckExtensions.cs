using Microsoft.Extensions.DependencyInjection;

namespace InsightCore.WebApi.Modules.HealthCheck
{
    public static class HealthCheckExtensions
    {
        //public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddHealthChecks()
        //        .AddSqlServer(configuration.GetConnectionString("NorthwindConnection"), tags: new[] { "database" })
        //        .AddRedis(configuration.GetConnectionString("RedisConnection"), tags: new[] { "cache" })
        //        .AddCheck<HealthCheckCustom>("HealthCheckCustom", tags: new[] { "custom" });

        //    services.AddHealthChecksUI().AddSqlServerStorage(configuration.GetConnectionString("InsightConnection"));

        //    return services;
        //}
    }
}
