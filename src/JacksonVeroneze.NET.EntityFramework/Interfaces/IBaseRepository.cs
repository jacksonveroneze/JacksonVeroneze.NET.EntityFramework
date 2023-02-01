using System.Linq.Expressions;
using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Interfaces;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    public IUnitOfWork UnitOfWork { get; set; }

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default);

    Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default);

    Task<Page<TEntity>> GetPagedAsync(
        PaginationParameters pagination,
        Expression<Func<TEntity, bool>>? expression = null,
        CancellationToken cancellationToken = default);

    void Remove(TEntity entity);

    void Update(TEntity entity);
}