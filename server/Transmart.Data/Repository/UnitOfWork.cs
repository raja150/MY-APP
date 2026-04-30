using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TranSmart.Data
{
	public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>
		where TContext : DbContext, IDisposable
	{
		private Dictionary<Type, object> _repositories;

		public UnitOfWork(TContext context)
		{
			Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : Domain.Entities.BaseEntity
		{
			_repositories ??= new Dictionary<Type, object>();

			var type = typeof(TEntity);
			if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(Context);
			return (IRepository<TEntity>)_repositories[type];
		}

		public virtual IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : Domain.Entities.BaseEntity
		{
			_repositories ??= new Dictionary<Type, object>();

			var type = typeof(TEntity);
			if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryAsync<TEntity>(Context);
			return (IRepositoryAsync<TEntity>)_repositories[type];
		}

		public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : Domain.Entities.BaseEntity
		{
			_repositories ??= new Dictionary<Type, object>();

			var type = typeof(TEntity);
			if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryReadOnly<TEntity>(Context);
			return (IRepositoryReadOnly<TEntity>)_repositories[type];
		}

		public virtual TContext Context { get; }

		public virtual int SaveChanges()
		{ 
			return Context.SaveChanges(); 
		}

		public virtual async Task<int> SaveChangesAsync()
		{ 
			return await Context.SaveChangesAsync(); 
		}

		public void Dispose()
		{
			Context?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
