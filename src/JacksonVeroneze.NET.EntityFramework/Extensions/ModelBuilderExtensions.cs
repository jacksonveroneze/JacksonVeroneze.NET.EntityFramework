using JacksonVeroneze.NET.DomainObjects.Domain;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddDeletedAtFilter<TEntity, TKey>(
        this ModelBuilder modelBuilder) where TEntity : Entity<TKey>
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(x => x.DeletedAt == null);

        return modelBuilder;
    }
}