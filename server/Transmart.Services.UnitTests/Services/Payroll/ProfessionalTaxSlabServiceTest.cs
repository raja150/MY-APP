using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class ProfessionalTaxSlabServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<IUnitOfWork<TranSmartContext>> uow;
		private readonly ProfessionalTaxSlabService _service;
		private readonly Mock<DbContext> _context;
		public ProfessionalTaxSlabServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<IUnitOfWork<TranSmartContext>>();
			_service = new ProfessionalTaxSlabService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public void CustomValidation_ProfessionalTaxSlab_AlreadyExist_ThrowException()
		{
			// Arrange
			IEnumerable<ProfessionalTaxSlab> professionalTaxSlabs = new List<ProfessionalTaxSlab>
				{
					new ProfessionalTaxSlab
					{
						ID=Guid.Parse("137a6255-9f02-4d0f-96e3-a00d7cd25e9e"),
						ProfessionalTaxId = Guid.Parse("cc356e49-bc78-494d-9021-088948d5baef"),
						Amount = 100000,
						From = 2020,
						To = 2022
					},
				}.AsQueryable();

			// Mock
			var mockProfessionalTaxSlabs = professionalTaxSlabs.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ProfessionalTaxSlab>()).Returns(mockProfessionalTaxSlabs.Object);
			var repository = new RepositoryAsync<ProfessionalTaxSlab>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ProfessionalTaxSlab>()).Returns(repository);

			//Act
			Result<ProfessionalTaxSlab> result = new();
			_ =_service.CustomValidation(new ProfessionalTaxSlab
			{
				ID = Guid.NewGuid(),
				ProfessionalTaxId = Guid.Parse("cc356e49-bc78-494d-9021-088948d5baef"),
				Amount = 100000,
				From = 2021,
				To = 2021
			}, result);

			//Assert
			Assert.True(result.HasError);
		}
		[Fact]
		public void CustomValidation_NewProfessionalTaxSlab_NoException()
		{
			// Arrange
			IEnumerable<ProfessionalTaxSlab> professionalTaxSlabs = new List<ProfessionalTaxSlab>
				{
					new ProfessionalTaxSlab
					{
						ID=Guid.Parse("137a6255-9f02-4d0f-96e3-a00d7cd25e9e"),
						ProfessionalTaxId = Guid.Parse("2f30a711-6326-43b3-b7a9-25fefce6e440"),
						Amount = 100000,
						From = 2020,
						To = 2022
					},
				}.AsQueryable();

			// Mock
			var mockProfessionalTaxSlabs = professionalTaxSlabs.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ProfessionalTaxSlab>()).Returns(mockProfessionalTaxSlabs.Object);
			var repository = new RepositoryAsync<ProfessionalTaxSlab>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ProfessionalTaxSlab>()).Returns(repository);

			//Act
			Result<ProfessionalTaxSlab> result = new();
			_ = _service.CustomValidation(new ProfessionalTaxSlab
			{
				ID = Guid.NewGuid(),
				ProfessionalTaxId = Guid.Parse("cc356e49-bc78-494d-9021-088948d5baef"),
				Amount = 100000,
				From = 2021,
				To = 2021
			} , result);

			//Assert
			Assert.True(result.HasNoError);
		}
	}
}
