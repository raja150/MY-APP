using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Leave
{
    public partial interface IHolidaysService : IBaseService<Holidays>
    {
        Task<IPaginate<Holidays>> Future(BaseSearch search);
        Task<IPaginate<Holidays>> Past(BaseSearch search);
    }

    public partial class HolidaysService : BaseService<Holidays>, IHolidaysService
    {
        public override async Task CustomValidation(Holidays item, Result<Holidays> result)
        {
            await base.CustomValidation(item, result);
            if (await UOW.GetRepositoryAsync<Holidays>().HasRecordsAsync(x => x.Date == item.Date && x.ID != item.ID))
            {
                result.AddMessageItem(new MessageItem(nameof(Holidays.Date), Resource.Date_Is_Already_Exist));
            }
        }
        public async Task<IPaginate<Holidays>> Future(BaseSearch search)
        {
            HolidaysSearch holidaysSearch = (HolidaysSearch)search;
            var result = await UOW.GetRepositoryAsync<Holidays>().GetPageListAsync(
                predicate: x => ( x.Date.Date > DateTime.Today
                                 && (holidaysSearch.FromDate == null || x.Date.Date >= holidaysSearch.FromDate.Value.Date)
                                 && (holidaysSearch.ToDate == null || x.Date.Date <= holidaysSearch.ToDate.Value.Date)),
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Date", ascending: !search.IsDescend);//string.IsNullOrEmpty(search.Name) || x.Name.Contains(search.Name) &&
            return result;
        }
        public async Task<IPaginate<Holidays>> Past(BaseSearch search)
        {
            HolidaysSearch holidaysSearch = (HolidaysSearch)search;
            var result = await UOW.GetRepositoryAsync<Holidays>().GetPageListAsync(
                predicate: x => (x.Date.Date < DateTime.Today
                                 && (holidaysSearch.FromDate == null || x.Date.Date >= holidaysSearch.FromDate.Value.Date)
                                 && (holidaysSearch.ToDate == null || x.Date.Date <= holidaysSearch.ToDate.Value.Date)),
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Date", ascending: !search.IsDescend);
            return result;
        }
    }

}
