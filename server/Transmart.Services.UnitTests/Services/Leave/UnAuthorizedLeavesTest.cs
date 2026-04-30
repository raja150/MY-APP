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
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;
using Xunit;
using Transmart.Services.UnitTests.Services.Setup;
using TranSmart.Core.Result;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class UnAuthorizedLeavesTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IUnAuthorizedLeavesService _service;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;

		public UnAuthorizedLeavesTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new UnAuthorizedLeavesService(uow.Object);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task GetPaginate_SearchWithReferenceAndFromDateToToDate_GetValidRecords()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			UnAuthorizedLeavesSearch baseSearch = new()
			{
				FromDate = DateTime.Parse("2022-07-9"),
				ToDate = DateTime.Parse("2022-07-30"),
				Status = 0,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ID
			};
			IEnumerable<UnAuthorizedLeaves> data = new List<UnAuthorizedLeaves>
			{
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-11"),
					RefId = employee.ID,
					LeaveStatus = 1,
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-20"),
					RefId = employee.ID,
					LeaveStatus = 1,
					
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-1"),
					RefId =Guid.NewGuid(),
					LeaveStatus = 1,

				}

			};


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockUnAuthorizedLeaves(uow, _context, mockSet);

			//Act
			var unAuthorizedLeaves = await _service.GetPaginate(baseSearch);

			//Assert
			Assert.Equal(2, unAuthorizedLeaves.Items.Count(x => x.RefId == employee.ID));
		}

		[Fact]
		public async Task GetById_UnAuthorizedLeavesGetById_GetValidRecords()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var unAuthorizedId=Guid.NewGuid();
			
			IEnumerable<UnAuthorizedLeaves> data = new List<UnAuthorizedLeaves>
			{
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = unAuthorizedId,
					Date= DateTime.Parse("2022-07-11"),
					RefId = employee.ID,
					LeaveStatus = 1,
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-20"),
					RefId = employee.ID,
					LeaveStatus = 1,

				},
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-1"),
					RefId =Guid.NewGuid(),
					LeaveStatus = 1,

				}

			};


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockUnAuthorizedLeaves(uow, _context, mockSet);

			//Act
			var unAuthorizedLeaves = await _service.GetById(unAuthorizedId);

			//Assert
			Assert.Equal(unAuthorizedId, unAuthorizedLeaves.ID);
		}

		[Fact]
		public async Task CustomValidation_ValidateWithDate_ThrowException()
		{
			#region Arrange
			var result = new Result<UnAuthorizedLeaves>();
			var employee = _employeeData.GetEmployeeData();
			var unAuthorizedId=Guid.NewGuid();
			
			IEnumerable<UnAuthorizedLeaves> data = new List<UnAuthorizedLeaves>
			{
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = unAuthorizedId,
					Date= DateTime.Parse("2022-07-11"),
					RefId = employee.ID,
					LeaveStatus = 1,
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-20"),
					RefId = employee.ID,
					LeaveStatus = 1,

				},
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-1"),
					RefId =Guid.NewGuid(),
					LeaveStatus = 1,

				}

			};
			var unAuthorized = new UnAuthorizedLeaves
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Date = DateTime.Parse("2022-07-20"),
				RefId = Guid.NewGuid(),
				LeaveStatus = 1,

			};


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockUnAuthorizedLeaves(uow, _context, mockSet);

			//Act
			 await _service.CustomValidation(unAuthorized, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_ValidateWithDate_WithoutException()
		{
			#region Arrange
			var result = new Result<UnAuthorizedLeaves>();
			var employee = _employeeData.GetEmployeeData();
			var unAuthorizedId = Guid.NewGuid();

			IEnumerable<UnAuthorizedLeaves> data = new List<UnAuthorizedLeaves>
			{
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = unAuthorizedId,
					Date= DateTime.Parse("2022-07-11"),
					RefId = employee.ID,
					LeaveStatus = 1,
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-20"),
					RefId = employee.ID,
					LeaveStatus = 1,

				},
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Date= DateTime.Parse("2022-07-1"),
					RefId =Guid.NewGuid(),
					LeaveStatus = 1,

				}

			};
			var unAuthorized = new UnAuthorizedLeaves
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Date = DateTime.Parse("2022-07-3"),
				RefId = Guid.NewGuid(),
				LeaveStatus = 1,

			};


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockUnAuthorizedLeaves(uow, _context, mockSet);

			//Act
			await _service.CustomValidation(unAuthorized, result);

			//Assert
			Assert.True(result.HasNoError);
		}
	}
}
