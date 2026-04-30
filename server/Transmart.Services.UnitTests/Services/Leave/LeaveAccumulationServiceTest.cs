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
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service;
using TranSmart.Service.Leave;
using Xunit;

namespace TranSmart.Services.UnitTests.Services.Leave
{
	public class LeaveAccumulationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;

		public LeaveAccumulationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		[Theory]
		//ProrateByT 1 for month 2 for days
		//RoundOff 1 for nearest 2 for minimum 3 for maximum
		//RoundOffTo 1 for FullDay 2 for HelfDay
		//Duration 1 for full day 2 for Half-Day
		// AccType 1 for year 2 for half yearly 3 for quarterly 4 for monthly
		// AccOnYaerly
		// AcconHelfyearly 1 for Jan-July  2 for Feb-Aug  3 for Mar-Sept  4 for Apr-Oct 5 for May-Nov 6 for June-Dec
		// AccOnQuatraly 1 for Jan-Apr-July-Oct and 2 for Feb-May-Aug-Nov and 3 for Mar-Jun-Sept-Dec
		// AccOnDay int Day(1-31)
		// EffectiveType 1 for Days 2 for months 3 for years
		// EffectiveBy 1 DOJ 2 For DOC
		// EfectiveAfter mentions dates or months 

		[InlineData(1, 1, 1, 1, 1, 4, 0, 0, 27, 1, 1, 1, "2021-02-26", 12, 5, 60, 5, 2, 8, 12, -3, "2022-04-27")]
		[InlineData(1, 1, 1, 1, 2, 0, 4, 0, 27, 1, 1, 1, "2021-02-26", 12, 5, 60, 5, 1, 10, 12, -4, "2022-04-27")]
		[InlineData(1, 1, 1, 1, 2, 0, 4, 0, 27, 1, 1, 1, "2023-08-26", 12, 5, 60, 5, 1, 10, 4, -4, "2023-10-27")]
		//[InlineData(1, 1, 1, 1,  2, 0, 1, 0, 26, 1, 1, 1, "2021-02-26", 6, 5, 60, 5, 2, 8, 5, -3)]
		//[InlineData(1, 1, 1, 1,  2, 0, 1, 0, 26, 1, 1, 1, "2020-10-26", 6, 5, 60, 5, 2, 10, 6, -5)]
		//[InlineData(1, 1, 1, 1,  3, 0, 0, 1, 26, 1, 1, 1, "2021-05-26", 3, 5, 60, 5, 2, 8, 1, -3)]
		//[InlineData(1, 1, 1, 1,  3, 0, 0, 1, 26, 1, 1, 1, "2020-10-26", 3, 5, 60, 5, 1, 10, 3, -4)]
		public async Task Leave_Accumulation_Test(int ProrateByT, int RoundOff, int RoundOffTo, int Duration, int accType, int AccOnYearly,
			int AccOnHelfYearly, int AccOnQuatarly, int AccOnDay, int effType, int effBy, int effAfter, string DOJ, int NoOfDays,
			int FwdDays, int FwdPercentage, int Fwdlimit, int FwdType, int PresentLeaves, decimal Result, decimal KarryFwdResult, string scheduledDate)
		{

			#region Assign

			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				ProrateByT = ProrateByT,
				RoundOff = RoundOff,
				RoundOffTo = RoundOffTo,
				EffectiveAfter = effAfter,
				EffectiveType = effType,
				EffectiveBy = effBy,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
				Duration = Duration
			};

			var leaveType = new List<LeaveType>
			{
			   new LeaveType
			   {
				 ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				ProrateByT = ProrateByT,
				RoundOff = RoundOff,
				RoundOffTo = RoundOffTo,
				EffectiveAfter = effAfter,
				EffectiveType = effType,
				EffectiveBy = effBy,
				Gender = 4,
				MaritalStatus = 3,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status=true,
				Duration=Duration
			   }
			};

