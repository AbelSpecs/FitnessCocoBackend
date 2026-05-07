using FluentValidation;
using InsightCore.Application.Interface.UseCases;
using InsightCore.Application.UseCases.Common.Behaviours;
using InsightCore.Application.UseCases.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InsightCore.Application.UseCases
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                // Register handlers from this assembly explicitly. Using the type guarantees
                // the correct assembly is used even when called from the Web project.
                cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IUsersApplication, UsersApplication>();

            return services;
        }
    }
}
