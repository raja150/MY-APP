using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities;

namespace TranSmart.API.ManageServices
{
    public interface IManageBaseService<T> where T : BaseEntity
    {
        public Task UpdateAsync(T item);
        public Task AddAsync(T item);
        public Task GetAsync(Guid id);
        public Task ClearCache(string key);
        public Task UpdateCache(string key, T item);

    }
    public class ManageBaseService<T> : IManageBaseService<T> where T : BaseEntity
    {
        public virtual Task AddAsync(T item)
        {
            return Task.CompletedTask;
        }

        public virtual Task ClearCache(string key)
        {
            return Task.CompletedTask;
        }

        public virtual Task GetAsync(Guid id)
        {
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(T item)
        {
            return Task.CompletedTask;
        }

        public virtual Task UpdateCache(string key, T item)
        {
            return Task.CompletedTask;
        }
    }
}
