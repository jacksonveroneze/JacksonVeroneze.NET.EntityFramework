using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static class ListExtensions
{
    public static Page<TEntity> ToPage<TEntity>(
        this List<TEntity> source,
        PaginationParameters pagination,
        int count)
    {
        PageInfo pageInfo = new(
            pagination.Page,
            pagination.PageSize,
            count,
            pagination.OrderBy,
            pagination.Direction);

        return new Page<TEntity>(source, pageInfo);
    }
}