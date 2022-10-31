namespace JacksonVeroneze.NET.EntityFramework.Configuration;

public class DatabaseOptions
{
    public string ConnectionString { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }

    public bool UseLazyLoadingProxies { get; set; }
}