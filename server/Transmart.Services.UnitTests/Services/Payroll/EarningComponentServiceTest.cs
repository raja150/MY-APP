using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class EarningComponentServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IEarningComponentService _earningComponentService;
		private readonly Mock<DbContext> _context;

		public EarningComponentServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_earningComponentService = new EarningComponentService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task OnBeforeAdd_AddDeductionComponent_SaveData()
		{
			#region Arrange
			var earningComponents = new List<EarningComponent>
			{
				new EarningComponent
				{
				ID =Guid.NewGuid(),
				Name="Mahesh",
				EarningType=9,
				Status=true
				}
			};
			var deductionComponents = new List<DeductionComponent>
			{
				new DeductionComponent
				{
				ID =Guid.NewGuid(),
				Name="Mahesh",
				Status=true,
				IsEditable=true,
				ProrataBasis=true
				}
			};
			var earningComponent = new EarningComponent
			{
				ID = Guid.NewGuid(),
				EarningType = (int)TranSmart.Domain.Enums.EarningType.FoodCoupon,
				Name = "Vamshi",
				Status = false
			};
			#endregion

			#region Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, earningComponents);
			_ = _context.GetRepositoryAsyncDbSet(uow, deductionComponents);
			#endregion

			//Asset
			var src = new EarningComponentService(uow.Object);
			var excutionResult = new Result<EarningComponent>();
			await src.OnBeforeAdd(earningComponent, excutionResult);
			var list = await uow.Object.GetRepositoryAsync<DeductionComponent>().GetAsync();
			//Assert
			Assert.Equal(2, list.Count());
		}

		[Fact]
		public async Task OnBeforeUpdate_UpdateEarningComponent_UpdateData()
		{
			var earningId = Guid.NewGuid();
			var deductionId = Guid.NewGuid();
			#region Arrange
			var earningComponents = new List<EarningComponent>
			{
				new EarningComponent
				{
				ID =earningId,
				Name="Mahesh",
				Status=true
				}
			};
			var deductionComponents = new List<DeductionComponent>
			{
				new DeductionComponent
				{
				ID =deductionId,
				Name="Mahesh",
				Status=true,
				EarningId=earningId,
				ProrataBasis=false,
				IsEditable=true
				}
			};
			var earningComponent = new EarningComponent
			{
				ID = earningId,
				Name = "Mahesh",
				Status = true,
				ProrataBasis = true
			};
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, earningComponents);
			var _repository = _context.GetRepositoryAsyncDbSet(uow, deductionComponents);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<DeductionComponent, bool>>>(),
			 It.IsAny<Func<IQueryable<DeductionComponent>, IOrderedQueryable<DeductionComponent>>>(),
			 It.IsAny<Func<IQueryable<DeductionComponent>, IIncludableQueryable<DeductionComponent, object>>>(), true)).ReturnsAsync(
				 deductionComponents.FirstOrDefault(x => x.ID == deductionId));

			//Act
			var src = new EarningComponentService(uow.Object);
			var excutionResult = new Result<EarningComponent>();
			await src.OnBeforeUpdate(earningComponent, excutionResult);
			var list = await uow.Object.GetRepositoryAsync<DeductionComponent>().GetAsync();
			var component = list.FirstOrDefault(x => x.EarningId == earningId);
			//Assert
			Assert.True(component.ProrataBasis == earningComponent.ProrataBasis);
		}
	}
}
