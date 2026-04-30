using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TranSmart.Data.Paging;
using System.Reflection;
using TranSmart.Domain.Attributes;
using System.Collections.Generic;

namespace TranSmart.Data
{
    public abstract class BaseRepository<T> : IReadRepository<T> where T : Domain.Entities.BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(null, nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> Query(string sql, params object[] parameters) => _dbSet.FromSqlRaw(sql, parameters);

        public virtual T Search(params object[] keyValues) => _dbSet.Find(keyValues);


        public virtual T GetSingle(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            return query.FirstOrDefault();
        } 

        public IEnumerable<TResult> GetWithSelect<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).Select(selector) : query.Select(selector);

        }

        public virtual int GetCount(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            return query.Count();
        }


        public IPaginate<T> GetPageList(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        int index = 0, int size = 10, string sortBy = "AddedAt", bool ascending = true)
        {
            IQueryable<T> query = _dbSet;
            query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = GetOrderByFunc(sortBy, ascending);
            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

		public IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
			int size = 10, bool disableTracking = true)
		{
			IQueryable<T> query = _dbSet;
			if (disableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
		}

		public IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 20, bool disableTracking = true) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginate(index, size)
                : query.Select(selector).ToPaginate(index, size);
        }


        public IPaginate<T> GetRefList(Guid id, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
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

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        public virtual IEnumerable<T> GetAllList(string OrderBy, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            if (string.IsNullOrEmpty(OrderBy))
            {
                return query.ToList();
            }
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = GetOrderByFunc(OrderBy, true);
            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public IEnumerable<TEntity> ExecuteStoreQuery<TEntity>(String commandText) where TEntity : class
        {
            return _dbContext.Set<TEntity>().FromSqlRaw(commandText);
        }

        private static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByFunc(string name, bool ascending)
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
