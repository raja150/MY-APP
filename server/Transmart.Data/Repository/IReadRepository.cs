using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using TranSmart.Data.Paging;
using System.Collections.Generic;

namespace TranSmart.Data
{
	public interface IReadRepository<T> where T : Domain.Entities.BaseEntity
	{

		IQueryable<T> Query(string sql, params object[] parameters);

		T Search(params object[] keyValues);

		T GetSingle(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool disableTracking = true);

		IEnumerable<TResult> GetWithSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null); 

		int GetCount(Expression<Func<T, bool>> predicate = null);

		IPaginate<T> GetPageList(Expression<Func<T, bool>> predicate = null,
		Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
		int index = 0, int size = 10, string sortBy = "AddedAt", bool ascending = true);
		IPaginate<T> GetRefList(Guid id, Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 0,
			int size = 10,
			bool disableTracking = true);

		IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 0,
			int size = 10,
			bool disableTracking = true);

		IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
			Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 0,
			int size = 20,
			bool disableTracking = true) where TResult : class;

		IEnumerable<T> GetAllList(string OrderBy, Expression<Func<T, bool>> predicate = null);

		IEnumerable<TEntity> ExecuteStoreQuery<TEntity>(String commandText) where TEntity : class;
	}
}
