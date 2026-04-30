using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.LM_Attendance;

namespace TranSmart.Service.Leave
{
	public partial interface IApplyLeaveService : IBaseService<ApplyLeave>
	{
		#region Self service
		Task<IPaginate<ApplyLeave>> SelfServiceSearch(ApplyLeaveSearch search);
		Task<Result<ApplyLeave>> CancelAsync(Guid id, Guid employeeId);
		Task<decimal> GetLeaveBalanceByLeaveType(Guid leaveTypeId, Guid empId, DateTime fromDate, DateTime toDate);
		Task<Result<ApplyLeave>> SelfServiceLeaveValidation(ApplyLeave item);
		//Task LeaveApprovedOnPresentDay(ApplyLeave entity);
		Task UpdateLBIfLeaveApproved(ApplyLeave applyLeave);
		#endregion

		#region HR 
		Task<IEnumerable<ApplyLeave>> GetLeaves(int month, Guid departmentId, Guid reportingToId);
		Task<IEnumerable<ApplyLeave>> GetLeavesBetweenTwoDates(DateTime fromDate, DateTime toDate, Guid departmentId, Guid reportingToId);
		Task<Result<ApplyLeave>> AddApprovedLeaveAsync(ApplyLeave item);
		Task<IEnumerable<ApplyLeave>> GetWeekLeaves(Guid employeeId, DateTime fromDate, DateTime toDate);
		Task<Result<ApplyLeave>> RejectAfterApprove(Guid Id, string rejectReason, Guid rejectedEmpId);
		#endregion


		/// <summary>
		/// Approve employee leave request
		/// </summary>
		/// <param name="Id">Leave id</param>
		/// <param name="approvedEmpId">Leave approved employee id</param>
		/// <param name="isAdminRequest">True: when leave is approved by HR/Leave management, otherwise false</param>
		/// <returns></returns>
		Task<Result<ApplyLeave>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest);

		/// <summary>
		/// Reject employee leave request
		/// </summary>
		/// <param name="Id">Leave id</param>
		/// <param name="rejectReson">Leave reject reason</param>
		/// <param name="rejectedEmpId">Leave reject employee id</param>
		/// <returns></returns>
		Task<Result<ApplyLeave>> Reject(Guid Id, string rejectReason, Guid rejectedEmpId);

		/// <summary>
		/// Leave request pagination list
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		Task<IPaginate<ApplyLeave>> ApprovalPaginate(BaseSearch search);
		Task<IEnumerable<LeaveBalanceModel>> GetEmployeeLeaveTypes(Guid empId);
		#region Approval  

		/// <summary>
		/// Get leave request details by approval id. This is verify the employee reporting id
		/// </summary>
		/// <param name="Id">Leave request id</param>
		/// <param name="approverId">Leave approver id</param>
		/// <returns></returns>
		Task<ApplyLeave> GetLeave(Guid Id, Guid approverId);

