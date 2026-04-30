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
using TranSmart.Service;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Helpdesk
{
	public class DeskDepartmentServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IDeskDepartmentService _service;
		private readonly Mock<DbContext> _context;

		public DeskDepartmentServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new DeskDepartmentService(uow.Object);
			_context = new Mock<DbContext>();

		}


		[Fact]
		public async Task GetDepartments_StatusTrue_ReturnValidDeskDepartments()
		{
			//Arrange & Mock
			var mockToDatabase = new List<DeskDepartment>()
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					Status = true
				},
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					Status = true
				},
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					Status = false
				}
			};
			var mockSetDeskDepartment = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockSetDeskDepartment);


			//Act
			var src = await _service.GetDepartments();

			//Assert
			Assert.Equal(2, src.Count());
		}


		[Fact]  
		public  void  CustomValidation_NewDeskDepartment_NoException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<DeskDepartment>{  new DeskDepartment{ } };

			var _repository = _context.GetRepositoryAsyncDbSet(uow, mockToDatabase);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DeskDepartment, bool>>>())).ReturnsAsync(false);

			var deskDepartment = new DeskDepartment()
			{
				ID = Guid.NewGuid(),
				Department = "IT Department"
			};

			//Act
			var src = new DeskDepartmentService(uow.Object);
			var result = new Result<DeskDepartment>();

			_ =  src.CustomValidation(deskDepartment, result);

			//Assert
			Assert.True(result.HasNoError);
		}



		[Fact]
		public  void CustomValidation_ExistDeskDepartment_ThrowException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<DeskDepartment>
			{
				  new DeskDepartment
				  {             
					 ID = Guid.NewGuid(),
					 Department = "CSE Department"
				  },
				  new DeskDepartment
				  {
				  	 ID = Guid.NewGuid(),
					 Department = "ECE Department"
				  }
			};

			var _repository = _context.GetRepositoryAsyncDbSet(uow, mockToDatabase);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DeskDepartment, bool>>>())).ReturnsAsync(true);

			var deskDepartment = new DeskDepartment()
			{
				ID = Guid.NewGuid(),
				Department = "CSE Department"
			};

			//Act
			var src = new DeskDepartmentService(uow.Object);
			var result = new Result<DeskDepartment>();

			 _= src.CustomValidation(deskDepartment, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
