using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;

namespace TranSmart.Service.Helpdesk
{
	public partial interface IDeskGroupService : IBaseService<DeskGroup>
	{

	}
	public partial class DeskGroupService : BaseService<DeskGroup>, IDeskGroupService
	{
		public override async Task CustomValidation(DeskGroup item, Result<DeskGroup> result)
		{
			if (await UOW.GetRepositoryAsync<DepartmentGroup>().HasRecordsAsync(x => x.GroupsId == item.ID) && !item.Status)
			{
				result.AddMessageItem(new MessageItem("This group is used in some department(s)"));
			}
			await base.CustomValidation(item, result);
		}
	}
}
