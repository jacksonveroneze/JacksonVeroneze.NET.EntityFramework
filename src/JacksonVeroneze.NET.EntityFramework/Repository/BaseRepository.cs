namespace JacksonVeroneze.NET.EntityFramework.Repository
{
    public abstract class BaseRepository<TEntity, TType> :
        IBaseRepository<TEntity, TType> where TEntity : BaseEntity<TType>
    {
        protected readonly ILogger<BaseRepository<TEntity, TType>> _logger;
        protected readonly DbContext _context;

        protected readonly DbSet<TEntity> _dbSet;

        public IUnitOfWork UnitOfWork { get; set; }

        protected BaseRepository(
            ILogger<BaseRepository<TEntity, TType>> logger,
            DbContext context,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            _dbSet = context.Set<TEntity>();

            UnitOfWork = unitOfWork;
        }

        public async Task AddAsync(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            _logger.LogInformation("{class} - {method}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(AddAsync));
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);

            _logger.LogInformation("{class} - {method}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(Update));
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);

            _logger.LogInformation("{class} - {method}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(Remove));
        }

        public async ValueTask<TEntity> GetByIdAsync(TType id,
            CancellationToken cancellationToken = default)
        {
            TEntity result = await _dbSet
                .FindAsync(id, cancellationToken);

            _logger.LogInformation("{class} - {method} - Id: {id} - Found: {found}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(GetByIdAsync),
                id,
                result != null);

            return result;
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            List<TEntity> data = await _dbSet
                .Where(expression)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("{class} - {method} - Count: {count}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(GetAllAsync),
                data.Count);

            return data;
        }

        public async Task<Page<TEntity>> GetPagedAsync(
            PaginationParameters pagination,
            Expression<Func<TEntity, bool>> expression = null,
            CancellationToken cancellationToken = default)
        {
            expression ??= entity => true;
            
            Task<int> totalTask = _dbSet
                .AsNoTracking()
                .Where(expression)
                .CountAsync(cancellationToken);

            List<TEntity> result = await _dbSet
                .AsNoTracking()
                .Where(expression)
                .OrderByDescending(x => x.CreatedAt)
                .ConfigurePagination(pagination)
                .ToListAsync(cancellationToken);

            int total = await totalTask;

            int totalPages = total > 0
                ? (int)Math.Ceiling(total / (decimal)(pagination.PageSize))
                : 0;

            Page<TEntity> data = new(result,
                new PageInfo(pagination, totalPages, total));

            _logger.LogInformation("{class} - {method} - Count: {count}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(GetPagedAsync),
                data.Pagination.TotalElements);

            return data;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            int total = await _dbSet
                .AsNoTracking()
                .Where(expression)
                .CountAsync(cancellationToken);

            _logger.LogInformation("{class} - {method} - Count: {count}",
                nameof(BaseRepository<TEntity, TType>),
                nameof(CountAsync),
                total);

            return total;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}