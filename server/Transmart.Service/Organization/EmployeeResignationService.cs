using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Service.Organization
{
    public partial class EmployeeResignationService : IBaseService<EmployeeResignation>
    {

    }

    public partial class EmployeeResignationService : BaseService<EmployeeResignation>, IEmployeeResignationService
    {

        public override async Task CustomValidation(EmployeeResignation item, Result<EmployeeResignation> result)
        {
            Employee resign = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == item.EmployeeId);
            if (resign == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Employee));
            }
            if (resign != null && resign.LastWorkingDate > item.ApprovedOn)
            {
                result.AddMessageItem(new MessageItem(Resource.Resignation_Approval_Is_Greater_Than_Are_Equal_To_Resigned));
            }
            await base.CustomValidation(item, result);
        }



    }
}