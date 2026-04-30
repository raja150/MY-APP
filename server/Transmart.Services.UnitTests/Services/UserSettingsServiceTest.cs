using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Service.AppSettings;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{
	[CollectionDefinition("Collection #1")]
	[Xunit.TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class UserSettingsServiceTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly UserSettingsService _userSettingsService;
		private readonly UserSettings userSettings = new() { ID = Guid.NewGuid() };
		public UserSettingsServiceTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_userSettingsService = new UserSettingsService(inMemory.UnitOfWork);
		}

		[Fact(DisplayName = "Should_Add_New"), TestPriority(1)]
		[Trait("Category", "Integration")]
		[Trait("Issue", "123")]
		public async Task Should_Add_New()
		{
			var addResult = await _userSettingsService.AddAsync(userSettings);

			Assert.True(addResult.IsSuccess);
			Assert.True(addResult.Messages.Count == 0);

			Assert.True(inMemory.DbContext.AppSettings_UserSettings.Count() == 1);
		}

		[Fact(DisplayName = "Should_Update_Existing"), TestPriority(2)]
		[Trait("Category", "Integration")]
		[Trait("Issue", "123")]
		public async Task Should_Update_Existing()
		{
			var setting = inMemory.DbContext.AppSettings_UserSettings.FirstOrDefault();
			inMemory.DbContext.ChangeTracker.Clear();
			var updateResult = await _userSettingsService.UpdateAsync(setting);

			Assert.True(updateResult.IsSuccess);
			Assert.True(updateResult.Messages.Count == 0);

			Assert.True(inMemory.DbContext.AppSettings_UserSettings.Count() == 1);
		}

		[Fact(DisplayName = "Should_Not_Allow_Duplicate"), TestPriority(3)]
		[Trait("Category", "Integration")]
		[Trait("Issue", "123")]
		public async Task Should_Not_Allow_Duplicate()
		{

			var addDuplicateResult = await _userSettingsService.AddAsync(new UserSettings { ID = Guid.NewGuid() });

			Assert.False(addDuplicateResult.IsSuccess);
			Assert.True(inMemory.DbContext.AppSettings_UserSettings.Count() == 1);
		}

		[Fact(DisplayName = "ShouldReturnErrorWhenRecordExistsTest"), TestPriority(4)]
		public async Task ShouldReturnErrorWhenRecordExistsTest()
		{
			var result = new Result<UserSettings>();
			await _userSettingsService.OnBeforeAdd(new UserSettings { ID = Guid.NewGuid() }, result);

			Assert.True(result.HasError);
		}
	}
}
