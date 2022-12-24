using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class GlobalFilterExtension
{
    public static ModelBuilder AddDeletedAtFilter<T>(
        this ModelBuilder modelBuilder) where T : BaseEntity<Guid>
    {
        modelBuilder.Entity<T>()
            .HasQueryFilter(x => x.DeletedAt == null);

        return modelBuilder;
    }
}