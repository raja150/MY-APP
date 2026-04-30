using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class OldRegimeSlabServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IOldRegimeSlabService _oldRegimeSlabService;
		private readonly Mock<DbContext> _context;

		public OldRegimeSlabServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_oldRegimeSlabService = new OldRegimeSlabService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task CustomValidation_IncomeToLessThanIncomeFrom_ThrowError()
		{
			var oldRegimeSlabs = new List<OldRegimeSlab>
			{
				new OldRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new OldRegimeSlab
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					IncomeFrom=120,
					IncomeTo=120
				}
			};

			//Mock
			var mockSet = oldRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<OldRegimeSlab>();
			await _oldRegimeSlabService.CustomValidation(new OldRegimeSlab
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5127-08DA64AB8559"),
				IncomeFrom = 70,
				IncomeTo = 20
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_SlabExists_ThrowError()
		{
			var oldRegimeSlabs = new List<OldRegimeSlab>
			{
				new OldRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new OldRegimeSlab
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					IncomeFrom=120,
					IncomeTo=120
				}
			};

			//Mock
			var mockSet = oldRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<OldRegimeSlab>();
			await _oldRegimeSlabService.CustomValidation(new OldRegimeSlab
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5127-08DA64AB8559"),
				IncomeFrom = 100,
				IncomeTo = 200
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);

		}

		[Fact]
		public async Task CustomValidation_NoError_IsSuccess()
		{
			var oldRegimeSlabs = new List<OldRegimeSlab>
			{
				new OldRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new OldRegimeSlab
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					IncomeFrom=120,
					IncomeTo=120
				}
			};

			//Mock
			var mockSet = oldRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<OldRegimeSlab>();
			await _oldRegimeSlabService.CustomValidation(new OldRegimeSlab
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5127-08DA64AB8559"),
				IncomeFrom = 150,
				IncomeTo = 180
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}
	}
}
