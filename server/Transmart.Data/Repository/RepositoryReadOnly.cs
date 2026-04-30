using Microsoft.EntityFrameworkCore;

namespace TranSmart.Data
{
    public class RepositoryReadOnly<T> : BaseRepository<T>, IRepositoryReadOnly<T> where T : Domain.Entities.BaseEntity
    {
        public RepositoryReadOnly(DbContext context) : base(context)
        {
        }
    }
}