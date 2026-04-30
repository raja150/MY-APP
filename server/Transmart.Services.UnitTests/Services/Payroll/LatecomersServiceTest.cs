using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class LatecomersServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILatecomersService _service;
		private readonly Mock<DbContext> _context;
		public LatecomersServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LatecomersService(uow.Object);
			_context = new Mock<DbContext>();
		}
		[Fact]
		public async Task AddBulk_AddUpdateAndDeleteData_IsSuccess()
		{
			var existingId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			#region Arrange
			var latecomers = new List<Latecomers>()
			{
				new Latecomers
				{
					ID=existingId,
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=6,
					Year=1996,
					NumberOfDays=2.5m
				},
				new Latecomers
				{
					ID = deleteId,
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					NumberOfDays=4.5m
				},
				new Latecomers
				{
					ID = Guid.NewGuid(),
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=4,
					Year=1996,
					NumberOfDays=6m
				},
			};
			var latecomersList = new List<Latecomers>()
			{
				new Latecomers
				{
					ID=existingId,
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=6,
					Year=1996,
					NumberOfDays=5.5m
				},
				new Latecomers
				{
					ID=newId,
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					NumberOfDays=8m
				}
			};
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, latecomers);

			// Asset
			var dd = await _service.AddBulk(latecomersList, 5, 1996);

			var list = await uow.Object.GetRepositoryAsync<Latecomers>().GetAsync();
			var updateItem = list.FirstOrDefault(x => x.ID == existingId);
			//Act
			Assert.True(dd.HasNoError);
			Assert.Equal(1, list.Count(x => x.ID == newId));//add
			Assert.Equal(5.5m, updateItem.NumberOfDays);//Update
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//Delete
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task Get_LatecomersWithId()
		{
			var latecomerId = Guid.NewGuid();
			//Arrange
			var latecomers = new List<Latecomers>()
			{
				new Latecomers
				{
					ID=latecomerId,
					EmployeeID=Guid.NewGuid(),
					Month=DateTime.Now.Month,
					Year=DateTime.Now.Year,
					NumberOfDays=2,
				}
			};

			//Mock
			var mockSet = latecomers.AsQueryable().BuildMockDbSet();
			SetData.MockLatecomers(uow, _context, mockSet);
			//Act
			var result = await _service.GetById(latecomerId);

			//Assert
			Assert.Equal(latecomerId, result.ID);
		}

		[Fact]
		public async Task GetPaginate_PaginateList_GetData()
		{
			//Arrange
			var latecomers = new List<Latecomers>
			{
				new Latecomers
				{
					ID=Guid.NewGuid(),
					Month=DateTime.Now.Month,
					Year=DateTime.Now.Year,
					EmployeeID=Guid.NewGuid(),
					NumberOfDays=2,
				},
				new Latecomers
				{
					ID=Guid.NewGuid()
				}
			};

			//Mock
			var mockSet = latecomers.AsQueryable().BuildMockDbSet();
			SetData.MockLatecomers(uow, _context, mockSet);

			//Act
			var dd = await _service.GetPaginate(new TranSmart.Domain.Models.BaseSearch { });

			//Assert
			Assert.Equal(2, dd.Count);
		}

		[Fact]
		public async Task UpdateAsync_InvalidLatecomers_ThrowError()
		{
			#region Arrange
			var latecomers = new List<Latecomers>()
			{
				new Latecomers
				{
					ID=Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559"),
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
				},
				new Latecomers
				{
					 ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					EmployeeID=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					Month=5,
					Year=1996,
				}
			};
			var latecomer = new Latecomers()
			{
				ID = Guid.Parse("6D72C8BB-4B04-4C34-5157-08DA64AB8559"),
				EmployeeID = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
				Month = 7,
				Year = 1997,
			};

			#endregion

			//Mock
			var mockSet = latecomers.AsQueryable().BuildMockDbSet();
			SetData.MockLatecomers(uow, _context, mockSet);

			// Asset
			var dd = await _service.UpdateAsync(latecomer);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateAsync_Validlatecomer_SaveData()
		{
			var employeeId = Guid.NewGuid();
			var latecomerId = Guid.NewGuid();
			#region Arrange
			var latecomers = new List<Latecomers>()
			{
				new Latecomers
				{
					ID=latecomerId,
					EmployeeID=Guid.NewGuid(),
					Month=5,
					Year=1996,
					NumberOfDays = 2.5m
				},
				new Latecomers
				{
					ID=Guid.NewGuid(),
					EmployeeID=Guid.NewGuid(),
					Month=6,
					Year=1996,
					NumberOfDays = 4.5m
				}
			};
			Latecomers latecomer = new Latecomers()
			{
				ID = latecomerId,
				EmployeeID = employeeId,
				Month = 7,
				Year = 1997,
				NumberOfDays = 6
			};

			#endregion

			//Mock
			var mockSet = latecomers.AsQueryable().BuildMockDbSet();
			SetData.MockLatecomers(uow, _context, mockSet);

			// Asset
			await _service.UpdateAsync(latecomer);
			var list = await uow.Object.GetRepositoryAsync<Latecomers>().GetAsync();
			var updateItem = list.FirstOrDefault(x => x.ID == latecomer.ID);
			//Assert
			uow.Verify(x => x.SaveChangesAsync());
			Assert.Equal(latecomer.EmployeeID, updateItem.EmployeeID);
			Assert.Equal(latecomer.NumberOfDays, updateItem.NumberOfDays);
		}
	}
}
