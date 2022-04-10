using System.Linq;

namespace JacksonVeroneze.NET.EntityFramework.Data
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> ConfigureSkipTake<TSource>(
            this IQueryable<TSource> queryable,
            Pagination.Pagination pagination)
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
