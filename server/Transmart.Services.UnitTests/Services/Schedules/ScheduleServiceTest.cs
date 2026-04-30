using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Leave;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Schedules
{
	public class ScheduleServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IShiftService _shiftService;
		private readonly IScheduleService _scheduleService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		public ScheduleServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_shiftService = new ShiftService(uow.Object);
			_scheduleService = new ScheduleService(uow.Object, _shiftService);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();


		}

		[Fact]
		public async Task GetSchedules_ScheduleWithAllocation_ReturnValidShift()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			Guid shiftId = Guid.NewGuid();

			var employees = new List<Employee> { employee };

			IEnumerable<Shift> shift = new List<Shift>
			{
				new Shift
				{
					ID = shiftId,
					Name="Day Shift",
					Desciption="GGG",
					BreakTime=1,
					NoOfBreaks=2,
					StartFrom=9,
					EndsOn=18,
					loginGraceTime= 9
				}
			};

			IEnumerable<Allocation> allocation = new List<Allocation>
			{
				new Allocation
				{
					ID = Guid.NewGuid(),
					EmployeeId=employee.ID,
					Employee=employee,
					ShiftId=shiftId,
				}
			};

			// Mock
			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			var mockSetShift = shift.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSetShift);

			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployees(uow, _context, mockSetEmployee);

			//Act
			var schedule = await _scheduleService.GetSchedules(employee.WorkLocationId);
			var scheduleSelected = schedule.FirstOrDefault(x => x.EmployeeID == employee.ID);

			//Assert
			Assert.Equal(shift.FirstOrDefault().StartFrom, scheduleSelected.StartAt);
		}

		[Fact]
		public async Task GetSchedules_ScheduleWithTeam_ReturnValidShift()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			 
			var employees = new List<Employee> { employee };

			IEnumerable<Shift> shift = new List<Shift>
			{
				new Shift
				{
					ID = employee.Team.Shift.ID,
					Name="Day Shift",
					Desciption="GGG",
					BreakTime=1,
					NoOfBreaks=2,
					StartFrom=9,
					EndsOn=18,
					loginGraceTime= 9
				} 
			};

			// Mock  
			var mockSetShift = shift.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSetShift);

			var mockSetAllocation = new List<Allocation>().AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployees(uow, _context, mockSetEmployee);

			//Act
			var schedule = await _scheduleService.GetSchedules(employee.WorkLocationId);
			var scheduleSelected = schedule.FirstOrDefault(x => x.EmployeeID == employee.ID);

			//Assert
			Assert.Equal(shift.FirstOrDefault().StartFrom, scheduleSelected.StartAt);
		}

		[Fact]
		public async Task GetSchedules_ScheduleWithDesignation_ReturnValidShift()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team = null;
			employee.TeamId = Guid.NewGuid();
			var employees = new List<Employee> { employee };

			IEnumerable<Shift> shift = new List<Shift>
			{
				new Shift
				{
					ID = employee.Department.Shift.ID,
					Name="Day Shift",
					Desciption="GGG",
					BreakTime=1,
					NoOfBreaks=2,
					StartFrom=9,
					EndsOn=18,
					loginGraceTime= 9
				}
			}; 

			// Mock
			var mockSetAllocation = new List<Allocation>().AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			var mockSetShift = shift.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSetShift);

			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployees(uow, _context, mockSetEmployee);

			//Act
			var schedule = await _scheduleService.GetSchedules(employee.WorkLocationId);
			var scheduleSelected = schedule.FirstOrDefault(x => x.EmployeeID == employee.ID);

			//Assert
			Assert.Equal(shift.FirstOrDefault().StartFrom, scheduleSelected.StartAt);
		}

		[Fact]
		public async Task GetEmployeeSchedule_ScheduleWithShift_ReturnSchedule()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team = null;
			employee.TeamId = Guid.NewGuid(); 
			IEnumerable<Shift> shift = new List<Shift>
			{
				new Shift
				{
					ID = employee.Department.Shift.ID,
					Name="Day Shift",
					Desciption="GGG",
					BreakTime=1,
					NoOfBreaks=2,
					StartFrom=9,
					EndsOn=18,
					loginGraceTime= 9
				},
				
			};

			IEnumerable<Allocation> allocation = new List<Allocation>
			{
				new Allocation
				{
					ID = Guid.NewGuid(),
					EmployeeId=employee.ID,
					Employee=employee
				},
				new Allocation
				{
					ID = Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					Employee=employee
				}
			};

			// Mock
			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			var mockSetShift = shift.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSetShift);

			//Act
			var schedule = await _scheduleService.GetEmployeeSchedule(employee);

			//Assert
			Assert.NotNull(schedule);
			Assert.Equal(employee.ID, schedule.EmployeeID);
		}
	}
}

