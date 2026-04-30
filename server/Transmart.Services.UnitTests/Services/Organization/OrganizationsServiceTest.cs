using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class OrganizationsServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IOrganizationsService _service;
		private readonly Mock<DbContext> _context;
		public OrganizationsServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new OrganizationsService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task GetAllWeekOffLoc_ReturnFirstOrganizationData()
		{
			// Arrange & Mock
			var id = Guid.Parse("40d2d55f-7244-4dd3-9dfb-facec005b588");
			var mockToDatabase = new List<Organizations>
			{
			   new Organizations
			   {
				  ID = id
			   },
			   new Organizations
			   {
				  ID = Guid.NewGuid()
			   },
			   new Organizations
			   {
				  ID = Guid.NewGuid()
			   }
			};
			
			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockLocation);

			//Act
		    var result = await _service.GetOrg();

			//Assert
			Assert.Equal(id,result.ID);
		}


		
		[Fact]
		public void  CustomValidation_ValidData_NoException()
		{
			//Arrange &	Mock
			var mockToDatabase = new List<Organizations>
			{
			    new Organizations
			    {
				   ID = Guid.NewGuid(),
				   PAN = "PEVFV4506E",
				   TAN = "TGPO02911G"
				}
			};
			var mockOrganizations = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockOrganizations);

			var item = new Organizations
			{
				ID = Guid.NewGuid(),
				PAN = "QEVFV4506E",
				TAN = "NGPO02911G"
			};

			//Act
			var src = new OrganizationsService(uow.Object);
			var result = new Result<Organizations>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasNoError);
		}



		[Fact]                  
		public void  CustomValidationTest_InvalidPANFormat_ThrowException()
		{
			//Arrange 
			var mockToDatabase = new List<Organizations>
			{
			   new Organizations
			   {
				  ID = Guid.NewGuid(),
				  PAN = "PEVFV4506E",
				  TAN = "PDES03028F"
			   }
			};

			// Mock
			var mockOrganizations = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockOrganizations);
			var item = new Organizations
			{
				ID = Guid.NewGuid(),
				PAN = "25%$",
				TAN = "NGPO02911G"
			};

			//Act
			var src = new OrganizationsService(uow.Object);
			var result = new Result<Organizations>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}


		[Fact]      
		public void  CustomValidationTest_ExistPAN_ThrowException()
		{
			//Arrange & Mock
			var pan = "PEVFV4506E";
			var mockToDatabase = new List<Organizations>
			{
			   new Organizations
			   {
				  ID = Guid.NewGuid(),
				  PAN = pan,
				  TAN = "NGPO02911G"
			   }
			};
			var mockOrganizations = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockOrganizations);

			var item = new Organizations
			{
				ID = Guid.NewGuid(),
				PAN = pan,
				TAN = "TGPO02911G"
			};

			//Act
			var src = new OrganizationsService(uow.Object);
			var result = new Result<Organizations>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}


		[Fact]         
		public void  CustomValidation_InvalidTANFormat_ThrowException()
		{
			//Arrange & Mock
			var mockToDatabase = new List<Organizations>
			{
			    new Organizations
			    {
				   ID = Guid.NewGuid(),
				   PAN = "PEVFV4506E",
				   TAN = "TGPO02911G"
				}
			};
			var mockOrganizations = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockOrganizations);

			var item = new Organizations
			{
				ID = Guid.NewGuid(),
				PAN = "JEVFV4506E",
				TAN = "67fghn"
			};

			//Act
			var src = new OrganizationsService(uow.Object);
			var result = new Result<Organizations>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}


		[Fact]   
		public void  CustomValidation_ExistTAN_ThrowException()
		{
			//Arrange & Mock
			var tan = "NGPO02911G";
			var mockToDatabase = new List<Organizations>
			{
			   new Organizations
			   {
				  ID = Guid.NewGuid(),
				  PAN ="PEVFV4506E",
				  TAN = tan
			   }
			};
			var mockOrganizations = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, mockOrganizations);

			var item = new Organizations
			{
				ID = Guid.NewGuid(),
				PAN = "BEVFV4506E",
				TAN = tan
			};

			//Act
			var src = new OrganizationsService(uow.Object);
			var result = new Result<Organizations>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
