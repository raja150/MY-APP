using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models.Helpdesk;

namespace TranSmart.Service.Helpdesk
{
    public partial interface IHelpTopicSubService : IBaseService<HelpTopicSub>
    {
        Task<IEnumerable<HelpTopicSub>> GetHelpTopicSubs(Guid helptopicId);
        
    }
    public  partial class HelpTopicSubService : BaseService<HelpTopicSub>
    {
        public async Task<IEnumerable<HelpTopicSub>> GetHelpTopicSubs(Guid helptopicId)
        {
            return await UOW.GetRepositoryAsync<HelpTopicSub>().GetAsync(x => x.HelpTopicId == helptopicId);
        }
        public override async Task CustomValidation(HelpTopicSub item, Result<HelpTopicSub> result)
        {
            if (await UOW.GetRepositoryAsync<HelpTopicSub>().HasRecordsAsync(x => x.SubTopic == item.SubTopic && x.ID != item.ID))
            {
                result.AddMessageItem(new MessageItem(nameof(HelpTopicSubModel.SubTopic), Resource.Name_already_exists));
            }

        }
    }
}
