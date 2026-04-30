using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TranSmart.Data.Repository;
using Microsoft.EntityFrameworkCore;
using TranSmart.Data.Paging;

namespace TranSmart.Data
{
	public class Repository<T> : BaseRepository<T>, IRepository<T> where T : Domain.Entities.BaseEntity
	{
		public Repository(DbContext context) : base(context)
		{
		}

		public virtual void Add(T entity)
		{
			entity.ID = Guid.NewGuid();
			_dbSet.Add(entity);
		}

		public void Add(params T[] entities)
		{
			_dbSet.AddRange(entities);
		}


		public void Add(IEnumerable<T> entities)
		{
			_dbSet.AddRange(entities);
		}

		public void Add(T entity, Guid id)
		{
			entity.ID = id;
			_dbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			var existing = _dbSet.Find(entity.ID);
			if (existing != null) _dbSet.Remove(existing);
		}

		public void Delete(object id)
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
				if (entity != null) Delete(entity);
			}
		}

		public void Delete(params T[] entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public void Delete(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public virtual void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public void Update(params T[] entities)
		{
			_dbSet.UpdateRange(entities);
		}

		public void Update(IEnumerable<T> entities)
		{
			_dbSet.UpdateRange(entities);
		}

		public void Dispose()
		{
			_dbContext?.Dispose();
			GC.SuppressFinalize(this);
		}

		public IEnumerable<T> GetAll(ISpecification<T> specification = null)
		{
			return ApplySpecification(specification);
		}

		private IEnumerable<T> ApplySpecification(ISpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
		}

		public virtual bool HasRecords(Expression<Func<T, bool>> predicate)
		{
			IQueryable<T> query = _dbSet;
			query = query.AsNoTracking();

			if (predicate != null) query = query.Where(predicate);

			return query.Any();
		}

		public decimal SumOfDecimal(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> sumBy)
		{
			IQueryable<T> query = _dbSet;
			query = query.AsNoTracking();

			if (predicate != null) query = query.Where(predicate);

			return query.Sum(sumBy);
		}
	}
}
