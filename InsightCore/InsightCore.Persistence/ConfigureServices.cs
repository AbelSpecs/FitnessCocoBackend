using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using InsightCore.Persistence.Interceptors;
using InsightCore.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Persistence
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DapperContext>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("InsightConnection"),
                    npgsql => npgsql.UseNetTopologySuite().MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICoachesRepository, CoachesRepository>();
            services.AddScoped<ICoachStudentsRepository, CoachStudentsRepository>();
            services.AddScoped<IStudentsRepository, StudentsRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<IExercisesRepository, ExercisesRepository>();
            services.AddScoped<IGymsRepository, GymsRepository>();
            services.AddScoped<IDailyStudentExercisesRepository, DailyStudentExercisesRepository>();

            services.AddScoped<IDailyExerciseSetsRepository, DailyExerciseSetsRepository>();
            services.AddScoped<IMuscleGroupsRepository, MuscleGroupsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGenericRepository<Country>, CountriesRepository>();
            //services.AddScoped<IGenericRepository<City>, CitiesRepository>();
            services.AddScoped<IQrsRepository, QrsRepository>();
            services.AddScoped<ICitiesRepository, CitiesRepository>();
            // Register generic repository for other uses
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
