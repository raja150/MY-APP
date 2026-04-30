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
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class ShiftServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IShiftService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		public ShiftServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new ShiftService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}
		
		[Fact]
		public async Task GetEmployeShift_NoShift_ReturnNullTest()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			IEnumerable<Shift> data = new List<Shift>
			{
				new Shift
				{
					ID = Guid.NewGuid(),
					Name="Day Shift",
					Desciption="GGG"
				},
				new Shift
				{
					ID = Guid.NewGuid(),
					Name="Nidht Shift",
					Desciption="GGG"
				}

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
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSet);

			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			//Act
			var shift = await _service.GetEmployeShift(employee);

			//Assert
			Assert.Null(shift);
		}

		[Fact]
		public async Task GetEmployeShift_ShiftWithTeam_ReturnValidShift()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			IEnumerable<Shift> data = new List<Shift>
			{
				new Shift
				{
					ID = employee.Team.Shift.ID,
					Name="Day Shift",
					Desciption="GGG"
				},
				new Shift
				{
					ID = Guid.NewGuid(),
					Name="Nidht Shift",
					Desciption="GGG"
				}

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
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSet);

			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			//Act
			var shift = await _service.GetEmployeShift(employee);

			//Assert
			Assert.Equal(shift.ID, data.FirstOrDefault().ID);
		}

		[Fact]
		public async Task GetEmployeShift_ShiftWithDesignation_WithoutException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.ShiftId = null;
			IEnumerable<Shift> data = new List<Shift>
			{
				new Shift
				{
					ID = employee.Designation.Shift.ID,
					Name="Day Shift",
					Desciption="GGG"
				},
				new Shift
				{
					ID = Guid.NewGuid(),
					Name="Nidht Shift",
					Desciption="GGG"
				}

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
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSet);

			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			//Act
			var shift = await _service.GetEmployeShift(employee);

			//Assert
			Assert.NotNull(shift);
			Assert.Equal(shift.ID, data.FirstOrDefault().ID);
		}

		[Fact]
		public async Task GetEmployeShift_ShiftWithDepartment_WithoutException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.ShiftId = null;
			employee.Designation.ShiftId = null;
			IEnumerable<Shift> data = new List<Shift>
			{
				new Shift
				{
					ID = employee.Department.Shift.ID,
					Name="Day Shift",
					Desciption="GGG"
				},
				new Shift
				{
					ID = Guid.NewGuid(),
					Name="Nidht Shift",
					Desciption="GGG"
				}

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
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSet);

			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			//Act
			var shift = await _service.GetEmployeShift(employee);

			//Assert
			Assert.NotNull(shift);
			Assert.Equal(shift.ID, data.FirstOrDefault().ID);
		}

		[Fact]
		public async Task GetEmployeShift_ShiftWithAllocation_WithoutException()
		{
			//Arrange
			Guid shiftId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			employee.Team.ShiftId = null;
			employee.Designation.ShiftId = null;
			employee.Department.ShiftId = null;

			IEnumerable<Shift> data = new List<Shift>
			{
				new Shift
				{
					ID = shiftId,
					Name="Day Shift",
					Desciption="GGG"
				},
				new Shift
				{
					ID = shiftId,
					Name="Nidht Shift",
					Desciption="GGG"
				}

			};

			IEnumerable<Allocation> allocation = new List<Allocation>
			{
				new Allocation
				{
					ID = shiftId,
					EmployeeId=employee.ID,
					Employee=employee,
					ShiftId=shiftId
				},
				new Allocation
				{
					ID = shiftId,
					EmployeeId=Guid.NewGuid(),
					Employee=employee,
					ShiftId=shiftId
				}

			};

			// Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockShift(uow, _context, mockSet);

			var mockSetAllocation = allocation.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockSetAllocation);

			//Act
			var shift = await _service.GetEmployeShift(employee);

			//Assert
			Assert.NotNull(shift);
			Assert.Equal(shift.ID, data.FirstOrDefault().ID);
		}
	}
}
