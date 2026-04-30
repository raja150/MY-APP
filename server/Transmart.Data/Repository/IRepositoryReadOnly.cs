using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using TranSmart.Data.Paging;

namespace TranSmart.Data
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : Domain.Entities.BaseEntity
    {
       
    }
}