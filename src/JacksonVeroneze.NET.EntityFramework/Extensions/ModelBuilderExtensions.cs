using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddDeletedAtFilter<TEntity, TKey>(
        this ModelBuilder modelBuilder) where TEntity : BaseEntity<TKey>
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(x => x.DeletedAt == null);

        return modelBuilder;
    }
}