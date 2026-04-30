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
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class EmployeeFamilyServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IEmployeeFamilyService _service;
		private readonly Mock<DbContext> _context;
		public EmployeeFamilyServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new EmployeeFamilyService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task UpdateProfileContactTest_SpecificEmployee_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("1D73C8BB-4B04-4C34-5157-08DA64AB8559");
            var mockToDatabase = new List<EmployeeFamily>
			{
				new EmployeeFamily
			    {
					ID = Guid.NewGuid(),                
					EmployeeId = id
				}
			};

			var mockMockEmployeeFamily = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockMockEmployeeFamily);

			var test = new EmployeeFamilyModel()
			{
				ID = Guid.NewGuid(),
				ContactNo = "9866682334",
				EmployeeId = id
			};

			//Act
			var dd = await _service.UpdateProfileContact(test);

			//Assert
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public async Task UpdateProfileContactTest_NewEmployeeFamily_NoException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<EmployeeFamily> { new EmployeeFamily { ID = Guid.NewGuid() } };
			var mockMockEmployeeFamily = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockMockEmployeeFamily);

			var test = new EmployeeFamilyModel()
			{
				ContactNo = "9866682334",
				PersonName = "Arjun",
				EmployeeId = Guid.NewGuid(),
				HumanRelation = 1
			};

			//Act
			var dd = await _service.UpdateProfileContact(test);

			//Assert
			Assert.True(dd.HasNoError);
		}






		[Fact] 
		public void UpdateProfileContactTest_InvalidContact_ThrowException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<EmployeeFamily> {	new EmployeeFamily{} };

			var mockMockEmployeeFamily = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockMockEmployeeFamily);

			var test = new EmployeeFamilyModel() {  ContactNo = "fgjngh55"	};

			//Act
			var dd = _service.UpdateProfileContact(test).Result;

			//Assert
			Assert.True(dd.HasError);
		}


		

		[Fact]
		public void DeleteProfileContact_ExistEmployeeFamily_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var mockToDatabase = new List<EmployeeFamily>
			{
				new EmployeeFamily
				{
					ID = id
				}
			};

			var mockEmployeeFamily = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockEmployeeFamily);
			
			//Act
			var src = new EmployeeFamilyService(uow.Object);
			var dd = src.DeleteProfileContact(id).Result;

			//Assert
			Assert.True(dd.IsSuccess); 
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public void DeleteProfileContact_NewEmployeeFamily_ThrowException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<EmployeeFamily>
			{
				new EmployeeFamily
				{
					ID = Guid.NewGuid()
				}
			};

			var mockEmployeeFamily = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockEmployeeFamily);

			//Act
			var src = new EmployeeFamilyService(uow.Object);
			var dd = src.DeleteProfileContact(Guid.NewGuid()).Result;

			//Assert
			Assert.True(dd.HasError);
		}
	}
}

