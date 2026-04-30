using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TranSmart.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : Domain.Entities.BaseEntity;
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : Domain.Entities.BaseEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
