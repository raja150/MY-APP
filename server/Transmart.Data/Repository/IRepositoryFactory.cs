namespace TranSmart.Data
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : Domain.Entities.BaseEntity;
        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : Domain.Entities.BaseEntity;
		IRepositoryReadOnly<T> GetReadOnlyRepository<T>() where T : Domain.Entities.BaseEntity;
	}
}
