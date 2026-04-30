using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class LeaveTypeServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILeaveTypeService _service;
		private readonly Mock<DbContext> _context;

		public LeaveTypeServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LeaveTypeService(uow.Object);
			_context = new Mock<DbContext>();
		}
		
		private void LeaveTypeMockData()
		{
			#region Arrange
			IEnumerable<LeaveType> data = new List<LeaveType>
			{
				new LeaveType
				{
					ID = Guid.NewGuid(),
					Name="Casual Leave",
					Code="CL",
					Status = true,
					DefaultPayoff = true,
				},
				new LeaveType
				{
					ID = Guid.NewGuid(),
					Name="Earned Leave",
					Code="EL",
					Status = true,
					DefaultPayoff = false,
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSet);
		}

		[Fact]
		public async Task CustomValidation_DataSaved_WithOutException()
		{
			#region Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();

			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Team Leave",
				Code = "TL",
				MaxLeaves = 5,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr. SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true,
				DefaultPayoff = false,
			};

			#endregion

			//Act
			var service = new LeaveTypeService(uow.Object);
			await service.CustomValidation(leave, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]	
		public async Task CustomValidation_MaxLeavesGreaterThanOrEqualToMinLeaves_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();
			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name= "Team Leave",
				Code = "TL",
				MaxLeaves=5,
				MinLeaves=6,
				Department="IT Development",
				Designation="Jr.SoftwareDeveloper",
				Location= "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true,
				DefaultPayoff = false,
			};

			//Act
			var service = new LeaveTypeService(uow.Object);
			await service.CustomValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_NameAlreadyExists_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();

			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Casual Leave",
				Code = "Casual Leave",
				MaxLeaves = 6,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr.SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true,
				DefaultPayoff = false,
			};

			//Act
			var service = new LeaveTypeService(uow.Object);
			await service.CustomValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_DefaultPayOffLeaveTypeIsFalse_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();

			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Leave",
				Code = "Leave",
				MaxLeaves = 6,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr.SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true,
				DefaultPayoff = true
			};

			//Act
			var service = new LeaveTypeService(uow.Object);
			await service.CustomValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void DeptAndDesignValidation_DepartmentAndExDepartmentIsSame_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();

			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Team Leave",
				Code = "TL",
				MaxLeaves = 5,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr.SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "IT Development",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true
			};

			//Act
			_service.DeptAndDesignValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void DeptAndDesignValidation_DesignationAndExDesignationIsSame_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();
			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Team Leave",
				Code = "TL",
				MaxLeaves = 5,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr.SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.SoftwareDeveloper",
				ExLocation = "Tarnaka",
				Status = true
			};

			//Act
			_service.DeptAndDesignValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void DeptAndDesignValidation_LocationAndExLocationIsSame_ThrowException()
		{
			// Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();
			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Team Leave",
				Code = "TL",
				MaxLeaves = 5,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr.SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Jubilee Hills",
				Status = true
			};

			//Act
			_service.DeptAndDesignValidation(leave, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void DeptAndDesignValidation_DataSaved_WithOutException()
		{
			#region Arrange
			LeaveTypeMockData();
			Result<LeaveType> result = new();
			LeaveType leave = new()
			{
				ID = Guid.NewGuid(),
				Name = "Team Leave",
				Code = "TL",
				MaxLeaves = 5,
				MinLeaves = 2,
				Department = "IT Development",
				Designation = "Jr. SoftwareDeveloper",
				Location = "Jubilee Hills",
				ExDepartment = "Training",
				ExDesignation = "Jr.Dot Net Developer",
				ExLocation = "Tarnaka",
				Status = true
			};

			#endregion

			//Act
			_service.DeptAndDesignValidation(leave, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task GetDefaultPayOffLeaveType_DefaultPayOffLeaveTypeIsTrue_GetValidRecords()
		{
			// Arrange
			LeaveTypeMockData();

			//Act
			var service = new LeaveTypeService(uow.Object);
			var DefaultPayOff = await service.GetDefaultPayOffLeaveType();

			//Assert
			Assert.True(DefaultPayOff.DefaultPayoff);
		}

		[Fact]
		public async Task GetPaidLeaveTypeList_DefaultPayOffLeaveTypeIsFalse_GetValidRecords()
		{
			#region Arrange
			IEnumerable<LeaveType> data = new List<LeaveType>
			{
				new LeaveType
				{
					ID = Guid.NewGuid(),
					Name="Casual Leave",
					Code="CL",
					Status = true,
					DefaultPayoff = true,
				},
				new LeaveType
				{
					ID = Guid.NewGuid(),
					Name="Earned Leave",
					Code="EL",
					Status = true,
					DefaultPayoff = false,
				},
				new LeaveType
				{
					ID = Guid.NewGuid(),
					Name="Earned Leave",
					Code="EL",
					Status = true,
					DefaultPayoff = false,
				}


			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSet);

			//Act
			var service = new LeaveTypeService(uow.Object);
			var DefaultPayOff = await service.GetPaidLeaveTypeList();

			//Assert
			Assert.Equal(2, DefaultPayOff.Count());
		}
	}
}
