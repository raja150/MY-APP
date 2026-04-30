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
	public class EmployeeEmergencyAdServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IEmployeeEmergencyAdService _service;
		private readonly Mock<DbContext> _context;
		public EmployeeEmergencyAdServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new EmployeeEmergencyAdService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public void  CustomValidation_ValidEmergencyContactNo_NoException()
		{
			// Arrange & Mock
			var data = new List<EmployeeEmergencyAd>{	new EmployeeEmergencyAd{}	};
	
			var mockEmployeeEmergencyAd = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeEmergencyAd(uow, _context, mockEmployeeEmergencyAd);

			var item = new EmployeeEmergencyAd()
			{
				EmergencyConNo = "9866682334"
			};

			//Act
			var src = new EmployeeEmergencyAdService(uow.Object);
			var result = new Result<EmployeeEmergencyAd>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasNoError);
		}


		[Fact]  
		public void  CustomValidation_InValidEmergencyContactNo_ThrowException()
		{
			// Arrange & Mock
			var data = new List<EmployeeEmergencyAd> { new EmployeeEmergencyAd{ } };

			var mockEmployeeEmergencyAd = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeEmergencyAd(uow, _context, mockEmployeeEmergencyAd);

			var item = new EmployeeEmergencyAd(){  EmergencyConNo = "986668"  };

			//Act
			var src = new EmployeeEmergencyAdService(uow.Object);
			var result = new Result<EmployeeEmergencyAd>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
