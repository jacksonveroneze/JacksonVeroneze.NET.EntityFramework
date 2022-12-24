using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.DatabaseContext;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions<BaseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BaseDbContext).Assembly);
    }
}