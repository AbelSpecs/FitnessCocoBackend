using InsightCore.Application.Interface.Presentation;
using InsightCore.WebApi.Modules.GlobalException;
using InsightCore.WebApi.Services;

namespace InsightCore.WebApi.Modules.Injection
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<GlobalExceptionHandler>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IQrService, QrService>();

            return services;
        }
    }
}
