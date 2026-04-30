using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.VariantTypes;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Domain.Models.LM_Attendance.Request;
using TranSmart.Service.Leave;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace TranSmart.Service.LM_Attendance
{
	public interface ILeaveBalanceService : IBaseService<LeaveBalance>
	{
		Task<List<LeaveBalance>> GetEmpLeaveBalance(Guid employeeId, Guid leaveTypeId, DateTime AttDate);
		Task<Result<LeaveBalance>> AddLeave(Guid empId, Guid lTypeID, DateTime attDate, bool IsHalfDay, bool IsFirstHalf, Guid approverId);
		Task<Result<LeaveBalance>> UpdateLeave(Guid empId, Guid lTypeID, DateTime attDate, bool isHalfDay);
		Task<Result<LeaveBalance>> LeaveToLeave(Guid empId, Guid presentLTypeID, Guid requiredLTypeID, DateTime attDate, bool presentHalfDayType, bool requiredHalfDayType, bool IsFirstHalf, Guid approverId);
		Task<Result<LeaveBalance>> LeavesModifyBasedOnAttendance(Attendance presentAttendance, AttendanceDetails requiredAttendance, Guid approverId);
		Task<IEnumerable<LeaveBalance>> GetConsolidatedLB(Guid leaveTypeId, Guid empId, DateTime fromDate, DateTime toDate);

		Task AdjustLB(List<LeaveBalance> LB, ApplyLeave applyLeave, ApplyLeaveType applyLeaveType);
	}
	public class LeaveBalanceService : BaseService<LeaveBalance>, ILeaveBalanceService
	{
		public LeaveBalanceService(IUnitOfWork uow) : base(uow)
		{
		}
		public async Task<List<LeaveBalance>> GetEmpLeaveBalance(Guid employeeId, Guid leaveTypeId, DateTime attDate)
		{
			var list = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.EmployeeId == employeeId && x.LeaveTypeId == leaveTypeId &&
								x.EffectiveFrom.Date <= attDate.Date && x.EffectiveTo.Date >= attDate.Date,
								include: i => i.Include(x => x.LeaveType));
			return list.ToList();
		}

		public async Task<Result<LeaveBalance>> AddLeave(Guid empId, Guid lTypeID, DateTime attDate, bool IsHalfDay, bool IsFirstHalf, Guid approverId)
		{
			var result = new Result<LeaveBalance>();
			var applyLeaveDetails = await UOW.GetRepositoryAsync<ApplyLeaveDetails>().SingleAsync(x => x.LeaveTypeId == lTypeID &&
												x.ApplyLeave.EmployeeId == empId && x.ApplyLeave.Status == (int)ApplyLeaveSts.Approved &&
												x.LeaveDate.Date == attDate.Date,
												include: x => x.Include(x => x.ApplyLeave));

			List<LeaveBalance> lBalanceDetails = await GetEmpLeaveBalance(empId, lTypeID, attDate);
			if (lBalanceDetails.Sum(x => x.Leaves) < (IsHalfDay ? 0.5m : 1))
			{
				result.AddMessageItem(new MessageItem("Employee doesn't have enough leaves"));
				return result;
			}
			if (applyLeaveDetails == null)
			{
				var noOfLeaves = IsHalfDay ? 0.5m : 1;
				var applyLeave = new ApplyLeave
				{
					Status = (int)ApplyLeaveSts.Approved,
					EmployeeId = empId,
					FromDate = attDate,
					ToDate = attDate,
					ToHalf = IsHalfDay && !IsFirstHalf,
					FromHalf = IsHalfDay && IsFirstHalf,
					ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
					{
						LeaveTypeId = lTypeID,
						NoOfLeaves = noOfLeaves,
					} },
					NoOfLeaves = noOfLeaves,
					LeaveTypes = JsonConvert.SerializeObject(new List<LeaveTypeObj>() { new LeaveTypeObj { Days = noOfLeaves, Name = lBalanceDetails.First().LeaveType.Name } }),
					ApprovedById = approverId
				};
				await UOW.GetRepositoryAsync<ApplyLeave>().AddAsync(applyLeave);

				applyLeaveDetails = new ApplyLeaveDetails
				{
					ApplyLeaveId = applyLeave.ID,
					LeaveTypeId = lTypeID,
					LeaveDate = attDate,
					IsHalfDay = IsHalfDay,
					IsFirstHalf = IsHalfDay && IsFirstHalf,
					LeaveCount = IsHalfDay ? 0.5m : 1,
				};
				await UOW.GetRepositoryAsync<ApplyLeaveDetails>().AddAsync(applyLeaveDetails);

				var LB = await GetConsolidatedLB(applyLeaveDetails.LeaveTypeId, applyLeave.EmployeeId, attDate.Date.Date, attDate.Date.Date);

				await AdjustLB(LB.ToList(), applyLeave, new ApplyLeaveType() { LeaveTypeId = lTypeID, NoOfLeaves = noOfLeaves });
				// Adding Leave Balance
			}
			else
			{
				var LB = await GetConsolidatedLB(applyLeaveDetails.LeaveTypeId, applyLeaveDetails.ApplyLeave.EmployeeId, attDate.Date.Date, attDate.Date.Date);

				await AdjustLB(LB.ToList(), applyLeaveDetails.ApplyLeave, new ApplyLeaveType() { LeaveTypeId = lTypeID, NoOfLeaves = IsHalfDay ? 0.5m : 1 });

				applyLeaveDetails.LeaveCount += IsHalfDay ? 0.5m : 1;
				UOW.GetRepositoryAsync<ApplyLeaveDetails>().UpdateAsync(applyLeaveDetails);
			}
			return result;
		}

		public async Task<Result<LeaveBalance>> UpdateLeave(Guid empId, Guid lTypeID, DateTime attDate, bool isHalfDay)
		{
			var result = new Result<LeaveBalance>();

			var applyLeaveDetails = await UOW.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync
				   (x => x.LeaveCount > 0 && x.ApplyLeave.EmployeeId == empId
					   && x.ApplyLeave.Status == (int)ApplyLeaveSts.Approved && x.LeaveDate.Date == attDate.Date,
					   include: x => x.Include(x => x.ApplyLeave));
			if (!applyLeaveDetails.Any())
			{
				result.AddMessageItem(new MessageItem("Previously didn't have any applied leave"));
				return result;
			}
			int LeaveDetailsupdated = 0;
			foreach (var item in applyLeaveDetails)
			{
				var leaveType = await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.ID == item.LeaveTypeId);
				if (!leaveType.DefaultPayoff)
				{
					var lBalance = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.LeaveTypeId == item.LeaveTypeId && x.ApplyLeaveId == item.ApplyLeaveId);

					isHalfDay = item.LeaveCount == 0.5m || isHalfDay;

					if (lBalance.Any() && lBalance.Sum(x => x.Leaves) != 0)//== (isHalfDay ? -0.5m : -1))
					{
						List<LeaveBalance> lBalanceDetails = await GetEmpLeaveBalance(empId, lTypeID, attDate);
						if (lBalanceDetails.Sum(x => x.Leaves) < (isHalfDay ? -0.5m : -1))
						{
							result.AddMessageItem(new MessageItem("Employee doesn't have enough leaves"));
							return result;
						}
						decimal requredLeaves = isHalfDay ? -0.5m : -1;
						foreach (var LB in lBalance.OrderByDescending(x => x.EffectiveFrom))
						{
							if (isHalfDay)
							{
								if (LB.Leaves <= -0.5m)
								{
									LB.Leaves += 0.5m;
									UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(LB);
									break;
								}
							}
							else
							{
								if (LB.Leaves <= requredLeaves)
								{
									LB.Leaves += -requredLeaves;
									requredLeaves = 0;
									UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(LB);
									break;
								}
								else if (LB.Leaves == -0.5m)
								{
									LB.Leaves += 0.5m;
									requredLeaves += 0.5m;
									UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(LB);
								}
							}
						}
						item.LeaveCount -= (isHalfDay ? 0.5m : 1);
						if (LeaveDetailsupdated == 0)
						{
							UOW.GetRepositoryAsync<ApplyLeaveDetails>().UpdateAsync(item);
							LeaveDetailsupdated++;
						}
					}
					else
					{
						result.AddMessageItem(new MessageItem("Previously didn't have any applied leave"));
					}
				}
			}

			return result;
		}

		public async Task<Result<LeaveBalance>> LeaveToLeave(Guid empId, Guid presentLTypeID, Guid requiredLTypeID, DateTime attDate, bool presentHalfDayType, bool requiredHalfDayType, bool IsFirstHalf, Guid approverId)
		{
			var result = new Result<LeaveBalance>();
			//checking the leave types if not same then allowing
			if (presentLTypeID != requiredLTypeID)
			{
				// reverting the previous half day leave
				result = await UpdateLeave(empId, presentLTypeID, attDate, presentHalfDayType);

				if (!result.HasError)
				{
					result = await AddLeave(empId, requiredLTypeID, attDate, requiredHalfDayType, IsFirstHalf, approverId);
				}
			}//if leave type is same but full day to half day half day to full day marking is required
			else if (presentHalfDayType != requiredHalfDayType)
			{
				if (presentHalfDayType)
					result = await AddLeave(empId, requiredLTypeID, attDate, true, IsFirstHalf, approverId);
				else
					result = await UpdateLeave(empId, presentLTypeID, attDate, true);

			}
			return result;
		}
		public async Task<Result<LeaveBalance>> LeavesModifyBasedOnAttendance(Attendance presentAttendance, AttendanceDetails requiredAttendance, Guid approverId)
		{

			var result = new Result<LeaveBalance>();
			if (presentAttendance.AttendanceStatus is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave) ||
					presentAttendance.HalfDayType is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave))
			{
				var applyLeaveDetails = await UOW.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync
				(x => x.LeaveCount > 0 && x.ApplyLeave.EmployeeId == presentAttendance.EmployeeId
				   && x.ApplyLeave.Status == (int)ApplyLeaveSts.Approved && x.LeaveDate.Date == presentAttendance.AttendanceDate.Date);

				if (applyLeaveDetails.Count() > 1 && requiredAttendance.LeaveTypeId != null)
				{
					result.AddMessageItem(new MessageItem("Not possible to modify"));
					return result;
				}

				//[Mohan]
				//Null value issue
				if (presentAttendance.IsHalfDay == null)
				{
					presentAttendance.IsHalfDay = false;
				}
				if (int.Parse(requiredAttendance.AttendanceStatus) is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave) ||
					 requiredAttendance.HalfDayType is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave))
				{
					result = await LeaveToLeave(presentAttendance.EmployeeId, (Guid)presentAttendance.LeaveTypeID, (Guid)requiredAttendance.LeaveTypeId, presentAttendance.AttendanceDate, (bool)presentAttendance.IsHalfDay, requiredAttendance.IsHalfDay, requiredAttendance.IsFirstOff, approverId);
				}
				else
				{
					result = await UpdateLeave(presentAttendance.EmployeeId, (Guid)presentAttendance.LeaveTypeID, presentAttendance.AttendanceDate, (bool)presentAttendance.IsHalfDay);
				}
				return result;
			}

			if (int.Parse(requiredAttendance.AttendanceStatus) is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave) ||
				requiredAttendance.HalfDayType is ((int)AttendanceStatus.Leave) or ((int)AttendanceStatus.HalfDayLeave))
			{
				result = await AddLeave(presentAttendance.EmployeeId, (Guid)requiredAttendance.LeaveTypeId, presentAttendance.AttendanceDate, requiredAttendance.IsHalfDay, requiredAttendance.IsFirstOff, approverId);

			}

			return result;
		}

		public async Task<IEnumerable<LeaveBalance>> GetConsolidatedLB(Guid leaveTypeId, Guid empId, DateTime fromDate, DateTime toDate)
		{
			List<LeaveBalance> leaveBalances = new List<LeaveBalance>();
			var leaveBlnc = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.EffectiveFrom.Date <= fromDate.Date
																						&& x.EffectiveTo.Date >= toDate.Date
																						&& x.LeaveTypeId == leaveTypeId
																						&& x.EmployeeId == empId);
			//if (leaveBlnc.Any(x => x.EffectiveFrom == null))//For Earned leaves(doesn't have EffectFrom and To)
			//{
			//	if (leaveBlnc.Sum(x => x.Leaves) > 0)
			//	{
			//		leaveBalances.Add(new LeaveBalance()
			//		{
			//			EmployeeId = empId,
			//			LeaveTypeId = leaveTypeId,
			//			LeavesAddedOn = DateTime.Now,
			//			Leaves = leaveBlnc.Sum(x => x.Leaves),
			//			Type = (int)LeaveTypesScreens.LeaveType
			//		});
			//	}
			//} [Mohan review 06/25/2023]
			//else//For Other leaves(have EffectFrom and To) CL,SL....
			//{
			foreach (var LBIem in leaveBlnc.GroupBy(x => x.EffectiveFrom))
			{
				var list = leaveBlnc.Where(x => x.EffectiveFrom == LBIem.Key);
				{
					if (list.Sum(x => x.Leaves) > 0)
					{
						leaveBalances.Add(new LeaveBalance()
						{
							EmployeeId = empId,
							LeaveTypeId = leaveTypeId,
							LeavesAddedOn = DateTime.Now,
							Leaves = list.Sum(x => x.Leaves),
							Type = (int)LeaveTypesScreens.LeaveType,
							EffectiveFrom = list.First().EffectiveFrom,
							EffectiveTo = list.First().EffectiveTo,
						});
					}
				}
			}
			//}
			return leaveBalances;
		}

		public async Task AdjustLB(List<LeaveBalance> LB, ApplyLeave applyLeave, ApplyLeaveType applyLeaveType)
		{
			decimal requiredLeaves = applyLeaveType.NoOfLeaves;
			if (LB.Any(x => x.EffectiveFrom == null))//For earned leaves
			{
				// Adding Leave Balance
				var balance = new LeaveBalance
				{
					EmployeeId = applyLeave.EmployeeId,
					LeaveTypeId = applyLeaveType.LeaveTypeId,
					LeavesAddedOn = DateTime.Now,
					Leaves = 0 - requiredLeaves, //update as -ve values
					ApplyLeaveId = applyLeave.ID,
					Type = (int)LeaveTypesScreens.ApplyLeaves
				};
				await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(balance);

			}
			else
			{//Non Earned Leaves

				foreach (var leave in LB.OrderBy(x => x.EffectiveFrom))
				{
					if (leave.Leaves >= requiredLeaves)
					{
						// Adding Leave Balance
						var balance = new LeaveBalance
						{
							EmployeeId = applyLeave.EmployeeId,
							LeaveTypeId = applyLeaveType.LeaveTypeId,
							LeavesAddedOn = DateTime.Now,
							Leaves = 0 - requiredLeaves, //update as -ve values
							ApplyLeaveId = applyLeave.ID,
							Type = (int)LeaveTypesScreens.ApplyLeaves,
							EffectiveFrom = leave.EffectiveFrom,
							EffectiveTo = leave.EffectiveTo,
						};
						await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(balance); ;
						requiredLeaves = 0;
						break;
					}
					else
					{
						requiredLeaves = requiredLeaves - leave.Leaves;
						// Adding Leave Balance
						var balance = new LeaveBalance
						{
							EmployeeId = applyLeave.EmployeeId,
							LeaveTypeId = applyLeaveType.LeaveTypeId,
							LeavesAddedOn = DateTime.Now,
							//Leaves = 0 - requiredLeaves, //update as -ve values
							Leaves = 0 - leave.Leaves,
							ApplyLeaveId = applyLeave.ID,
							Type = (int)LeaveTypesScreens.ApplyLeaves,
							EffectiveFrom = leave.EffectiveFrom,
							EffectiveTo = leave.EffectiveTo,
						};
						await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(balance);
					}
				}
			}
		}
	}
}
