using System.Linq.Expressions;
using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using JacksonVeroneze.NET.EntityFramework.Extensions;
using JacksonVeroneze.NET.EntityFramework.Interfaces;
using JacksonVeroneze.NET.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.NET.EntityFramework.Repository;

public abstract class BaseRepository<TEntity, TKey> :
    IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected readonly ILogger<BaseRepository<TEntity, TKey>> _logger;
    protected readonly DbContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    public IUnitOfWork UnitOfWork { get; set; }

    protected BaseRepository(
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
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        bool any = await _dbSet
            .AsNoTracking()
            .Where(expression)
            .AnyAsync(cancellationToken);

        _logger.LogInformation("{class} - {method} - Any: {any}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(AnyAsync),
            any);

        return any;
    }

    public async Task<long> CountAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        int count = await _dbSet
            .AsNoTracking()
            .Where(expression)
            .CountAsync(cancellationToken);

        _logger.LogInformation("{class} - {method} - Count: {count}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(CountAsync),
            count);

        return count;
    }

    public async Task CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);

        _logger.LogInformation("{class} - {method} - Created",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(CreateAsync));
    }

    public async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        List<TEntity> data = await _dbSet
            .Where(expression)
            .OrderByDescending(conf => conf.CreatedAt)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{class} - {method} - Count: {count}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(GetAllAsync),
            data.Count);

        return data;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default)
    {
        TEntity? result = await _dbSet
            .FindAsync(new object[] { id! }, cancellationToken);

        _logger.LogInformation("{class} - {method} - Id: {id} - Found: {found}",
            nameof(BaseRepository<TEntity, TKey>),
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
        whereExpression ??= entity => true;
        orderExpression ??= entity => entity.Id!;

        int count = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .CountAsync(cancellationToken);

        List<TEntity> result = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .OrderByDescending(orderExpression)
            .ConfigureQueryPagination(pagination)
            .ToListAsync(cancellationToken);

        Page<TEntity> data = result
            .ToPage(pagination, count);

        _logger.LogInformation("{class} - {method} - Pagination: {@pagination}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(GetPagedAsync),
            data.Pagination);

        return data;
    }

    public async Task<TEntity?> GetSingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> whereExpression,
        CancellationToken cancellationToken = default)
    {
        TEntity? result = await _dbSet
            .AsNoTracking()
            .Where(whereExpression)
            .SingleOrDefaultAsync(cancellationToken);

        _logger.LogInformation("{class} - {method} - Found: {found}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(GetSingleOrDefaultAsync),
            result != null);

        return result;
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);

        _logger.LogInformation("{class} - {method} - Id: {id} - Removed",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(Remove),
            entity.Id);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);

        _logger.LogInformation("{class} - {method} - Id: {id} - Updated",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(Update),
            entity.Id);
    }
}