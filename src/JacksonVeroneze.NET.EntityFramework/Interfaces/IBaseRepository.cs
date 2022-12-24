using System.Linq.Expressions;
using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Interfaces;

public interface IBaseRepository<TEntity, TKey> : IDisposable
    where TEntity : BaseEntity<TKey>
{
    public IUnitOfWork UnitOfWork { get; set; }

    Task AddAsync(TEntity entity,
        CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    ValueTask<TEntity> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<Page<TEntity>> GetPagedAsync(
        PaginationParameters pagination,
        Expression<Func<TEntity, bool>> expression = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);
}