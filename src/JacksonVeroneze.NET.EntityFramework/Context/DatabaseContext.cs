using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.Context;

public abstract class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
}