		/// <summary>
		/// Get reporting by employees leaves to display in calender 
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="approverId">Leave approved employee id</param>
		/// <returns></returns>
		#endregion
		void UploadLeaveCount(ApplyLeave item);
		void SpecifidePeriodValidation(int count, LeaveType leaveType, Result<ApplyLeave> result);
		Task<Result<ApplyLeave>> MaximumLeavesValidation(ApplyLeave leave);
		Task UpdateLeaveBalanceAfterReject(ApplyLeave applyLeave);
		Task<IPaginate<ApplyLeaveDetails>> LeaveDetails(BaseSearch search);
	}
	public partial class ApplyLeaveService : BaseService<ApplyLeave>, IApplyLeaveService
	{
		private readonly IAttendanceService _attService;
		private readonly ILeaveBalanceService _leaveBalanceAddUpDate;
		public ApplyLeaveService(IUnitOfWork uow, IAttendanceService attService, ILeaveBalanceService leaveBalanceService) : base(uow)
		{
			_attService = attService;
			_leaveBalanceAddUpDate = leaveBalanceService;
		}
		public override async Task<ApplyLeave> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation).Include(x => x.ApplyLeaveType).ThenInclude(x => x.LeaveType),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<IEnumerable<ApplyLeave>> GetWeekLeaves(Guid employeeId, DateTime fromDate, DateTime toDate)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(
				predicate: p => p.EmployeeId == employeeId && p.FromDate >= fromDate.AddDays(-7) && p.ToDate <= toDate.AddDays(7),
				 include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderBy(x => x.FromDate));
		}

		#region Self service
		public async Task<IPaginate<ApplyLeave>> SelfServiceSearch(ApplyLeaveSearch search)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(search.Name) || x.Employee.Name.Contains(search.Name))
				&& (!search.FromDate.HasValue || x.FromDate.Date >= search.FromDate.Value)
				&& (!search.ToDate.HasValue || x.ToDate.Date <= search.ToDate.Value)
				&& x.EmployeeId == search.RefId,
			   include: i => i.Include(x => x.ApprovedBy).Include(x => x.ApplyLeaveType).Include(x => x.Employee),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Employee.Name", ascending: !search.IsDescend);
		}

		public override async Task<Result<ApplyLeave>> AddAsync(ApplyLeave item)
		{
			item.Status = (int)ApplyLeaveSts.Applied;
			var result = await SelfServiceLeaveValidation(item);
			if (result.HasError) return result;
			return await base.AddAsync(item);
		}

		public override async Task<Result<ApplyLeave>> UpdateAsync(ApplyLeave item)
		{
			var result = new Result<ApplyLeave>();
			var entity = await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(x => x.ID == item.ID);
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid));
				return result;
			}
			if (entity.Status != (int)ApplyLeaveSts.Applied)
			{
				result.AddMessageItem(new MessageItem(
					entity.Status == (int)ApplyLeaveSts.Approved ?
					Resource.Leave_Already_Approved :
					Resource.Leave_Already_Rejected));
				return result;
			}
			var entityList = await UOW.GetRepositoryAsync<ApplyLeaveType>().GetAsync(x => x.ApplyLeaveId == item.ID);
			CollectionCompareResult<ApplyLeaveType> compareList = entityList.Compare(item.ApplyLeaveType, (x, y) => x.LeaveTypeId.Equals(y.LeaveTypeId));
			foreach (ApplyLeaveType entitys in compareList.Same)
			{
				ApplyLeaveType editItem = item.ApplyLeaveType.FirstOrDefault(x => x.LeaveTypeId == entitys.LeaveTypeId);
				if (editItem != null && !entity.Equals(editItem))
				{
					entitys.NoOfLeaves = editItem.NoOfLeaves;
					UOW.GetRepositoryAsync<ApplyLeaveType>().UpdateAsync(entitys);
				}
			}
			foreach (ApplyLeaveType comp in compareList.Added)
			{
				await UOW.GetRepositoryAsync<ApplyLeaveType>().AddAsync(comp);
			}

			foreach (ApplyLeaveType comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<ApplyLeaveType>().DeleteAsync(comp);
			}
			if (entity.Status == (int)ApplyLeaveSts.Applied)
			{
				result = await SelfServiceLeaveValidation(item);
				if (result.HasError)
				{
					return result;
				}
				entity.Update(item);
				return await base.UpdateAsync(entity);
			}

			return result;
		}

		public async Task<Result<ApplyLeave>> CancelAsync(Guid id, Guid employeeId)
		{
			var result = new Result<ApplyLeave>();
			ApplyLeave entity = await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(x => x.ID == id);
			if (entity == null || entity.EmployeeId != employeeId)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid));
				return result;
			}
			if (entity.Status == (int)ApplyLeaveSts.Cancelled)
			{
				result.AddMessageItem(new MessageItem(Resource.LEAVE_ALREADY_CANCELLED));
				return result;
			}

			if (entity.Status != (int)ApplyLeaveSts.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)ApplyLeaveSts.Approved ?
					Resource.Leave_Already_Approved : Resource.Leave_Already_Rejected));
				return result;
			}
			entity.Status = (int)ApplyLeaveSts.Cancelled;
			return await base.UpdateAsync(entity);
		}

		public async Task<decimal> GetLeaveBalanceByLeaveType(Guid leaveTypeId, Guid empId, DateTime fromDate, DateTime toDate)
		{
			return await UOW.GetRepositoryAsync<LeaveBalance>().SumOfDecimalAsync(predicate: x => x.LeaveTypeId == leaveTypeId
						   && x.EmployeeId == empId
						   && x.EffectiveFrom <= fromDate.Date && x.EffectiveTo >= toDate.Date,
						   sumBy: x => x.Leaves);
		}
		public async Task<IEnumerable<LeaveBalanceModel>> GetEmployeeLeaveTypes(Guid empId)
		{
			var leaveBalances = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(predicate: p => !p.LeaveType.DefaultPayoff, include: i => i.Include(x => x.LeaveType));
			var model = new List<LeaveBalanceModel>();
			foreach (var item in leaveBalances.GroupBy(x => new { x.EmployeeId, x.LeaveTypeId }).Select(x => x.Key))
			{
				var data = leaveBalances.FirstOrDefault(x => x.LeaveTypeId == item.LeaveTypeId);
				var availableLeaves = leaveBalances.Where(x => x.LeaveTypeId == item.LeaveTypeId && x.EmployeeId == item.EmployeeId).Sum(x => x.Leaves);

				model.Add(new LeaveBalanceModel
				{
					LeaveTypeId = item.LeaveTypeId,
					LeaveTypeName = data.LeaveType.Name,
					EmployeeId = item.EmployeeId,
					Leaves = availableLeaves
				});

			}
			return model.Where(x => x.EmployeeId == empId);
		}
		#endregion

		public void UploadLeaveCount(ApplyLeave item)
		{
			item.NoOfLeaves = (item.ToDate - item.FromDate).Days + 1;
			if (item.FromHalf)
			{
				item.NoOfLeaves = Math.Max(item.NoOfLeaves - 0.5m, 0m);
			}
			if (item.ToHalf)
			{
				item.NoOfLeaves = Math.Max(item.NoOfLeaves - 0.5m, 0m);
			}
		}
		public void SpecifidePeriodValidation(int count, LeaveType leaveType, Result<ApplyLeave> result)
		{
			if (count >= leaveType.MaxApplications)
			{
				result.AddMessageItem(new MessageItem
				(nameof(ApplyLeave.FromDate), Resource.LEAVES_EXCEEDED_LEAVE_APPLICATIONS));
			}
		}
		public async Task<Result<ApplyLeave>> MaximumLeavesValidation(ApplyLeave leave)
		{
			Result<ApplyLeave> result = new();
			foreach (var lvtype in leave.ApplyLeaveType)
			{
				var leaveType = await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.ID == lvtype.LeaveTypeId);
				if (leaveType.MaxLeaves < lvtype.NoOfLeaves)
				{
					result.AddMessageItem(new MessageItem
						(nameof(ApplyLeave.FromDate), leaveType.Name + " " + Resource.Leaves_are_Exceed_The_Maximum_Leaves));
					return result;
				}
				if (leaveType.MinLeaves > lvtype.NoOfLeaves)
				{
					result.AddMessageItem(new MessageItem
						(nameof(ApplyLeave.FromDate), Resource.Applied_Leaves_are_less_Minimum_Leave));
					return result;
				}
			}
			return result;
		}
		public async Task<Result<ApplyLeave>> SelfServiceLeaveValidation(ApplyLeave item)
		{
			var result = new Result<ApplyLeave>();
			try
			{
				var selectedLeaveTypes = JsonConvert.DeserializeObject<List<ApplyLeave>>(item.LeaveTypes);

				if (!selectedLeaveTypes.Any())
				{
					result.AddMessageItem(new MessageItem(Resource.Something_Went_Wrong));
					return result;
				}
			}
			catch
			{
				result.AddMessageItem(new MessageItem(Resource.Something_Went_Wrong));
				return result;
			}

			if (item.FromDate.Date == item.ToDate.Date && item.FromHalf && item.ToHalf)
			{
				result.AddMessageItem(new MessageItem(
				   nameof(ApplyLeave.ToDate), Resource.For_OneDay_Leave_Both_Halfs_Should_Not_Select));
				return result;
			}

			UploadLeaveCount(item);
			if (item.EmergencyContNo != "" && !Regex.IsMatch(item.EmergencyContNo, Resource.Regx_Mobile_Number))
			{
				result.AddMessageItem(new MessageItem(
				   nameof(ApplyLeave.EmergencyContNo), Resource.Lead_ContactNo_Is_Digits_Only));
				return result;
			}
			if (item.ToDate < item.FromDate)
			{
				result.AddMessageItem(new MessageItem
				   (nameof(ApplyLeave.ToDate), Resource.To_Date_Always_Greater_than_or_Equal_to_From_Date));
				return result;
			}
			if (item.FromDate.Day <= 25 && item.ToDate.Day >= 26)
			{
				result.AddMessageItem(new MessageItem("please select dates in between payroll dates"));
				return result;
			}
			var applyLeave = await UOW.GetRepositoryAsync<ApplyLeave>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
							&& x.ID != item.ID && x.Status != (int)ApplyLeaveSts.Cancelled && x.Status != (int)ApplyLeaveSts.Rejected
							 && (item.FromDate.Date <= x.ToDate.Date && item.ToDate.Date >= x.FromDate.Date));

			if (applyLeave)
			{
				result.AddMessageItem(new MessageItem
					(nameof(ApplyLeave.ToDate), Resource.Already_Applied));
				return result;
			}
			#region WFH Validation
			//List of Applied WFH's in between fromDate and toDate
			var wfh = await UOW.GetRepositoryAsync<ApplyWfh>().GetAsync(x => x.EmployeeId == item.EmployeeId
															&& (x.Status == (int)WfhStatus.Approved || x.Status == (int)WfhStatus.Applied)
															&& item.FromDate.Date <= x.ToDateC.Date && item.ToDate.Date >= x.FromDateC.Date);
			//Applied Wfh is more then one day(fromDate != toDate) then fromHalf represents secondHalf, toHalf represents firstHalf
			//if now applied Leave for oneDay(fromDate == toDate) then fromHalf represents firstHalf and toHalf represents secondHalf
			//any applyLeave Date Matched or halfDays matched Then we are throwing exception
			if (wfh.Any(x => x.FromDateC != x.ToDateC))
			{
				var wfhValidate = wfh.Any(x => x.FromDateC.Date == item.FromDate.Date && x.FromHalf == item.ToHalf
															|| x.ToDateC.Date == item.ToDate.Date && x.ToHalf == item.FromHalf
															|| x.FromDateC.Date == item.FromDate.Date && x.FromDateC == item.ToDate && !item.ToHalf && !item.FromHalf
															|| x.FromDateC.Date == item.FromDate.Date && x.FromDateC == item.ToDate && item.ToHalf && !item.FromHalf
															|| x.ToDateC.Date == item.FromDate.Date && x.ToDateC == item.ToDate && !item.ToHalf && !item.FromHalf
															|| x.ToDateC.Date == item.FromDate.Date && x.ToDateC == item.ToDate && !item.ToHalf && item.FromHalf);
				if ((item.FromDate.Date != item.ToDate.Date && wfh.Any()) || wfhValidate)
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), Resource.Already_WFH_Applied_For_this_Dates));
					return result;
				}
				//applied one day leave,which is in between wfh dates,throwing exception
				if (wfh.Any(x => item.FromDate.Date < x.ToDateC.Date && item.ToDate.Date > x.FromDateC.Date))
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), Resource.Already_WFH_Applied_For_this_Dates));
					return result;
				}
			}
			//Applied Wfh is one day(fromDate == toDate) then fromHalf represents firstHalf and toHalf represents secondHalf
			//if now applied Leave for more then one day then fromHalf represents secondHalf and toHalf represents firstHalf
			//we are not allowing to apply leave if any date matched
			if (wfh.Any(x => x.FromDateC == x.ToDateC))
			{
				var wfhSameDay = wfh.Any(x => x.FromDateC.Date == item.FromDate.Date && x.FromHalf == item.FromHalf
																|| x.ToDateC.Date == item.ToDate.Date && x.ToHalf == item.ToHalf);
				if ((item.FromDate != item.ToDate && wfh.Any()) || wfhSameDay)
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), Resource.Already_WFH_Applied_For_this_Dates));
					return result;
				}
			}
			#endregion
			var clientVisit = await UOW.GetRepositoryAsync<ApplyClientVisits>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
															&& (x.Status == (int)ClientVisitStatus.Approved || x.Status == (int)ClientVisitStatus.Applied)
															&& item.FromDate.Date <= x.ToDate.Date && item.ToDate.Date >= x.FromDate.Date);
			if (clientVisit)
			{
				result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), Resource.Client_Visit_Applied_For_These_Days));
				return result;
			}
			//ADDED
			foreach (var lvtype in item.ApplyLeaveType)
			{
				var leaveType = await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.ID == lvtype.LeaveTypeId);
				if (leaveType == null)
				{
					result.AddMessageItem(new MessageItem
					   (nameof(ApplyLeaveDetails.LeaveTypeId), "Invalid leave type."));
					return result;
				}


				#region Added
				var applyLeaves = await UOW.GetRepositoryAsync<ApplyLeaveType>().GetAsync(predicate: x => x.ApplyLeave.EmployeeId == item.EmployeeId
							 && x.LeaveTypeId == lvtype.LeaveTypeId
							 && x.ApplyLeave.Status != (int)ApplyLeaveSts.Cancelled && x.ApplyLeave.Status != (int)ApplyLeaveSts.Rejected,
							 include: i => i.Include(x => x.ApplyLeave));
				if (item.ID == Guid.Empty)
				{
					// specified period is Weeks
					if (leaveType.specifiedperio == 1)
					{
						//assuming Monday is the Starting day of a week 
						var dayNumber = DateUtil.Week(item.FromDate);
						var startingDayOfWeek = item.FromDate.AddDays(-dayNumber);
						var endingDayOfWeek = startingDayOfWeek.AddDays(6);
						var count = applyLeaves.Count(x => x.ApplyLeave.FromDate.Date >= startingDayOfWeek.Date && x.ApplyLeave.ToDate.Date <= endingDayOfWeek.Date);
						SpecifidePeriodValidation(count, leaveType, result);
						if (result.HasError)
						{
							return result;
						}
					}
					// specified period is Months
					else if (leaveType.specifiedperio == 2)
					{
						var count = applyLeaves.Count(x => x.ApplyLeave.FromDate.Month == item.FromDate.Month && x.ApplyLeave.FromDate.Year == item.FromDate.Year); // ||  x.ToDate.Month == item.FromDate.Month
						SpecifidePeriodValidation(count, leaveType, result);
						if (result.HasError)
						{
							return result;
						}
					}
					// specified period is Years
					else
					{
						var count = applyLeaves.Count(x => x.ApplyLeave.FromDate.Year == item.FromDate.Year);
						SpecifidePeriodValidation(count, leaveType, result);
						if (result.HasError)
						{
							return result;
						}
					}
				}
				//Suppose Past days = 2 ,FromDate = 08/07/2022 ,So leave Should accept from 06/07/2022
				if (item.FromDate < DateTime.Today.AddDays(-leaveType.PastDate))
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), "Not acceptable to apply for past dates."));
					return result;
				}
				//Suppose FutureDate = 2 ,FromDate = 08/07/2022 ,So leave Should accept before 10/07/2022
				if (item.FromDate >= DateTime.Today.AddDays(leaveType.FutureDate))
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), "Not acceptable to apply leave for future dates."));
					return result;
				}
				//Leave Type Duration Validation 
				if (leaveType.Duration == 1 && ((item.FromHalf && !item.ToHalf) || (!item.FromHalf && item.ToHalf)))
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), Resource.LEAVE_TYPE_NOT_ACCEPT_HALF_DAY_LEAVES));
					return result;
				}
				if (leaveType.Gender != 4 || leaveType.MaritalStatus != 3)
				{
					//Employee Gender, MaritalStatus  Should be match With LeaveType Applicable Gender,MartalStatus
					var employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == item.EmployeeId);
					if (employee.Gender != leaveType.Gender && leaveType.Gender != 4)
					{
						result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), leaveType.Name + Resource.Different_Gender + ConstUtil.Gender(leaveType.Gender)));
						return result;
					}
					else if (employee.MaritalStatus != leaveType.MaritalStatus && leaveType.MaritalStatus != 3)
					{
						result.AddMessageItem(new MessageItem(nameof(ApplyLeave.FromDate), leaveType.Name + Resource.Different_Martial_Status + ConstUtil.MartialStatus(leaveType.MaritalStatus)));
						return result;
					}
				}
				if (!leaveType.DefaultPayoff)
				{
					var lopBalance = await UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(x => x.LeaveTypeId == lvtype.LeaveTypeId && x.EmployeeId == item.EmployeeId);
					if (lopBalance == null)
					{
						result.AddMessageItem(new MessageItem
							(nameof(ApplyLeave.ToDate), Resource.Leaves_Not_Avaible_To_Apply));
						return result;
					}
					var leaveBalance = await UOW.GetRepositoryAsync<LeaveBalance>()
						.SumOfDecimalAsync(x => x.EmployeeId == item.EmployeeId
								&& x.LeaveTypeId == lvtype.LeaveTypeId, x => x.Leaves);

					if (leaveBalance <= 0)
					{
						result.AddMessageItem(new MessageItem
							(nameof(ApplyLeave.ToDate), Resource.Leaves_Not_Avaible_To_Apply));
						return result;
					}
					else if (lvtype.NoOfLeaves > leaveBalance)
					{
						result.AddMessageItem(new MessageItem
						(nameof(ApplyLeave.ToDate), Resource.Leaves_Exceed_The_Available_Leaves));
						return result;
					}
				}

			}//END
			#endregion Added
			return result;
		}

		public override async Task<IPaginate<ApplyLeave>> GetPaginate(BaseSearch baseSearch)
		{
			ApplyLeaveSearch leaveSearch = (ApplyLeaveSearch)baseSearch;
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(leaveSearch.Name)) //|| x.LeaveType.Code.Contains(leaveSearch.Name))
				&& (string.IsNullOrEmpty(leaveSearch.ToDate.ToString()) && string.IsNullOrEmpty(leaveSearch.FromDate.ToString())
				|| x.FromDate.Date >= leaveSearch.FromDate.Value.Date && x.ToDate.Date <= leaveSearch.ToDate.Value.Date),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
						.Include(x => x.Employee).ThenInclude(x => x.Designation),
				index: leaveSearch.Page, size: leaveSearch.Size, sortBy: leaveSearch.SortBy ?? "Employee.Name", ascending: !leaveSearch.IsDescend);
		}

		public async Task<Result<ApplyLeave>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest)
		{
			var result = new Result<ApplyLeave>();

			var entity = await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(predicate: x => x.ID == Id,
				include: i => i.Include(x => x.ApplyLeaveType));

			if (entity.Status != (int)ApplyLeaveSts.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)ApplyLeaveSts.Approved ? Resource.Leave_Already_Approved : Resource.Leave_Already_Rejected));
				return result;
			}

			//Checking leave approver is reporting to employee or not. 
			//Value of reportingEmpId is null when approve from leave management
			if (!isAdminRequest)
			{
				var employeeEntity = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == entity.EmployeeId,
					include: x => x.Include(o => o.ReportingTo));
				if (employeeEntity != null && approvedEmpId != employeeEntity.ReportingToId && approvedEmpId != employeeEntity.ReportingTo.ReportingToId)
				{
					result.AddMessageItem(new MessageItem(Resource.You_DoNot_Have_Permission));
					return result;
				}
			}

			foreach (var lvType in entity.ApplyLeaveType)
			{
				var leaveType = await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.ID == lvType.LeaveTypeId);
				if (!leaveType.DefaultPayoff)
				{
					var availableLeaves = await UOW.GetRepositoryAsync<LeaveBalance>()
									.SumOfDecimalAsync(x => x.EmployeeId == entity.EmployeeId
										&& x.LeaveTypeId == lvType.LeaveTypeId, x => x.Leaves);
					if (availableLeaves < lvType.NoOfLeaves)
					{
						result.AddMessageItem(new MessageItem(Resource.Leaves_are_Not_available_to_approve));
						return result;
					}
				}
			}
			await UpdateLBIfLeaveApproved(entity);
			entity.Status = (int)ApplyLeaveSts.Approved;
			entity.ApprovedById = approvedEmpId;
			return await base.UpdateAsync(entity);
		}
		public async Task<Result<ApplyLeave>> Reject(Guid Id, string rejectReason, Guid rejectedEmpId)
		{
			var result = new Result<ApplyLeave>();
			var entity = await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(x => x.ID == Id);

			if (entity.Status != (int)ApplyLeaveSts.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)ApplyLeaveSts.Approved ? Resource.Leave_Already_Approved : Resource.Leave_Already_Rejected));
				return result;
			}
			var balance = await UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(x => x.ApplyLeaveId == entity.ID);
			if (balance != null)
			{
				UOW.GetRepositoryAsync<LeaveBalance>().DeleteAsync(balance);
			}
			entity.Status = (int)ApplyLeaveSts.Rejected;
			entity.RejectReason = rejectReason;
			entity.ApprovedById = rejectedEmpId;
			return await base.UpdateAsync(entity);
		}

		public async Task<Result<ApplyLeave>> RejectAfterApprove(Guid Id, string rejectReason, Guid rejectedEmpId)
		{
			var result = new Result<ApplyLeave>();
			var entity = await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(x => x.ID == Id);
			var attendance = await UOW.GetRepositoryAsync<Attendance>().HasRecordsAsync(x => x.EmployeeId == entity.EmployeeId && x.AttendanceDate.Date >= entity.FromDate.Date && x.AttendanceDate.Date <= entity.ToDate.Date);
			if (attendance)
			{
				result.AddMessageItem(new MessageItem(Resource.Attendance_Already_Executed));
				return result;
			}
			await UpdateLeaveBalanceAfterReject(entity);
			entity.Status = (int)ApplyLeaveSts.Rejected;
			entity.RejectReason = rejectReason;
			entity.ApprovedById = rejectedEmpId;
			return await base.UpdateAsync(entity);
		}

		//To display in calender
		public async Task<IEnumerable<ApplyLeave>> GetLeaves(int month, Guid departmentId, Guid reportingToId)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(x => x.FromDate.Month == month
					&& x.Employee.DepartmentId == departmentId && (x.Employee.ReportingToId == reportingToId
					  || x.Employee.ReportingTo.ReportingToId == reportingToId)
					&& (x.Status == (int)ApplyLeaveSts.Applied || x.Status == (int)ApplyLeaveSts.Approved),
					include: x => x.Include(x => x.Employee));
		}
		public async Task<IEnumerable<ApplyLeave>> GetLeavesBetweenTwoDates(DateTime fromDate, DateTime toDate, Guid departmentId, Guid reportingToId)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(x => x.FromDate.Date >= fromDate.Date && x.ToDate.Date <= toDate.Date
					&& x.Employee.DepartmentId == departmentId && (x.Employee.ReportingToId == reportingToId
						|| x.Employee.ReportingTo.ReportingToId == reportingToId)
					&& (x.Status == (int)ApplyLeaveSts.Applied || x.Status == (int)ApplyLeaveSts.Approved),
					include: x => x.Include(x => x.Employee));
		}

		public async Task<Result<ApplyLeave>> AddApprovedLeaveAsync(ApplyLeave item)
		{
			var applyLeave = new Result<ApplyLeave>();
			item.Status = (int)ApplyLeaveSts.Approved;
			var result = await SelfServiceLeaveValidation(item);
			if (result.HasError) return result;
			await UOW.GetRepositoryAsync<ApplyLeave>().AddAsync(item);
			await UpdateLBIfLeaveApproved(item);
			try
			{
				await UOW.SaveChangesAsync();
				applyLeave.ReturnValue = item;
				applyLeave.IsSuccess = true;
			}
			catch (Exception ex)
			{
				applyLeave.IsSuccess = false;
				applyLeave.AddMessageItem(new MessageItem(ex.Message));
			}
			return applyLeave;
		}
		#region Approval
		public async Task<IPaginate<ApplyLeave>> ApprovalPaginate(BaseSearch search)
		{
			ApplyLeaveSearch leaveSearch = (ApplyLeaveSearch)search;

			//default display applied leaves and also when status is not selected display applied leaves
			leaveSearch.Status = leaveSearch.Status == 0 ? (int)ApplyLeaveSts.Applied : leaveSearch.Status;

			//leaveSearch.RefId is null when searching from leave management approval

			return await UOW.GetRepositoryAsync<ApplyLeave>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(leaveSearch.Name) || x.Employee.Name.Contains(search.Name))
				&& (leaveSearch.FromDate == null || x.FromDate.Date >= leaveSearch.FromDate.Value.Date)
				&& (leaveSearch.ToDate == null || x.ToDate.Date <= leaveSearch.ToDate.Value.Date)
				&& (leaveSearch.RefId == null || x.Employee.ReportingToId == leaveSearch.RefId || x.Employee.ReportingTo.ReportingToId == leaveSearch.RefId)
				&& (leaveSearch.IsPlanned == 1 ? x.IsPlanned : leaveSearch.IsPlanned != 2 || !x.IsPlanned)
				&& x.Status == leaveSearch.Status,
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
							  .Include(x => x.Employee).ThenInclude(x => x.Department)
							  .Include(x => x.Employee).ThenInclude(x => x.ReportingTo)
							  .Include(x => x.ApprovedBy),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "FromDate", ascending: !search.IsDescend);
		}
		public async Task<ApplyLeave> GetLeave(Guid Id, Guid approverId)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().SingleAsync(x => x.ID == Id
					&& (x.Employee.ReportingToId == approverId || x.Employee.ReportingTo.ReportingToId == approverId),
					include: x => x.Include(i => i.Employee).ThenInclude(x => x.Designation)
									.Include(i => i.Employee).ThenInclude(x => x.ReportingTo));
		}
		#endregion

		public async Task UpdateLBIfLeaveApproved(ApplyLeave applyLeave)
		{
			DateTime FromDate = applyLeave.FromDate.Date;
			// Adding two leavetypes for single day then using remainingDayisHalf
			bool remainingDayisHalf = false;

			foreach (var item in applyLeave.ApplyLeaveType)
			{
				decimal remainBal = item.NoOfLeaves;

				for (var attDate = FromDate; attDate <= applyLeave.ToDate.Date; attDate = attDate.AddDays(1))
				{
					var IsHalfDay = (attDate == applyLeave.FromDate && applyLeave.FromHalf)
						|| (attDate == applyLeave.ToDate && applyLeave.ToHalf) ? true : false;
					decimal leavesconsidered;
					var IsDayHalf = IsHalfDay;
					IsHalfDay = remainingDayisHalf || IsHalfDay;

					// remainBal is zero or less then zero means another leave type must be add for remaining days
					if (remainBal <= 0)
					{
						break;
					}

					// remainBal is less then 1 then half day adding from current leavetype and remaing half day adding from next leave type
					if (remainBal > 0.5m)
					{ leavesconsidered = IsHalfDay ? 0.5m : 1m; }
					else
					{ leavesconsidered = 0.5m; }
					remainBal -= leavesconsidered;

					if ((IsHalfDay && leavesconsidered == 0.5m) || (!IsHalfDay && leavesconsidered == 1))
					{
						FromDate = attDate.AddDays(1);
						remainingDayisHalf = false;
					}
					else
					{
						FromDate = attDate;
						remainingDayisHalf = true;
					}


					var details = new ApplyLeaveDetails
					{
						ApplyLeaveId = applyLeave.ID,
						LeaveDate = attDate,
						IsHalfDay = IsDayHalf,
						//if leave applies for single day and the leave applied for half day then considering as second half as leave
						IsFirstHalf = IsDayHalf && Leave_WFH_Util.IsFirstHalf(applyLeave.FromDate, applyLeave.ToDate, applyLeave.FromHalf, attDate),
						LeaveTypeId = item.LeaveTypeId,
						//IsFirstHalf -> considering HalfDay for FirstDate , IsSecondHalf -> considering HalfDay for LastDate ,
						//Remaining days taken as FullDay
						LeaveCount = leavesconsidered
					};

					await UOW.GetRepositoryAsync<ApplyLeaveDetails>().AddAsync(details);

					var att = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == applyLeave.EmployeeId && x.AttendanceDate.Date == attDate.Date);


					//Change for past days attendance
					//remainingDayisHalf is true means same day having multiple leave types(halfday,halfday) not require to update the attendance two times
					if (att != null && attDate.Date < DateTime.Now.Date && !remainingDayisHalf)
					{
						// if applied leave is Half Day Leave
						if (IsDayHalf)
						{
							var isFirstHalf = Leave_WFH_Util.IsFirstHalf(applyLeave.FromDate, applyLeave.ToDate, applyLeave.FromHalf, attDate);
							var remainingHalf = _attService.GetAttRemainingHalfDay((int)att.AttendanceStatus);

							await _attService.ChangeAttStatus(att, isFirstHalf ? (int)AttendanceStatus.HalfDayLeave : remainingHalf, 0, 0, 0, 0, true,
							isFirstHalf ? remainingHalf : (int)AttendanceStatus.HalfDayLeave, isFirstHalf, item.LeaveTypeId, 0);
						}
						else // Full Day Leave
						{
							await _attService.ChangeAttStatus(att, (int)AttendanceStatus.Leave, 0, 0, 1, 0, false, 0, false, item.LeaveTypeId, 0);
						}
					}
				}
				//to verify that, paid leave or unpaid leave
				var leaveType = await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.ID == item.LeaveTypeId);
				if (leaveType != null && !leaveType.DefaultPayoff)
				{
					var LB = await _leaveBalanceAddUpDate.GetConsolidatedLB(item.LeaveTypeId, applyLeave.EmployeeId, applyLeave.FromDate.Date, applyLeave.ToDate.Date);

					await _leaveBalanceAddUpDate.AdjustLB(LB.ToList(), applyLeave, item);
				}
			}
		}
		public async Task UpdateLeaveBalanceAfterReject(ApplyLeave applyLeave)
		{
			var leaveDetails = await UOW.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync(predicate: x => x.ApplyLeaveId == applyLeave.ID);
			foreach (var leave in leaveDetails)
			{
				leave.LeaveCount = 0;
				UOW.GetRepositoryAsync<ApplyLeaveDetails>().UpdateAsync(leave);
			}

			var leaveBalance = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(predicate: x => x.ApplyLeaveId == applyLeave.ID);
			foreach (var balance in leaveBalance)
			{
				balance.Leaves = 0;
				UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(balance);
			}
		}

		public async Task<IPaginate<ApplyLeaveDetails>> LeaveDetails(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<ApplyLeaveDetails>().GetPaginateAsync(
				predicate: x => x.ApplyLeave.EmployeeId == search.RefId && x.LeaveCount > 0,
				include: i => i.Include(x => x.ApplyLeave).Include(x => x.LeaveType),
				index: search.Page, size: search.Size, orderBy: o => o.OrderByDescending(x => x.LeaveDate));
		}
	}
}
