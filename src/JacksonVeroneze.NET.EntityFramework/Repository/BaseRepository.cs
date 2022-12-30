using System.Linq.Expressions;
using JacksonVeroneze.NET.EntityFramework.Context;
using JacksonVeroneze.NET.EntityFramework.DomainObjects;
using JacksonVeroneze.NET.EntityFramework.Extensions;
using JacksonVeroneze.NET.EntityFramework.Interfaces;
using JacksonVeroneze.NET.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.NET.EntityFramework.Repository;

public abstract class BaseRepository<TEntity, TKey> :
    IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IDisposable
{
    protected readonly ILogger<BaseRepository<TEntity, TKey>> _logger;
    protected readonly DatabaseContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    public IUnitOfWork UnitOfWork { get; set; }

    protected BaseRepository(
        ILogger<BaseRepository<TEntity, TKey>> logger,
        DatabaseContext context,
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
            .OrderByDescending(x => x.CreatedAt)
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
        Expression<Func<TEntity, bool>>? expression = null,
        CancellationToken cancellationToken = default)
    {
        expression ??= entity => true;

        int count = await _dbSet
            .AsNoTracking()
            .Where(expression)
            .CountAsync(cancellationToken);

        List<TEntity> result = await _dbSet
            .AsNoTracking()
            .Where(expression)
            .OrderByDescending(x => x.CreatedAt)
            .ConfigurePagination(pagination)
            .ToListAsync(cancellationToken);

        int totalPages = count > 0
            ? (int)Math.Ceiling(count / (decimal)(pagination.PageSize))
            : 0;

        Page<TEntity> data = new(result,
            new PageInfo(pagination, totalPages, (int)count));

        _logger.LogInformation("{class} - {method} - Pagination: {@pagination}",
            nameof(BaseRepository<TEntity, TKey>),
            nameof(GetPagedAsync),
            data.Pagination);

        return data;
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

    public virtual void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}