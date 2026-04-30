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
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service;
using TranSmart.Service.Leave.Approval;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave.Approval
{
	public class ClientVisitServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ApprovalClientVisitsService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		public ClientVisitServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new ApprovalClientVisitsService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}
		[Fact]
		public async Task ClientVisit_Paginate_Test()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			ApplyClientVisitSearch search = new ApplyClientVisitSearch
			{
				Page = 0,
				Size = 10,
				FromDate = DateTime.Today.AddDays(-2),
				ToDate = DateTime.Today,
				RefId = employee.ReportingToId,
				Name = "AVONTIX1822"
			};
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
				},
				new ApplyClientVisits
				{
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
				}
			};
			// Mock
			var mockSetApplyClientVisit = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockSetApplyClientVisit.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			//Act
			var list = await _service.ClientVisit(search);
			//Assert
			Assert.True(list.Count == 1);
		}

		[Fact]
		public async Task Approve_Invalid_ApplyClientVisit_ThrowException()
		{
			var id = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e2");
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					ID = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7"),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				},
				new ApplyClientVisits
				{
					ID = Guid.NewGuid(),
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				}
			};
			// Mock
			var mockApplyClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockApplyClientVisits.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			// Act
			var approve = await _service.Approve(id,Guid.NewGuid() ,true);
			// Assert
			Assert.True(approve.HasError);
		}

		[Theory]
		[InlineData(true, "", (int)ClientVisitStatus.Approved)]//ClientVisit Already Approved 
		[InlineData(false, "Reject", (int)ClientVisitStatus.Rejected)]//ClientVistAlready Rejected
		public async Task Approve_ApplyClientvisit_Status_NotApplied_ThrowException(bool isApproved, string rejectReason, byte status)
		{
			var id = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7");
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					ID = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7"),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = status
				},
				new ApplyClientVisits
				{
					ID = Guid.NewGuid(),
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = status
				}
			};
			// Mock
			var mockApplyClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockApplyClientVisits.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			// Act
			var approve = await _service.Approve(id, Guid.NewGuid(), true);
			// Assert
			Assert.True(approve.HasError);
		}

		[Fact]
		public async Task Approve_ApplyClientvisit_isApproved_True()
		{
			var id = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7");
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					ID = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7"),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				},
				new ApplyClientVisits
				{
					ID = Guid.NewGuid(),
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				}
			};
			// Mock
			var mockApplyClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockApplyClientVisits.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			// Act
			var approve = await _service.Approve(id,Guid.NewGuid(), true);
			// Assert
			Assert.True(approve.ReturnValue.Status == (int)ClientVisitStatus.Approved);
		}

		[Fact]
		public async Task Approve_ApplyClientvisit_isRejected_True()
		{
			var id = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7");
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					ID = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7"),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				},
				new ApplyClientVisits
				{
					ID = Guid.NewGuid(),
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = (int)ClientVisitStatus.Applied
				}
			};
			// Mock
			var mockApplyClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockApplyClientVisits.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			// Act
			var approve = await _service.Approve(id, Guid.NewGuid(), false);
			// Assert
			Assert.True((int)ClientVisitStatus.Rejected == approve.ReturnValue.Status);
			Assert.True("VisitRejected" == approve.ReturnValue.AdminReason);
		}

		[Fact]
		public void GetClientVisit_Test()
		{
			var id = Guid.Parse("00f2f9ff-df10-4727-83ee-2ba4ae42f9e7");
			var approvarId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac");
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			IEnumerable<ApplyClientVisits> clientVisits = new List<ApplyClientVisits>()
			{
				new ApplyClientVisits
				{
					ID = id,
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = 0
				},
				new ApplyClientVisits
				{
					ID = Guid.NewGuid(),
					EmployeeId = employees.LastOrDefault().ID,
					Employee = employees.LastOrDefault(),
					FromDate = DateTime.Today.Date,
					ToDate = DateTime.Today.Date,
					Status = 1
				}
			};
			// Mock
			var mockApplyClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockApplyClientVisits.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
			//Act
			var applyClienVisit = _service.GetClientVisit(id, approvarId);
			//Assert
			Assert.Equal(id, applyClienVisit.Result.ID);
		}
	}
}
