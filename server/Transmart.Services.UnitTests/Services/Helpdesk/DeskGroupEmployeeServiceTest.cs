using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Helpdesk
{
	public class DeskGroupEmployeeServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IDeskGroupEmployeeService _service;
		private readonly Mock<DbContext> _context;
		public DeskGroupEmployeeServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new DeskGroupEmployeeService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public void CustomValidation_NewDeskGroupEmployee_NoException()
		{
			// Arrange & Mock
			var data = new List<DeskGroupEmployee> { new DeskGroupEmployee { } };

			var _repository = _context.GetRepositoryAsyncDbSet(uow, data);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DeskGroupEmployee, bool>>>())).ReturnsAsync(false);


			var deskGroupEmployee = new DeskGroupEmployee()
			{
				EmployeeId = Guid.Parse("103A8B11-CED2-4A09-5158-08DA64AB8559")
			};

			//Act
			var src = new DeskGroupEmployeeService(uow.Object);
			var result = new Result<DeskGroupEmployee>();

			_ = src.CustomValidation(deskGroupEmployee, result);

			//Assert
			Assert.True(result.HasNoError);
		}




		[Fact]
		public void CustomValidation_ExistDeskGroupEmployee_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			var data = new List<DeskGroupEmployee>
			{
				new DeskGroupEmployee
				{
					ID = Guid.NewGuid(),
					EmployeeId = id
				},
				new DeskGroupEmployee
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid()
				}
			};

			var _repository = _context.GetRepositoryAsyncDbSet(uow, data);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<DeskGroupEmployee, bool>>>())).ReturnsAsync(true);


			var deskGroupEmployee = new DeskGroupEmployee()
			{
				EmployeeId = id
			};

			//Act
			var src = new DeskGroupEmployeeService(uow.Object);
			var result = new Result<DeskGroupEmployee>();

			_ = src.CustomValidation(deskGroupEmployee, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
