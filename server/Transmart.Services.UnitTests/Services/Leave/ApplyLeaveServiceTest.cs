using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transmart.Services.UnitTests;
using Transmart.Services.UnitTests.Services.Leave.ApplyLeaveData;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Data.Repository.Leave;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace TranSmart.Services.UnitTests.Services.Leave
{
	public class ApplyLeaveServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly SequenceNoService seqNo;
		private readonly IApplyLeaveService _service;
		private readonly IApplyLeaveService service;
		private readonly LeaveBalanceRepository _repository;
		private readonly IAttendanceService _attService;
		private readonly Mock<IAttendanceService> _mockAttService;
		private readonly EmployeeDataGenerator _empployeeData;
		private readonly Mock<DbContext> _context;
		private readonly IScheduleService _scheduleService;
		private readonly IShiftService _shiftService;
		private readonly ILeaveBalanceService _leaveBalanceService;
		private readonly IAttendanceRepository _attrepository;
		private bool saveCalled;
		public ApplyLeaveServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			seqNo = new SequenceNoService(uow.Object);
			_repository = new LeaveBalanceRepository(_dbContext.Object);
			_leaveBalanceService = new LeaveBalanceService(uow.Object);
			_attrepository = new AttendanceRepository(_dbContext.Object);
			_shiftService = new ShiftService(uow.Object);
			_scheduleService = new ScheduleService(uow.Object, _shiftService);
			_attService = new AttendanceService(uow.Object, _scheduleService, _leaveBalanceService, _attrepository);
			_mockAttService = new Mock<IAttendanceService>();
			_service = new ApplyLeaveService(uow.Object, _mockAttService.Object, _leaveBalanceService);
			service = new ApplyLeaveService(uow.Object, _attService, _leaveBalanceService);
			_empployeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}
		private void Mock_LeavesAlreadyApplied_LeaveBalanceLessThenApplyLeaves(int leaveBalance, Guid leaveTypeId)
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(-1);
			DateTime toDate = DateTime.Now.AddDays(0);
			var employee = _empployeeData.GetEmployeeData();
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 2,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  //LeaveTypeId = leaveTypeId,
				  FromDate = fromDate,
				  ToDate = toDate,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = 1
				},
			}.AsQueryable();

			var applyleaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				ID = Guid.NewGuid(),
				NoOfLeaves = 5,
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = fromDate,
				  ToDate = toDate,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = 1
				},
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=leaveBalance,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveTypeId,
					},
				};
			var applyWfh = new List<ApplyWfh> {
				new ApplyWfh {
					FromDateC = DateTime.Now.AddDays(-1),
					ToDateC = DateTime.Now.AddDays(0),
					Status = (int)WfhStatus.Rejected,
					EmployeeId = employee.ID,
				} };
			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mocksetApplyLeaveType = applyleaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mocksetApplyLeaveType);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Employee Mock
			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			//Wfh Mock
			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion
		}

		private void Mock_CancelAsync(byte status, Guid applyLeaveId)
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = status,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			#endregion

			// Mock
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);
		}

		[Theory]
		[InlineData(false, 3, 1, "9381742192", "9fdd7ce4-62dd-45a5-93ef-594a279e80bb")]//Apply leave fromDate More then the ToDate Test
		[InlineData(true, -1, 0, "9381742192", "9fdd7ce4-62dd-45a5-93ef-594a279e80bb")]//Apply leave fromDate Less then the ToDate Test
		[InlineData(false, -1, 0, "9381742", "9fdd7ce4-62dd-45a5-93ef-594a279e80bb")]//Emergency Contact No Given in wrong format
		[InlineData(false, -1, 0, "9381742192", "b11ad30a-1b20-4c4a-bd11-d8e42b1a0503")]//Mock LeaveType Id And apllyleave LeaveType Id Not matched(leaveType == null)
		public void ApplyLeaves_Test(bool verify, int addDaysFrom, int addDaysTo, string emergencyContactNo, Guid leaveTypeId)
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(addDaysFrom);
			DateTime toDate = DateTime.Now.AddDays(addDaysTo);
			var employee = _empployeeData.GetEmployeeData();
			var lvTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb");

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",

				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					LeaveTypeId = leaveTypeId,
					ApplyLeave = new ApplyLeave
					{
						ID = Guid.NewGuid(),
						EmployeeId = employee.ID,
						FromDate = DateTime.Now.Date,
						ToDate = DateTime.Now.Date,
						Reason = "Corona",
						EmergencyContNo = "9381742192"
					}
				}
			};

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Parse("2022-06-04"),
				ToDate = DateTime.Parse("2022-06-05"),
				Status = (int)ClientVisitStatus.Rejected
			} };


			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						EffectiveFrom = fromDate,
						EffectiveTo = fromDate.AddMonths(1).AddDays(-1)
					},
				};
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = fromDate.AddDays(3),
				ToDateC = fromDate.AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };

			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var MockApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, MockApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Employee Mock
			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);

			#endregion
			//Act
			var dd = _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = emergencyContactNo,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = leaveTypeId,
					NoOfLeaves= 2,
				} }
			});
			//Assert
			Assert.Equal(verify, dd.Result.IsSuccess);
		}

		//From Here


		[Theory]
		[InlineData(false, 4, 1)] //specified Period In LeaveType Is 1(In a week)  then Applying Leave In same week
		[InlineData(false, 4, 2)] //specified Period In LeaveType Is 2(In a month)  then Applying Leave In same month
		[InlineData(false, 4, 3)] //specified Period In LeaveType Is 3(In a Year)  then Applying Leave In same Year
		public void SpecifiedPeriod_Validation_Failed_Case(bool verify, int noOfLeavesApplied, int specifiedPeriod)
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var lvTypeId = Guid.NewGuid();
			var effectiveFrom = DateTime.Now.Date;
			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = specifiedPeriod,
				}
			}.AsQueryable();


			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.AddDays(-1),
				  ToDate = DateTime.Now.AddDays(-1),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				NoOfLeaves = 1,
				LeaveTypeId = lvTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			} };
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(-3),
				ToDate = DateTime.Now.AddDays(-3),
				Status = (int)ClientVisitStatus.Rejected
			} };
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = DateTime.Now.AddDays(-1),
				ToDateC = DateTime.Now.AddDays(-1),
				Status = (int)WfhStatus.Applied,
				EmployeeId = employee.ID
			} };
			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId= lvTypeId,
					   EffectiveFrom = effectiveFrom,
					   EffectiveTo = effectiveFrom.AddMonths(1).AddDays(-1)
					},
				}.AsQueryable();
			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mocksetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mocksetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion

			#region Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{
				EmployeeId = employee.ID,
				NoOfLeaves = noOfLeavesApplied,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays(3),
				FromHalf = false,
				ToHalf = false,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":4}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = noOfLeavesApplied,
					LeaveTypeId = lvTypeId,
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Applied
					}
				} }
			}
			);
			#endregion

			//Assert
			Assert.Equal(verify, dd.Result.IsSuccess);

		}

		[Theory]
		[InlineData(true, 4, 1, "2022-07-06", "2022-07-08")] //specified Period In LeaveType Is 1(In a week)  then Applying Leave In next week
		[InlineData(true, 4, 2, "2022-06-06", "2022-06-08")] //specified Period In LeaveType Is 2(In a month)  then Applying Leave In next month
		[InlineData(true, 4, 3, "2022-06-06", "2022-06-08")] //specified Period In LeaveType Is 3(In a Year)  then Applying Leave In next Year
		public void SpecifiedPeriod_Validation_True_Case(bool verify, int noOfLeavesApplied, int specifiedPeriod, string fromDate, string toDate)
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var effectiveFrom = DateTime.Now.Date;
			var effectiveTo = effectiveFrom.AddMonths(1).AddDays(-1);
			var lvTypeId = Guid.NewGuid();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = specifiedPeriod,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Parse(fromDate),
				  ToDate = DateTime.Parse(toDate),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();

			var applyLeaveTypes = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				LeaveTypeId = lvTypeId,
				NoOfLeaves = 2,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Parse(fromDate),
				  ToDate = DateTime.Parse(toDate),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Parse("2022-06-04"),
				ToDate = DateTime.Parse("2022-06-05"),
				Status = (int)ClientVisitStatus.Rejected
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=lvTypeId,
						EffectiveFrom = effectiveFrom,
						EffectiveTo = effectiveTo,
					},
				}.AsQueryable();

			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = DateTime.Parse(toDate).AddDays(3),
				ToDateC = DateTime.Parse(toDate).AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveTypes.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion

			#region Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{
				EmployeeId = employee.ID,
				NoOfLeaves = noOfLeavesApplied,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays(2),
				FromHalf = false,
				ToHalf = false,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":4}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 4,
					LeaveTypeId = lvTypeId,
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Applied
					}
				} }
			}
			);
			#endregion
			//Assert
			Assert.Equal(verify, dd.Result.HasNoError);

		}
		[Fact]
		public void SelfServiceLeaveValidation_BothHalfsSelected_In_OneDayLeave_Throw_Exception()
		{
			//Arrange
			var employee = _empployeeData.GetEmployeeData();
			var lvTypeId = Guid.NewGuid();
			// Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				NoOfLeaves = 1,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date,
				FromHalf = true,
				ToHalf = true,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 1,
					LeaveTypeId = lvTypeId,
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Applied
					}
				} }
			});

			//Assert
			Assert.True(dd.Result.HasError);
		}

		//LEAVE REQUEST ALLOW TEST
		[Theory]
		[InlineData(false, -3, 0, 1)] //Past days Are given 2,today is 2022-07-14,Iam applying leave on 2022-07-11 ThrowException(Pastday validation crossed)
		[InlineData(true, -2, 0, 1)]  //Past days are given 2 , today is 2022-07-14 , Iam applying leave on 2022-07-12 Without Exception
		[InlineData(false, 5, 6, 1)]  // Future days are given 5 , today is 2022-07-14 , Iam applying leave on 2022-07-19 ThrowException(Futureday validation crossed)
		[InlineData(true, 4, 4, 1)]   // Future days are given 5 , today is 2022-07-14 , Iam applying leave on 2022-07-18 Without Exception
		public void SelfServiceLeaveValidation_PastAndFutureDays_Validations(bool verify, int addDaysFromDate, int addDaysToDate, int specifiedPeriod)//string fromDate, string toDate
		{
			DateTime start = DateTime.Today.AddDays(-4);
			DateTime end = start.AddMonths(1).AddDays(-1);
			DateTime fromDate = DateTime.Now.AddDays(addDaysFromDate);
			DateTime toDate = DateTime.Now.AddDays(addDaysToDate);
			var daysDifference = (fromDate - toDate).Days;
			#region Arrange
			LeaveType leaveType = new LeaveType
			{
				ID = Guid.NewGuid(),
				Code = "EL",
				Name = "Earned Leave",
				EffectiveAfter = 1,
				EffectiveType = 2,
				EffectiveBy = 1,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 4,
				MaritalStatus = 3,
				PastDate = 2,
				FutureDate = 5,
				MaxApplications = 1,
				MinLeaves = 1,
				MaxLeaves = 5,
				specifiedperio = specifiedPeriod,
			};

			Employee employee = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "456212347895",
				DateOfJoining = DateTime.Parse("2020-01-20"),
				Gender = 1,
				Name = "Chisteshwar",
				No = "Avontix1827",
				DesignationId = Guid.NewGuid(),
				DepartmentId = Guid.NewGuid(),
				WorkLocationId = Guid.NewGuid()
			};

			List<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.AddDays(-10),
				  ToDate = DateTime.Now.AddDays(-8),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			};
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType {
				  NoOfLeaves = 3,
				  LeaveTypeId = leaveType.ID,
				  ApplyLeave = new ApplyLeave
				  {
					  ID = Guid.NewGuid(),
					  EmployeeId = employee.ID,
					  FromDate = DateTime.Now.AddDays(-10),
					  ToDate = DateTime.Now.AddDays(-8),
					  Reason = "Corona",
					  EmergencyContNo = "9381742192",
					  Status = (int)ApplyLeaveSts.Approved,
					  FromHalf = true,
					  ToHalf = false
				  }
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(-11),
				ToDate = DateTime.Now.AddDays(-11),
				Status = (int)ClientVisitStatus.Rejected
			} };

			List<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=leaveType.ID,
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveType.ID
					},
				};
			var lvBal = new LeaveBalance
			{
				ID = leaveType.ID,
				Leaves = 5,
				EmployeeId = employee.ID,
				LeaveTypeId = leaveType.ID
			};
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = fromDate.AddDays(3),
				ToDateC = fromDate.AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock
			//LeaveType Mock
			var leaveTypeRepo = new Mock<RepositoryAsync<LeaveType>>(_dbContext.Object);

			leaveTypeRepo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<LeaveType, bool>>>(),
				  It.IsAny<Func<IQueryable<LeaveType>, IOrderedQueryable<LeaveType>>>(),
				  It.IsAny<Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>>>(), true)).ReturnsAsync(leaveType);

			uow.Setup(m => m.GetRepositoryAsync<LeaveType>()).Returns(leaveTypeRepo.Object);

			//Apply Leave Mock
			var applyLeaveRepo = new Mock<RepositoryAsync<ApplyLeave>>(_dbContext.Object);

			applyLeaveRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplyLeave, bool>>>(),
				   It.IsAny<Func<IQueryable<ApplyLeave>, IOrderedQueryable<ApplyLeave>>>(),
				   It.IsAny<Func<IQueryable<ApplyLeave>, IIncludableQueryable<ApplyLeave, object>>>())).ReturnsAsync(applyLeave);

			applyLeaveRepo.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<ApplyLeave, bool>>>())).ReturnsAsync(false);

			uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(applyLeaveRepo.Object);

			//ApplyLeaveType Mock
			var applyLeaveTypeRepo = new Mock<RepositoryAsync<ApplyLeaveType>>(_dbContext.Object);
			applyLeaveTypeRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplyLeaveType, bool>>>(),
				   It.IsAny<Func<IQueryable<ApplyLeaveType>, IOrderedQueryable<ApplyLeaveType>>>(),
				   It.IsAny<Func<IQueryable<ApplyLeaveType>, IIncludableQueryable<ApplyLeaveType, object>>>())).ReturnsAsync(applyLeaveType);

			applyLeaveTypeRepo.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<ApplyLeaveType, bool>>>())).ReturnsAsync(false);

			uow.Setup(m => m.GetRepositoryAsync<ApplyLeaveType>()).Returns(applyLeaveTypeRepo.Object);

			//client visit
			var clientvisitRepo = new Mock<RepositoryAsync<ApplyClientVisits>>(_dbContext.Object);

			clientvisitRepo.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<ApplyClientVisits, bool>>>())).ReturnsAsync(false);

			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(clientvisitRepo.Object);

			//Employee Mock
			var employeeRepo = new Mock<RepositoryAsync<Employee>>(_dbContext.Object);

			employeeRepo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Employee, bool>>>(),
				  It.IsAny<Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>(),
				  It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(), true)).ReturnsAsync(employee);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(employeeRepo.Object);
			//LeaveBalance Mock
			var LeaveBalRepo = new Mock<RepositoryAsync<LeaveBalance>>(_dbContext.Object);

			LeaveBalRepo.Setup(x => x.SumOfDecimalAsync(It.IsAny<Expression<Func<LeaveBalance, bool>>>(),
			   It.IsAny<Expression<Func<LeaveBalance, decimal>>>())).ReturnsAsync(5);

			LeaveBalRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<LeaveBalance, bool>>>(),
				   It.IsAny<Func<IQueryable<LeaveBalance>, IOrderedQueryable<LeaveBalance>>>(),
				   It.IsAny<Func<IQueryable<LeaveBalance>, IIncludableQueryable<LeaveBalance, object>>>())).ReturnsAsync(leaveBalances);

			LeaveBalRepo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<LeaveBalance, bool>>>(),
				  It.IsAny<Func<IQueryable<LeaveBalance>, IOrderedQueryable<LeaveBalance>>>(),
				  It.IsAny<Func<IQueryable<LeaveBalance>, IIncludableQueryable<LeaveBalance, object>>>(), true)).ReturnsAsync(lvBal);

			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(LeaveBalRepo.Object);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion

			#region Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = fromDate,
				ToDate = toDate,
				FromHalf = false,
				ToHalf = false,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":" + daysDifference + "}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = daysDifference,
					LeaveTypeId = leaveType.ID,
				} }
			}
			);
			#endregion
			//Assert
			Assert.Equal(verify, dd.Result.HasNoError);

		}

		[Theory]
		[InlineData(true, true, false, 2)]//LeaveType Duration Taken As HalfDay And Applied Leaves Are 2.5 days,No exception
		[InlineData(false, true, false, 1)]//LeaveType Duration Taken As FullDay And Applied Leaves Are 2.5 days throw Exception(LeaveType not-accept halfday leaves)
		public void SelfServiceLeaveValidation_LeaveTypeDuration_Validations(bool verify, bool fromHalf, bool toHalf, int duration)
		{
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);

			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = duration,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(-4),
				  ToDate = start.AddDays(-2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				NoOfLeaves = 3,
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Parse("2022-06-06"),
				  ToDate = DateTime.Parse("2022-06-08"),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			} };
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = start.AddDays(-6),
				ToDate = start.AddDays(-5),
				Status = (int)ClientVisitStatus.Rejected
			} };
			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
					   ID=Guid.NewGuid(),
					   Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId= leaveTypeId,
					   EffectiveFrom = start,
					   EffectiveTo = end
					},
				}.AsQueryable();

			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(3),
				ToDateC = start.AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock
			//LeaveType Mock
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			//Apply Leave Mock
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mocksetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mocksetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			//Employee Mock
			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);

			#endregion

			#region Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{

				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				//NoOfLeaves = (decimal)2.5,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = start.AddDays(1),
				ToDate = start.AddDays(2),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2.5}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 2.5m,
					LeaveTypeId = leaveTypeId,
				} }
			}
			);
			#endregion
			//Assert
			Assert.Equal(verify, dd.Result.HasNoError);

		}

		[Theory]
		[InlineData(true, 2, 2, 5)]//Employee gender and maritual status Given same as LeaveType gender and maritual status ,No Exception
		[InlineData(false, 1, 2, 5)]//Employee gender missmatch with LeaveType gender and maritual status Given same as LeaveType  maritual status, ThrowException
		[InlineData(false, 2, 1, 5)]//Employee maritual status missmatch with LeaveType  maritual status Throw Exception
		[InlineData(false, 2, 2, -1)]//Employee gender and maritual status Given same as LeaveType gender and maritual status and
									 //LeaveBalance Is -1 throws exception(Leaves_Not_Avaible_To_Apply)
		[InlineData(false, 2, 2, 0)]//Employee gender and maritual status Given same as LeaveType gender and maritual status and
									//LeaveBalance Is null(EmployeeId not matched) throws exception
		public void ApplyLeave_Gender_And_MaritualStatus_LeaveBalance_Validation(bool verify, int gender, int martialStatus, int leaveBalance)
		{
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);
			Guid leaveTypeId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			#region Arrange
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 2,
					MaritalStatus = 2,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			var employee = new List<Employee>
			{
				new Employee
			{
				ID = empId,
				AadhaarNumber = "456212347895",
				DateOfJoining = DateTime.Parse("2020-01-20"),
				Gender = gender,
				Name = "Chisteshwar",
				No = "Avontix1827",
				DesignationId = Guid.NewGuid(),
				DepartmentId = Guid.NewGuid(),
				WorkLocationId = Guid.NewGuid(),
				MaritalStatus = martialStatus
			}
		}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = empId,
				  FromDate = start.AddDays(-4),
				  ToDate = start.AddDays(-5),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false,
				  ApplyLeaveType=new List<ApplyLeaveType>{new ApplyLeaveType
				  {
					  ID=Guid.NewGuid(),
					  LeaveTypeId = leaveTypeId
				  } }
				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = empId,
				  FromDate = start.AddDays(-4),
				  ToDate = start.AddDays(-5),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= empId,
				FromDate = start.AddDays(-3),
				  ToDate = start.AddDays(-2),
				Status = (int)ClientVisitStatus.Rejected
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=leaveBalance,
						EmployeeId=leaveBalance == 0?Guid.NewGuid():empId,
						LeaveTypeId=leaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end
					},
				};

			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(3),
				ToDateC = start.AddDays(4),
				EmployeeId = empId,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion


			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);
			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion

			#region Act
			var dd = _service.SelfServiceLeaveValidation(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = empId,
				NoOfLeaves = 2.5m,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2.5}]",
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = start,
				ToDate = start.AddDays(1),
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType {
					NoOfLeaves = 2.5m,
					LeaveTypeId=leaveTypeId,
				} }
			}
			);
			#endregion
			//Assert
			Assert.Equal(verify, dd.Result.HasNoError);
		}

		#region Approvals
		[Fact]
		public async Task ApprovalPaginate_FirstLevel_Approve()
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var fromDate = DateTime.Now.Date;
			var toDate = fromDate.AddMonths(1).AddDays(-1);
			ApplyLeaveSearch applyLeaveSearch = new ApplyLeaveSearch
			{
				FromDate = fromDate,
				ToDate = toDate,
				Status = 1,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ReportingToId
			};

			IEnumerable<ApplyLeave> data = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				   ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = fromDate.AddDays(2),
				  ToDate = fromDate.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = fromDate.AddDays(6),
				  ToDate = fromDate.AddDays(8),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				}

			}.AsQueryable();
			#endregion

			// Mock
			var mockSetApplyLeave = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			// Act
			var list = await _service.ApprovalPaginate(applyLeaveSearch);
			//Assert
			Assert.True(list.Count == 1);
		}

		[Fact]
		public async Task ApprovalPaginate_SecondLevel_Approve()
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var fromDate = DateTime.Now.Date;
			var toDate = fromDate.AddMonths(1).AddDays(-1);
			ApplyLeaveSearch applyLeaveSearch = new ApplyLeaveSearch
			{
				FromDate = fromDate,
				ToDate = toDate,
				Status = 1,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ReportingTo.ReportingToId,
			};

			IEnumerable<ApplyLeave> data = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				   ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = fromDate.AddDays(2),
				  ToDate = fromDate.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = fromDate.AddDays(6),
					ToDate = fromDate.AddDays(8),
					Reason = "Corona",
					EmergencyContNo = "9381742192",
					Status = (int)ApplyLeaveSts.Approved,
					FromHalf = true,
					ToHalf = false
				}

			}.AsQueryable();
			#endregion

			// Mock
			var mockSetApplyLeave = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			// Act
			var list = await _service.ApprovalPaginate(applyLeaveSearch);
			//Assert
			Assert.True(list.Count == 1);
		}

		[Fact]
		public async Task GetLeave_By_LeaveId_And_ApproverId()
		{
			var employee = _empployeeData.GetEmployeeData();
			Guid approvalId = (Guid)employee.ReportingToId;
			Guid applyLeaveId = Guid.NewGuid();
			#region Arrange
			IEnumerable<ApplyLeave> data = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				   ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.AddDays(1),
				  ToDate = DateTime.Now.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.AddDays(1),
				  ToDate = DateTime.Now.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},

			}.AsQueryable();
			#endregion

			// Mock
			var mockSetApplyLeave = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			// Act
			var src = new ApplyLeaveService(uow.Object, _attService, _leaveBalanceService);
			var list = await src.GetLeave(applyLeaveId, approvalId);

			//Assert
			Assert.Equal(applyLeaveId, list.ID);
		}
		#endregion

		#region LeaveManagement,Approval - Reject,Approve
		[Theory]
		[InlineData(false, 1, 5, "80ccbc50-ebf9-4654-9160-c36201d1783c")]//Approve Not By AdminRequest and status is Applied,
																		 //ExceptionThrows(You dont have permission)
		[InlineData(false, 1, 5, "2b604b40-96e3-4718-b9de-7731147c2c4a")]//Approve Not By AdminRequest and status is Applied,
																		 //Leaves More then the AvailableLeaves throw exception(Leaves Not Available to Apply)
		[InlineData(true, 3, 5, "80ccbc50-ebf9-4654-9160-c36201d1783c")]//Approve Not By AdminRequest and status is other then Applied,
																		//Exception Throws(Leaves already approved or Leaves already rejected)
		[InlineData(true, 1, 0, "80ccbc50-ebf9-4654-9160-c36201d1783c")]//Approve By AdminRequest and status is Applied , but available LeaveBalance is 0
																		//Exception Throws(Leaves not available to apply)
		public async Task Approve(bool isAdminRequest, byte status, int availableLeaves, Guid employeeId)
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employeeId,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = status,
				  FromHalf = true,
				  ToHalf = false,
				  NoOfLeaves = 3,
				  ApplyLeaveType = new List<ApplyLeaveType>
				  {
					  new ApplyLeaveType
					  {
						  NoOfLeaves= 4,
						  LeaveTypeId = leaveTypeId,
						  ApplyLeaveId = applyLeaveId,
						  LeaveType=new LeaveType
						  {
							  Name = "Cl"
						  }
					  }
				  }
				},
			}.AsQueryable();
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				NoOfLeaves= 4,
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employeeId,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = status,
				  FromHalf = true,
				  ToHalf = false,
				  NoOfLeaves = 3
				},
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=availableLeaves,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveTypeId
					},
				}.AsQueryable();
			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.Date
				},
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.Date.AddDays(1)
				},
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.Date.AddDays(2)
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			#endregion
			// Act
			var dd = await _service.Approve(applyLeave.FirstOrDefault().ID, employee.ID, isAdminRequest);
			// Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		//If User has a Cl Bal:4 and El Bal:4 He need to apply Leave For 10days then he can combine both the leave types and he Can apply leave for 10 days with 2days Lop 
		public async Task Approve_WithLOP()
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var casualLeaveTypeId = Guid.NewGuid();
			var earnedLeaveTypeId = Guid.NewGuid();
			var lopLvTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			DateTime startDate = DateTime.Today.AddDays(25);
			DateTime endDate = startDate.AddMonths(1).AddDays(-1);
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = casualLeaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
					DefaultPayoff = false,
				},
				new LeaveType
				{
					ID = earnedLeaveTypeId,
					Code = "EL",
					Duration = 1,
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
					DefaultPayoff = false,
				},
				new LeaveType
				{
					ID = lopLvTypeId,
					Code = "LOP",
					Duration = 1,
					Name = "Loss Of Pay",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
					DefaultPayoff = true
				}
			}.AsQueryable();
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = startDate.AddDays(2),
				  ToDate = startDate.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = 1,
				  FromHalf = true,
				  ToHalf = false,
				  NoOfLeaves = 10,
				  ApplyLeaveType = new List<ApplyLeaveType>
				  {
					  new ApplyLeaveType
					  {
						  NoOfLeaves= 4,
						  LeaveTypeId = casualLeaveTypeId,
						  ApplyLeaveId = applyLeaveId,
						  LeaveType=new LeaveType{DefaultPayoff=false},
					  },
					  new ApplyLeaveType
					  {
						  NoOfLeaves= 4,
						  LeaveTypeId = earnedLeaveTypeId,
						  ApplyLeaveId = applyLeaveId,
						  LeaveType=new LeaveType{DefaultPayoff=false},
					  },
					   new ApplyLeaveType
					  {
						  NoOfLeaves= 2,
						  LeaveTypeId = lopLvTypeId,
						  ApplyLeaveId = applyLeaveId,
						  LeaveType=new LeaveType{DefaultPayoff=true},
					  },
				  }
				},
			}.AsQueryable();

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=4,
						EmployeeId=employee.ID,
						LeaveTypeId=casualLeaveTypeId,
						EffectiveFrom = startDate,
						EffectiveTo = endDate

					},
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=4,
						EmployeeId=employee.ID,
						LeaveTypeId=earnedLeaveTypeId,
						EffectiveFrom = startDate,
						EffectiveTo = endDate
					},
				}.AsQueryable();

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.Parse("2b604b40-96e3-4718-b9de-7731147c2c4a"),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = DateTime.Parse("2022-06-06"),
						ToDate = DateTime.Parse("2022-06-08"),

						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = true,
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			#endregion

			#region Mock
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			#endregion
			// Act
			var dd = await _service.Approve(applyLeave.FirstOrDefault().ID, employee.ID, true);
			var result = dd.ReturnValue.ApplyLeaveType.ToList();
			// Assert
			Assert.Equal(casualLeaveTypeId, result[0].LeaveTypeId);
			Assert.Equal(4, result[0].NoOfLeaves);

			Assert.Equal(earnedLeaveTypeId, result[1].LeaveTypeId);
			Assert.Equal(4, result[1].NoOfLeaves);

			Assert.Equal(lopLvTypeId, result[2].LeaveTypeId);
			Assert.Equal(2, result[2].NoOfLeaves);
			Assert.True(dd.HasNoError);
		}

		[Fact]
		public async Task Approve_IsAdminRequest_LeaveStatusIsApproved()
		{
			#region Arrange
			bool isAdminRequest = true;
			int availableLeaves = 5;
			var leaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var employee = _empployeeData.GetEmployeeData();
			Guid employeeId = employee.ID;
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employeeId,
				  FromDate = fromDate,
				  ToDate = fromDate.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = false,
				  ToHalf = false,
				  NoOfLeaves = 3,
				  ApplyLeaveType = new List<ApplyLeaveType>{new ApplyLeaveType
				  {
					  NoOfLeaves= 2,
					  LeaveTypeId = leaveTypeId,
					  LeaveType = new LeaveType
					  {
						  Name = "CL"
					  }
				  } }
				},
			}.AsQueryable();

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=availableLeaves,
						EmployeeId=employee.ID,
						LeaveTypeId= leaveTypeId,
						EffectiveFrom = fromDate,
						EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
					},
				};
			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = DateTime.Parse("2022-06-06"),
						ToDate = DateTime.Parse("2022-06-08"),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = false,
						ToHalf = false
					},
					IsFirstHalf = true,
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			#endregion
			// Act
			var dd = await _service.Approve(applyLeave.FirstOrDefault().ID, employee.ID, isAdminRequest);
			// Assert
			Assert.Equal((int)ApplyLeaveSts.Approved, applyLeave.FirstOrDefault().Status);
			Assert.True(dd.HasNoError);
		}

		[Fact]//rejecting the applied leave
		public void Reject()
		{
			#region Arrange
			var employee = _empployeeData.GetAllEmployeesData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.FirstOrDefault().ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();

			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.FirstOrDefault().ID,
					   LeaveTypeId=leaveTypeId,
					   ApplyLeaveId = applyLeaveId
					},
				}.AsQueryable();

			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);
			#endregion

			// Act
			var dd = _service.Reject(applyLeave.FirstOrDefault().ID, "Due to Auditing", employee.FirstOrDefault(x => x.Name == "Shiva").ID);
			//Assert
			Assert.Equal((int)ApplyLeaveSts.Rejected, dd.Result.ReturnValue.Status);
		}

		[Fact]
		public async Task Reject_Throw_Exception()
		{
			#region Arrange
			var employee = _empployeeData.GetAllEmployeesData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.FirstOrDefault().ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();

			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.FirstOrDefault().ID,
					   LeaveTypeId=leaveTypeId,
					   ApplyLeaveId = applyLeaveId
					},
				}.AsQueryable();

			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);
			#endregion

			// Act
			var dd = await _service.Reject(applyLeave.FirstOrDefault().ID, "Due to Auditing", employee.FirstOrDefault(x => x.Name == "Shiva").ID);
			//Assert
			Assert.True(dd.HasError);
		}
		[Fact]
		public void RejectLeaveAfterApprove_Attendance_Already_Executed_Throw_Exception()
		{
			//Arrange
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var rejectEmpId = Guid.NewGuid();
			var employee = _empployeeData.GetEmployeeData();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
			} };
			var applyLeave = new List<ApplyLeave> {
				new ApplyLeave
				{
					ID = applyLeaveId,
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(4),
					Status = (int)ApplyLeaveSts.Approved,
					FromHalf = true,
					ToHalf = false
				}
			};
			var applyLeveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				LeaveTypeId= leaveTypeId,
				NoOfLeaves= 1,
				ApplyLeave=new ApplyLeave
				{
					ID = applyLeaveId,
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(4),
					Status = (int)ApplyLeaveSts.Approved,
					FromHalf = true,
					ToHalf = false
				}
			} };
			var attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now.AddDays(2),
					AttendanceStatus = (int)AttendanceStatus.Present,
				},
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now.AddDays(3),
					AttendanceStatus = (int)AttendanceStatus.Present,
				}
			};
			//Mock
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mocksetApplyLeaveType = applyLeveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mocksetApplyLeaveType);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockAttendance = attendance.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Attendance>()).Returns(mockAttendance.Object);
			var repository = new RepositoryAsync<Attendance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
			//Act
			var dd = _service.RejectAfterApprove(applyLeave.FirstOrDefault().ID, "Rejected after approve", rejectEmpId);
			//Assert
			Assert.True(dd.Result.HasError);
		}

		[Fact]
		public async Task RejectLeaveAfterApprove_Status_Verify_And_Leaves_Reverted()
		{
			//Arrange
			DateTime today = DateTime.Today;
			DateTime start = new DateTime(today.Year, today.Month, 26);
			DateTime end = start.AddMonths(1).AddDays(-1);
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var rejectEmpId = Guid.NewGuid();
			var employee = _empployeeData.GetEmployeeData();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
			} };
			var applyLeave = new List<ApplyLeave> {
				new ApplyLeave
				{
					ID = applyLeaveId,
					EmployeeId = employee.ID,
					FromDate = start.AddDays(2),
					ToDate = start.AddDays(3),
					Status = (int)ApplyLeaveSts.Approved,
					FromHalf = false,
					ToHalf = false,
					NoOfLeaves = 2
				}
			};
			var applyLeaveType = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves= 2,
					AddedAt = start,
					ApplyLeave = new ApplyLeave
				{
					ID = applyLeaveId,
					EmployeeId = employee.ID,
					FromDate = start.AddDays(2),
					ToDate = start.AddDays(3),
					Status = (int)ApplyLeaveSts.Approved,
					FromHalf = false,
					ToHalf = false
				}
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = start.AddDays(1),
					AttendanceStatus = (int)AttendanceStatus.Present,
				}
			};
			var leaveBalances = new List<LeaveBalance>
				{
				new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						ApplyLeaveId=null,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=-2,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						ApplyLeaveId=applyLeaveId,
						EffectiveFrom = start,
						EffectiveTo = end,
						LeavesAddedOn = start,
					},
				};
			//Mock
			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockAttendance = attendance.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Attendance>()).Returns(mockAttendance.Object);

			var mockLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockLeaveBalance);

			var repository = new RepositoryAsync<Attendance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
			//Act
			var dd = await _service.RejectAfterApprove(applyLeave.FirstOrDefault().ID, "Rejected after approve", rejectEmpId);
			var list = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();
			//Assert
			Assert.True(dd.ReturnValue.Status == (int)ApplyLeaveSts.Rejected);
			Assert.Equal(5, list.Sum(x => x.Leaves));
			Assert.Equal(0, list.LastOrDefault().Leaves);
		}

		[Fact]//Apply Leaves Are Less Then the LeaveBalance Available Leaves
		public async Task AddApprovedLeaveAsync_PostLeaveFromLeaveManagement()
		{
			#region Arrange
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(1);
			DateTime fromDate = start;
			DateTime toDate = start.AddDays(5);
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeID = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeID,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(-4),
				  ToDate = start.AddDays(-2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				  ApplyLeaveType =new List<ApplyLeaveType>{new ApplyLeaveType
				  {
					  LeaveTypeId = leaveTypeID,
					  NoOfLeaves = 2,
				  } }
				},
			}.AsQueryable();
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				LeaveTypeId = leaveTypeID,
				NoOfLeaves = 2,
				ApplyLeave = new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(-4),
				  ToDate = start.AddDays(-2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = start.AddDays(-6),
				ToDate = start.AddDays(-4),
				Status = (int)ClientVisitStatus.Applied
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId= leaveTypeID,
						EffectiveFrom = start,
						EffectiveTo= end,
					},
			};
			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = start.AddDays(-4),
						ToDate = start.AddDays(-2),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = true,
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(6),
				ToDateC = start.AddDays(7),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock

			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion
			// Act
			var dd = await _service.AddApprovedLeaveAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType {
					//ApplyLeaveId = Guid.Parse("0aaac7a7-a7cf-4e2e-9b36-3b9d7c2f3851"),
					LeaveTypeId = leaveTypeID,
					NoOfLeaves = 1
				} }
			});
			// Assert
			Assert.Equal((int)ApplyLeaveSts.Approved, dd.ReturnValue.Status);
		}

		[Fact]
		public async Task AddApprovedLeaveAsync_PostLeave_FromLeaveManagement_ThrowException()
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(-1);
			DateTime toDate = DateTime.Now.AddDays(0);
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();

			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = fromDate.AddDays(-5),
				  ToDate = fromDate.AddDays(-4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				LeaveTypeId = leaveTypeId,
				NoOfLeaves = 2,
				ApplyLeave = new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = fromDate.AddDays(-5),
				  ToDate = fromDate.AddDays(-4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			} };
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = fromDate.AddDays(-3),
				ToDate = fromDate.AddDays(-2),
				Status = (int)ClientVisitStatus.Rejected
			} };
			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						EffectiveFrom = fromDate.AddDays(-1),
						EffectiveTo = toDate.AddDays(1),
					},
				}.AsQueryable();
			IEnumerable<ApplyLeaveDetails> applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.Parse("2b604b40-96e3-4718-b9de-7731147c2c4a"),
					ApplyLeaveId= Guid.Parse("0aaac7a7-a7cf-4e2e-9b36-3b9d7c2f3851"),
					ApplyLeave = new ApplyLeave
					{
						ID = Guid.Parse("0aaac7a7-a7cf-4e2e-9b36-3b9d7c2f3851"),
						EmployeeId = employee.ID,
						FromDate = DateTime.Parse("2022-06-06"),
						ToDate = DateTime.Parse("2022-06-08"),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = true,
				}
			};
			IEnumerable<Attendance> attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = fromDate.AddDays(3),
				ToDateC = fromDate.AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);

			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Exception occurred while executing method"));
			#endregion
			// Act
			var dd = await _service.AddApprovedLeaveAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves = 1
				} }
			});
			// Assert
			Assert.False(saveCalled);
		}
		#endregion

		[Fact]
		public async Task CancelAsync_NoException()
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			#endregion

			// Mock
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			// Act
			var dd = await _service.CancelAsync(applyLeaveId, employee.ID);
			// Assert
			Assert.Equal((int)ApplyLeaveSts.Cancelled, dd.ReturnValue.Status);
			Assert.Equal((int)ApplyLeaveSts.Cancelled, applyLeave.FirstOrDefault().Status);
			Assert.True(dd.IsSuccess);
		}

		[Fact]
		public async Task CancelAsync_ApplyLeaveAlreadyApproved_ThrowException()
		{
			var employee = _empployeeData.GetEmployeeData();
			Guid applyLeaveId = Guid.NewGuid();
			Mock_CancelAsync((int)ApplyLeaveSts.Approved, applyLeaveId);
			// Act
			var dd = await _service.CancelAsync(applyLeaveId, employee.ID);
			// Assert
			Assert.True(dd.HasError);

		}
		[Fact]
		public async Task CancelAsync_ApplyLeaveAlreadyRejected_ThrowException()
		{
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			Mock_CancelAsync((int)ApplyLeaveSts.Rejected, applyLeaveId);
			// Act
			var dd = await _service.CancelAsync(applyLeaveId, employee.ID);
			// Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task CancelAsync_LeaveAlreadyCancelled_ThrowException()
		{
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();

			Mock_CancelAsync((int)ApplyLeaveSts.Cancelled, applyLeaveId);
			// Act
			var dd = await _service.CancelAsync(applyLeaveId, employee.ID);
			// Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task CancelAsync_InValidApplyLeave_ThrowException()
		{
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();

			Mock_CancelAsync((int)ApplyLeaveSts.Cancelled, applyLeaveId);

			// Act
			var dd = await _service.CancelAsync(Guid.NewGuid(), employee.ID);
			// Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateLeave_WithoutException()
		{
			byte status = (int)ApplyLeaveSts.Applied;
			#region Arrange
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var clLeaveTypeId = Guid.NewGuid();
			var employee = _empployeeData.GetEmployeeData();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 20,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				},
				new LeaveType
				{
					ID = clLeaveTypeId,
					Code = "CL",
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					ProrateByT = 1,
					EffectiveBy = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 20,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(1),
				  ToDate = start.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status= status,
				  LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				  ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves = 2
				} }
				},
			}.AsQueryable();

			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(3),
				ToDateC = start.AddDays(4),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };

			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				NoOfLeaves = 2,
				ApplyLeave = new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(1),
				  ToDate = start.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = start.AddDays(5),
				ToDate = start.AddDays(6),
				Status = (int)ClientVisitStatus.Rejected
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=2,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						Leaves = 1,
						EmployeeId=employee.ID,
						LeaveTypeId = clLeaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end
					}
				};
			#endregion

			#region Mock

			var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeavetype);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);

			_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			#endregion

			//Act
			var dd = await _service.UpdateAsync(new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = employee.ID,
				FromDate = start.AddDays(8),
				ToDate = start.AddDays(9),
				Reason = "Fever",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1},{\"name\":\"Earned Leave\",\"days\":1}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = clLeaveTypeId,
					NoOfLeaves = 1
				},
				new ApplyLeaveType{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves = 1
				} }
			});
			// Assert
			var list = await uow.Object.GetRepositoryAsync<ApplyLeaveType>().GetAsync();
			Assert.Equal(clLeaveTypeId, list.LastOrDefault().LeaveTypeId);
			Assert.Equal(1, list.LastOrDefault().NoOfLeaves);
			Assert.Equal(leaveTypeId, list.FirstOrDefault().LeaveTypeId);
			Assert.Equal(1, list.FirstOrDefault().NoOfLeaves);
			Assert.True(dd.IsSuccess);
		}
		[Theory]
		[InlineData((int)ApplyLeaveSts.Approved)]
		[InlineData((int)ApplyLeaveSts.Rejected)]
		[InlineData((int)ApplyLeaveSts.Cancelled)]
		public async Task UpdateAsync_LeaveHasBeenAlreadyApprovedOrRejectedOrCancelled_ThrowException(byte leaveStatus)
		{
			var leaveId = Guid.NewGuid();
			var lvTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			//Arrange
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = leaveId,
				  EmployeeId =Guid.NewGuid(),
				  FromDate = fromDate.AddDays(-2),
				  ToDate = fromDate.AddDays(-1),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status= leaveStatus,
				},
			}.AsQueryable();
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				ApplyLeaveId = leaveId,
				NoOfLeaves= 2,
				LeaveTypeId = lvTypeId
			} };
			//Mock
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockApplyLeaveType);

			var result = await _service.UpdateAsync(new ApplyLeave
			{
				ID = leaveId,
				EmployeeId = Guid.NewGuid(),
				FromDate = DateTime.Today,
				ToDate = DateTime.Today.AddDays(1),
				Reason = "Fever",
				EmergencyContNo = "9381742192",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType {
					ApplyLeaveId = leaveId,
					LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
					NoOfLeaves = 2
				} }
			});

			Assert.True(result.HasError);
		}
		[Fact]
		public async Task UpdateAsync_InvalidApplyLeave_ThrowException()
		{
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave> { new ApplyLeave { ID = Guid.NewGuid(), Status = (int)ApplyLeaveSts.Applied } };
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var result = await _service.UpdateAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = Guid.NewGuid(),
				FromDate = DateTime.Today,
				ToDate = DateTime.Today.AddDays(1),
				Reason = "Fever",
				EmergencyContNo = "9381742192",
			});

			Assert.True(result.HasError);
		}

		[Fact]
		public async Task GetLeaves()
		{
			DateTime date = DateTime.Now.Date;
			var employee = _empployeeData.GetEmployeeData();
			var departmentId = employee.DepartmentId;
			var reportingToId = (Guid)employee.ReportingToId;
			var applyLeaveId = Guid.NewGuid();
			#region Arrange

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.AddDays(1),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.AddDays(2),
				  ToDate = DateTime.Now.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			#endregion
			// Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);
			//Act
			var dd = await _service.GetLeaves(date.Month, departmentId, reportingToId);
			// Assert
			Assert.True(dd.Count() == 2);
		}

		[Fact]
		public async Task Get_ApplyLeaveById_Test()
		{
			#region Arrange
			var applyLeaveId = Guid.NewGuid();
			var employee = _empployeeData.GetEmployeeData();
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = 1,
				  FromHalf = true,
				  ToHalf = false
				},
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			#endregion
			// Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//Act
			var dd = await _service.GetById(applyLeaveId);
			// Assert
			Assert.Equal(applyLeaveId, dd.ID);
			Assert.Equal((int)ApplyLeaveSts.Approved, dd.Status);
		}
		[Fact]
		public async Task GetLeavesBetween_FromDate_And_ToDate()
		{
			//Arrange
			DateTime fromDate = DateTime.Today;
			DateTime toDate = DateTime.Now.AddDays(8);
			var employee = _empployeeData.GetEmployeeData();
			var departmentId = employee.DepartmentId;
			var reportingToId = (Guid)employee.ReportingToId;
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Cancelled,
				  FromHalf = true,
				  ToHalf = false
				},
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.Date.AddDays(3),
				  ToDate = DateTime.Now.Date.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				},
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = DateTime.Now.Date.AddDays(5),
				  ToDate = DateTime.Now.Date.AddDays(6),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();
			// Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);
			//Act
			var dd = await _service.GetLeavesBetweenTwoDates(fromDate, toDate, departmentId, reportingToId);
			// Assert
			Assert.True(dd.Count() == 2);
		}

		[Fact]
		public async Task Selfservice_Validation_LeaveBalanceZero_ThrowException()
		{
			//Act
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			Mock_LeavesAlreadyApplied_LeaveBalanceLessThenApplyLeaves(0, leaveTypeId);
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(-1),
				ToDate = DateTime.Now.AddDays(0),
				Status = (int)ClientVisitStatus.Rejected
			} };
			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);
			var dd = await _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(2),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType {
					NoOfLeaves= 2,
					LeaveTypeId = leaveTypeId,
				} }

			});
			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task SelfService_Validation_ApplyLeavesMoreThenLeaveBalance_ThrowException()//applyLeaves = 5 , leaveBalance = 4
		{
			#region Arrange
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();

			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Duration = 1,
					Name = "Casual Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 5,
					specifiedperio = 2,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(2),
				  ToDate = start.AddDays(3),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				NoOfLeaves= 2,
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(2),
				  ToDate = start.AddDays(3),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false
				},
			} };
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = start.AddDays(0),
				ToDate = start.AddDays(1),
				Status = (int)ClientVisitStatus.Rejected
			} };
			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=4,
						EmployeeId=employee.ID,
						LeaveTypeId= leaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end
					},
				}.AsQueryable();
			IEnumerable<ApplyLeaveDetails> applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = start.AddDays(2),
						ToDate = start.AddDays(3),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = true,
				}
			};
			IEnumerable<Attendance> attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = applyLeave.FirstOrDefault().FromDate,
				  AttendanceStatus = 1
				},
			}.AsQueryable();
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(0),
				ToDateC = start.AddDays(1),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock

			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);
			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion
			// Act
			var dd = await _service.AddApprovedLeaveAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = start.AddDays(4),
				ToDate = start.AddDays(8),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":5}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 5,
					LeaveTypeId = leaveTypeId
				} }

			});
			// Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task SelfService_Validation_LeavesAlreadyApplied_ThrowException()
		{
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			Mock_LeavesAlreadyApplied_LeaveBalanceLessThenApplyLeaves(5, leaveTypeId);
			//Act
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(2),
				Status = (int)ClientVisitStatus.Rejected
			} };
			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);
			var dd = await _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(-1),
				ToDate = DateTime.Now.AddDays(0),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":3}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 3,
					LeaveTypeId=leaveTypeId,

				} }
			});
			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task GetPaginate_Test()
		{
			// Arrange
			var employee = _empployeeData.GetEmployeeData();
			var fromDate = DateTime.Now.Date;
			ApplyLeaveSearch applyLeaveSearch = new ApplyLeaveSearch
			{
				FromDate = fromDate,
				ToDate = fromDate.AddMonths(1).AddDays(-1),
				Status = 1,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ReportingToId
			};

			IEnumerable<ApplyLeave> data = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				   ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = fromDate.AddDays(2),
				  ToDate = fromDate.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  FromHalf = true,
				  ToHalf = false,
				},
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  Employee = employee,
				  FromDate = fromDate.AddDays(6),
				  ToDate = fromDate.AddDays(8),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = false
				}

			}.AsQueryable();

			// Mock
			var mockSetApplyLeave = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			// Act
			var list = await _service.GetPaginate(applyLeaveSearch);
			//Assert
			Assert.True(list.Count == 2);
		}
		[Fact]
		public async Task GetLeaveBalanceByLeaveType_GetSumOfLeaves()
		{
			// Arrange
			var employee = _empployeeData.GetEmployeeData();
			var fromDate = DateTime.Now.Date;
			var toDate = fromDate.AddMonths(1).AddDays(-1);
			var leaveTypeId = Guid.NewGuid();
			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=Guid.NewGuid(),
					   EffectiveFrom = toDate,
					   EffectiveTo = toDate.AddMonths(1).AddDays(-1),
					},
					 new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveTypeId,
					   EffectiveFrom = fromDate,
					   EffectiveTo = toDate,
					},
					  new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=3,
						LeaveTypeId=leaveTypeId,
					   EffectiveFrom = fromDate,
					   EffectiveTo = toDate,
					}
				};
			// Mock
			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			uow.Setup(x => x.SaveChanges()).Callback(() => saveCalled = true).Returns(1);
			//Act
			var src = new ApplyLeaveService(uow.Object, _attService, _leaveBalanceService);
			var dd = await src.GetLeaveBalanceByLeaveType(leaveTypeId, employee.ID, fromDate, toDate);
			//Assert
			Assert.Equal(8, dd);
		}

		[Theory]
		[InlineData(1, true, true)]// FromHalf And ToHalf Are True
		[InlineData(1.5, true, false)]// FromHalf Is true , ToHalf is False
		[InlineData(1.5, false, true)]// FromHalf Is False , ToHalf is true
		public void UploadLeaveCount_NoOfLeaves_Test(decimal noOfLeaves, bool fromHalf, bool toHalf)
		{
			var fromDate = DateTime.Now.AddDays(-1);
			var toDate = DateTime.Now.AddDays(0);
			var employee = _empployeeData.GetEmployeeData();
			var leave = new ApplyLeave
			{

				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = (fromDate),
				ToDate = (toDate),
				FromHalf = fromHalf,
				ToHalf = toHalf
			};
			_service.UploadLeaveCount(leave);
			//Assert
			Assert.Equal(noOfLeaves, leave.NoOfLeaves);
		}

		[Fact]
		public void SpecifiedPeriodValidation_LeavesExceedLeaveApplications_ThrowException()
		{
			var LeaveAppliCount = 2;
			Result<ApplyLeave> result = new();
			var leaveType = new LeaveType
			{
				MaxApplications = 1
			};
			_service.SpecifidePeriodValidation(LeaveAppliCount, leaveType, result);
			Assert.True(result.HasError);
		}
		[Fact]
		public void SpecifiedPeriodValidation_LeavesNotExceedLeaveApplications_NoException()
		{
			var LeaveApplicationCount = 1;
			Result<ApplyLeave> result = new();
			var leaveType = new LeaveType
			{
				MaxApplications = 2
			};
			_service.SpecifidePeriodValidation(LeaveApplicationCount, leaveType, result);
			Assert.True(result.HasNoError);
		}

		[Theory]
		[InlineData("2022-07-05", "2022-07-05", (int)AttendanceStatus.Present, true)] // IsFirstHalf - True
		[InlineData("2022-07-05", "2022-07-06", (int)AttendanceStatus.Present, false)] // IsFirstHalf - False,
		[InlineData("2022-07-05", "2022-07-05", (int)AttendanceStatus.Absent, true)]
		[InlineData("2022-07-05", "2022-07-05", (int)AttendanceStatus.WFH, true)]
		[InlineData("2022-07-05", "2022-07-05", (int)AttendanceStatus.WeekOff, true)]
		public async Task UpdateLBIfLeaveApproved_IsFirstHalf_Verification(DateTime fromDate, DateTime toDate, int status, bool verify)
		{
			// Arrange
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = DateTime.Parse("2022-06-06"),
						ToDate = DateTime.Parse("2022-06-08"),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = false,
					IsHalfDay = false
				}
			};

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveTypeId,
					},
				};

			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = attendanceId,
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Parse("2022-07-05"),
				  AttendanceStatus = status
				},
			};

			var modifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
					EmployeeID = employee.ID,
					IsFirstOff = true,
					IsHalfDay = true,
					AttendanceID = attendanceId,
				}
			};

			var applyLeave = new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Employee = employee,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				FromHalf = true,
				ToHalf = false,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 1.5m
					}
				}
			};

			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				Name = "Casual leave",
				DefaultPayoff = false,
				PayType = 1,
				Status = true
			} };

			// Mock 
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveDetails);

			_ = _context.GetRepositoryAsyncDbSet(uow, leaveBalances);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);

			var mockLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockLeaveType);

			_ = _context.GetRepositoryAsyncDbSet(uow, modifyLogs);

			// Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var list = await uow.Object.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync();


			//Assert
			Assert.Equal(verify, list.LastOrDefault().IsFirstHalf);

		}

		[Fact]
		public async Task UpdateLBIfLeaveApproved_Leaves_Verification()
		{
			// Arrange
			var employee = _empployeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			DateTime today = DateTime.Today;
			DateTime start = new DateTime(today.Year, today.Month, 26);
			DateTime end = start.AddMonths(1).AddDays(-1);

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					IsFirstHalf = false,
					IsHalfDay = false
				}
			};

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=leaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
				};

			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = attendanceId,
				  EmployeeId = employee.ID,
				  AttendanceDate = new DateTime(today.Year, today.Month, 15),
				  AttendanceStatus = (int)AttendanceStatus.Present
				},
			};

			var modifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
					EmployeeID = employee.ID,
					IsFirstOff = true,
					IsHalfDay = true,
					AttendanceID = attendanceId,
				}
			};

			var applyLeave = new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Employee = employee,
				FromDate = new DateTime(end.Year, end.Month, 15),
				ToDate = new DateTime(end.Year, end.Month, 16),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				FromHalf = true,
				ToHalf = false,
				NoOfLeaves = 1.5m,
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 1.5m,
					LeaveTypeId = leaveTypeId,
				} }
			};
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				Name = "Casual leave",
				DefaultPayoff = false,
				PayType = 1,
				Status = true
			} };

			// Mock 
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveDetails);

			_ = _context.GetRepositoryAsyncDbSet(uow, leaveBalances);

			_ = _context.GetRepositoryAsyncDbSet(uow, attendance);

			_ = _context.GetRepositoryAsyncDbSet(uow, modifyLogs);

			var mockLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockLeaveType);
			// Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var list = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();

			//Assert

			Assert.Equal(-1.5m, list.LastOrDefault().Leaves);
		}

		[Fact]
		public async Task UpdateLBIfLeaveApproved_FullDay_Verification()
		{
			//Arrange
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var clLvTypeId = Guid.NewGuid();
			var eLLvTypeId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date.AddDays(-3);
			var toDate = fromDate.AddMonths(1).AddDays(-1);
			var applyLeave = new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Employee = employee,
				FromDate = fromDate,
				ToDate = fromDate.AddDays(2),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				FromHalf = true,
				ToHalf = true,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = clLvTypeId,
						NoOfLeaves= 1m
					},
					new ApplyLeaveType
					{
						LeaveTypeId = clLvTypeId,
						NoOfLeaves = 2m
					}
				}
			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					LeaveDate = fromDate,
					IsFirstHalf = false,
					IsHalfDay = false
				},
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveDate = fromDate.AddDays(1),
					IsFirstHalf = false,
					IsHalfDay = false
				},
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveDate = fromDate.AddDays(2),
					IsFirstHalf = false,
					IsHalfDay = false
				}
			};

			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = attendanceId,
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.Date.AddDays(-3),
				  AttendanceStatus = (int)AttendanceStatus.Present
				},
				new Attendance
				{
				  ID = attendanceId,
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.Date.AddDays(-2),
				  AttendanceStatus = (int)AttendanceStatus.Present
				},
				new Attendance
				{
				  ID = attendanceId,
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.Date.AddDays(-1),
				  AttendanceStatus = (int)AttendanceStatus.Present
				},
			};

			var leaveType = new List<LeaveType> {
				new LeaveType
				{
					ID = clLvTypeId,
					Name = "Casual leave",
					DefaultPayoff = false,
					PayType = 1,
					Status = true
				},
				new LeaveType
				{
					ID = eLLvTypeId,
					Name = "Earned Leave",
					DefaultPayoff = false,
					PayType = 1,
					Status = true
				}
			};

			var modifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
					EmployeeID = employee.ID,
					IsFirstOff = true,
					IsHalfDay = true,
					AttendanceID = attendanceId,
				}
			};

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
					   ID=Guid.NewGuid(),
					   Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=clLvTypeId,
					   EffectiveFrom = fromDate,
					   EffectiveTo = toDate,
					},
				};

			//Mock
			var leaveDetailsMock = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, leaveDetailsMock);

			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);

			var mockLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockLeaveType);

			var mockLogs = modifyLogs.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceModifyLogs(uow, _context, mockLogs);

			_ = _context.GetRepositoryAsyncDbSet(uow, leaveBalances);
			//Act
			await service.UpdateLBIfLeaveApproved(applyLeave);
			//Assert
			Assert.Equal(2, leaveBalances.Sum(x => x.Leaves));
			Assert.Equal(3, leaveBalances.Count);

		}
		[Theory]
		[InlineData(true, 0, 1)]//new Appliy leave dates less then the existing applied leave dates Test
		[InlineData(false, 1, 2)]//new Applie leave todate matched with already applied leave fromdate
		[InlineData(false, 1, 3)]//new Applie leave todate inbetween already applied leave dates
		[InlineData(false, 3, 4)]//new Applie leave fromdate inbetween already applied leave dates and todate matched with existing leave todate
		[InlineData(false, 4, 5)]//new Applie leave fromdate matched with already applied leave todate
		[InlineData(false, 3, 5)]//new Applie leave fromdate inbetween already applied leave dates
		[InlineData(true, 5, 6)] //new Applie leave dates greater then the existing applied leave dates
		[InlineData(false, 1, 5)]//new Applie leave fromdate less then the existing leave from date and todate greater then existing leave todate
		[InlineData(false, 2, 4)] //new Applie leave dates same as existing applied leave dates
		public void SelfServiceValidation_BasedOn_ApplyLeaveDates(bool verify, int addDaysFrom, int addDaysTo)
		{
			#region Arrange
			var leaveTypeId = Guid.NewGuid();
			DateTime fromDate = DateTime.Now.AddDays(addDaysFrom);
			DateTime toDate = DateTime.Now.AddDays(addDaysTo);
			var daysDifference = (fromDate - toDate).Days;
			var employee = _empployeeData.GetEmployeeData();

			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.AddDays(2),
				  ToDate = DateTime.Now.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied
				},
			}.AsQueryable();

			var applyLeaveTypes = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				NoOfLeaves= 3,
				LeaveTypeId = leaveTypeId,
				ApplyLeave = new ApplyLeave
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.AddDays(2),
				  ToDate = DateTime.Now.AddDays(4),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied
				},
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(1),
				Status = (int)ClientVisitStatus.Rejected
			} };

			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
					   EmployeeId=employee.ID,
					   LeaveTypeId=leaveTypeId,
					   EffectiveFrom = fromDate,
					   EffectiveTo = fromDate.AddMonths(1).AddDays(-1)
					},
				};
			var newApplyLeave = new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":" + daysDifference + "}]",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						NoOfLeaves = daysDifference,
						LeaveTypeId = leaveTypeId
					}
				}
			};
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = toDate.AddDays(1),
				ToDateC = toDate.AddDays(2),
				Status = (int)WfhStatus.Applied,
				EmployeeId = employee.ID,
			} };
			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockSetApplyLeaveType = applyLeaveTypes.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockSetApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Employee Mock
			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);

			#endregion
			//Act
			var dd = _service.AddAsync(newApplyLeave);
			//Assert
			Assert.Equal(verify, dd.Result.IsSuccess);
		}

		#region New
		[Fact]
		public void SelfServiceValidation_BasedOn_LeaveType()
		{
			#region Arrange
			var ClLeaveTypeId = Guid.NewGuid();
			var ELLeaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var newApplyLeaveId = Guid.NewGuid();
			DateTime fromDate = DateTime.Now;
			DateTime toDate = DateTime.Now.AddDays(5);
			var employee = _empployeeData.GetEmployeeData();

			IEnumerable<LeaveType> leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = ClLeaveTypeId,
					Code = "CL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				},
				new LeaveType
				{
					ID = ELLeaveTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.AddDays(-2),
				  ToDate = DateTime.Now.AddDays(-1),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  ApplyLeaveType = new List<ApplyLeaveType>
				  {
					  new ApplyLeaveType
					  {
						  ID = Guid.NewGuid(),
						  ApplyLeaveId = applyLeaveId,
						  LeaveTypeId = ClLeaveTypeId,
						  NoOfLeaves = 2
					  }
				  }
				},
			}.AsQueryable();
			IEnumerable<ApplyLeaveType> applyLeaveTypes = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ClLeaveTypeId,
					NoOfLeaves = 3,
				},
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ELLeaveTypeId,
					NoOfLeaves = 2,
				}
			}.AsQueryable();

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(1),
				Status = (int)ClientVisitStatus.Rejected
			} };

			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=3,
						EmployeeId=employee.ID,
						LeaveTypeId=ClLeaveTypeId,
						EffectiveFrom = fromDate,
						EffectiveTo = fromDate.AddMonths(1).AddDays(-1)
					},
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=ELLeaveTypeId,
						EffectiveFrom = fromDate,
						EffectiveTo = fromDate.AddMonths(1).AddDays(-1)
					},
				};
			var newApplyLeave = new ApplyLeave
			{
				ID = newApplyLeaveId,
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":3},{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ClLeaveTypeId,
					NoOfLeaves = 3,
				},
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ELLeaveTypeId,
					NoOfLeaves = 2,
				}
				}
			};

			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = fromDate.AddDays(-2),
				ToDateC = fromDate.AddDays(-1),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "Reason"
			} };
			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveType Mock
			var mockApplyLeaveType = applyLeaveTypes.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Employee Mock
			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion

			//Act
			var dd = _service.AddAsync(newApplyLeave);
			//Assert
			Assert.True(dd.Result.IsSuccess);
		}
		[Theory]
		[InlineData(true, -1, 0, 5)]//Apply Leaves Are Less Then the LeaveBalance Available Leaves
		public async Task AddApprovedLeaveAsync_Without_Exception(bool verify, int addDaysFrom, int addDaysTo, int leaveBalance)
		{
			#region Arrange
			var ClLeaveTypeId = Guid.NewGuid();
			var ELLeaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var newApplyLeaveId = Guid.NewGuid();
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);

			var employee = _empployeeData.GetEmployeeData();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = ClLeaveTypeId,
					Code = "CL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				},
				new LeaveType
				{
					ID = ELLeaveTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			};

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = start.AddDays(1),
				  ToDate = start.AddDays(2),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Applied,
				  LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
				  ApplyLeaveType = new List<ApplyLeaveType>
				  {
					  new ApplyLeaveType
					  {
						  ID = Guid.NewGuid(),
						  ApplyLeaveId = applyLeaveId,
						  LeaveTypeId = ClLeaveTypeId,
						  NoOfLeaves = 2,
					  }
				  }
				},
			};

			var applyLeaveTypes = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ClLeaveTypeId,
					NoOfLeaves = 3,
				},
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ELLeaveTypeId,
					NoOfLeaves = 2,
				}
			};

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(1),
				Status = (int)ClientVisitStatus.Rejected
			} };

			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=ClLeaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=ELLeaveTypeId,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						EmployeeId = employee.ID,
						LeaveTypeId = ClLeaveTypeId,
						Leaves = -2,
						EffectiveFrom=start,
						EffectiveTo=end
					}
				};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					//ApplyLeave = new ApplyLeave
					//{
					//	ID = applyLeaveId,
					//	EmployeeId = employee.ID,
					//	FromDate = DateTime.Now.AddDays(-1),
					//	ToDate = DateTime.Now.AddDays(-1),
					//	Reason = "Corona",
					//	EmergencyContNo = "9381742192",
					//	Status = (int)ApplyLeaveSts.Applied,
					//	FromHalf = true,
					//	ToHalf = false
					//},
					LeaveTypeId = ClLeaveTypeId,
					IsFirstHalf = false,
					LeaveCount = 1,
					LeaveDate = start.AddDays(1)
				},
				new ApplyLeaveDetails
				{
					ID =Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					//ApplyLeave = new ApplyLeave
					//{
					//	ID = applyLeaveId,
					//	EmployeeId = employee.ID,
					//	FromDate = DateTime.Now.AddDays(-2),
					//	ToDate = DateTime.Now.AddDays(-2),
					//	Reason = "Corona",
					//	EmergencyContNo = "9381742192",
					//	Status = (int)ApplyLeaveSts.Applied,
					//	FromHalf = true,
					//	ToHalf = false
					//},
					LeaveTypeId = ELLeaveTypeId,
					IsFirstHalf = false,
					LeaveCount= 1,
					LeaveDate = start.AddDays(2)
				}
			};
			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.AddDays(-1),
				  AttendanceStatus = 1
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.AddDays(-2),
				  AttendanceStatus = 1
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.AddDays(-3),
				  AttendanceStatus = 1
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = DateTime.Now.AddDays(-4),
				  AttendanceStatus = 1
				},
			};
			var newApplyLeave = new ApplyLeave
			{
				ID = newApplyLeaveId,
				EmployeeId = employee.ID,
				FromDate = start.AddDays(4),
				ToDate = start.AddDays(8),
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":3},{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					//ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ClLeaveTypeId,
					NoOfLeaves = 3,
				},
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					//ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = employee.ID,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = ELLeaveTypeId,
					NoOfLeaves = 2,
				}
				}
			};
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				FromDateC = start.AddDays(10),
				ToDateC = start.AddDays(11),
				EmployeeId = employee.ID,
				Status = (int)WfhStatus.Applied,
			} };
			#endregion

			#region Mock

			// Mock 
			//_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);

			//_ = _context.GetRepositoryAsyncDbSet(uow, applyLeave);
			//_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveTypes);

			//_ = _context.GetRepositoryAsyncDbSet(uow, leaveBalances);

			//_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveDetails);
			//_ = _context.GetRepositoryAsyncDbSet(uow, attendance);

			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var mockApplyLeaveType = applyLeaveTypes.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, mockApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			var mockSetEmployee = _empployeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);

			var mockWfh = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
			#endregion
			// Act
			var dd = await _service.AddApprovedLeaveAsync(newApplyLeave);
			var leaveBal = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();
			// Assert
			Assert.Equal((int)ApplyLeaveSts.Approved, dd.ReturnValue.Status);
			Assert.Equal(verify, dd.IsSuccess);
		}

		#endregion

		[Theory]
		[InlineData(4, true)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public async Task MaximumLeavesValidation(decimal noOfLeaves, bool result)
		{
			//Arrange
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var type = new List<LeaveType>
			{
				new LeaveType
				{
				  ID = leaveTypeId,
				  Code = "EL",
				  Name = "Earned Leave",
				  EffectiveAfter = 1,
				  EffectiveType = 2,
				  EffectiveBy = 1,
				  ProrateByT = 1,
				  RoundOff = 1,
				  RoundOffTo = 1,
				  Gender = 4,
				  MaritalStatus = 3,
				  PastDate = 2,
				  FutureDate = 5,
				  MaxApplications = 2,
				  MinLeaves = 2,
				  MaxLeaves = 3,
				  specifiedperio = 1,
				}
			}.AsQueryable();
			var applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date.AddDays(-12),
				  ToDate = DateTime.Now.Date.AddDays(-10),
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved,
				  FromHalf = true,
				  ToHalf = true,
				  ApplyLeaveType = new List<ApplyLeaveType> {new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					NoOfLeaves= 4,
					LeaveTypeId=leaveTypeId
				} }
				},
			}.AsQueryable();
			//Mock
			var mockSetLeaveType = type.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//Act
			var dd = await _service.MaximumLeavesValidation(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Reason = "Raining",
				EmergencyContNo = "9392440499",
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays((double)noOfLeaves - 1),
				FromHalf = true,
				ToHalf = false,
				ApplyLeaveType = new List<ApplyLeaveType> {new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					NoOfLeaves= noOfLeaves,
					LeaveTypeId=leaveTypeId
				} }
			});
			//Assert
			Assert.Equal(dd.HasError, result);
		}
		[Fact]
		public async Task SelfServiceLeaveValidation_AlreadyAppliedClientVisit_Throw_Exception()
		{
			//Arrange
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var employee = _empployeeData.GetEmployeeData();
			var applyLeaveData = new List<ApplyLeave> { new ApplyLeave
			{

				FromDate = DateTime.Now.AddDays(-4),
				ToDate = DateTime.Now.AddDays(-2),
				NoOfLeaves = 2,
				EmployeeId = employee.ID,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				Status = (int)ApplyLeaveSts.Approved,
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves = 2
				} }
			} };
			var applyLeave = new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays(2),
				FromHalf = true,
				ToHalf = false,
				EmergencyContNo = "9392440422",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				Status = (int)ApplyLeaveSts.Applied,
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId = leaveTypeId,
					NoOfLeaves = 2
				} }
			};
			var wfh = new List<ApplyWfh> { new ApplyWfh
			{
				EmployeeId = employee.ID,
				FromDateC = DateTime.Now.AddDays(-1),
				FromHalf = false,
				ToDateC = DateTime.Now.AddDays(-1),
				ToHalf = false,
				ReasonForWFH = "due to health",
				Status = (int)WfhStatus.Applied
			} };
			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays(2),
				Status = (int)ClientVisitStatus.Applied
			} };
			//Mock
			var mockApplyLeave = applyLeaveData.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockWFH = wfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);
			//Act
			var result = await _service.SelfServiceLeaveValidation(applyLeave);
			Assert.True(result.HasError);
		}
	}
}
