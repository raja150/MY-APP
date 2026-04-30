using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Helpdesk.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Helpdesk
{
	public class DepartmentGroupServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IDepartmentGroupService _service;
		private readonly Mock<DbContext> _context;
		public DepartmentGroupServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new DepartmentGroupService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public void  CustomValidation_New_Department_Group_NoException()	
		{
			// Arrange & Mock
			var mockToDatabase = new List<DepartmentGroup>
			{	
				new DepartmentGroup
				{ 
					ID = Guid.NewGuid(),
					GroupsId = Guid.NewGuid()
				} 
			};
			var _repository = _context.GetRepositoryAsyncDbSet(uow, mockToDatabase);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DepartmentGroup, bool>>>())).ReturnsAsync(false);

			var deppGroup = new DepartmentGroup()
			{
				ID = Guid.NewGuid(),
				GroupsId = Guid.NewGuid()
			};


			//Act
			var src = new DepartmentGroupService(uow.Object);
			var result = new Result<DepartmentGroup>();

			_= src.CustomValidation(deppGroup, result);

			//Assert
			Assert.True(result.HasNoError);
		}


		[Fact]  
		public void  CustomValidation_ExistDepartmentGroup_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("304A8B11-CED2-4A09-5158-08DA64AB8559");
			var mockToDatabase = new List<DepartmentGroup>
			{
				new DepartmentGroup
				{
					ID =  Guid.NewGuid(),
					GroupsId = id
				}
			};

			var _repository   = _context.GetRepositoryAsyncDbSet(uow, mockToDatabase);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DepartmentGroup, bool>>>())).ReturnsAsync(true);

			var deppGroup = new DepartmentGroup()
			{
				ID = Guid.NewGuid(),
				GroupsId = id
			};


			//Act
			var src = new DepartmentGroupService(uow.Object);
			var result = new Result<DepartmentGroup>();
			_= src.CustomValidation(deppGroup, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
