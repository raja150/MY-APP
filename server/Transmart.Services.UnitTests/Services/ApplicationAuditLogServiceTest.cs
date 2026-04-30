using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Service;
using Xunit;


namespace Transmart.Services.UnitTests.Services
{
	public class ApplicationAuditLogServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IApplicationAuditLogService _service;
		private readonly Mock<DbContext> _context;
		public ApplicationAuditLogServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new ApplicationAuditLogService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Theory]
		[InlineData("Arjun", "My Action", "192.168.200.185", "This is an entity")]
		public async Task GetAccesedUser_AplicationAuditLog_SavedData(string accesedBy, string action, string iPAddress, string entity)
		{
			var mockToDatabase = new List<AplicationAuditLog>();
			//SetData.MockAplicationAuditLog(uow, _context, mockToDatabase.AsQueryable().BuildMockDbSet()); 

			//Act
			await _service.GetAccesedUser(accesedBy, action, iPAddress, entity);

			//Assert
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}
	}
}
