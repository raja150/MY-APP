using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class ArrearsServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IArrearService _arrearService;
		private readonly Mock<DbContext> _context;
		public ArrearsServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_arrearService = new ArrearService(uow.Object);
			_context = new Mock<DbContext>();
		}
		[Fact]
		public async Task AddBulk_AddUpdateAndDeleteData_IsSuccess()
		{
			var existingId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			#region Arrange
			var arrears = new List<Arrear>()
			{
				new Arrear
				{
					ID=existingId,
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
					Pay=20
				},
				new Arrear
				{
					ID = deleteId,
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
					Pay=10
				},
				new Arrear
				{
					ID = Guid.NewGuid(),
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					Pay=10
				},
			};
			var arrearsList = new List<Arrear>()
			{
				new Arrear
				{
					ID=existingId,
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
					Pay=50
				},
				new Arrear
				{
					ID=newId,
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					Pay=50
				}
			};
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, arrears);

			// Asset
			var dd = await _arrearService.AddBulk(arrearsList, 5, 1996);

			var list = await uow.Object.GetRepositoryAsync<Arrear>().GetAsync();
			var updateItem = list.FirstOrDefault(x => x.ID == existingId);
			//Act
			Assert.True(dd.HasNoError);
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//Delete
			Assert.Equal(50, updateItem.Pay);//Update
			Assert.Equal(1, list.Count(x => x.ID == newId));//add
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task Get_GetArrearWithId_ReturnsSingleArrear()
		{
			var arrearId = Guid.NewGuid();
			//Arrange
			var arrears = new List<Arrear>()
			{
				new Arrear
				{
					ID=arrearId,
					EmployeeID=Guid.NewGuid(),
					Month=DateTime.Now.Month,
					Year=DateTime.Now.Year,
					Pay=100
				}
			};

			//Mock
			var mockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, mockSet);
			//Act
			var dd = await _arrearService.GetById(arrearId);

			//Assert
			Assert.Equal(arrearId, dd.ID);
		}

		[Fact]
		public async Task GetPaginate_PaginateList_GetData()
		{
			//Arrange
			var arrears = new List<Arrear>
			{
				new Arrear
				{
					ID=Guid.NewGuid(),
					Month=DateTime.Now.Month,
					Year=DateTime.Now.Year,
					EmployeeID=Guid.NewGuid(),
					Pay=1200
				},
				new Arrear
				{
					ID=Guid.NewGuid()
				}
			};

			//Mock
			var mockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, mockSet);

			//Act
			var dd = await _arrearService.GetPaginate(new TranSmart.Domain.Models.BaseSearch { });

			//Assert
			Assert.Equal(2, dd.Count);
		}

		[Fact]
		public async Task UpdateAsync_InvalidArrear_ThrowError()
		{
			#region Arrange
			var arrears = new List<Arrear>()
			{
				new Arrear
				{
					ID=Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559"),
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
					Pay=20
				},
				new Arrear
				{
					 ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
					Pay=20
				}
			};
			var arrear = new Arrear()
			{
				ID = Guid.Parse("6D72C8BB-4B04-4C34-5157-08DA64AB8559"),
				EmployeeID = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
				Month = 7,
				Year = 1997,
				Pay = 140
			};

			#endregion

			//Mock
			var mockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, mockSet);

			// Asset
			var dd = await _arrearService.UpdateAsync(arrear);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateAsync_ValidArrear_SaveData()
		{
			var employeeId = Guid.NewGuid();
			var arrearId = Guid.NewGuid();
			#region Arrange
			var arrears = new List<Arrear>()
			{
				new Arrear
				{
					ID=arrearId,
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					Pay=20
				},
				new Arrear
				{
					ID=Guid.NewGuid(),
					EmployeeID=Guid.NewGuid(),
					Month=6,
					Year=1996,
					Pay=20
				}
			};
			Arrear arrear = new Arrear()
			{
				ID = arrearId,
				EmployeeID = employeeId,
				Month = 7,
				Year = 1997,
				Pay = 140
			};

			#endregion

			//Mock
			var mockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, mockSet);

			// Asset
			await _arrearService.UpdateAsync(arrear);
			var list = await uow.Object.GetRepositoryAsync<Arrear>().GetAsync();
			var updateItem = list.FirstOrDefault(x => x.ID == arrear.ID);
			//Assert
			uow.Verify(x => x.SaveChangesAsync());
			Assert.Equal(arrear.EmployeeID, updateItem.EmployeeID);
			Assert.Equal(arrear.Pay, updateItem.Pay);
		}
	}
}
