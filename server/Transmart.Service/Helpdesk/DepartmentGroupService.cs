using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;

namespace TranSmart.Service.Helpdesk
{

	public partial interface IDepartmentGroupService : IBaseService<DepartmentGroup>
	{

	}

	public partial class DepartmentGroupService : BaseService<DepartmentGroup>, IDepartmentGroupService
	{
		public override async Task CustomValidation(DepartmentGroup item, Result<DepartmentGroup> result)
		{
			if (await UOW.GetRepositoryAsync<DepartmentGroup>().HasRecordsAsync(x => x.GroupsId == item.GroupsId
							&& x.DeskDepartmentId == item.DeskDepartmentId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(DepartmentGroup.GroupsId), Resource.Name_already_exists));
			}

		}
	}
}
