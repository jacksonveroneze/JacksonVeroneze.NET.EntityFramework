using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<TSource> ConfigurePagination<TSource>(
        this IQueryable<TSource> queryable,
        PaginationParameters pagination)
    {
        int skip = pagination.Page;
        int take = pagination.PageSize;

        if (skip < 0)
        {
            skip = 0;
        }

        if (skip > 0)
        {
            skip--;
        }

        return queryable
            .Skip(skip * take)
            .Take(take);
    }
}