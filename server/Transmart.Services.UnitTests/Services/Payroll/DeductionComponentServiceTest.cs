using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class DeductionComponentServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IDeductionComponentService _deductionComponentService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;

		public DeductionComponentServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_deductionComponentService = new DeductionComponentService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		[Fact]
		public async Task OnBeforeAdd_EditableValidate_UpdateAsTrue()
		{
			var deductionComponents = new List<DeductionComponent>
			{
				new DeductionComponent
				{
				ID =Guid.NewGuid(),
				Name="Mahesh",
				Status=true,
				IsEditable=true
				}
			};
			var mockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponent(uow, _context, mockSet);
			var deductionComponent = new DeductionComponent
			{
				ID = Guid.NewGuid(),
				Name = "Vamshi",
				Status = true,
				IsEditable = false
			};

			//Act
			var src = new DeductionComponentService(uow.Object);
			var res = new Result<DeductionComponent>();

			//Assert
			await src.OnBeforeAdd(deductionComponent, res);
			Assert.True(deductionComponent.IsEditable);
		}

		[Fact]
		public async Task OnBeforeUpdate_DeductionComponentIsNotNull_UpdateData()
		{
			var earningId = Guid.NewGuid();
			var componentId = Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559");
			// Arrange
			var deductionComponents = new List<DeductionComponent>
			{
				new DeductionComponent
				{
					ID = componentId,
					EarningId=earningId,
					Name="Mahesh",
					Status=true,
					IsEditable=true,
					ProrataBasis=true,
					Deduct=120,
					DeductionPlan=1
				}
			};

			//Mock
			var mockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponent(uow, _context, mockSet);

			//Act
			var src = new DeductionComponentService(uow.Object);
			var res = new Result<DeductionComponent>();
			_ = src.OnBeforeUpdate(new DeductionComponent
			{
				ID = componentId,
				Name = "Vishnu",
				Status = true
			}, res);
			var list = await uow.Object.GetRepositoryAsync<DeductionComponent>().GetAsync();
			var component = list.FirstOrDefault(x => x.ID == componentId);
			//Assert
			Assert.True(res.HasNoError);
			Assert.True(component.IsEditable);
			Assert.True(component.ProrataBasis);
			Assert.Equal(1, component.DeductionPlan);
			Assert.Equal(120, component.Deduct);
		}
		[Fact]
		public async Task OnBeforeUpdate_DeductionComponentIsNull_NoError()
		{
			var componentId = Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559");
			// Arrange
			var deductionComponents = new List<DeductionComponent>
			{
				new DeductionComponent
				{
					ID = componentId,
					Name="Mahesh",
					Status=true,
					IsEditable=true,
					ProrataBasis=true,
					Deduct=120,
					DeductionPlan=1
				}
			};

			//Mock
			var mockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponent(uow, _context, mockSet);

			//Act
			var src = new DeductionComponentService(uow.Object);
			var res = new Result<DeductionComponent>();
			await src.OnBeforeUpdate(new DeductionComponent
			{
				ID = Guid.NewGuid(),
				Name = "Vishnu",
				Status = true
			}, res);

			//Assert
			Assert.True(res.HasNoError);
		}
	}
}
