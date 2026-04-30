using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using TranSmart.Core.Result;
using System.Threading.Tasks;

namespace TranSmart.Service.Leave
{
	public interface ILeaveAccumulationService : IBaseService<LeaveBalance>
	{
		Task<Result<LeaveBalance>> LeavesAccumulationSchedule(DateTime scheduleDate);
		Task<decimal> GetEmpLeaves(Guid empId, Guid leaveTypeId, DateTime attDate);
	}
	public class LeaveAccumulationService : BaseService<LeaveBalance>, ILeaveAccumulationService
	{
		public LeaveAccumulationService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<decimal> GetEmpLeaves(Guid empId, Guid leaveTypeId, DateTime attDate)
		{
			return await UOW.GetRepositoryAsync<LeaveBalance>().SumOfDecimalAsync(x => x.EmployeeId == empId
							&& x.LeaveTypeId == leaveTypeId && x.EffectiveFrom.Date <= attDate.Date
																								&& x.EffectiveTo.Date >= attDate.Date, sumBy: x => x.Leaves);
		}
		public virtual async Task<Result<LeaveBalance>> LeavesAccumulationSchedule(DateTime scheduleDate)
		{
			var Result = new Result<LeaveBalance>();
			try
			{

				List<LeaveType> LeaveTypes = (await UOW.GetRepositoryAsync<LeaveType>().GetAsync(x => x.Status)).ToList();
				foreach (LeaveType LType in LeaveTypes)
				{
					#region Leaves Lapse
					#endregion

					List<LeaveTypeSchedule> LTypeSchedules = (await UOW.GetRepositoryAsync<LeaveTypeSchedule>().GetAsync(x => x.LeaveTypeId == LType.ID)).ToList();
					foreach (LeaveTypeSchedule schedule in LTypeSchedules)
					{
						#region Leaves accumulation Process 
						//Getting EmployeesList From LeaveType Rules
						List<Employee> EmloyeesList = await GetApplicableEmpList(LType);

						//After Exceptions Employees
						EmloyeesList = GetEmpListAfterExceptions(LType, EmloyeesList);

						foreach (Employee emp in EmloyeesList)
						{
							var leaveBalance = new LeaveBalance();

							//Schedule validation from LeaveTypeSchedules
							if (IsScheduledDate(schedule, emp, scheduleDate))
							{
								//Pre -consumable leaves
								ApprovedLeaves preConsuLeaves = await UOW.GetRepositoryAsync<ApprovedLeaves>().SingleAsync(x => x.EmployeeId == emp.ID && x.LeaveTypeId == LType.ID);

								//employee present having leaves
								List<LeaveBalance> presentLBalance = (await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.LeaveTypeId == LType.ID && x.EmployeeId == emp.ID)).ToList();
								presentLBalance ??= new List<LeaveBalance>();

								//checking if employee is eligible based on employee joining date or date of confirmation 
								if (EffectiveDate(LType, emp.DateOfJoining, emp.DateOfJoining, scheduleDate))
								{
									leaveBalance.EmployeeId = emp.ID;
									leaveBalance.LeavesAddedOn = scheduleDate;
									leaveBalance.Type = (int)LeaveTypesScreens.LeaveType;
									leaveBalance.LeaveTypeId = LType.ID;

									if (schedule.AccType == 1)//yearly
										leaveBalance.Leaves = EntitlementLeavesYearly(LType, emp.DateOfJoining, emp.DateOfJoining, schedule);
									else if (schedule.AccType == 2)//Helf_Yearly
										leaveBalance.Leaves = EntitlementLeavesHelfYearly(LType, emp.DateOfJoining, emp.DateOfJoining, schedule);
									else if (schedule.AccType == 3)//Quarterly
										leaveBalance.Leaves = EntitlementLeavesQuatraly(LType, emp.DateOfJoining, emp.DateOfJoining, schedule);
									else if (schedule.AccType == 4)//Monthly
										leaveBalance.Leaves = EntitlementLeavesMonthly(LType, emp.DateOfJoining, emp.DateOfJoining, schedule);

									// leaves rounding based on duration
									leaveBalance.Leaves = LeavesBasedOnDuration(LType.Duration, leaveBalance.Leaves);

									// PreConsumableLeaves Validation
									if (preConsuLeaves != null && preConsuLeaves.NoOfLeaves > 0)
									{
										Tuple<decimal, decimal> leavedays = LeavesAfterPreconsumableLeaves(leaveBalance.Leaves, preConsuLeaves.NoOfLeaves);
										leaveBalance.Leaves = leavedays.Item1;
										preConsuLeaves.NoOfLeaves = (int)leavedays.Item2;
										leaveBalance.PreconsumableLeave = preConsuLeaves;
									}
									else
										leaveBalance.PreconsumableLeave = preConsuLeaves;

									//Carry forward leaves calculations
									var CarryLBalance = new LeaveBalance
									{
										EmployeeId = emp.ID,
										Type = (int)LeaveTypesScreens.DeductionFromPreConsumed,
										LeaveTypeId = LType.ID,
										LeavesAddedOn = scheduleDate
									};
									decimal leaves = GetCarryForwardLeaves(presentLBalance.Sum(x => x.Leaves), schedule.FwdType, schedule.FwdLimit, schedule.FwdDays == null ? 0 : (int)schedule.FwdDays, schedule.FwdPercentage == null ? 0 : (int)schedule.FwdPercentage);
									CarryLBalance.Leaves = GetLeavesCountAfterRoundTypeCalc(leaves, LType);
									if (CarryLBalance.Leaves != 0)
										UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(CarryLBalance);

									//checking the open balance and max balance
									leaveBalance.Leaves = leaveBalance.Leaves < schedule.OpeningBalance ? Convert.ToDecimal(schedule.OpeningBalance) : leaveBalance.Leaves;
									leaveBalance.Leaves = leaveBalance.Leaves > schedule.MaxBalance ? Convert.ToDecimal(schedule.MaxBalance) : leaveBalance.Leaves;

								}

							}
							if (leaveBalance.Leaves != 0)
							{
								UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(leaveBalance);
							}
						}

						#endregion
					}
				}
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Result.AddMessageItem(new MessageItem(ex.Message));
			}
			return Result;
		}

