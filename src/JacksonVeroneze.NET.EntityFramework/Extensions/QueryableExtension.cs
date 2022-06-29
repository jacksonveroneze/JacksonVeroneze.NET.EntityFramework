namespace JacksonVeroneze.NET.EntityFramework.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> ConfigurePagination<TSource>(
            this IQueryable<TSource> queryable, PaginationParameters pagination)
        {
            int skip = pagination.Page;

            if (skip < 0) skip = 0;

            if (skip > 0) skip--;

            int take = pagination.PageSize;

            return queryable
                .Skip(skip * take)
                .Take(take);
        }
    }
}