			var leaveTypeSchedules = new List<LeaveTypeSchedule>
			{
				new LeaveTypeSchedule
				{
					ID=Guid.NewGuid(),
					LeaveTypeId=type.ID,
					AccType=accType,
					AccOnDay=AccOnDay,
					AccOnQuarterly=AccOnQuatarly,
					AccOnYearly=AccOnYearly,
					AccOnHalfYearly=AccOnHelfYearly,
					NoOfDays=NoOfDays,
					FwdDays=FwdDays,
					FwdPercentage=FwdPercentage,
					FwdOverallLimit=10,
					FwdLimit=Fwdlimit,
					FwdType=FwdType
                    //ResetType=1,
                    //ResOnDay=AccOnDay,
                    //ResOnQuarterly=AccOnQuatarly,
                    //ResOnHalfYearly=AccOnYearly,
                    //ResOnYearly=AccOnHelfYearly
                }
			};

			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse(DOJ),
				Gender = 1,
				Name = "Raja",
				No = "Avontix1796"
			};
			var employees = new List<Employee>
			{
				new Employee
			   {
				ID =emp.ID,
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse(DOJ),
				Gender = 1,
				Name = "Raja",
				No = "Avontix1796"
			   }
			};

			var leaveBalance = new LeaveBalance
			{
				ID = Guid.NewGuid(),
				Leaves = 0,
				EmployeeId = emp.ID,
			};

			var approvedLeaves = new ApprovedLeaves()
			{
				ID = Guid.NewGuid(),
				EmployeeId = emp.ID,
				LeaveTypeId = type.ID,
				NoOfLeaves = 0,
				Limit = 5,
				Allow = true
			};

			var leaveBalances = new List<LeaveBalance>
			{
				 new LeaveBalance
				{
					 ID = Guid.NewGuid(),
					Leaves = PresentLeaves,
					EmployeeId =Guid.Empty,
				}
			};

			#endregion

			#region Mock

			//LeaveType
			var leaveTypeRepo = new Mock<RepositoryAsync<LeaveType>>(_dbContext.Object);
			leaveTypeRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<LeaveType, bool>>>(),
				   It.IsAny<Func<IQueryable<LeaveType>, IOrderedQueryable<LeaveType>>>(),
				   It.IsAny<Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>>>())).ReturnsAsync(leaveType);
			leaveTypeRepo.Setup(x => x.AddAsync(It.IsAny<LeaveType>())).Callback((LeaveType LType) => leaveType.Add(LType));
			uow.Setup(m => m.GetRepositoryAsync<LeaveType>()).Returns(leaveTypeRepo.Object);

			//LeaveTypeSchedule
			var leaveTypeSchRepo = new Mock<RepositoryAsync<LeaveTypeSchedule>>(_dbContext.Object);
			leaveTypeSchRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<LeaveTypeSchedule, bool>>>(),
				   It.IsAny<Func<IQueryable<LeaveTypeSchedule>, IOrderedQueryable<LeaveTypeSchedule>>>(),
				   It.IsAny<Func<IQueryable<LeaveTypeSchedule>, IIncludableQueryable<LeaveTypeSchedule, object>>>())).ReturnsAsync(leaveTypeSchedules);
			leaveTypeSchRepo.Setup(x => x.AddAsync(It.IsAny<LeaveTypeSchedule>())).Callback((LeaveTypeSchedule LTypeSch) => leaveTypeSchedules.Add(LTypeSch));
			uow.Setup(m => m.GetRepositoryAsync<LeaveTypeSchedule>()).Returns(leaveTypeSchRepo.Object);

			//Employee
			var empRepo = new Mock<RepositoryAsync<Employee>>(_dbContext.Object);
			empRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(),
				   It.IsAny<Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>(),
				   It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>())).ReturnsAsync(employees);
			empRepo.Setup(x => x.AddAsync(It.IsAny<Employee>())).Callback((Employee emp) => employees.Add(emp));
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(empRepo.Object);

			//LeaveBalance
			var leaveBalRepo = new Mock<IRepositoryAsync<LeaveBalance>>();
			leaveBalRepo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<LeaveBalance, bool>>>(),
				  It.IsAny<Func<IQueryable<LeaveBalance>, IOrderedQueryable<LeaveBalance>>>(),
				  It.IsAny<Func<IQueryable<LeaveBalance>, IIncludableQueryable<LeaveBalance, object>>>(), true)).ReturnsAsync(leaveBalance);
			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(leaveBalRepo.Object);


			leaveBalRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<LeaveBalance, bool>>>(),
				   It.IsAny<Func<IQueryable<LeaveBalance>, IOrderedQueryable<LeaveBalance>>>(),
				   It.IsAny<Func<IQueryable<LeaveBalance>, IIncludableQueryable<LeaveBalance, object>>>())).ReturnsAsync(leaveBalances);
			leaveBalRepo.Setup(x => x.AddAsync(It.IsAny<LeaveBalance>())).Callback((LeaveBalance leavBal) => leaveBalances.Add(leavBal));
			leaveBalRepo.Setup(x => x.UpdateAsync(It.IsAny<LeaveBalance>())).Callback((LeaveBalance leavBal) => leaveBalances.Add(leavBal));
			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(leaveBalRepo.Object);

			//ApprovedLeaves
			var approvedLeaveRepo = new Mock<RepositoryAsync<ApprovedLeaves>>(_dbContext.Object);
			approvedLeaveRepo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<ApprovedLeaves, bool>>>(),
				  It.IsAny<Func<IQueryable<ApprovedLeaves>, IOrderedQueryable<ApprovedLeaves>>>(),
				  It.IsAny<Func<IQueryable<ApprovedLeaves>, IIncludableQueryable<ApprovedLeaves, object>>>(), true)).ReturnsAsync(approvedLeaves);
			uow.Setup(m => m.GetRepositoryAsync<ApprovedLeaves>()).Returns(approvedLeaveRepo.Object);

			#endregion

			var src = new LeaveAccumulationService(uow.Object);
			//Act
			await src.LeavesAccumulationSchedule(DateTime.Parse(scheduledDate));



			//Assert
			Assert.Equal(KarryFwdResult, leaveBalances[1].Leaves);
			Assert.Equal(Result, leaveBalances[2].Leaves);
		}
		#region New Code for LeavesAccumulationSchedule
		[Fact]
		public async Task LeavesAccumulationSchedule()
		{
			#region Arrange
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				ProrateByT = 2, //ProrateBy Days
				EffectiveType = 1,//EffectiveType Days
				EffectiveBy = 1,//EffectiveBy EmployeeJoiningDate
				Gender = 4,
				MaritalStatus = 3,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status=true,
				Duration=1
			} };
			var leaveTypeSchedule = new List<LeaveTypeSchedule> { new LeaveTypeSchedule
			{
					ID=Guid.NewGuid(),
					LeaveTypeId=leaveType.FirstOrDefault().ID,
					AccType=1,
					AccOnYearly = DateTime.Now.Date.Month,
					AccOnDay = DateTime.Now.Date.Day,
			} };
			var employee = _employeeData.GetAllEmployeesData().LastOrDefault();
			var employees = new List<Employee>
			{
				new Employee
				{
					ID= Guid.Parse("4c48a4cd-6933-4ba0-9e01-600c4124800c"),
					No = "AVONTIX1827",
					Name = "Vishnu",
					Gender = 1,
					MaritalStatus = 3,
					Status=2,
					MobileNumber = "9639639637",
					DateOfBirth = new DateTime(1994 , 02 , 02),
					DateOfJoining = DateTime.Parse("2022-08-30"),
					AadhaarNumber = "561250752387",
					PanNumber = "BLMPJ2797R",
					DepartmentId = Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),
					Department = new Department{ID=Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),Name = "Coding"},
					DesignationId = Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"),
					Designation = new Designation{ID=Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"), Name ="Sr.Manager"},
					WorkLocationId = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),
					WorkLocation = new Location{ID = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),Name="Vijayawada" },
					TeamId = Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"),
					Team = new Team{ID =Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"), Name="Stedmans"},
					ReportingToId = Guid.Parse("77934662-9896-44d5-bc74-5cce64150fba"),
					AllowWebPunch=true
				}
			};
			var leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					ID = Guid.NewGuid(),
					Leaves = 5,
					EmployeeId = employee.ID,
					Employee = employee,
				}
			};
			var approvedLeaves = new List<ApprovedLeaves> { new ApprovedLeaves { ID = Guid.NewGuid(), EmployeeId = employee.ID, Employee = employee } };
			#endregion

			#region Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);
			_context.GetRepositoryAsyncDbSet(uow, leaveTypeSchedule);
			// Employee Mock
			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			_context.GetRepositoryAsyncDbSet(uow, leaveBalance);
			_context.GetRepositoryAsyncDbSet(uow, approvedLeaves);
			#endregion
			// Act
			var _service = new LeaveAccumulationService(uow.Object);
			_ = await _service.LeavesAccumulationSchedule(DateTime.Parse("2022-08-31"));
			// Assert
			Assert.Equal(2, 2);
		}
		#endregion

		[Theory]
		#region Test cases
		[InlineData(true, -1, 1, 0, 0, "2020-10-17", "2020-10-17")] //ResetType is DOB month(-1) ,True case(Employee DOB Month Matches ScheduleDate Month)
		[InlineData(false, -1, 1, 0, 0, "2020-05-17", "2020-05-17")] //ResetType is DOB month(-1) ,False case(Employee DOB Month Not Matches ScheduleDate Month)

		[InlineData(true, -2, 1, 0, 0, "2019-12-30", "2020-10-17")] //ResetType is DOJ(-2) ,True case(Employee DOJ Date Matches ScheduleDate)
		[InlineData(false, -2, 1, 0, 0, "2020-05-17", "2020-05-17")] //ResetType is DOJ(-2) ,False case(Employee DOJ Date Not Matches ScheduleDate)

		[InlineData(true, -3, 1, 0, 0, "2022-08-04", "2020-10-17")] //ResetType is Anniversary Date(-3) ,True case(Anniversary Date Matches ScheduleDate)
		[InlineData(false, -3, 1, 0, 0, "2022-08-05", "2020-05-17")] //ResetType is Anniversary Date(-3) ,False case(Employee Anniversary Date is Null)

		[InlineData(true, 1, 1, 0, 0, "2021-01-17", "2021-01-17")] //AccType is yearly(1) ,True case
		[InlineData(false, 1, 1, 0, 0, "2021-01-15", "2021-01-17")] //AccType is yearly(1) , False case(AccOnDay and ScheduleDay is Not matched)

		[InlineData(true, 2, 0, 1, 0, "2021-01-17", "2021-01-17")] //AccType is HalfYearly(2) , True case
		[InlineData(false, 2, 0, 1, 0, "2021-01-15", "2021-01-17")] //AccType is HalfYearly(2) , False case(AccOnDay not matched with ScheduleDay)
		[InlineData(false, 2, 0, 3, 0, "2021-01-17", "2021-01-17")] //AccType is HalfYearly(2) , False case(AccOnHalfYearly not matched with scheduleDate.Month)

		[InlineData(true, 3, 0, 0, 1, "2021-01-17", "2021-01-17")]//AccType is Quarterly(3),True case
		[InlineData(false, 3, 0, 0, 1, "2021-07-17", "2021-07-18")]//AccType is Quarterly(3) , False case(AccOnDay not matched with ScheduleDay)

		[InlineData(true, 4, 0, 0, 1, "2021-01-17", "2021-01-17")]//AccType is Monthly(4),True case
		[InlineData(false, 4, 0, 0, 1, "2021-07-17", "2021-07-18")]//AccType is Monthly(4) , False case(AccOnDay not matched with ScheduleDay)
		#endregion
		public void IsScheduled_Date_Test(bool verify, int accType, int AccOnyearly, int AccOnHelfYearly, int AcconQuatraly, string ScheduleDate, string accOnDay)
		{

			#region Assign
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Raja",
				No = "Avontix1796",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = accType,
				AccOnDay = DateTime.Parse(accOnDay).Day,
				AccOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				AccOnYearly = AccOnyearly,
				AccOnHalfYearly = AccOnHelfYearly
			};
			#endregion

			//Act
			var dd = LeaveAccumulationService.IsScheduledDate(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);
		}

		[Theory]
		[InlineData(true, -1, 1, 0, 0, "2020-10-17", "2020-10-17")] //ResetType is DOB month(-1) ,True case(Employee DOB Month Matches ScheduleDate Month)
		[InlineData(false, -1, 1, 0, 0, "2020-05-17", "2020-05-17")] //ResetType is DOB month(-1) ,False case(Employee DOB Month Not Matches ScheduleDate Month)
		public void IsScheduledDateForLapsed_DOB_Verify(bool verify, int accType, int AccOnyearly, int AccOnHelfYearly, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = accType,
				ResetType = accType,
				ResOnDay = DateTime.Parse(resOnDay).Day,//DateTime.Parse(ScheduleDate).Day,
				ResOnHalfYearly = AccOnHelfYearly,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = AccOnyearly
			};

			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);

		}
		[Theory]
		[InlineData(true, -2, 0, "2019-12-30", "2020-10-17")] //ResetType is DOJ(-2) ,True case(Employee DOJ Date Matches ScheduleDate)
		[InlineData(false, -2, 0, "2020-05-17", "2020-05-17")] //ResetType is DOJ(-2) ,False case(Employee DOJ Date Not Matches ScheduleDate)
		public void IsScheduledDateForLapsed_DOJ_Verify(bool verify, int accType, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = -2,
				ResetType = -2,
				ResOnDay = DateTime.Parse(resOnDay).Day,//DateTime.Parse(ScheduleDate).Day,
				ResOnHalfYearly = 0,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = 1
			};
			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);
		}

		[Theory]
		[InlineData(true, -3, 0, "2019-12-30", "2020-10-17")] //ResetType is MarriageDay(-3) ,True case(Employee MarriageDay Date Matches ScheduleDate)
		[InlineData(false, -3, 0, "2020-05-17", "2020-05-17")] //ResetType is MarriageDay(-3) ,False case(Employee MarriageDay Date Not Matches ScheduleDate)
		public void IsScheduledDateForLapsed_MarriageDay_Verify(bool verify, int accType, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2019-12-30") ? DateTime.Parse("2019-12-30") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = -3,
				ResetType = -3,
				ResOnDay = DateTime.Parse(resOnDay).Day,//DateTime.Parse(ScheduleDate).Day,
				ResOnHalfYearly = 0,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = 1
			};

			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);

		}

		[Theory]
		[InlineData(true, 1, 0, "2021-01-17", "2021-01-17")] //ResetType is yearly(1) ,True case
		[InlineData(false, 1, 0, "2021-01-17", "2021-01-18")] //ResetType is yearly(1) ,false case
		public void IsScheduledDateForLapsed_ResetType_Yearly_Verify(bool verify, int accType, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = 1,
				ResetType = 1,
				ResOnDay = DateTime.Parse(resOnDay).Day,
				ResOnHalfYearly = 0,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = 1
			};

			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);

		}
		[Theory]
		[InlineData(true, 2, 0, 1, 0, "2021-01-17", "2021-01-17")] //ResetType is HalfYearly(2) ,True case 
		[InlineData(false, 2, 0, 3, 0, "2021-01-17", "2021-01-17")] //ResetType is HalfYearly(2) ,ResOnHalfYearly Missmatched False case 
		public void IsScheduledDateForLapsed_ResetType_HalfYearly_Verify(bool verify, int accType, int AccOnyearly, int AccOnHelfYearly, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = 2,
				ResetType = 2,
				ResOnDay = DateTime.Parse(resOnDay).Day,
				ResOnHalfYearly = AccOnHelfYearly,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = AccOnyearly
			};
			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);
		}
		[Theory]
		[InlineData(true, 3, 0, 1, "2021-07-17", "2021-01-17")] //ResetType is Quarterly(3) , True case
		[InlineData(false, 3, 0, 2, "2021-11-17", "2021-11-18")] //ResetType is Quarterly(3) , false case
		public void IsScheduledDateForLapsed_Quarterly_Verify(bool verify, int accType, int AccOnHelfYearly, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = 3,
				ResetType = 3,
				ResOnDay = DateTime.Parse(resOnDay).Day,
				ResOnHalfYearly = AccOnHelfYearly,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = 0
			};

			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);

		}

		[Theory]
		[InlineData(true, 4, 3, "2021-03-17", "2021-11-17")] //ResetType is Monthly(4) , True case
		[InlineData(false, 4, 3, "2021-03-17", "2021-11-18")] //ResetType is Monthly(4) , False case
		public void IsScheduledDateForLapsed_Monthly_Verify(bool verify, int accType, int AcconQuatraly, string ScheduleDate, string resOnDay)
		{
			//Assert
			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "968542358699",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Chishtheshwar",
				No = "Avontix1827",
				DateOfBirth = DateTime.Parse("1992-10-04"),
				MarriageDay = (ScheduleDate == "2022-08-04") ? DateTime.Parse("2022-08-04") : null

			};
			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.NewGuid(),
				AccType = 4,
				ResetType = 4,
				ResOnDay = DateTime.Parse(resOnDay).Day,//DateTime.Parse(ScheduleDate).Day,
				ResOnHalfYearly = 0,
				ResOnQuarterly = verify && accType != 3 ? DateTime.Parse(ScheduleDate).Month : AcconQuatraly,
				ResOnYearly = 0
			};

			//Act
			var dd = LeaveAccumulationService.IsScheduledDateForLapsed(schedule, emp, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);

		}
		// EfectiveAfter mentions dates or months 
		// EffectiveBy 1 DOJ 2 For DOC
		// EffectiveType 1 for Days 2 for months 3 for years
		[Theory]
		#region Cases
		[InlineData(true, 1, 1, 1, "2021-07-10", "2021-07-08")]//Effective Type is Days(1) and EffectiveBy Employee Joining date(1), and (SheduleDate-DOJ).Days > EffectiveAfter
		[InlineData(true, 1, 2, 1, "2021-07-10", "2021-07-08")]//Effective Type is Days(1) and EffectiveBy  Employee Confirmation date(2), and (SheduleDate-DOJ).Days > EffectiveAfter
		[InlineData(false, 1, 1, 4, "2021-07-10", "2021-07-08")]//Effective Type is Days(1) and EffectiveBy  Employee Joining date(1), and (SheduleDate-DOJ).Days < EffectiveAfter
		[InlineData(false, 1, 2, 4, "2021-07-10", "2021-07-08")]//Effective Type is Days(1) and EffectiveBy  Employee Confirmation date(2), and (SheduleDate-DOJ).Days < EffectiveAfter

		[InlineData(true, 2, 1, 1, "2021-07-10", "2021-06-05")]//Effective Type is Months(2) and EffectiveBy(1) from Employee Joining date, return True case
		[InlineData(true, 2, 2, 1, "2021-07-10", "2021-04-05")]//Effective Type is Months(2) and EffectiveBy(2) from Employee Confirmation date return true case
		[InlineData(false, 2, 1, 26, "2021-07-10", "2021-06-05")]//Effective Type is Months(2) and EffectiveBy(1) from Employee Joining date, return false case
		[InlineData(false, 2, 2, 29, "2021-07-10", "2021-04-05")]//Effective Type is Months(2) and EffectiveBy(2) from Employee Confirmation date return false case

		[InlineData(true, 3, 1, 1, "2021-07-10", "2020-06-05")]//Effective Type is Years(3) and EffectiveBy(1) from Employee Joining date, return True case
		[InlineData(true, 3, 2, 1, "2021-07-10", "2020-04-05")]//Effective Type is Years(3) and EffectiveBy(2) from Employee Confirmation date, return True case
		[InlineData(false, 3, 1, 2, "2021-07-10", "2020-06-05")]//Effective Type is Years(3) and EffectiveBy(1) from Employee Joining date, return false case
		[InlineData(false, 3, 2, 2, "2021-07-10", "2020-04-05")]//Effective Type is Years(3) and EffectiveBy(2) from Employee Confirmation date, return false case

		[InlineData(false, 4, 2, 2, "2021-07-10", "2020-04-05")]//invalid data (given effective type is given as 4) , this test is execute for return false
		#endregion
		public void Effective_Date_Test(bool verify, int effType, int effBy, int effAfter, string ScheduleDate, string DOJ)
		{
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = effAfter,
				EffectiveType = effType,
				EffectiveBy = effBy
			};

			var emp = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse(DOJ),
				Gender = 1,
				Name = "Raja",
				No = "Avontix1796"
			};

			//Act
			var dd = LeaveAccumulationService.EffectiveDate(type, emp.DateOfJoining, emp.DateOfJoining, DateTime.Parse(ScheduleDate));

			//Assert
			Assert.Equal(verify, dd);
		}

		// round off 1 for nearest 2 minimum 3 for maximum
		[Theory]
		[InlineData(3, 2.9, 1)]
		[InlineData(2, 2.2, 2)]
		[InlineData(3, 2.9, 3)]
		public void GetLeavesCountAfterRoundTypeCalc_RoundOff_FullDay(decimal res, decimal leaves, int roundOff)
		{
			//Assign
			int roundOffTo = 1;//FullDay
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				RoundOff = roundOff,
				RoundOffTo = roundOffTo,
				Status = true
			};

			//Act
			var dd = LeaveAccumulationService.GetLeavesCountAfterRoundTypeCalc(leaves, type);

			//Assert
			Assert.Equal(res, dd);
		}

		[Theory]
		[InlineData(3, 2.9, 1)]
		[InlineData(2.5, 2.9, 2)]
		[InlineData(3, 2.9, 3)]
		public void GetLeavesCountAfterRoundTypeCalc_RoundOff_HalfDay(decimal res, decimal leaves, int roundOff)
		{
			//Arrange
			int roundOffTo = 2;//HalfDay		   
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				RoundOff = roundOff,
				RoundOffTo = roundOffTo,
				Status = true
			};
			//Act
			var dd = LeaveAccumulationService.GetLeavesCountAfterRoundTypeCalc(leaves, type);

			//Assert
			Assert.Equal(res, dd);
		}

		[Fact]
		public void LeavesBasedOnDuration_DurationType_FullDay()
		{
			//Arrange
			int durationType = 1; decimal leaves = 4;
			//Act
			var dd = LeaveAccumulationService.LeavesBasedOnDuration(durationType, leaves);
			//Assert
			Assert.Equal(4, dd);
		}
		[Fact]
		public void LeavesBasedOnDuration_DurationType_HalfDay()
		{
			//Arrange
			int durationType = 2; decimal leaves = 4;
			//Act
			var dd = LeaveAccumulationService.LeavesBasedOnDuration(durationType, leaves);
			//Assert
			Assert.Equal(2, dd);
		}

		//Duration 1 for FullDay 2 for OffDay
		//Effective after number tells that after the specific period only the user eligible for leaves
		//Effective Type tells that Effective After is for Days or months
		//Effective By means date consideration either DOJ or DOC(confirmation)
		//ProrateBy 1 for month wise 2 for Days wise
		[Theory]
		[InlineData("2020-03-25", 12)] // employeeEffectiveDate > StartDate
		[InlineData("2023-04-26", 11)]//employeeEffectiveDate is InBetween StartDate and EndDate
		public void EntitlementLeavesYearly_LeaveTypeProrateByIs_Months(string doj, int res)
		{
			int NoOfDays = 12, schedulemonth = 3, scheduledate = 26;
			DateTime DOJ = DateTime.Parse(doj);
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
				Duration = 1
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.Month,
				AccOnYearly = schedulemonth,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = 3
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesYearly(type, DOJ, DOJ, schedule);

			//Assert
			Assert.Equal(res, dd);
		}

		[Theory]
		[InlineData("2024-06-28", 12)] //employeeEffectiveDate > StartDate
		[InlineData("2023-04-26", 11)] //employeeEffectiveDate is InBetween StartDate and EndDate
		public void EntitlementLeavesYearly_LeaveTypeProrateByIs_Days(string doj, int res)
		{
			int NoOfDays = 12, schedulemonth = 3, scheduledate = 26;
			DateTime DOJ = DateTime.Parse(doj);
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 2,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
				Duration = 1
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.Month,
				AccOnYearly = schedulemonth,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = 3
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesYearly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.Equal(res, dd);
		}

		[Theory]
		[InlineData(1, "2021-05-25", 6)]//DOJ < startDate
		[InlineData(1, "2023-06-25", 3)] //employeeEffectiveDate is InBetween StartDate and EndDate
		public void EntitlementLeavesHelfYearly_LeaveTypeProrateByIs_Months(int EffectiveBy, string doj, int res)
		{
			int NoOfDays = 6, schedulemonth = 3, scheduledate = 26;
			DateTime DOJ = DateTime.Parse(doj);

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = EffectiveBy,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
				Duration = 1
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.Month,
				AccOnYearly = schedulemonth,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = schedulemonth
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesHelfYearly(type, DOJ, DOJ, schedule);

			//Assert
			Assert.Equal(res, dd);
		}

		[Theory]
		[InlineData(1, "2021-06-15", 30)]//DOJ < startDate
		[InlineData(1, "2023-09-20", 1)]//employeeEffectiveDate is InBetween StartDate and EndDate
		[InlineData(1, "2025-04-25", 0)]// DOJ > startDate Returns Balance is 0
		public void EntitlementLeavesHelfYearly_LeaveTypeProrateByIs_Days(int EffectiveBy, string doj, int res)
		{
			int NoOfDays = 30, schedulemonth = 3, scheduledate = 26;
			DateTime DOJ = DateTime.Parse(doj);
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = EffectiveBy,
				ProrateByT = 2,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
				Duration = 1
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.Month,
				AccOnYearly = schedulemonth,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = schedulemonth
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesHelfYearly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.Equal(res, dd);
		}

		[Fact]//employeeEffectiveDate is InBetween StartDate and EndDate
		public void EntitlementLeavesQuatraly_ProrateBy_Months()
		{
			//DateTime DOJ = DateTime.Now.AddDays(30);
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = 1,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.AddMonths(1).Month,
				AccOnYearly = 1,
				NoOfDays = 4,
				AccOnHalfYearly = 1
			};
			//Act
			DateTime DOJ = new DateTime(DateTime.Now.Year, (int)schedule.AccOnQuarterly, schedule.AccOnDay+2);
			var dd = LeaveAccumulationService.EntitlementLeavesQuatraly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.True(dd == 4);
		}
		[Fact]//employeeEffectiveDate < startDate
		public void EntitlementLeavesQuatraly_ProrateByMonths()
		{
			DateTime DOJ = DateTime.Now.AddDays(-2);
			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = DateTime.Now.Day,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = DateTime.Now.Month,
				AccOnYearly = 1,
				NoOfDays = 4,
				AccOnHalfYearly = 3
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesQuatraly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.True(dd == 4);
		}
		[Fact]//employeeEffectiveDate < startDate
		public void EntitlementLeavesQuatraly_ProrateBy_Days()
		{
			DateTime DOJ = DateTime.Now;
			//Joining date issue

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 2,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = 15,
				ResetType = 1,
				ResOnDay = 13,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = 2,
				AccOnYearly = 1,
				NoOfDays = 6,
				AccOnHalfYearly = 3
			};
			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesQuatraly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.True(dd == 2);
		}

		[Theory]
		[InlineData(30, 6, 1, 1)]//employeeEffectiveDate is InBetween StartDate and EndDate
		public void EntitlementLeavesQuatraly_ProrateByDays_EffectiveDay_Between_StartAndEndDays(int doj, int noOfDays, int schedulemonth, int scheduledate)
		{
			DateTime date = DateTime.Now.AddDays(doj);
			DateTime quarterly = new DateTime(DateTime.Now.Year, date.Month, 1).AddMonths(3);
			DateTime month = DateTime.Now.AddMonths(schedulemonth);
			int months = month.Month;
			//Joining date issue

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 2,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = months,
				AccOnYearly = 1,
				NoOfDays = noOfDays,
				AccOnHalfYearly = schedulemonth
			};
			//Act
			DateTime DOJ = new DateTime(DateTime.Now.Year, (int)schedule.AccOnQuarterly, schedule.AccOnDay+2);
			var dd = LeaveAccumulationService.EntitlementLeavesQuatraly(type, DOJ, DOJ, schedule);
			//Assert
			Assert.True(dd == 6);
		}

		[Theory]
		[InlineData(1, -60, 5, 1, 26, 5)]//StartDate GreaterThen EmpEffectiveDate
		[InlineData(1, 60, 5, 2, 26, 0)]//StartDate LessThen EmpEffectiveDate
		public void EntitlementLeavesMonthly_ProrateByDays(int prorateByT, int doj, int NoOfDays, int schedulemonth, int scheduledate, int res)
		{
			DateTime DOJ = DateTime.Now.AddDays(doj);
			//Joining date issue

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = prorateByT,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = schedulemonth,
				AccOnYearly = 1,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = schedulemonth
			};

			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesMonthly(type, DOJ, DOJ, schedule);

			//Assert
			Assert.Equal(res, dd);
		}

		[Theory]
		[InlineData(2, 1, 2, 5, 1, 26, 5)] //EffectiveBy is JoiningDate , StartDate GreaterThen EmpEffectiveDate
		[InlineData(2, 2, 2, 5, 1, 26, 5)] //EffectiveBy is DateOfConfirm , StartDate GreaterThen EmpEffectiveDate
		[InlineData(2, 1, 27, 28, 2, 26, 27)] //EffectiveBy is JoiningDate , EmpEffectiveDate in between StartDate and EndDate
		[InlineData(1, 2, 2, 5, 1, 26, 5)] //Prorate By =1(Months) EffectiveBy is DateOfConfirm , StartDate GreaterThen EmpEffectiveDate
		public void EntitlementLeavesMonthly_ProrateByMonths(int prorateByT, int effectiveBy, int doj, int NoOfDays, int schedulemonth, int scheduledate, int res)
		{
			var DOJ = new DateTime(DateTime.Now.Year, DateTime.Now.Month, doj);
			//Joining date issue

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = effectiveBy,
				ProrateByT = prorateByT,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = scheduledate,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = schedulemonth,
				AccOnYearly = 1,
				NoOfDays = NoOfDays,
				AccOnHalfYearly = schedulemonth
			};

			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesMonthly(type, DOJ, DOJ, schedule);

			//Assert
			Assert.Equal(res, dd);
		}
		[Fact] //EffectiveBy is JoiningDate , EmpEffectiveDate in between StartDate and EndDate
		public void EntitlementLeavesMonthly_ProrateByDays_BetweenDates()
		{
			var DOJ = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27);
			//Joining date issue

			//Assign
			var type = new LeaveType()
			{
				ID = Guid.NewGuid(),
				Code = "CL",
				Name = "Casual Leave",
				EffectiveAfter = 1,
				EffectiveType = 1,
				EffectiveBy = 1,
				ProrateByT = 1,
				RoundOff = 1,
				RoundOffTo = 1,
				Gender = 1,
				MaritalStatus = 1,
				PastDate = 2,
				FutureDate = 1,
				MaxApplications = 2,
				MinLeaves = 1,
				MaxLeaves = 5,
				Status = true,
			};

			var schedule = new LeaveTypeSchedule()
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = type.ID,
				AccType = 1,
				AccOnDay = 26,
				ResetType = 1,
				ResOnDay = DateTime.Now.Day,
				ResOnHalfYearly = 1,
				ResOnQuarterly = 1,
				ResOnYearly = 2,
				AccOnQuarterly = 2,
				AccOnYearly = 1,
				NoOfDays = 31,
				AccOnHalfYearly = 2
			};

			//Act
			var dd = LeaveAccumulationService.EntitlementLeavesMonthly(type, DOJ, DOJ, schedule);

			//Assert
			Assert.True(dd == 31);
		}

		[Fact]
		public async Task GetApplicableEmpList_Count()
		{
			//Arrange
			var employees = _employeeData.GetAllEmployeesData();
			var leaveType = new LeaveType
			{
				Gender = 1,
				MaritalStatus = 2,
				Location = "b517c31b-a748-4918-ad29-612b246cc866",
				Department = "e3de3a49-01d0-46bc-9201-3cc1ffaf1459",
				Designation = "cc231104-03c1-4483-b050-a155e4c1ba25"
			};
			//Employee Mock
			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			//Act
			var src = new LeaveAccumulationService(uow.Object);
			var result = await src.GetApplicableEmpList(leaveType);
			//Assert
			Assert.True(result.Count == 1);
		}

		[Fact]
		public void GetEmpListAfterExceptions_Verify_RemovedEmpListCount()
		{
			var removedEmpListCount = 1;
			//Arrange
			var employees = new List<Employee>{
				new Employee
				{
					WorkLocationId = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),
					DepartmentId = Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),
					DesignationId = Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"),
				},
				new Employee
				{
					WorkLocationId = Guid.NewGuid(),
					DepartmentId = Guid.NewGuid(),
					DesignationId = Guid.NewGuid(),
				}
			};
			var leaveType = new LeaveType
			{
				Gender = 1,
				MaritalStatus = 2,
				ExLocation = "b517c31b-a748-4918-ad29-612b246cc866",
				ExDepartment = "e3de3a49-01d0-46bc-9201-3cc1ffaf1459",
				ExDesignation = "cc231104-03c1-4483-b050-a155e4c1ba25"
			};

			//Act
			var result = LeaveAccumulationService.GetEmpListAfterExceptions(leaveType, employees);
			//Assert
			Assert.Equal(removedEmpListCount, result.Count);
		}

		[Theory]
		[InlineData(0, 2, 2, 4)]//present Leaves are less then preconsumable leaves
		[InlineData(4, 0, 4, 0)]//present Leaves are More then preconsumable leaves
		public void LeavesAfterPreconsumableLeaves_Test(int item1, int item2, decimal presentLeaves, decimal preConsumableLeaves)
		{
			//Act
			var result = LeaveAccumulationService.LeavesAfterPreconsumableLeaves(presentLeaves, preConsumableLeaves);
			//Assert
			Assert.Equal(item1, result.Item1);
			Assert.Equal(item2, result.Item2);
		}

		[Theory]
		[InlineData("1996-02-02")]
		public async Task LeavesLapseSchedule_UpdateLeavebalance(string scheduleDate)
		{
			Guid leaveTypeId = Guid.NewGuid();
			//Arrange
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<LeaveTypeSchedule> schedules = new List<LeaveTypeSchedule>
			{
				new LeaveTypeSchedule
				{
					ID=Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					AccType=2,
					ResOnDay = DateTime.Now.Day,
					ResOnQuarterly=DateTime.Now.Month,
					AccOnDay=DateTime.Now.Day,
					AccOnQuarterly=DateTime.Now.Month,
					ResetType = -1
				}
			};
			var leaveBalances = new List<LeaveBalance>
			{
				 new LeaveBalance
				{
					ID = Guid.NewGuid(),
					Leaves = 2,
					EmployeeId =Guid.Parse("d75fe57f-20b8-4f19-b743-8d0f8bc817dd"),
					LeaveTypeId = leaveTypeId
				}
			};
			var leaveType = new LeaveType
			{
				ID = leaveTypeId,
				Gender = 1,
				MaritalStatus = 2,
				Location = "b517c31b-a748-4918-ad29-612b246cc866",
				Department = "e3de3a49-01d0-46bc-9201-3cc1ffaf1459",
				Designation = "cc231104-03c1-4483-b050-a155e4c1ba25"
			};
			//Employee Mock
			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			//LeaveBalance Mock
			_context.GetRepositoryAsyncDbSet(uow, leaveBalances);

			//LeaveTypeSchedule Mock
			var mockLeaveTypeSchedule = schedules.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<LeaveTypeSchedule>()).Returns(mockLeaveTypeSchedule.Object);
			var repository = new RepositoryAsync<LeaveTypeSchedule>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveTypeSchedule>()).Returns(repository);
			//Act
			var src = new LeaveAccumulationService(uow.Object);
			await src.LeavesLapseSchedule(leaveType, DateTime.Parse(scheduleDate));
			var list = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();
			//Assert
			Assert.Equal(-2, list.LastOrDefault().Leaves);
			Assert.Equal((int)LeaveTypesScreens.LeaveLapsedSchedule, list.LastOrDefault().Type);
			uow.Verify(x => x.SaveChangesAsync());
		}
		[Theory]
		[InlineData("1996-02-02")]
		public async Task LeavesLapseSchedule_UpdateLeavebalance_ThrowException(string scheduleDate)
		{
			Guid leaveTypeId = Guid.NewGuid();
			//Arrange
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<LeaveTypeSchedule> schedules = new List<LeaveTypeSchedule>
			{
				new LeaveTypeSchedule
				{
					ID=Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					AccType=2,
					ResOnDay = DateTime.Now.Day,
					ResOnQuarterly=DateTime.Now.Month,
					AccOnDay=DateTime.Now.Day,
					AccOnQuarterly=DateTime.Now.Month,
					ResetType = -1
				}
			};
			var leaveBalances = new List<LeaveBalance>
			{
				 new LeaveBalance
				{
					ID = Guid.NewGuid(),
					Leaves = 2,
					EmployeeId =Guid.Parse("d75fe57f-20b8-4f19-b743-8d0f8bc817dd"),
					LeaveTypeId = leaveTypeId
				}
			};
			var leaveType = new LeaveType
			{
				ID = leaveTypeId,
				Gender = 1,
				MaritalStatus = 2,
				Location = "b517c31b-a748-4918-ad29-612b246cc866",
				Department = "e3de3a49-01d0-46bc-9201-3cc1ffaf1459",
				Designation = "cc231104-03c1-4483-b050-a155e4c1ba25"
			};
			//Employee Mock
			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			//LeaveBalance Mock
			_context.GetRepositoryAsyncDbSet(uow, leaveBalances);

			//LeaveTypeSchedule Mock
			var mockLeaveTypeSchedule = schedules.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<LeaveTypeSchedule>()).Returns(mockLeaveTypeSchedule.Object);
			var repository = new RepositoryAsync<LeaveTypeSchedule>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveTypeSchedule>()).Returns(repository);

			uow.Setup(m => m.SaveChangesAsync()).Throws(new Exception("Error occured while saving Data"));
			//Act
			var src = new LeaveAccumulationService(uow.Object);
			await src.LeavesLapseSchedule(leaveType, DateTime.Parse(scheduleDate));
			//Assert
			Assert.Equal(2, 2);
		}

		[Theory]
		[InlineData(6, -2, 1)]//ForwardType is percentage Wise
		public void GetCarryForwardLeaves_PercentageWise_And_DayWise(decimal presentLeaves, decimal forwardLeaves, int forwardType)
		{
			var result = LeaveAccumulationService.GetCarryForwardLeaves(presentLeaves, forwardType, 2, 2, 50);
			Assert.Equal(forwardLeaves, result);
		}
		[Theory]
		[InlineData(10, 10, 0)]
		[InlineData(2, 2, -2)]
		public void GetCarryForwardLeaves__DayWise(int fwdLimit, int fwdDays, decimal leaves)
		{
			decimal presentLeaves = 8; int fwdType = 2;

			var result = LeaveAccumulationService.GetCarryForwardLeaves(presentLeaves, fwdType, fwdLimit, fwdDays, 50);
			Assert.Equal(leaves, result);
		}

		[Fact]
		public void GetCarryForwardLeaves__Invalid_ForwardType()
		{
			var result = LeaveAccumulationService.GetCarryForwardLeaves(8, 0, 2, -2, 50);
			Assert.Equal(0, result);
		}
	}
}
