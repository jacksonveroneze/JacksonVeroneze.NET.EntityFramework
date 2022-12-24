using JacksonVeroneze.NET.EntityFramework.Configuration;
using JacksonVeroneze.NET.EntityFramework.DatabaseContext;
using JacksonVeroneze.NET.EntityFramework.Interfaces;
using JacksonVeroneze.NET.EntityFramework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class RegisterServices
{
    public static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, BaseUnitOfWork>();

        return services;
    }

    public static IServiceCollection AddPostgreSql<T>(
        this IServiceCollection services,
        Action<DatabaseOptions> action) where T : BaseDbContext
    {
        services.AddScoped<IUnitOfWork, BaseUnitOfWork>();

        DatabaseOptions optionsConfig = new();

        action?.Invoke(optionsConfig);

        return services.AddDbContext<T>((_, options) =>
            options.UseNpgsql(optionsConfig.ConnectionString, optionsBuilder =>
                    optionsBuilder
                        .CommandTimeout(optionsConfig.CommandTimeout)
                        .EnableRetryOnFailure(optionsConfig.MaxRetryCount,
                            optionsConfig.MaxRetryDelay, null)
                )
                .ConfigureOptions(optionsConfig)
        );
    }

    private static DbContextOptionsBuilder ConfigureOptions(
        this DbContextOptionsBuilder options,
        DatabaseOptions optionsConfig)
    {
        options.UseSnakeCaseNamingConvention()
            .UseLazyLoadingProxies(optionsConfig.UseLazyLoadingProxies)
            .EnableDetailedErrors(optionsConfig.EnableDetailedErrors)
            .EnableSensitiveDataLogging(
                optionsConfig.EnableSensitiveDataLogging);

        return options;
    }
}