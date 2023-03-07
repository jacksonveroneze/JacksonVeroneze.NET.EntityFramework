using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class PaginationExtension
{
    public static IQueryable<TSource> ConfigureQueryPagination<TSource>(
        this IQueryable<TSource> queryable,
        PaginationParameters pagination)
    {
        int skip = pagination.Page;

        if (skip < 0) skip = 0;

        if (skip > 0) skip--;

        int take = pagination.PageSize;

        return queryable
            .Skip(skip * take)
            .Take(take);
    }

    public static Page<TEntity> ToPage<TEntity>(this List<TEntity> source,
        PaginationParameters pagination, int count)
    {
        int totalPages = count > 0
            ? (int)Math.Ceiling(count / (decimal)(pagination.PageSize))
            : 0;

        return new(source, new PageInfo(pagination, totalPages, count));
    }
}