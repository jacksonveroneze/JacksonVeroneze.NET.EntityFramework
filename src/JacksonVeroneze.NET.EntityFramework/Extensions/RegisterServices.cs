using JacksonVeroneze.NET.EntityFramework.Configuration;
using JacksonVeroneze.NET.EntityFramework.Context;
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
        Action<DatabaseConfiguration> action) where T : DatabaseContext
    {
        DatabaseConfiguration configurationConfig = new();

        action?.Invoke(configurationConfig);

        return services.AddDbContext<T>((_, options) =>
            options.UseNpgsql(configurationConfig.ConnectionString, optionsBuilder =>
                    optionsBuilder
                        .CommandTimeout(configurationConfig.CommandTimeout)
                        .EnableRetryOnFailure(configurationConfig.MaxRetryCount,
                            configurationConfig.MaxRetryDelay, null)
                )
                .ConfigureOptions(configurationConfig)
        );
    }

    private static DbContextOptionsBuilder ConfigureOptions(
        this DbContextOptionsBuilder options,
        DatabaseConfiguration configurationConfig)
    {
        options.UseSnakeCaseNamingConvention()
            .UseLazyLoadingProxies(configurationConfig.UseLazyLoadingProxies)
            .EnableDetailedErrors(configurationConfig.EnableDetailedErrors)
            .EnableSensitiveDataLogging(
                configurationConfig.EnableSensitiveDataLogging);

        return options;
    }
}