using System.Linq.Expressions;
using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using JacksonVeroneze.NET.Pagination;

namespace JacksonVeroneze.NET.EntityFramework.Interfaces;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    public IUnitOfWork UnitOfWork { get; }

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default);

    void Delete(TEntity entity);

    Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default);

    Task<Page<TEntity>> GetPagedAsync(
        PaginationParameters pagination,
        Expression<Func<TEntity, bool>>? whereExpression = null,
        Expression<Func<TEntity, object>>? orderExpression = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetSingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default);

    void SoftDelete(TEntity entity);

    void Update(TEntity entity);
}