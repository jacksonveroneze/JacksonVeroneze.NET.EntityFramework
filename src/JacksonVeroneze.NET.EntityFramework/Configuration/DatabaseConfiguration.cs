namespace JacksonVeroneze.NET.EntityFramework.Configuration;

public class DatabaseConfiguration
{
    public string? ConnectionString { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }

    public bool UseLazyLoadingProxies { get; set; }

    public int CommandTimeoutSeconds { get; set; } = 1;

    public int MaxRetryCount { get; set; } = 3;

    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromMilliseconds(100);
}