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
	public class PaySettingsServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IPaySettingsService _paySettingsService;
		private readonly Mock<DbContext> _context;

		public PaySettingsServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_paySettingsService = new PaySettingsService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task CustomValidation_OganizationExists_ThrowError()
		{
			var organizationId = Guid.NewGuid();
			#region Arrange

			var paySettings = new List<PaySettings>
			{
				new PaySettings
				{
					ID=Guid.NewGuid(),
					OrganizationId=organizationId,
				},
				new PaySettings
				{
					ID=Guid.NewGuid(),
					OrganizationId=Guid.NewGuid(),
				}
			};

			#endregion

			//Mock
			var mockSet = paySettings.AsQueryable().BuildMockDbSet();
			SetData.MockPaySettings(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<PaySettings>();
			await _paySettingsService.CustomValidation(new PaySettings
			{
				ID = Guid.NewGuid(),
				OrganizationId = organizationId
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_NoError_IsSuccess()
		{
			//Arrange
			var paySettings = new List<PaySettings>
			{
				new PaySettings
				{
					ID=Guid.NewGuid(),
					OrganizationId=Guid.NewGuid(),
				}
			};
			//Mock
			var mockSet = paySettings.AsQueryable().BuildMockDbSet();
			SetData.MockPaySettings(uow, _context, mockSet);

			//Act
			var excutionResult = new Result<PaySettings>();
			await _paySettingsService.CustomValidation(new PaySettings
			{
				ID = Guid.NewGuid(),
				OrganizationId = Guid.NewGuid()
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}
	}
}
