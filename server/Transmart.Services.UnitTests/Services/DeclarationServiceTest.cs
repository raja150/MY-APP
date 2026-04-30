using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{

	public class DeclarationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly DeclarationService _declarationService;
		public DeclarationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_declarationService = new DeclarationService(uow.Object);
		}

		[Fact]
		public void WithEmptyDataClearInputDataTest()
		{
			var declaration = new Declaration();
			_declarationService.ClearInputData(declaration);

			Assert.True(declaration.Section80CLines.Count == 0);
			Assert.True(declaration.Section80DLines.Count == 0);
			Assert.True(declaration.SectionOtherLines.Count == 0);
			Assert.True(declaration.LetOutPropertyLines.Count == 0);
			Assert.True(declaration.HRALines.Count == 0);
			Assert.True(declaration.HomeLoanPay == null);
			Assert.True(declaration.PrevEmployment == null);
			Assert.True(declaration.IncomeSource == null);
		}

		[Fact]
		public void WithValuesDataClearInputDataTest()
		{
			Declaration declaration = EmptyDate();
			_declarationService.ClearInputData(declaration);
			Assert.True(declaration.Section80CLines.Count == 0);
			Assert.True(declaration.Section80DLines.Count == 0);
			Assert.True(declaration.SectionOtherLines.Count == 0);
			Assert.True(declaration.LetOutPropertyLines.Count == 0);
			Assert.True(declaration.HRALines.Count == 0);
			Assert.True(declaration.HomeLoanPay == null);
			Assert.True(declaration.PrevEmployment == null);
			Assert.True(declaration.IncomeSource == null);
		}

		private static Declaration EmptyDate()
		{
			return new Declaration
			{
				Section80CLines = new List<Section6A80C>()
				{
					new Section6A80C { Amount = 0 }
				},
				Section80DLines = new List<Section6A80D>()
				{
					new Section6A80D { Amount = 0 }
				},
				SectionOtherLines = new List<Section6AOther>()
				{
					new Section6AOther { Amount = 0 }
				},
				LetOutPropertyLines = new List<LetOutProperty>()
				{
					new LetOutProperty { NetAnnualValue = 0 }
				},
				HRALines = new List<HraDeclaration>()
				{
					new HraDeclaration { Amount = 0 }
				},
				HomeLoanPay = new HomeLoanPay(),
				PrevEmployment = new PrevEmployment(),
				IncomeSource = new OtherIncomeSources()
			};
		}

		[Fact]
		public void WithEmptyIsDeclarationHasDataTest()
		{
			var declaration = new Declaration();
			_declarationService.ClearInputData(declaration);
			bool result = _declarationService.IsDeclarationHasData(declaration);

			Assert.False(result);

			declaration = EmptyDate();
			result = _declarationService.IsDeclarationHasData(declaration);

			Assert.True(result);
		}
	}
}
