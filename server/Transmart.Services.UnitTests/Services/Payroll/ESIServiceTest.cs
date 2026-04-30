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
	public class ESIServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IESIService _esiService;
		private readonly Mock<DbContext> _context;
		public ESIServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_esiService = new ESIService(uow.Object);
			_context = new Mock<DbContext>();

		}

		[Fact]
		public async Task CustomValidation_PaySettingsExists_ThrowError()
		{
			var paySettingsId = Guid.NewGuid();
			//Arrange
			var esiList = new List<ESI>()
			{
				new ESI
				{
					ID=Guid.NewGuid(),
					PaySettingsId=paySettingsId
				}
			};

			// Mock
			var mockSet = esiList.AsQueryable().BuildMockDbSet();
			SetData.MockESI(uow, _context, mockSet);

			//Asset
			var excutionResult = new Result<ESI>();
			await _esiService.CustomValidation(new ESI
			{
				ID = Guid.NewGuid(),
				PaySettingsId = paySettingsId
			}, excutionResult);

			//Act
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_PaySettingsNotExists_IsSuccess()
		{
			//Arrange
			var esiList = new List<ESI>()
			{
				new ESI
				{
					ID=Guid.NewGuid(),
					PaySettingsId=Guid.NewGuid()
				}
			};

			// Mock
			var mockSet = esiList.AsQueryable().BuildMockDbSet();
			SetData.MockESI(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<ESI>();
			await _esiService.CustomValidation(new ESI
			{
				ID = Guid.NewGuid(),
				PaySettingsId = Guid.NewGuid()
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}
	}
}
