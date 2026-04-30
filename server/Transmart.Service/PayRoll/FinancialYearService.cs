using System;
using TranSmart.Core.Result;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TranSmart.Data.Paging;
using System.Linq;
using TranSmart.Domain.Models;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using System.Threading.Tasks;

namespace TranSmart.Service.Payroll
{
	public partial interface IFinancialYearService : IBaseService<FinancialYear>
	{
		Task<IEnumerable<FinancialYear>> OpenYears();
	}

	public partial class FinancialYearService : BaseService<FinancialYear>, IFinancialYearService
	{
		public override async Task CustomValidation(FinancialYear item, Result<FinancialYear> result)
		{
			int months = await UOW.GetRepositoryAsync<PayMonth>().GetCountAsync(x => x.Status == (int)PayMonthStatus.Active ||
			x.Status == (int)PayMonthStatus.Open || x.Status == (int)PayMonthStatus.InProcess);
			if (months > 0)
			{
				result.AddMessageItem(new MessageItem
					(nameof(FinancialYear.FromYear), Resource.The_Previous_Financial_Year_Still_In_Process_));
			}
			if (result.HasNoError)
			{
				if (item.FromYear < DateTime.Now.AddYears(-2).Year)
				{
					result.AddMessageItem(new MessageItem
						(nameof(FinancialYear.FromYear), $"Financial year from should not be less than {DateTime.Now.AddYears(-2).Year}."));
				}

				if ((item.ToYear - item.FromYear) != 1)
				{
					result.AddMessageItem(new MessageItem
						(nameof(FinancialYear.ToYear), Resource.The_Financial_Year_Should_Have_Only_12_Months_));
				}

				int count = await UOW.GetRepositoryAsync<FinancialYear>().GetCountAsync(x => x.FromYear == item.FromYear && x.ToYear == item.ToYear);
				if (count > 0)
				{
					result.AddMessageItem(new MessageItem
					   (nameof(FinancialYear.ToYear), Resource.The_Financial_Year_Already_Added_));
				}
			}

			//While edit
			FinancialYear financialYear = await UOW.GetRepositoryAsync<FinancialYear>().SingleAsync(x => x.ID == item.ID);

			if (financialYear != null)
			{
				if (item.FromYear != financialYear.FromYear)
				{
					result.AddMessageItem(new MessageItem
						(nameof(FinancialYear.FromYear), Resource.Financial_From_Year_Is_Not_Editable));
				}
				if (item.ToYear != financialYear.ToYear)
				{
					result.AddMessageItem(new MessageItem
						(nameof(FinancialYear.ToYear), Resource.Financial_From_Year_Is_Not_Editable));
				}
			}
		}

		/// <summary>
		/// Adding Pay month records when a financial year is creating
		/// </summary>
		/// <param name="item"></param>
		/// <param name="executionResult"></param>
		public override async Task OnBeforeAdd(FinancialYear item, Result<FinancialYear> executionResult)
		{
			await base.OnBeforeAdd(item, executionResult);

			if (!executionResult.HasNoError)
			{
				return;
			}
			PaySettings paySettings = await UOW.GetRepositoryAsync<PaySettings>().SingleAsync(x => x.ID == item.PaySettingsId,
				include: x => x.Include(i => i.Organization));
			if (paySettings == null)
			{
				executionResult.AddMessageItem(new MessageItem
							(nameof(FinancialYear.FromYear), Resource.Invalid));
				return;
			}
			//[Mohan] Unknown code. 
			//IEnumerable<PayMonth> months = await _UOW.GetRepositoryAsync<PayMonth>().GetAsync(x =>
			//    x.FinancialYear.PaySettingsId == item.PaySettingsId);
			//if (months.Any())
			//{
			//    executionResult.AddMessageItem(new MessageItem
			//                 (nameof(FinancialYear.FromYear), Resource.Invalid));
			//    return;
			//}
			int day = paySettings.Organization.MonthStartDay;
			DateTime monthStart = new(item.FromYear, paySettings.FYFromMonth, day);
			if (day != 1)
			{
				monthStart = new DateTime(item.FromYear, paySettings.FYFromMonth, day).AddMonths(-1);
			}

			item.FromDate = new DateTime(item.FromYear, paySettings.FYFromMonth, 1);
			item.ToDate = item.FromDate.AddMonths(12).AddDays(-1);

			for (int i = 0; i < 12; i++)
			{
				DateTime start = monthStart.AddMonths(i);
				DateTime end = monthStart.AddMonths(i + 1).AddDays(-1);
				await UOW.GetRepositoryAsync<PayMonth>().AddAsync(new PayMonth
				{
					Month = end.Month,
					Year = end.Year,
					Start = start,
					End = end,
					Name = $"{end:MMMM} - {end.Year}",
					Status = (int)(i == 0 ? PayMonthStatus.Open : PayMonthStatus.Active),
					FinancialYear = item,
					Days = (end - start).Days + 1,
				});
			}
		}



		public async Task<IEnumerable<FinancialYear>> OpenYears()
		{
			return await UOW.GetRepositoryAsync<FinancialYear>().GetAsync(x => !x.Closed, include: i => i.Include(x => x.PaySettings));
		}
	}
}
