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
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service;
using TranSmart.Service.Approval;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave.Approval
{
	public class ApprovalCompensatoryWorkingDayServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private ApprovalCompensatoryWorkingDayService _service;
		private Mock<DbContext> _context;
		private EmployeeDataGenerator _empployeeData;
		public ApprovalCompensatoryWorkingDayServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new ApprovalCompensatoryWorkingDayService(uow.Object);
			_context = new Mock<DbContext>();
			_empployeeData = new EmployeeDataGenerator();
		}
		[Fact]
		public async Task GetAllList_Count_Verification_Test()
		{
			// Arrange
			var employee = _empployeeData.GetEmployeeData();
			IEnumerable<ApplyCompo> applyCompos = new List<ApplyCompo>
			{
				new ApplyCompo
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					Employee = employee,
					FromDate = DateTime.Now.AddDays(1),
					ToDate = DateTime.Now.AddDays(2),
					Status = 1
				}
			};
			StatusSearch search = new StatusSearch
			{
				Status = 1,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ReportingToId
			};

			//Mock
			var mockSetapplyCompo = applyCompos.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyCompo>()).Returns(mockSetapplyCompo.Object);
			var repository = new RepositoryAsync<ApplyCompo>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyCompo>()).Returns(repository);

			//Act
			var dd = await _service.GetAllList(search);
			//Assert
			Assert.True(dd.Count == 1);
		}
		[Fact]
		public void OnBeforeUpdate_Test()
		{
			#region Arrange
			var employee = _empployeeData.GetEmployeeData();
			IEnumerable<LeaveSettings> leaveSettings = new List<LeaveSettings>
			{
				new LeaveSettings
				{
					ID = Guid.NewGuid(),
					CompoLeaveTypeId = Guid.NewGuid(),
				}
			};
			IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					Employee = employee,
					LeaveTypeId = Guid.NewGuid()
				}
			};

			#endregion
			#region Mock
			var mockLeaveSettings = leaveSettings.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<LeaveSettings>()).Returns(mockLeaveSettings.Object);
			var repository = new RepositoryAsync<LeaveSettings>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveSettings>()).Returns(repository);
			//LeaveBalance Mock
			var mockLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<LeaveBalance>()).Returns(mockLeaveBalance.Object);
			var leaveBalanceRepository = new RepositoryAsync<LeaveBalance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(leaveBalanceRepository);
			#endregion
			var executionResult = new Result<ApplyCompo>();
			//Act
			_ = _service.OnBeforeUpdate(new ApplyCompo
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				Employee = employee,
				Status = 1
			}, executionResult);
			//Assert
			Assert.True(executionResult.HasNoError);
		}
	}
}
