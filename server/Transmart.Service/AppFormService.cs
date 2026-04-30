using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;

namespace TranSmart.Service
{
    public class AppFormService : BaseService<AppForm>
    {
        public AppFormService(IUnitOfWork uow) : base(uow)
        {

        } 
    }

}
