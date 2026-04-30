using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Helpdesk.Setup;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Helpdesk
{
	public class HelpTopicServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IHelpTopicService _service;
		private readonly Mock<DbContext> _context;
		public HelpTopicServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new HelpTopicService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]  
		public async Task GetHelpTopics_SpecificDepartment_ReturnValidHelpTopics()
		{
			//Arrange & Mock
			var departmentId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			var data = new List<HelpTopic>
			{
				new HelpTopic
				{
					DepartmentId = departmentId,
					Status = true, 
					
				},
				new HelpTopic
				{
					DepartmentId = departmentId,
					Status = true,
				},
				new HelpTopic
				{
					DepartmentId = Guid.NewGuid(),
					Status = false,
				}
			};
			var mockHelpTopicService = data.AsQueryable().BuildMockDbSet();
			SetData.MockHelpTopicService(uow, _context, mockHelpTopicService);

			//Act
			var result = await _service.GetHelpTopics(departmentId);

			//Assert
			Assert.Equal(2, result.Count());
		}
	}
}
