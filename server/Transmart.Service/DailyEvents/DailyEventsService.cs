using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.DailyEvents;
using TranSmart.Domain.Models;

namespace TranSmart.Service.DailyEvents
{
    public interface IDailyEventsService : IBaseService<DailyEvent>
    {

    }
    public class DailyEventsService : BaseService<DailyEvent>, IDailyEventsService
    {
        public DailyEventsService(IUnitOfWork uow) : base(uow)
        {

        }

		public override Task<DailyEvent> GetById(Guid id)
		{
			return UOW.GetRepositoryAsync<DailyEvent>().SingleAsync(
				predicate: x => x.ID == id,
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}

		public override Task<IEnumerable<DailyEvent>> Search(string name)
		{
			return UOW.GetRepositoryAsync<DailyEvent>().GetAsync(x => x.EventName == name);
		}

		public override Task<IPaginate<DailyEvent>> GetPaginate(BaseSearch baseSearch)
		{
			return UOW.GetRepositoryAsync<DailyEvent>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(baseSearch.Name)),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Name", ascending: !baseSearch.IsDescend);
		}
		 
	}
}
