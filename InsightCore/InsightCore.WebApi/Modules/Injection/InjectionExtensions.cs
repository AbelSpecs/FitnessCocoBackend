using InsightCore.Application.Interface.Presentation;
using InsightCore.Infrastructure.Notification;
using InsightCore.WebApi.Modules.GlobalException;
using InsightCore.WebApi.Services;
using System;

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

            // Email service using HttpClient to call Resend API
            services.AddHttpClient();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
