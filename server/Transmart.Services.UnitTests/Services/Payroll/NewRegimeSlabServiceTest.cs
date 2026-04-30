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
	public class NewRegimeSlabServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly INewRegimeSlabService _newRegimeSlabService;
		private readonly Mock<DbContext> _context;

		public NewRegimeSlabServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_newRegimeSlabService = new NewRegimeSlabService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task CustomValidation_IncomeToLessThanIncomeFrom_ThrowError()
		{
			var newRegimeSlabId = Guid.NewGuid();
			#region Arrange

			var newRegimeSlabs = new List<NewRegimeSlab>
			{
				new NewRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new NewRegimeSlab
				{
					ID=newRegimeSlabId,
					IncomeFrom=120,
					IncomeTo=120
				}
			};
			#endregion

			// Mock
			var mockSet = newRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<NewRegimeSlab>();
			await _newRegimeSlabService.CustomValidation(
				new NewRegimeSlab
				{
					ID = newRegimeSlabId,
					IncomeFrom = 120,
					IncomeTo = 100
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_SlabExists_ThrowError()
		{
			var newRegimeSlabId = Guid.NewGuid();
			#region Arrange

			var newRegimeSlabs = new List<NewRegimeSlab>
			{
				new NewRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new NewRegimeSlab
				{
					ID=newRegimeSlabId,
					IncomeFrom=120,
					IncomeTo=120
				}
			};
			#endregion

			// Mock
			var mockSet = newRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<NewRegimeSlab>();
			await _newRegimeSlabService.CustomValidation(
				new NewRegimeSlab
				{
					ID = newRegimeSlabId,
					IncomeFrom = 100,
					IncomeTo = 200
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_NoError_IsSuccess()
		{
			var newRegimeSlabId = Guid.NewGuid();
			#region Arrange

			var newRegimeSlabs = new List<NewRegimeSlab>
			{
				new NewRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new NewRegimeSlab
				{
					ID=newRegimeSlabId,
					IncomeFrom=120,
					IncomeTo=120
				}
			};
			#endregion

			// Mock
			var mockSet = newRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, mockSet);

			//Actt
			var excutionResult = new Result<NewRegimeSlab>();
			await _newRegimeSlabService.CustomValidation(
				new NewRegimeSlab
				{
					ID = newRegimeSlabId,
					IncomeFrom = 150,
					IncomeTo = 180
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}
	}
}
