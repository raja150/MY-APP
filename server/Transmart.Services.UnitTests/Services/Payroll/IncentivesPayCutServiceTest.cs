using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollSetData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class IncentivesPayCutServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IIncentivesPayCutService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;

		public IncentivesPayCutServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new IncentivesPayCutService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		private void IncentivePayMockData()
		{
			var payCuts = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2021,
					Month=5,
				},
				  new IncentivesPayCut
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2022,
					Month=8,
				},
				  new IncentivesPayCut
				{
					ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8111"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2022,
					Month=1,
				}
			};
			var mockSet = payCuts.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, mockSet);
		}


		[Fact]
		public async Task Getmethod()
		{
			IncentivePayMockData();

			var id = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");

			//Act
			var src = await _service.GetById(id);

			//Assert
			Assert.Equal(id, src.ID);
		}

		[Fact]
		public async Task GetPaginate()
		{
			IncentivePayMockData();

			// Act
			var src = await _service.GetPaginate(new BaseSearch()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 1,
				RefId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
			});
			//Assert
			Assert.Equal(3, src.Count);
		}

		[Fact]
		public async Task UpdateAsync_ValidIncentivesPayCut_NoException()
		{
			var IncentivesPayCutId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562");

			IncentivePayMockData();
			//Act
			var incentive = await _service.UpdateAsync(new IncentivesPayCut()
			{
				ID = IncentivesPayCutId,
				EmployeeId = employeeId,
				FaxFilesAndArrears = 100,
				OtherInc =50,
				UnauthorizedLeaveDed =100,
				OtherDed =20
			});

			//Assert
			Assert.True(incentive.IsSuccess);
			//[Mohan] Verify EmployeeId, Incentives, PayCut
			Assert.Equal(employeeId, incentive.ReturnValue.EmployeeId);
			Assert.Equal(150, incentive.ReturnValue.Incentives);
			Assert.Equal(120, incentive.ReturnValue.PayCut);

		}

		[Fact]
		public async Task UpdateAsync_InValidIncentivesPayCut_ThrowException()
		{
			var IncentivesPayCutId = Guid.Parse("9D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562");

			IncentivePayMockData();
			//Act
			var src = await _service.UpdateAsync(new IncentivesPayCut()
			{
				ID = IncentivesPayCutId,
				EmployeeId = employeeId,
			});

			//Assert
			Assert.False(src.IsSuccess);
		}

		[Fact]
		public async Task AddBulk_AddUpdateAndDelete_IsSuccess()
		{
			var existingId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			var payCut = 1900;
			#region Arrange

			var employee = _employeeData.GetEmployeeData();
			var employeeList = _employeeData.GetAllEmployeesData();

			var incentivePayCuts = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=deleteId,
					Employee=employee,
					EmployeeId=employeeId,
					Year=2021,
					Month=11,
				},
				new IncentivesPayCut
				{
					ID=existingId,
					Year=2021,
					Month=5,
				}
			};
			var IncentiveList = new List<IncentivesPayCut>()
			{ 
				new IncentivesPayCut
				{
					ID=existingId,
					EmployeeId=employeeId,
					Year=2021,
					Month=11,
					UnauthorizedLeaveDed=1000,
					OtherDed=900,
				},
				 new IncentivesPayCut
				 {
					 ID = newId,
				 },
			};
			#endregion

			// Mock
			var mockSet = incentivePayCuts.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			//Act
			var dd = await _service.AddBulk(IncentiveList);
			var list = await uow.Object.GetRepositoryAsync<IncentivesPayCut>().GetAsync();
			var result1 = list.FirstOrDefault(x => x.EmployeeId == employeeId);

			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(1, list.Count(x => x.ID == deleteId));
			Assert.Equal(payCut, result1.PayCut);
			Assert.Equal(0, list.Count(x => x.ID == newId));
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task AddBulk_IsCatch_Exception()
		{
			var employee = _employeeData.GetEmployeeData();
			var employeeList = _employeeData.GetAllEmployeesData();

			var incentivesList = new List<IncentivesPayCut>()
			{
				  new IncentivesPayCut
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					Employee=employee
				}
			};
			var incentives = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=Guid.NewGuid(),
					Employee=employee
				}
			};
			// Mock

			var mockSet = incentivesList.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());
			
			//Act
			var PayCut = await _service.AddBulk(incentives);

			//Assert
			Assert.False(PayCut.IsSuccess);
		}

	}
}

