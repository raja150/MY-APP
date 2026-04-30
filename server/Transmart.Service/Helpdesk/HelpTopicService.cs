using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;

namespace TranSmart.Service.Helpdesk
{
	public partial interface IHelpTopicService : IBaseService<HelpTopic>
	{
		Task<IEnumerable<HelpTopic>> GetHelpTopics(Guid departmentId);
	}
	public partial class HelpTopicService : BaseService<HelpTopic>, IHelpTopicService
	{
		public async Task<IEnumerable<HelpTopic>> GetHelpTopics(Guid departmentId)
		{

			return await UOW.GetRepositoryAsync<HelpTopic>().GetAsync(x => x.DepartmentId == departmentId && x.Status);
		}
		public override async Task CustomValidation(HelpTopic item, Result<HelpTopic> result)
		{
			if (await UOW.GetRepositoryAsync<TicketStatus>().HasRecordsAsync(x => x.ID == item.TicketStatusId && x.IsClosed))
			{
				result.AddMessageItem(new MessageItem(nameof(HelpTopic.TicketStatusId), "closed status not allowed"));
			}
		  await base.CustomValidation(item, result);
		}
	}

}
