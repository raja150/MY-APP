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
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class DepartmentServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IDepartmentService _service;
		private readonly Mock<DbContext> _context;
		public DepartmentServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new DepartmentService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public void  CustomValidationTest_NewEmail_NoException()
		{
			// Arrange &  Mock
			var data = new List<Department>
			{
				new Department
				{
					ID = Guid.NewGuid(),
					Email = "vishnu@gmail.com"
				}
			};

			var mockDepartment = data.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			var item = new Department()
			{
				ID = Guid.NewGuid(),
				Email = "shiva@gmail.com"
			};

			//Act
			var src = new DepartmentService(uow.Object);
			var result = new Result<Department>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasNoError);
		}




		[Fact]  
		public void  CustomValidation_ExistEmail_ThrowException()
		{
			// Arrange &  Mock
			var email = "vishnu@gmail.com";
			var data = new List<Department>
			{
				new Department
				{
					Email = email
				},
				new Department
				{
					Email = "Shiva@gmail.com"
				}
			};

			var mockDepartment = data.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			var item = new Department()
			{
				ID = Guid.NewGuid(),
				Email = email
			};

			//Act
			var src = new DepartmentService(uow.Object);
			var result = new Result<Department>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}


		[Fact]         
		public void  CustomValidation_InvalidEmailFormat_ThrowException()
		{
			// Arrange &  Mock
			var data = new List<Department>
			{
				new Department
				{
					Email = "vishnu@gmail.com"
				}
			};

			var mockDepartment = data.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			var item = new Department()
			{
				Email = "vishnu"
			};

			//Act
			var src = new DepartmentService(uow.Object);
			var result = new Result<Department>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}



		[Fact]   
		public async Task GetAllWeekOffDepts_SpecificWeekOffSetupAndReference_ReturnValidDepartmentsList()
		{
			// Arrange & Mock
			var id = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<Department>
			{
				new Department  
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId = id
				},
				new Department  
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId = id
				},
				new Department
				{ 
					ID = Guid.NewGuid(),
					WeekOffSetupId = Guid.NewGuid()
				}
			};

			var mockAllocation = data.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockAllocation);

			var baseSearch = new BaseSearch()
			{
				RefId = id
			};

			//Assert
			var dd = await _service.GetAllWeekOffDepts(baseSearch);

			//Act
			Assert.Equal(2, dd.Count);
		}



		[Fact]
		public async Task UpdateWeekoff_SpecificDepartment_NoException()
		{
			//Arrange 
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var departmentList = new List<Department>
			{
				new Department
			    {
				   ID = id,
			    }
			};

			var departmentAllocationModel = new DepartmentAllocationModel()
			{
				DepartmentId = id,
				WeekOffSetupId = Guid.NewGuid()
			};

			//Mock
			var mockDepartment = departmentList.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			//Assert
			var dd = await _service.UpdateWeekoff(departmentAllocationModel);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public void UpdateWeekoffTest_ExistWeekOffSetup_ThrowException()
		{
			//Arrange 
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var departmentList = new List<Department>
			{
			   new Department
			   {
				   ID = id,
				   WeekOffSetupId= Guid.NewGuid()
			   }
			};

			var departmentAllocationModel = new DepartmentAllocationModel()
			{
				DepartmentId = id
			};

			//Mock
			var mockDepartment = departmentList.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			//Assert
			var dd = _service.UpdateWeekoff(departmentAllocationModel).Result;

			//Act
			Assert.True(dd.HasError);
		}



		[Fact]
		public void DeleteWeekOffSetup_SpecificDepartment_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var departmentList = new List<Department>
			{
				new Department
				{
					ID = id,
					WeekOffSetupId = Guid.NewGuid()
				}
			};
			var mockDepartment = departmentList.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			var departmentAllocationModel = new DepartmentAllocationModel()
			{
				ID = id,
			};

			//Assert
			var dd = _service.DeleteWeekOffSetup(departmentAllocationModel).Result;

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public void DeleteWeekOffSetupTest_NewDepartment_ThrowException()
		{
			// Arrange & Mock
			var departmentList = new List<Department>
			{
				new Department
				{
					ID = Guid.NewGuid()
				}
			};

			var mockDepartment = departmentList.AsQueryable().BuildMockDbSet();
			SetData.MockDepartment(uow, _context, mockDepartment);

			var departmentAllocationModel = new DepartmentAllocationModel()
			{
				ID = Guid.NewGuid()
			};

			//Assert
			var dd = _service.DeleteWeekOffSetup(departmentAllocationModel).Result;

			//Act
			Assert.True(dd.HasError);
		}
	}
}
