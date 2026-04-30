using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository;

namespace TranSmart.Data
{
    public interface IRepositoryAsync<T> where T : Domain.Entities.BaseEntity
    {
		IQueryable<T> Queryable(Expression<Func<T, bool>> predicate = null,
		   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		   Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true);

		Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        Task<IEnumerable<T>> GetListAsync(string orderBy, Expression<Func<T, bool>> predicate = null);
        Task<IPaginate<T>> GetRefListAsync(Guid id, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 10, bool disableTracking = true);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<IEnumerable<TResult>> GetWithSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<IPaginate<T>> GetPaginateAsync(Expression<Func<T, bool>> predicate = null,
               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
               Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
               int index = 0, int size = 20,
               CancellationToken cancellationToken = default);
        Task<IPaginate<T>> GetPageListAsync(Expression<Func<T, bool>> predicate = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
          int index = 0, int size = 10, string sortBy = "AddedAt", bool ascending = true
          );//CancellationToken cancellationToken = default(CancellationToken)
		Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task AddAsync(params T[] entities);
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, Guid id, CancellationToken cancellationToken = default);

        void UpdateAsync(T entity);
        void UpdateAsync(IEnumerable<T> entities);

        void DeleteAsync(object id);
        void DeleteAsync(T entity);

        Task<bool> HasRecordsAsync(Expression<Func<T, bool>> predicate);
        Task<decimal> SumOfDecimalAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> sumBy);
        Task<int> SumOfIntAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> sumBy); 
    }
}
