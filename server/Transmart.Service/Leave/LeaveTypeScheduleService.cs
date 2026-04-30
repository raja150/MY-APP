using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Service.Leave
{
	public partial class LeaveTypeScheduleService : BaseService<LeaveTypeSchedule>, ILeaveTypeScheduleService
	{
		public override async Task CustomValidation(LeaveTypeSchedule item, Result<LeaveTypeSchedule> result)
		{
			if (await UOW.GetRepositoryAsync<LeaveTypeSchedule>().HasRecordsAsync(x => x.ID != item.ID && x.LeaveTypeId == item.LeaveTypeId && x.AccOnQuarterly == item.AccOnQuarterly))
			{
				result.AddMessageItem(new MessageItem(nameof(LeaveTypeSchedule), Resource.Already_Record_Exist));
			}
			await base.CustomValidation(item, result);
		}
	}
}
