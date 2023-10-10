using System.Linq.Expressions;
using JacksonVeroneze.NET.DomainObjects.Domain;
using JacksonVeroneze.NET.EntityFramework.Extensions;
using JacksonVeroneze.NET.EntityFramework.Interfaces;
using JacksonVeroneze.NET.EntityFramework.UnitOfWork;
using JacksonVeroneze.NET.Pagination;
using JacksonVeroneze.NET.Pagination.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.Repository;

public class BaseRepository<TEntity, TKey> :
    IBaseRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    protected readonly ILogger<BaseRepository<TEntity, TKey>> _logger;
    protected readonly DbContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    public IUnitOfWork UnitOfWork { get; }

    public BaseRepository(
        ILogger<BaseRepository<TEntity, TKey>> logger,
        DbContext context,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _context = context;
        _dbSet = context.Set<TEntity>();

        UnitOfWork = unitOfWork;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            whereExpression, nameof(whereExpression));

        bool any = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .AnyAsync(cancellationToken);

        _logger.LogAny(nameof(BaseRepository<TEntity, TKey>),
            nameof(AnyAsync),
            any);

        return any;
    }

    public async Task<long> CountAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            whereExpression, nameof(whereExpression));

        int count = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .CountAsync(cancellationToken);

        _logger.LogCount(nameof(BaseRepository<TEntity, TKey>),
            nameof(CountAsync),
            count);

        return count;
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dbSet.Remove(entity);

        _logger.LogDelete(nameof(BaseRepository<TEntity, TKey>),
            nameof(Delete),
            entity.Id!);
    }

    public async Task CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dbSet.Attach(entity);
        await _dbSet.AddAsync(entity, cancellationToken);

        _logger.LogCreate(nameof(BaseRepository<TEntity, TKey>),
            nameof(CreateAsync));
    }

    public async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            whereExpression, nameof(whereExpression));

        List<TEntity> data = await _dbSet
            .AsNoTrackingWithIdentityResolution()
            .Where(whereExpression)
            .OrderByDescending(entity => entity.CreatedAt)
            .ToListAsync(cancellationToken);

        _logger.LogGetAll(nameof(BaseRepository<TEntity, TKey>),
            nameof(GetAllAsync),
            data.Count);

        return data;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        TEntity? result = await _dbSet
            .FindAsync(new object[] { id }, cancellationToken);

        _logger.LogGetById(nameof(BaseRepository<TEntity, TKey>),
            nameof(GetByIdAsync),
            id,
            result != null);

        return result;
    }

    public async Task<Page<TEntity>> GetPagedAsync(
        PaginationParameters pagination,
        Expression<Func<TEntity, bool>>? whereExpression = null,
        Expression<Func<TEntity, object>>? orderExpression = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pagination, nameof(pagination));

        whereExpression ??= entity => true;
        orderExpression ??= entity => entity.CreatedAt;

        int count = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .CountAsync(cancellationToken);

        List<TEntity> result = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .OrderByDescending(orderExpression)
            .ConfigurePagination(pagination)
            .ToListAsync(cancellationToken);

        Page<TEntity> data = result
            .ToPage(pagination, count);

        _logger.LogGetPaged(nameof(BaseRepository<TEntity, TKey>),
            nameof(GetPagedAsync),
            data.Pagination);

        return data;
    }

    public async Task<TEntity?> GetSingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            whereExpression, nameof(whereExpression));

        TEntity? result = await _dbSet
            .Where(whereExpression)
            .SingleOrDefaultAsync(cancellationToken);

        _logger.LogGetSingleOrDefault(nameof(BaseRepository<TEntity, TKey>),
            nameof(GetSingleOrDefaultAsync),
            result != null);

        return result;
    }

    public void SoftDelete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.MarkAsDeleted();

        _dbSet.Update(entity);

        _logger.LogSoftDelete(nameof(BaseRepository<TEntity, TKey>),
            nameof(SoftDelete),
            entity.Id!);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.MarkAsUpdated();

        _dbSet.Update(entity);

        _logger.LogUpdate(nameof(BaseRepository<TEntity, TKey>),
            nameof(Update),
            entity.Id!);
    }
}