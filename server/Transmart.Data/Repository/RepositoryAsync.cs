using TranSmart.Data.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TranSmart.Domain.Entities;
using TranSmart.Data.Repository;
using System.Reflection;
using TranSmart.Domain.Attributes;

namespace TranSmart.Data
{
	public class RepositoryAsync<T> : IRepositoryAsync<T> where T : Domain.Entities.BaseEntity
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		public RepositoryAsync(DbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		public virtual IQueryable<T> Queryable(Expression<Func<T, bool>> predicate = null,
		   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		   Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
		{
			IQueryable<T> query = _dbSet;
			if (disableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
			{
				return orderBy(query);
			}

			return query;
		}

		public async virtual Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
		{
			IQueryable<T> query = _dbSet;
			if (disableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
			{
				return await orderBy(query).FirstOrDefaultAsync();
			}

			return await query.FirstOrDefaultAsync();
		}
		public async virtual Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
		}

		public async Task<IEnumerable<TResult>> GetWithSelectAsync<TResult>(Expression<Func<T, TResult>> selector,
		 Expression<Func<T, bool>> predicate = null,
		 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		 Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			return orderBy != null ? await orderBy(query).Select(selector).ToListAsync() : await query.Select(selector).ToListAsync();
		}

		public async virtual Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();
			if (predicate != null) query = query.Where(predicate);
			return await query.CountAsync();
		}

		public async Task<IEnumerable<T>> GetListAsync(string orderBy, Expression<Func<T, bool>> predicate = null)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();
			if (predicate != null) query = query.Where(predicate);
			if (string.IsNullOrEmpty(orderBy))
			{
				return await query.ToListAsync();
			}
			Func<IQueryable<T>, IOrderedQueryable<T>> orderByExp = GetOrderByFunc(orderBy, true);
			return orderByExp != null ? await orderByExp(query).ToListAsync() : await query.ToListAsync();
		}

		public async Task<IPaginate<T>> GetRefListAsync(Guid id, Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 0, int size = 10, bool disableTracking = true)
		{
			IQueryable<T> query = _dbSet;
			if (disableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			RefColumnAttribute attribute = typeof(T).GetCustomAttribute<RefColumnAttribute>();
			if (attribute == null)
				throw new ArgumentException("Unable to find reference column");
			var parameterExpresion = Expression.Parameter(typeof(T), "d");
			var binaryExpression = Expression.Equal(
				Expression.Property(parameterExpresion, attribute.ColumnName),
				Expression.Constant(id));
			var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpresion);
			if (lambda != null) query = query.Where(lambda);

			return orderBy != null ? await orderBy(query).ToPaginateAsync(index, size) : await query.ToPaginateAsync(index, size);
		}

		public virtual async Task<IPaginate<T>> GetPaginateAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 0, int size = 20,
			CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
			{
				return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
			}

			return await query.ToPaginateAsync(index, size, 0, cancellationToken);
		}

		public virtual async Task<IPaginate<T>> GetPageListAsync(Expression<Func<T, bool>> predicate = null,
		  Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
		  int index = 0, int size = 10, string sortBy = "AddedAt", bool ascending = true)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = GetOrderByFunc(sortBy, ascending);
			return orderBy != null ? await orderBy(query).ToPaginateAsync(index, size) : await query.ToPaginateAsync(index, size);
		}
		public async virtual Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}
		public async virtual Task AddAsync(T entity, CancellationToken cancellationToken)
		{
			await _dbSet.AddAsync(entity, cancellationToken);
		}
		public async Task AddAsync(T entity, Guid id, CancellationToken cancellationToken = default)
		{
			entity.ID = id;
			await _dbSet.AddAsync(entity, CancellationToken.None);
		}
		public Task AddAsync(params T[] entities)
		{
			return _dbSet.AddRangeAsync(entities);
		}

		public Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
		{
			return _dbSet.AddRangeAsync(entities, cancellationToken);
		}

		public void UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
		}

		public void UpdateAsync(IEnumerable<T> entities)
		{
			_dbSet.UpdateRange(entities);
		}
		public virtual void DeleteAsync(T entity)
		{
			var existing = _dbSet.Find(entity.ID);
			if (existing != null) _dbSet.Remove(existing);
		}
		public void DeleteAsync(object id)
		{
			var typeInfo = typeof(T).GetTypeInfo();
			var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
			var property = typeInfo.GetProperty(key?.Name);
			if (property != null)
			{
				var entity = Activator.CreateInstance<T>();
				property.SetValue(entity, id);
				_dbContext.Entry(entity).State = EntityState.Deleted;
			}
			else
			{
				var entity = _dbSet.Find(id);
				if (entity != null) DeleteAsync(entity);
			}
		}

		public virtual async Task<bool> HasRecordsAsync(Expression<Func<T, bool>> predicate)
		{
			IQueryable<T> query = _dbSet;
			query = query.AsNoTracking();

			if (predicate != null) query = query.Where(predicate);

			return await query.AnyAsync();
		}
		public async virtual Task<decimal> SumOfDecimalAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> sumBy)
		{
			IQueryable<T> query = _dbSet;
			query = query.AsNoTracking();

			if (predicate != null) query = query.Where(predicate);

			return await query.SumAsync(sumBy);
		}

		public async Task<int> SumOfIntAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> sumBy)
		{
			IQueryable<T> query = _dbSet;
			query = query.AsNoTracking();

			if (predicate != null) query = query.Where(predicate);

			return await query.SumAsync(sumBy);
		}

		public Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByFunc(string name, bool ascending)
		{
			Type typeQueryable = typeof(IQueryable<T>);
			ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
			var outerExpression = Expression.Lambda(argQueryable, argQueryable);
			string[] props = name.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			foreach (string prop in props)
			{
				PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
				if (pi == null) return null;
				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}
			LambdaExpression lambda = Expression.Lambda(expr, arg);
			string methodName = ascending ? "OrderBy" : "OrderByDescending";

			MethodCallExpression resultExp =
				Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), type }, outerExpression.Body, Expression.Quote(lambda));
			var finalLambda = Expression.Lambda(resultExp, argQueryable);
			return (Func<IQueryable<T>, IOrderedQueryable<T>>)finalLambda.Compile();

		}
	}
}
