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
	public class HelpTopicSubServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IHelpTopicSubService _service;
		private readonly Mock<DbContext> _context;
		public HelpTopicSubServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new HelpTopicSubService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]  
		public  async Task GetHelpTopicSubs_SpecificHelpTopic_ReturnValidSubTopics()
		{
			// Arrange & Mock
			var helpTopicId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			var data = new List<HelpTopicSub>
			{
			   new HelpTopicSub
			   {
				  HelpTopicId = helpTopicId
			   },
			   new HelpTopicSub
			   {
				  HelpTopicId = helpTopicId
			   },
			   new HelpTopicSub
			   {
				  HelpTopicId = Guid.NewGuid()
			   },
			};

			var mockHelpTopicSubService = data.AsQueryable().BuildMockDbSet();
			SetData.MockHelpTopicSubService(uow, _context, mockHelpTopicSubService);
			

			//Act
			var result = await _service.GetHelpTopicSubs(helpTopicId);

			//Assert
			Assert.Equal(2, result.Count());
		}



		[Fact]
		public  void  CustomValidation_NewSubTopic_NoException()
		{
		  // Arrange & Mock
			var data = new List<HelpTopicSub>
			{
			   new HelpTopicSub
			   {
				   SubTopic = "This is Normal"
			   }
			};

			var _repository = _context.GetRepositoryAsyncDbSet(uow, data);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<HelpTopicSub, bool>>>())).ReturnsAsync(false);

			var check = new HelpTopicSub()
			{
				SubTopic = "This is Different"
			};

			//Act
			var src = new HelpTopicSubService(uow.Object);
			var result = new Result<HelpTopicSub>();

			_ = src.CustomValidation(check, result);

			//Assert
			Assert.True(result.HasNoError);
		}


		[Fact]    
		public void  CustomValidation_ExistSubTopic_ThrowException()
		{
			// Arrange & Mock
			var data = new List<HelpTopicSub>
			{
			   new HelpTopicSub
			   {
				   ID = Guid.NewGuid(),
				   SubTopic = "This is Normal"
			   }
			};

			var _repository = _context.GetRepositoryAsyncDbSet(uow, data);
			_repository.Setup(x => x.HasRecordsAsync(It.IsAny<Expression<Func<HelpTopicSub, bool>>>())).ReturnsAsync(true);

			var check = new HelpTopicSub() {  SubTopic = "This is Normal"  };

			//Act
			var src = new HelpTopicSubService(uow.Object);
			var result = new Result<HelpTopicSub>();

			_ = src.CustomValidation(check, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
