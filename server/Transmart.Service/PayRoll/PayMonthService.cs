using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.Payroll.Response;

namespace TranSmart.Service.Payroll
{
	public interface IPayMonthService : IBaseService<PayMonth>
	{
		Task<PayMonth> GetPayMonth(Guid Id);
		Task<PaySheet> GetEmployeePay(Guid Id, Guid EmployeeId);
		Task<IPaginate<PaySheet>> GetPaySheets(BaseSearch search);
		Task<IPaginate<PaySheet>> GetHoldPaysheet(BaseSearch search);
		Task HoldSalary(Guid id);
		Task<IEnumerable<PaySheet>> GetPaysheet(Guid Id);
		Task<IEnumerable<PaySheet>> GetSheets(int Year, int Month);
		Task<Result<PaySheet>> PaySlipSendedOn(Guid paySheetId);
		Task<PayMonth> GetPayMonthDetails(int Year, int Month);
		Task<Result<PaySheet>> AddUpdatePaysheet(List<PaySheet> psheet);
		Task<Result<PaySheet>> ReleaseSalary(Guid id);
		Task<TextDetailsModel> TaxDeductions(Guid id);
		Task<IEnumerable<PayMonth>> GetPayMonths();
		Task<IEnumerable<PayMonth>> GetAllMonths();
		Task<bool> CheckPayMonthIsOpen(int year, int month);
		Task<IEnumerable<PaySheet>> GetEmployeeAnnualSalary(Guid empId, Guid fyId);
		Task<List<dynamic>> GetEmpAnnualSalary(Guid empId, Guid fyId);
		Task<IEnumerable<PaySheet>> GetSalarySlips(Guid EmployeeId);
		Task<IPaginate<PayMonth>> GetPayMonthList(Guid? id, BaseSearch baseSearch);
	}
	public class PayMonthService : BaseService<PayMonth>, IPayMonthService
	{
		public PayMonthService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<PayMonth> GetPayMonth(Guid Id)
		{
			return await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == Id);

		}
		public async Task<IEnumerable<PayMonth>> GetPayMonths()
		{
			return await UOW.GetRepositoryAsync<PayMonth>().GetAsync(x => x.Status == (int)PayMonthStatus.Open
			|| x.Status == (int)PayMonthStatus.InProcess);
		}
		public async Task<IPaginate<PayMonth>> GetPayMonthList(Guid? id, BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<PayMonth>().GetPaginateAsync(x => (!id.HasValue || x.FinancialYearId == id.Value)
			&& (x.Status >= (int)PayMonthStatus.Open) && (!x.FinancialYear.Closed),
			orderBy: o => o.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month),
			  index: baseSearch.Page, size: baseSearch.Size);
		}

		public async Task<PayMonth> GetPayMonthDetails(int Year, int Month)
		{
			return await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.Month == Month && x.Year == Year);
		}
		public override async Task<PayMonth> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == id);
		}
		public async Task<IPaginate<PaySheet>> GetPaySheets(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<PaySheet>().GetPageListAsync(
			   predicate: x => x.PayMonthId == search.RefId.Value && !x.Hold
			   && (string.IsNullOrEmpty(search.Name) || x.Employee.No.Contains(search.Name) || x.Employee.Name.Contains(search.Name)),
			   include: o => o
			   .Include(x => x.PayMonth)
			   .Include(x => x.Employee)
			   .Include(x => x.Employee).ThenInclude(x => x.Department)
			   .Include(x => x.Employee).ThenInclude(x => x.Designation)
			   .Include(x => x.Employee).ThenInclude(x => x.WorkLocation),
			   index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Employee.Name", ascending: !search.IsDescend);
		}
		public async Task<IEnumerable<PaySheet>> GetSheets(int Year, int Month)
		{
			var payMonthId = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.Year == Year && x.Month == Month);
			return await UOW.GetRepositoryAsync<PaySheet>().GetAsync(x => x.PayMonthId == payMonthId.ID,
													include: o => o.Include(x => x.Employee).Include(x => x.PayMonth)
													.Include(x => x.Earnings).ThenInclude(x => x.Component)
													.Include(x => x.Deductions).ThenInclude(x => x.Component)
													.Include(x => x.Employee).ThenInclude(x => x.Department)
													.Include(x => x.Employee).ThenInclude(x => x.Designation)
													.Include(x => x.Employee).ThenInclude(x => x.WorkLocation));
		}

		public async Task<IPaginate<PaySheet>> GetHoldPaysheet(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<PaySheet>().GetPageListAsync(
				 predicate: x => x.PayMonthId == search.RefId.Value && x.Hold
				  && (string.IsNullOrEmpty(search.Name) || x.Employee.No.Contains(search.Name) || x.Employee.Name.Contains(search.Name)),
				   include: o => o
				   .Include(x => x.PayMonth)
				   .Include(x => x.Employee)
				   .Include(x => x.Employee).ThenInclude(x => x.Department)
				   .Include(x => x.Employee).ThenInclude(x => x.Designation)
				   .Include(x => x.Employee).ThenInclude(x => x.WorkLocation),
				 index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Employee.Name", ascending: !search.IsDescend);
		}

		public async Task HoldSalary(Guid id)
		{
			PaySheet entity = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.ID == id);
			if (entity != null)
			{
				entity.Hold = true;
				UOW.GetRepositoryAsync<PaySheet>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
			}
		}
		public async Task<Result<PaySheet>> ReleaseSalary(Guid id)
		{
			Result<PaySheet> result = new();
			PaySheet entity = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.ID == id);
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_PaySheet));
				return result;
			}
			entity.Hold = false;
			result.ReturnValue = entity;
			UOW.GetRepositoryAsync<PaySheet>().UpdateAsync(entity);
			await UOW.SaveChangesAsync();
			return result;
		}

		public async Task<IEnumerable<PaySheet>> GetPaysheet(Guid Id)
		{
			return await UOW.GetRepositoryAsync<PaySheet>().GetAsync(x => x.PayMonthId == Id,
		   include: o => o.Include(x => x.Employee).Include(x => x.PayMonth)
			  .Include(x => x.Earnings).ThenInclude(x => x.Component)
			  .Include(x => x.Deductions).ThenInclude(x => x.Component)
			  .Include(x => x.Employee).ThenInclude(x => x.Department)
			  .Include(x => x.Employee).ThenInclude(x => x.Designation)
			  .Include(x => x.Employee).ThenInclude(x => x.WorkLocation));
		}
		public async Task<PaySheet> GetEmployeePay(Guid Id, Guid EmployeeId)
		{
			return await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == EmployeeId
			&& x.PayMonthId == Id,
			include: o => o.Include(x => x.Employee).Include(x => x.Earnings).Include(x => x.Deductions).Include(x => x.PayMonth)
			   .Include(x => x.Employee).ThenInclude(x => x.Department)
			   .Include(x => x.Employee).ThenInclude(x => x.Designation)
			   .Include(x => x.Employee).ThenInclude(x => x.WorkLocation));
		}
		public async Task<IEnumerable<PaySheet>> GetSalarySlips(Guid EmployeeId)
		{
			return (await UOW.GetRepositoryAsync<PaySheet>().GetPaginateAsync(
			   predicate: x => x.PayMonth.Status == (int)PayMonthStatus.Released && x.EmployeeID == EmployeeId,
			   include: i => i.Include(x => x.PayMonth),
			   orderBy: i => i.OrderByDescending(x => x.AddedAt), index: 0, size: 6)).Items;
		}
		public async Task<Result<PaySheet>> PaySlipSendedOn(Guid paySheetId)
		{
			PaySheet sheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.ID == paySheetId);
			sheet.PayslipMailedOn = DateTime.Now;
			Result<PaySheet> result = new ();
			UOW.GetRepositoryAsync<PaySheet>().UpdateAsync(sheet);
			await UOW.SaveChangesAsync();
			return result;
		}

		public async Task<Result<PaySheet>> AddUpdatePaysheet(List<PaySheet> psheet)
		{

			foreach (var paySheet in psheet)
			{
				PaySheet entity = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.PayMonthId == paySheet.PayMonthId
									&& x.EmployeeID == paySheet.EmployeeID);
				if (entity == null)
				{
					await UOW.GetRepositoryAsync<PaySheet>().AddAsync(paySheet);
				}
				else
				{
					UOW.GetRepositoryAsync<PaySheet>().DeleteAsync(entity);
					//paySheet.ID = entity.ID;
					//UOW.GetRepositoryAsync<PaySheet>().UpdateAsync(paySheet);
					await UOW.GetRepositoryAsync<PaySheet>().AddAsync(paySheet);
				}
			}

			Result<PaySheet> result = new();
			try
			{
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<TextDetailsModel> TaxDeductions(Guid id)
		{
			TextDetailsModel details = new();
			List<DeductionsModel> deductModel = new();
			List<TaxesModel> taxes = new();

			var pays = await UOW.GetRepositoryAsync<PaySheet>().GetAsync(x => x.PayMonthId == id);
			var deductions = await UOW.GetRepositoryAsync<PaySheetDeduction>().GetAsync(x => x.PaySheet.PayMonthId == id && x.Component.Deduct == 1);
			var components = await UOW.GetRepositoryAsync<DeductionComponent>().GetAsync(x => x.Deduct == 1);

			taxes.Add(new TaxesModel { TaxName = "Income Tax", Tax = pays.Sum(x => x.Tax), });
			taxes.Add(new TaxesModel { TaxName = "Provisional Tax", Tax = pays.Sum(x => x.PTax), });

			foreach (var item in components)
			{
				deductModel.Add(new DeductionsModel
				{
					Component = item.Name,
					PaidAmt = 0,
					Deduction = deductions.Where(x => x.ComponentId == item.ID).Sum(x => x.Deduction)
				});
			}
			details.Deductions = deductModel;
			details.Taxes = taxes;
			return details;
		}
		public async Task<IEnumerable<PayMonth>> GetAllMonths()
		{
			return await UOW.GetRepositoryAsync<PayMonth>().GetAsync();
		}

		public async Task<bool> CheckPayMonthIsOpen(int year, int month)
		{
			return (await UOW.GetRepositoryAsync<PayMonth>().GetAsync(x => x.Year == year && x.Month == month
			&& (x.Status == (int)PayMonthStatus.Open || x.Status == (int)PayMonthStatus.InProcess))).Any();
		}

		public async Task<IEnumerable<PaySheet>> GetEmployeeAnnualSalary(Guid empId, Guid fyId)
		{
			return await UOW.GetRepositoryAsync<PaySheet>().GetAsync(
			   predicate: x => x.PayMonth.Status == (int)PayMonthStatus.Released && x.EmployeeID == empId && x.PayMonth.FinancialYearId == fyId,
			 include: i => i.Include(x => x.Earnings).ThenInclude(x => x.Component)
							.Include(x => x.Deductions).ThenInclude(x => x.Component)
							.Include(x => x.PayMonth),
			 orderBy: o => o.OrderByDescending(x => x.PayMonth.Start));
		}

		public async Task<List<dynamic>> GetEmpAnnualSalary(Guid empId, Guid fyId)
		{
			var list = new List<dynamic>();
			var earnings = await UOW.GetRepositoryAsync<EarningComponent>().GetAsync(x => x.Status);

			var er = await UOW.GetRepositoryAsync<PaySheetEarning>().GetAsync(x => x.PaySheet.EmployeeID == empId && x.PaySheet.PayMonth.FinancialYearId == fyId,
					include: x => x.Include(x => x.PaySheet).ThenInclude(x => x.PayMonth));

			foreach (var item in earnings)
			{
				list.Add(new
				{
					Component = item.Name,
					Amount = er.Where(x => x.ComponentId == item.ID).Sum(x => x.Earning),
					Month = er.FirstOrDefault(x => x.ComponentId == item.ID).PaySheet.PayMonth.Name
				});
			}
			return list;
		}
	}
}
