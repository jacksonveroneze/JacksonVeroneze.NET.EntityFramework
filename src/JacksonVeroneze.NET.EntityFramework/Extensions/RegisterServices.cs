using JacksonVeroneze.NET.EntityFramework.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.EntityFramework.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection AddPostgreSql<T>(
            this IServiceCollection services,
            Action<DatabaseOptions> action) where T : DbContext
        {
            DatabaseOptions optionsConfig = new();

            action?.Invoke(optionsConfig);

            return services.AddDbContext<T>((_, options) =>
                options.UseNpgsql(optionsConfig.ConnectionString, optionsBuilder =>
                        optionsBuilder
                            .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                    )
                    .ConfigureOptions(optionsConfig)
            );
        }

        public static IServiceCollection AddSqlite<T>(
            this IServiceCollection services,
            Action<DatabaseOptions> action) where T : DbContext
        {
            DatabaseOptions optionsConfig = new();

            action?.Invoke(optionsConfig);

            return services.AddDbContext<T>((_, options) =>
                options.UseSqlite(optionsConfig.ConnectionString, optionsBuilder =>
                        optionsBuilder
                            .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                    )
                    .ConfigureOptions(optionsConfig)
            );
        }

        private static DbContextOptionsBuilder ConfigureOptions(
            this DbContextOptionsBuilder options,
            DatabaseOptions optionsConfig)
        {
            options.UseSnakeCaseNamingConvention();

            if (optionsConfig.UseLazyLoadingProxies)
                options.UseLazyLoadingProxies();

            if (optionsConfig.EnableDetailedErrors)
                options.EnableDetailedErrors();

            if (optionsConfig.EnableSensitiveDataLogging)
                options.EnableSensitiveDataLogging();

            return options;
        }
    }
}