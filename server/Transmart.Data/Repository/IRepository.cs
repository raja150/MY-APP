using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository;

namespace TranSmart.Data
{
    public interface IRepository<T> : IReadRepository<T>, IDisposable where T : Domain.Entities.BaseEntity
    {
        void Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);
        void Add(T entity, Guid id);


        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
        
        
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities); 

        IEnumerable<T> GetAll(ISpecification<T> specification=null); 
        bool HasRecords(Expression<Func<T, bool>> predicate);
        decimal SumOfDecimal(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> sumBy);
    }
}
