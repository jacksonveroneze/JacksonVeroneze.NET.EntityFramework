using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using JacksonVeroneze.NET.Commons.DomainObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.NET.EntityFramework.Data
{
    public abstract class Repository<TEntity, TId> :
        IRepository<TEntity, TId>
        where TEntity : Entity, IAggregateRoot
        where TId : EntityId
    {
        protected readonly ILogger<Repository<TEntity, TId>> Logger;
        protected readonly DbContext Context;

        protected readonly DbSet<TEntity> DbSet;

        public IUnitOfWork UnitOfWork { get; set; }

        protected Repository(
            ILogger<Repository<TEntity, TId>> logger,
            DbContext context,
            IUnitOfWork unitOfWork)
        {
            Logger = logger;
            Context = context;
            UnitOfWork = unitOfWork;

            DbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);

            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(AddAsync));
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);

            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(Update));
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);

            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(Remove));
        }

        public ValueTask<TEntity> FindAsync(TId simpleId)
        {
            Logger.LogInformation("{class} - {method} - Id: {id}",
                nameof(Repository<TEntity, TId>), nameof(FindAsync), simpleId);

            return DbSet.FindAsync(simpleId.Id);
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(FindAsync));

            return BuidQueryable(new Pagination.Pagination(), expression)
                .FirstOrDefaultAsync();
        }

        public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(FilterAsync));

            return BuidQueryable(new Pagination.Pagination(), expression)
                .ToListAsync();
        }

        public Task<List<TEntity>> FilterAsync(Pagination.Pagination pagination,
            Expression<Func<TEntity, bool>> expression)
        {
            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(FilterAsync));

            return BuidQueryable(pagination, expression)
                .ToListAsync();
        }

        public async Task<Pagination.PageResult<TEntity>> FilterPaginateAsync(Pagination.Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            Logger.LogInformation("{class} - {method}",
                nameof(Repository<TEntity, TId>), nameof(FilterPaginateAsync));

            int total = await CountAsync(expression);

            List<TEntity> data = await BuidQueryable(pagination, expression)
                .ToListAsync();

            return FactoryPageable(data, total, pagination.Page,
                pagination.PageSize);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet
                .AsNoTracking()
                .Where(expression)
                .CountAsync();
        }

        private IQueryable<TEntity> BuidQueryable(Pagination.Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            return DbSet
                .Where(expression)
                .OrderByDescending(x => x.CreatedAt)
                .ConfigureSkipTake(pagination);
        }

        protected Pagination.PageResult<TType> FactoryPageable<TType>(
            IList<TType> data, int total, int page, int pageSize) where TType : class
        {
            return new()
            {
                Data = data,
                TotalElements = total,
                TotalPages = total > 0 ? (int)Math.Ceiling(total / (decimal)(pageSize)) : 0,
                CurrentPage = page <= 0 ? 1 : page
            };
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}