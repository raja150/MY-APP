using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
    public class ProfessionalTaxServiceTest
    {
        private readonly Mock<TranSmartContext> _dbContext;
        private readonly Mock<IUnitOfWork<TranSmartContext>> uow;
        private readonly EmployeeDataGenerator _employeeData;
        private readonly IProfessionalTaxService _service;
        private readonly Mock<DbContext> _context;
        public ProfessionalTaxServiceTest()
        {
            var builder = new DbContextOptionsBuilder<TranSmartContext>();
            var app = new Mock<IApplicationUser>();
            var dbContextOptions = builder.Options;
            _dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
            uow = new Mock<IUnitOfWork<TranSmartContext>>();
            _service = new ProfessionalTaxService(uow.Object);
            _employeeData = new EmployeeDataGenerator();
            _context = new Mock<DbContext>();
        }
        [Fact]
        public void CustomValidation_InvalidProfessionalTax_ExceptionThrown()
        {
            //Arrange
            IEnumerable<ProfessionalTax> professionalTaxes = new List<ProfessionalTax>
                {
                    new ProfessionalTax
                    {
                        ID=Guid.NewGuid(),
                        StateId = Guid.Parse("6e7a7cd3-1163-4663-be1a-796a7b3bec2b"),
                        State = new TranSmart.Domain.Entities.Organization.State{ID = Guid.Parse("6e7a7cd3-1163-4663-be1a-796a7b3bec2b"),Name = "Telangana"}
                    },
                };

            //ProfessionalTaxe Mock 
            var mockProfessionalTax = professionalTaxes.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<ProfessionalTax>()).Returns(mockProfessionalTax.Object);
            var repository = new RepositoryAsync<ProfessionalTax>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<ProfessionalTax>()).Returns(repository);

			//Act
			Result<ProfessionalTax> result = new();
			var service = new ProfessionalTaxService(uow.Object);
			var dd = service.CustomValidation(new ProfessionalTax
            {
               ID = Guid.NewGuid(),
               StateId = Guid.Parse("6e7a7cd3-1163-4663-be1a-796a7b3bec2b"),
            },result);
            //Assert
            Assert.True(result.HasError);
        }

		[Fact]
		public void CustomValidation_ValidProfessionalTax_NoException()
		{
			//Arrange
			IEnumerable<ProfessionalTax> professionalTaxes = new List<ProfessionalTax>
				{
					new ProfessionalTax
					{
						ID=Guid.NewGuid(),
						StateId = Guid.Parse("6e7a7cd3-1163-4663-be1a-796a7b3bec2b"),
						State = new TranSmart.Domain.Entities.Organization.State{ID = Guid.Parse("6e7a7cd3-1163-4663-be1a-796a7b3bec2b"),Name = "Telangana"}
					},
				};

			//ProfessionalTaxe Mock 
			var mockProfessionalTax = professionalTaxes.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ProfessionalTax>()).Returns(mockProfessionalTax.Object);
			var repository = new RepositoryAsync<ProfessionalTax>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ProfessionalTax>()).Returns(repository);

			//Act
			Result<ProfessionalTax> result = new();
			var service = new ProfessionalTaxService(uow.Object);
			var dd = service.CustomValidation(new ProfessionalTax
			{
				ID = Guid.NewGuid(),
				StateId = Guid.Parse("3edda061-78e1-432f-b32c-2ba71f4a6425"),
			} , result);
			//Assert
			Assert.True(result.HasNoError);
		}

    }
}
