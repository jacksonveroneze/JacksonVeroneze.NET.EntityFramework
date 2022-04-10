using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.EntityFramework.Data
{
    public interface IRepository<TEntity, in TId> :
        IDisposable where TEntity :
        IAggregateRoot
        where TId : EntityId
    {
        public IUnitOfWork UnitOfWork { get; set; }

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        ValueTask<TEntity> FindAsync(TId simpleId);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> FilterAsync(Pagination.Pagination pagination,
            Expression<Func<TEntity, bool>> expression);

        Task<Pagination.PageResult<TEntity>> FilterPaginateAsync(
            Pagination.Pagination pagination,
            Expression<Func<TEntity, bool>> expression);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
    }
}