using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Payroll
{
	public partial interface IEmployeePayInfoService : IBaseService<EmployeePayInfo>
	{
		public Task<Result<EmployeePayInfo>> AddPaymentInfo(List<EmployeePayInfo> items);
	}

	public partial class EmployeePayInfoService : BaseService<EmployeePayInfo>, IEmployeePayInfoService
	{
		public EmployeePayInfoService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task<EmployeePayInfo> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).Include(x => x.Bank),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}

		public override async Task<IPaginate<EmployeePayInfo>> GetPaginate(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<EmployeePayInfo>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(search.Name)
					 || x.Employee.Name.Contains(search.Name)),
				include: i => i.Include(x => x.Employee),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Employee.Name", ascending: !search.IsDescend);
		}
		public override async Task CustomValidation(EmployeePayInfo item, Result<EmployeePayInfo> result)
		{
			await base.CustomValidation(item, result);
			Bank bank = await UOW.GetRepositoryAsync<Bank>().SingleAsync(x => x.ID == item.BankId);

			if (!result.HasNoError) return;

			//Duplicate checking
			if (await UOW.GetRepositoryAsync<EmployeePayInfo>().GetCountAsync(x => x.EmployeeId == item.EmployeeId && x.ID != item.ID) > 0)
			{
				result.AddMessageItem(new MessageItem
									   (nameof(EmployeePayInfo.EmployeeId), Resource.Employee_Payment_Information_Is_Already_Added_));
			}
			if (bank == null || item.BankId == Guid.Empty)
			{
				result.AddMessageItem(new MessageItem
						(nameof(EmployeePayInfo.BankId), Resource.Bank_Is_Required));
				return;
			}
			//If user select payment mode as Bank transfer
			if (item.PayMode == 1)
			{
				await VerifyAccountNo(result, item);
				//If AccountNo Length doesn't match with that Particular Bank assigned BankAccountNo Length
				if (!string.IsNullOrEmpty(item.AccountNo) && bank.BankNoLength != item.AccountNo.Length)
				{
					result.AddMessageItem(new MessageItem
										   (nameof(EmployeePayInfo.AccountNo), string.Format(Resource.Account_No_Required_Length, bank.BankNoLength)));
				}
			}
			if (item.PayMode == 2)
			{
				await VerifyAccountNo(result, item);
				if (!Regex.IsMatch(item.IFSCCode, Resource.Regx_IFSC_Code))
				{
					result.AddMessageItem(new MessageItem(nameof(EmployeePayInfo.IFSCCode), Resource.Invalid_IFSC_Code));
				}
			}
		}

		private async Task VerifyAccountNo(Result<EmployeePayInfo> result, EmployeePayInfo item)
		{
			//If Account No is Null 
			Regex regexAccountNo = new("(^[0-9]*$)");
			if (string.IsNullOrEmpty(item.AccountNo))
			{
				result.AddMessageItem(new MessageItem
						(nameof(EmployeePayInfo.AccountNo), Resource.Account_No_Is_Required));
			}
			//If account number already exists
			else if (await UOW.GetRepositoryAsync<EmployeePayInfo>().HasRecordsAsync(x => x.AccountNo == item.AccountNo && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem
					(nameof(EmployeePayInfo.AccountNo), Resource.Account_Number_Already_Exists));
			}
			// If Account Doesn't contain Digits
			else if (!regexAccountNo.IsMatch(item.AccountNo))
			{
				result.AddMessageItem(new MessageItem(nameof(EmployeePayInfo.AccountNo), Resource.AccountNo_Should_Contain_Digits_Only));
			}
		}

		public override async Task OnBeforeAdd(EmployeePayInfo item, Result<EmployeePayInfo> executionResult)
		{
			await base.OnBeforeAdd(item, executionResult);
			if (!executionResult.HasError)
			{
				await addAudit(item);
			}
		}
		public override async Task OnBeforeUpdate(EmployeePayInfo item, Result<EmployeePayInfo> executionResult)
		{
			await base.OnBeforeUpdate(item, executionResult);
			if (!executionResult.HasError)
			{
				await addAudit(item);
			}
		}

		private async Task addAudit(EmployeePayInfo item)
		{
			Bank bank = await UOW.GetRepositoryAsync<Bank>().SingleAsync(x => x.ID == item.BankId);
			await UOW.GetRepositoryAsync<EmployeePayInfoAudit>().AddAsync(new EmployeePayInfoAudit
			{
				EmployeeId = item.EmployeeId,
				ModifiedBy = item.ModifiedBy,
				Bank = bank != null ? bank.Name : "",
				PayType = item.PayMode == 1 ? "BankTransfer" : "Online",
				AccountNo = item.AccountNo,
				BankName = item.BankName,
				IFSCCode = item.IFSCCode,
			});
			if (await UOW.GetRepositoryAsync<EmployeePayInfoAudit>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId))
			{
				await UOW.GetRepositoryAsync<EmployeePayInfoStatusAudit>().AddAsync(new EmployeePayInfoStatusAudit
				{
					EmployeeId = item.EmployeeId,
					ModifiedBy = item.ModifiedBy,
					Status = "Modified"
				});
			}
			else
			{
				await UOW.GetRepositoryAsync<EmployeePayInfoStatusAudit>().AddAsync(new EmployeePayInfoStatusAudit
				{
					EmployeeId = item.EmployeeId,
					ModifiedBy = item.ModifiedBy,
					Status = "Created"
				});
			}
		}

		public async Task<Result<EmployeePayInfo>> AddPaymentInfo(List<EmployeePayInfo> items)
		{
			foreach (var payInfo in items)
			{
				EmployeePayInfo entityInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == payInfo.EmployeeId);
				if (entityInfo == null)
				{
					await UOW.GetRepositoryAsync<EmployeePayInfo>().AddAsync(payInfo);
				}
				else
				{
					entityInfo.PayMode = payInfo.PayMode;
					entityInfo.BankId = payInfo.BankId;
					entityInfo.AccountNo = payInfo.AccountNo;
					entityInfo.BankName = payInfo.BankName;
					entityInfo.IFSCCode = payInfo.IFSCCode;
					UOW.GetRepositoryAsync<EmployeePayInfo>().UpdateAsync(entityInfo);
				}
			}

			Result<EmployeePayInfo> result = new();
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
	}
}