		public async Task<Result<LeaveBalance>> LeavesLapseSchedule(LeaveType lType, DateTime scheduledDate)
		{
			var Result = new Result<LeaveBalance>();
			try
			{
				List<LeaveTypeSchedule> lTypeSchedules = (await UOW.GetRepositoryAsync<LeaveTypeSchedule>().GetAsync(x => x.LeaveTypeId == lType.ID)).ToList();
				foreach (LeaveTypeSchedule schedule in lTypeSchedules)
				{
					List<Employee> EmloyeesList = await GetApplicableEmpList(lType);

					foreach (Employee Emp in EmloyeesList)
					{
						var LapsedBalance = new LeaveBalance();
						List<LeaveBalance> Balance = (await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.LeaveTypeId == lType.ID && x.EmployeeId == Emp.ID)).ToList();
						//Schedule validation from LeaveTypeSchedules
						bool SchedleStatus = IsScheduledDateForLapsed(schedule, Emp, scheduledDate);
						if (SchedleStatus && Balance.Sum(x => x.Leaves) > 0)
						{
							LapsedBalance.EmployeeId = Emp.ID;
							LapsedBalance.LeaveTypeId = lType.ID;
							LapsedBalance.Leaves = -Balance.Sum(x => x.Leaves);
							LapsedBalance.Type = (int)LeaveTypesScreens.LeaveLapsedSchedule;
							LapsedBalance.LeavesAddedOn = TimeStamp();
						}
						if (LapsedBalance.Leaves != 0)
						{
							await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(LapsedBalance);
							await UOW.SaveChangesAsync();
						}
					}

				}
			}
			catch (Exception Ex)
			{
				Result.AddMessageItem(new MessageItem(Ex.Message));
			}
			return Result;


		}


		#region ScheduleDate
		public static bool IsScheduledDate(LeaveTypeSchedule schedule, Employee emp, DateTime scheduleDate)
		{
			// AccType 1 for yearly, 2 for halfYearly, 3 for quarterly, 4 for monthly
			if (schedule.AccType == -1)// DOB month
			{
				return scheduleDate.Month == emp.DateOfBirth.Month;
			}
			else if (schedule.AccType == -2)// Date of join Date
			{
				return scheduleDate.Date == emp.DateOfJoining.Date;
			}
			else if (schedule.AccType == -3)//Wedding Anniversary
			{
				if (emp.MarriageDay == null || DateTime.MinValue.Date == emp.MarriageDay)
					return false;
				else
					return scheduleDate.Date == Convert.ToDateTime(emp.MarriageDay).Date;
			}
			else if (schedule.AccType == 1)
			{
				if (schedule.AccOnYearly == scheduleDate.Month && schedule.AccOnDay == scheduleDate.Day)
				{
					return true;
				}
				else
					return false;
			}
			else if (schedule.AccType == 2)
			{
				//AccOnHalfYearly means 1-Jan & Jul, 2-Feb & Aug, 3-Mar & Sep,4-Apr & Oct,5-May & Nov,6-Jun & Dec
				if (schedule.AccOnDay == scheduleDate.Day &&
					(scheduleDate.Month == schedule.AccOnHalfYearly ||
					scheduleDate.Month == schedule.AccOnHalfYearly + 6))
				{
					return true;
				}
			}
			else if (schedule.AccType == 3)
			{
				//AccOnQuarterly means 1 - Jan & Apr & Jul & Oct
				//AccOnQuarterly means 2 - Feb & May & Aug & Nov
				//AccOnQuarterly means 3 - Mar & Jun & Sep & Dec
				if (schedule.AccOnDay == scheduleDate.Day &&
					(scheduleDate.Month == schedule.AccOnQuarterly ||
					scheduleDate.Month == schedule.AccOnQuarterly + 3 ||
					scheduleDate.Month == schedule.AccOnQuarterly + 6 ||
					scheduleDate.Month == schedule.AccOnQuarterly + 9))
				{
					return true;
				}

			}
			else if (schedule.AccType == 4 && schedule.AccOnDay == scheduleDate.Day)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region ScheduleDateForLapsed
		public static bool IsScheduledDateForLapsed(LeaveTypeSchedule schedule, Employee emp, DateTime scheduleDate)
		{
			// ResetType 1 for yearly , 2 for half yearly , 3 for quarterly , 4 for monthly
			if (schedule.ResetType == -1)// DOB month
			{
				return scheduleDate.Month == emp.DateOfBirth.Month;
			}
			else if (schedule.ResetType == -2)// Date of join Date
			{
				return scheduleDate.Date == emp.DateOfJoining.Date;
			}
			else if (schedule.ResetType == -3)//Wedding Anniversary
			{
				if (emp.MarriageDay == null || DateTime.MinValue.Date == emp.MarriageDay)
					return false;
				else
					return scheduleDate.Date == Convert.ToDateTime(emp.MarriageDay).Date;
			}
			else if (schedule.ResetType == 1)
			{
				if (schedule.ResOnYearly == scheduleDate.Month && schedule.ResOnDay == scheduleDate.Day)
				{
					return true;
				}
				else
					return false;
			}
			else if (schedule.ResetType == 2)
			{
				if (schedule.ResOnDay == scheduleDate.Day &&
					(scheduleDate.Month == schedule.ResOnHalfYearly ||
					scheduleDate.Month == schedule.ResOnHalfYearly + 6))
				{
					return true;
				}
			}
			else if (schedule.ResetType == 3)
			{
				if (schedule.ResOnDay == scheduleDate.Day &&
					(scheduleDate.Month == schedule.ResOnQuarterly ||
					scheduleDate.Month == schedule.ResOnQuarterly + 3 ||
					scheduleDate.Month == schedule.ResOnQuarterly + 6 ||
					scheduleDate.Month == schedule.ResOnQuarterly + 9))
				{
					return true;
				}

			}
			else if (schedule.ResetType == 4 && schedule.ResOnDay == scheduleDate.Day)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region EffectiveType
		public static bool EffectiveDate(LeaveType lType, DateTime DOJ, DateTime dateOfConfirm, DateTime scheduleDate)
		{
			// EffectiveType: 1 for Days, 2 for Months, 3 for Years
			//EffectiveBy: 1 for Employee JaoningDate, 2 fro Employee ConfirmationDate
			if (lType.EffectiveType == 1)
			{
				if (lType.EffectiveBy == 1)
					return (scheduleDate - DOJ).Days >= lType.EffectiveAfter;
				else if (lType.EffectiveBy == 2)
					return (scheduleDate - dateOfConfirm).Days >= lType.EffectiveAfter;
			}
			else if (lType.EffectiveType == 2)
			{
				if (lType.EffectiveBy == 1)
					return (scheduleDate.Month - DOJ.Month) + 12 * (DateTime.Now.Year - DOJ.Year) >= lType.EffectiveAfter;
				else if (lType.EffectiveBy == 2)
					return (scheduleDate.Month - dateOfConfirm.Month) + 12
								* (DateTime.Now.Year - dateOfConfirm.Year) >= lType.EffectiveAfter;
			}
			else if (lType.EffectiveType == 3)
			{
				if (lType.EffectiveBy == 1)
					return (scheduleDate - DOJ).Days / 365 >= lType.EffectiveAfter;
				else if (lType.EffectiveBy == 2)
					return (scheduleDate - dateOfConfirm).Days / 365 >= lType.EffectiveAfter;
			}
			return false;
		}
		#endregion

		public static decimal LeavesBasedOnDuration(int durationType, decimal leaves)
		{
			//Duration 1 for FullDay 2 for HalfDay
			return durationType switch
			{
				1 => leaves,
				2 => leaves / 2,
				_ => leaves,
			};
		}

		public static Tuple<decimal, decimal> LeavesAfterPreconsumableLeaves(decimal presentLeaves, decimal preconsumableLeaves)
		{
			preconsumableLeaves = presentLeaves > preconsumableLeaves ? 0 : preconsumableLeaves - presentLeaves;
			presentLeaves = presentLeaves < preconsumableLeaves ? 0 : presentLeaves - preconsumableLeaves;
			return Tuple.Create(presentLeaves, preconsumableLeaves);
		}

		#region Entitlement Leaves Calculation
		public static decimal EntitlementLeavesYearly(LeaveType lType, DateTime DOJ, DateTime dateOfConfirm, LeaveTypeSchedule schedule)
		{
			decimal balance = 0;
			//AccOnYearly is the calender month number 
			var StartDate = new DateTime(DateTime.Now.Year, (int)schedule.AccOnYearly, schedule.AccOnDay);
			DateTime EndDate = StartDate.AddMonths(12);
			//Effective By: 1 for Joining Date, 2 for Confirmed Date
			DateTime employeeEffectiveDate = lType.EffectiveBy == 1 ? DOJ : dateOfConfirm;
			// Prorate By: 1 for Number of effective months,2 for Number of Effective Days
			if (lType.ProrateByT == 1) // Months
			{
				//Leave entitlement start date is less then the employee joining date
				//Calculate on prorate base
				if (StartDate < employeeEffectiveDate && EndDate > employeeEffectiveDate)
				{
					int diffMonths = ((EndDate.Year - employeeEffectiveDate.Year) * 12) + StartDate.AddMonths(12).Month - employeeEffectiveDate.Month;

					decimal leaves = (schedule.NoOfDays / 12m) * diffMonths;

					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
				else if (StartDate > employeeEffectiveDate)
				{
					decimal leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
			}

			else if (lType.ProrateByT == 2) //Days
			{
				decimal noOfDaysInYear = (EndDate - StartDate).Days;
				if (StartDate < employeeEffectiveDate && EndDate > employeeEffectiveDate)
				{
					decimal employeeDays = (EndDate - employeeEffectiveDate).Days;
					decimal leaves = (schedule.NoOfDays / noOfDaysInYear) * employeeDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
				else if (employeeEffectiveDate > StartDate)
				{
					decimal leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
			}
			return balance;
		}

		public static decimal EntitlementLeavesHelfYearly(LeaveType lType, DateTime DOJ, DateTime dateOfConfirm, LeaveTypeSchedule schedule)
		{
			decimal balance = 0;
			//AccOnHalfYearly is the calender month number 
			var startDate = new DateTime(DateTime.Now.Year, (int)schedule.AccOnHalfYearly, schedule.AccOnDay);
			DateTime endDate = startDate.AddMonths(6);
			//Effective By: 1 for Joining Date, 2 for Confirmed Date
			DateTime employeeEffectiveDate = lType.EffectiveBy == 1 ? DOJ : dateOfConfirm;
			// Prorate By: 1 for Number of effective months,2 for Number of Effective Days
			if (lType.ProrateByT == 1) // Months
			{
				//Leave entitlement start date is less then the employee joining date
				//Calculate on prorate base
				if (employeeEffectiveDate > startDate && employeeEffectiveDate < endDate)
				{
					int diffMonths = ((endDate.Year - employeeEffectiveDate.Year) * 12) + endDate.Month - employeeEffectiveDate.Month;
					decimal leaves = (schedule.NoOfDays / 6) * diffMonths;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);

				}
				else if (employeeEffectiveDate < startDate)
				{
					decimal leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
			}
			else if (lType.ProrateByT == 2) //Days
			{
				if (employeeEffectiveDate > startDate && employeeEffectiveDate < endDate)
				{
					double daysInThisPeriod = (endDate - startDate).Days;
					int diffDays = (endDate - DOJ).Days;
					decimal Leaves = (decimal)(schedule.NoOfDays / daysInThisPeriod) * diffDays;
					balance = GetLeavesCountAfterRoundTypeCalc(Leaves, lType);
				}
				else if (DOJ < startDate)
				{
					decimal Leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(Leaves, lType);
				}
				else
					balance = 0;
			}
			return balance;
		}

		public static decimal EntitlementLeavesQuatraly(LeaveType lType, DateTime DOJ, DateTime dateOfConfirm, LeaveTypeSchedule schedule)
		{
			decimal balance = 0;
			//AccOnHalfYearly is the calender month number 
			var startDate = new DateTime(DateTime.Now.Year, (int)schedule.AccOnQuarterly, schedule.AccOnDay);
			DateTime endDate = startDate.AddMonths(3);
			//Effective By: 1 for Joining Date, 2 for Confirmed Date
			DateTime employeeEffectiveDate = lType.EffectiveBy == 1 ? DOJ : dateOfConfirm;
			// Prorate By: 1 for Number of effective months,2 for Number of Effective Days
			if (lType.ProrateByT == 1)
			{
				if (employeeEffectiveDate > startDate && employeeEffectiveDate < endDate)
				{
					int Totalmonths = ((endDate.Year - employeeEffectiveDate.Year) * 12) + endDate.Month - employeeEffectiveDate.Month;
					decimal leaves = (schedule.NoOfDays / 3m) * Totalmonths;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
				else if (employeeEffectiveDate < startDate)
				{
					decimal leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
			}
			else if (lType.ProrateByT == 2) //Days
			{
				if (startDate < employeeEffectiveDate && endDate > employeeEffectiveDate)
				{
					int daysInThisPeriod = (endDate - startDate).Days;
					int diffDays = (endDate - employeeEffectiveDate).Days;
					decimal leaves = schedule.NoOfDays / Convert.ToDecimal(daysInThisPeriod) * diffDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
				else if (startDate > employeeEffectiveDate)
				{
					decimal leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
			}

			return balance;
		}

		public static decimal EntitlementLeavesMonthly(LeaveType lType, DateTime DOJ, DateTime dateOfConfirm, LeaveTypeSchedule schedule)
		{
			decimal balance = 0;
			//Effective By: 1 for Joining Date, 2 for Confirmed Date
			DateTime employeeEffectiveDate = lType.EffectiveBy == 1 ? DOJ : dateOfConfirm;
			//AccOnHalfYearly is the calender month number 
			var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, schedule.AccOnDay);
			DateTime endDate = startDate.AddMonths(1);
			// Prorate By: 1 for Number of effective months,2 for Number of Effective Days

			if (lType.ProrateByT == 1) // Months
			{
				if (endDate > employeeEffectiveDate)
				{
					decimal Leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(Leaves, lType);
				}
			}
			else if (lType.ProrateByT == 2) //Days
			{

				if (startDate < employeeEffectiveDate && endDate > employeeEffectiveDate)
				{
					int daysInThisPeriod = (endDate - startDate).Days;
					int diffDays = (endDate - employeeEffectiveDate).Days;
					decimal leaves = (Convert.ToDecimal(schedule.NoOfDays) / Convert.ToDecimal(daysInThisPeriod)) * diffDays;
					balance = GetLeavesCountAfterRoundTypeCalc(leaves, lType);
				}
				else if (startDate > employeeEffectiveDate)
				{
					decimal Leaves = schedule.NoOfDays;
					balance = GetLeavesCountAfterRoundTypeCalc(Leaves, lType);
				}


			}
			return balance;
		}
		#endregion

		//Let us take some examples to understand the round-off options.
		//Let us assume that an employee's prorated leave count is 3.43 days of leave. 
		//When enabled, let us see how each combination works:
		//Prorated Leave Count = 3.43 days
		//Nearest Day = 3 days(rounded-off to the nearest whole value)
		//Nearest Half Day =3.5 days(rounded-off to the nearest .5 value) 
		//Maximum Day = 4 days
		//Maximum Half Day = 3.5 days 
		//Minimum Day = 3 days
		//Minimum Half Day = 3 days
		//Minimum Quarter Day = 3.25 days
		public static decimal GetLeavesCountAfterRoundTypeCalc(decimal leaves, LeaveType lType)
		{
			//RoundOffTo :1 for FullDay 2 For HalfDay
			//RoundOff:  1 For Nearest 2 for Minimum 3 for maximum
			if (lType.RoundOffTo == 1) // FullDay
			{
				if (lType.RoundOff == 1)
				{
					leaves = Math.Round(leaves);
				}
				else if (lType.RoundOff == 2)
				{
					leaves = Math.Floor(leaves);
				}
				else if (lType.RoundOff == 3)
				{
					leaves = Math.Ceiling(leaves);
				}
			}
			else if (lType.RoundOffTo == 2) // HalfDay
			{
				// 1 For Nearest 2 for Minimum 3 for maximum
				if (lType.RoundOff == 1)
				{
					leaves = Math.Round(leaves);
				}
				else if (lType.RoundOff == 2)
				{
					leaves = (leaves - Math.Floor(leaves)) > (decimal)0.5 ? Math.Floor(leaves) + (decimal)0.5 : Math.Floor(leaves);
				}
				else if (lType.RoundOff == 3)
				{
					if ((leaves - Math.Floor(leaves)) > (decimal)0.5)
					{
						leaves = Math.Ceiling(leaves);
					}
					else
					{
						leaves = (leaves - Math.Floor(leaves)) > 0.1m ? Math.Floor(leaves) + 0.5m : Math.Floor(leaves);
					}
				}
			}
			return leaves;
		}

		public static decimal GetCarryForwardLeaves(decimal presentLeaves, int fwdType, int fwdLimit, int fwdDays, int fwdPercentage)
		{
			//fwdType:1 for percentage wise,2 for Days Wise
			if (fwdType == 1)
			{
				decimal CarryLeaves = ((decimal)fwdPercentage / (decimal)100) * presentLeaves;
				presentLeaves -= CarryLeaves;
				return presentLeaves > fwdLimit ? -fwdLimit : -presentLeaves;
			}
			else if (fwdType == 2)
			{
				decimal CarryLeaves = presentLeaves > fwdDays ? fwdDays : presentLeaves;
				presentLeaves -= CarryLeaves;
				return presentLeaves > fwdLimit ? -fwdLimit : -presentLeaves;
			}

			return 0;
		}

		public async Task<List<Employee>> GetApplicableEmpList(LeaveType lType)
		{
			var employeeList = (await UOW.GetRepositoryAsync<Employee>().GetAsync(x => x.Status == 1)).ToList();
			if (lType.Gender != 4)//
			{
				employeeList = employeeList.Where(g => g.Gender == lType.Gender).ToList();
			}
			if (lType.MaritalStatus != 3)
			{
				employeeList = employeeList.Where(m => m.MaritalStatus == lType.MaritalStatus).ToList();
			}
			if (!string.IsNullOrEmpty(lType.Location))
			{
				employeeList = employeeList.Where(l => lType.Location.Contains(l.WorkLocationId.ToString())).ToList();
			}
			if (!string.IsNullOrEmpty(lType.Department))
			{
				employeeList = employeeList.Where(d => lType.Department.Contains(d.DepartmentId.ToString())).ToList();
			}
			if (!string.IsNullOrEmpty(lType.Designation))
			{
				employeeList = employeeList.Where(d => lType.Designation.Contains(d.DesignationId.ToString())).ToList();
			}
			return employeeList;
		}

		public static List<Employee> GetEmpListAfterExceptions(LeaveType lType, List<Employee> empList)
		{
			if (!string.IsNullOrEmpty(lType.ExLocation))
			{
				empList.RemoveAll(l => !lType.ExLocation.Contains(l.WorkLocationId.ToString()));
			}
			if (!string.IsNullOrEmpty(lType.ExDepartment))
			{
				empList.RemoveAll(d => !lType.ExDepartment.Contains(d.DepartmentId.ToString()));
			}
			if (!string.IsNullOrEmpty(lType.ExDesignation))
			{
				empList.RemoveAll(d => !lType.ExDesignation.Contains(d.DesignationId.ToString()));
			}
			return empList;
		}
	}
}
