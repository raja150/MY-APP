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
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Service;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{
	public class IncomeTaxLimitServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IIncomeTaxLimitService _taxLimitService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;

		public IncomeTaxLimitServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object); 
			_taxLimitService = new IncomeTaxLimitService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		[Fact]
		public async Task AddBulk_AddUpdateAndDelete_IsSuccess()
		{
			var existingId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employeeList = _employeeData.GetAllEmployeesData();

			var incomeTaxLimits = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit
				{
					ID=deleteId,
					EmployeeId=employeeId,
					Year=2021,
					Month=11,
					Amount=1500
				},
				new IncomeTaxLimit
				{
					ID=existingId,
					Year=2022,
					Month=5,
					Amount=1400
				}
			};
			var IncomeTaxLimitList = new List<IncomeTaxLimit>()
			{
				 new IncomeTaxLimit
				 {
					 ID = Guid.NewGuid(),
					 EmployeeId=Guid.NewGuid(),
					 Amount= 2000
				 },
				new IncomeTaxLimit
				{
					ID=existingId,
					EmployeeId=employeeId,
					Year=2021,
					Month=11,
					Amount= 2000
				},
				new IncomeTaxLimit
				{
					ID = newId,
					EmployeeId = Guid.NewGuid()
				}
			};
			#endregion

			// Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, incomeTaxLimits);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			//Act
			var dd = await _taxLimitService.AddBulk(IncomeTaxLimitList, 11, 2022);
			var list = await uow.Object.GetRepositoryAsync<IncomeTaxLimit>().GetAsync();
			var result1 = list.FirstOrDefault(x => x.EmployeeId == employeeId);

			//Assert
			//[Mohan] check add and delete 
			Assert.True(dd.HasNoError);
			Assert.Equal(1, list.Count(x => x.ID == deleteId));
			Assert.Equal(0, list.Count(x => x.ID == newId));
			Assert.Equal(2, list.Count());
			Assert.Equal(2000, result1.Amount);
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task GetMethod()
		{
			// Arrange & Mock

			var taxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit
				{
					ID=Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2022,
					Month=5,
					Amount =200,
				},
				  new IncomeTaxLimit
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2021,
					Month=8,
					Amount =200
				},
			};

			var mockSet = taxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, mockSet);

			//Act
			var src = await _taxLimitService.GetById(Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"));

			//Assert
			Assert.Equal(Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"), src.ID);
		}

		[Fact]
		public async Task GetPaginate()
		{
			// Arrange & Mock
			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559")
			};

			IEnumerable<IncomeTaxLimit> taxLimitList = new List<IncomeTaxLimit>
			{
				new IncomeTaxLimit
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Year=2020,
					Month=7,
					Amount=110
				},
				new IncomeTaxLimit
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					Year=2021,
					Month=9,
					Amount=112
				}
			}.AsQueryable();

			var mockSet = taxLimitList.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, mockSet);

			// Act
			var src = await _taxLimitService.GetPaginate(baseSearch);

			//Assert
			Assert.Equal(2, src.Count);
		}

		[Fact]
		public async Task Update_If_DataExist()
		{
			//[Mohan] change function name
			var employeeId = Guid.NewGuid();
			//Arrange & Mock
			IEnumerable<IncomeTaxLimit> taxLimits = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit
				{ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"), EmployeeId = Guid.NewGuid(), Amount = 202},
			}.AsQueryable();

			var mockSet = taxLimits.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, mockSet);

			//Act
			var src = await _taxLimitService.UpdateAsync(new IncomeTaxLimit()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				EmployeeId = employeeId,
				Year = 2021,
				Month = 9,
				Amount = 365
			}
			);

			//Assert
			Assert.True(src.IsSuccess);
			Assert.Equal(employeeId, src.ReturnValue.EmployeeId);
			Assert.Equal(365, src.ReturnValue.Amount);
		}

		[Fact]
		public async Task Update_If_DataNotExist_Throw_Exception()
		{
			//Arrange & Mock
			IEnumerable<IncomeTaxLimit> taxLimits = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					EmployeeId=Guid.NewGuid(),
					Amount=202
				},
			}.AsQueryable();

			var mockSet = taxLimits.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, mockSet);

			//Act
			var src = await _taxLimitService.UpdateAsync(new IncomeTaxLimit()
			{
				ID = Guid.Parse("8E73C8BB-4B04-4C34-5157-08DA64AB8111"),
				EmployeeId = Guid.NewGuid(),
				Amount = 365
			}
			);

			//Assert
			Assert.False(src.IsSuccess);
		}

	}
}
