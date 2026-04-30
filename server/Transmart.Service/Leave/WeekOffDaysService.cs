using System;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Leave
{
	public partial interface IWeekOffDaysService : IBaseService<WeekOffDays>
	{
		Task<IPaginate<WeekOffDays>> GetAllWeekOffDays(BaseSearch search);
		Task<Result<WeekOffDays>> DeleteWeekOffDays(Guid id);

	}
	public partial class WeekOffDaysService : BaseService<WeekOffDays>, IWeekOffDaysService
	{

		public override async Task CustomValidation(WeekOffDays item, Result<WeekOffDays> result)
		{
			switch (item.Type)
			{
				case 1:
				case 2:
					if (!item.WeekDay.HasValue)
					{
						result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekDay), Resource.Week_Day_Required));
					}
					if (item.Type == 1 && string.IsNullOrEmpty(item.WeekNoInMonth))
					{
						result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekNoInMonth), Resource.Week_No_In_Month));
					}
					else if (item.Type == 2 && !item.WeekInYear.HasValue)
					{
						result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekInYear), Resource.Week_In_Year));
					}
					break;
				case 3:
					if (!item.WeekDate.HasValue)
					{
						result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekDate), Resource.Week_Date));
					}
					if (!item.Status.HasValue)
					{
						result.AddMessageItem(new MessageItem(nameof(WeekOffDays.Status), Resource.Status));
					}
					break;
			}
			if (!result.HasError)
			{
				switch (item.Type)
				{
					case 1:
					case 2:
						if (await UOW.GetRepositoryAsync<WeekOffDays>().HasRecordsAsync(x => x.WeekOffSetupId == item.WeekOffSetupId && x.WeekDay == item.WeekDay))
						{
							result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekDay), Resource.WEEK_DAY_ALREADY_EXIST));
						}
						break;
					case 3:
						if (await UOW.GetRepositoryAsync<WeekOffDays>().HasRecordsAsync(x => x.WeekOffSetupId == item.WeekOffSetupId && x.WeekDate == item.WeekDate))
						{
							result.AddMessageItem(new MessageItem(nameof(WeekOffDays.WeekDate), Resource.WEEK_DATE_ALREADY_EXIST));
						}
						break;
				}
			}
			await base.CustomValidation(item, result);
		}


		public async Task<IPaginate<WeekOffDays>> GetAllWeekOffDays(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<WeekOffDays>().GetPaginateAsync(
				predicate: x => x.WeekOffSetupId == search.RefId,
				orderBy: o => o.OrderBy(x => x.Type).ThenByDescending(x => x.WeekDate),
				index: search.Page, size: search.Size);  //, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend
		}

		public async Task<Result<WeekOffDays>> DeleteWeekOffDays(Guid id)
		{
			Result<WeekOffDays> result = new Result<WeekOffDays>();
			var weekOffDays = await UOW.GetRepositoryAsync<WeekOffDays>().SingleAsync(x => x.ID == id);
			if (weekOffDays == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
				return result;
			}
			try
			{
				UOW.GetRepositoryAsync<WeekOffDays>().DeleteAsync(weekOffDays);
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
	}
}
