using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;

namespace TranSmart.Service.Helpdesk
{

	public partial interface IDeskGroupEmployeeService : IBaseService<DeskGroupEmployee>
	{

	}

	public partial class DeskGroupEmployeeService : BaseService<DeskGroupEmployee>, IDeskGroupEmployeeService
	{
		public override async Task CustomValidation(DeskGroupEmployee item, Result<DeskGroupEmployee> result)
		{
			if (await UOW.GetRepositoryAsync<DeskGroupEmployee>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
								&& x.DeskGroupId == item.DeskGroupId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(DeskGroupEmployee.EmployeeId), Resource.Name_already_exists));
			}
			if (await UOW.GetRepositoryAsync<DeskDepartment>().HasRecordsAsync(x => x.ManagerId == item.EmployeeId && x.Status))
			{
				result.AddMessageItem(new MessageItem(nameof(DeskGroupEmployee.EmployeeId), Resource.Selected_User_Is_Manager));

			}
			await base.CustomValidation(item, result);
		}
	}

}
