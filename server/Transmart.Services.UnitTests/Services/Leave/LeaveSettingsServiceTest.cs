using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
    public class LeaveSettingsServiceTest
    {
        private readonly IUnitOfWork uow;
        private readonly TranSmartContext dbContext;
        private readonly LeaveSettingsService _leaveSettingsService;

        public LeaveSettingsServiceTest()
        {
            TranSmart.Services.UnitTests.InMemory inMemory = new();
            uow = inMemory.GetUnitWorkObject("InMemoryDatabase");
            dbContext = inMemory.libraryDbContext;
            _leaveSettingsService = new LeaveSettingsService(uow);
        }

        [Fact]
        public async Task Should_Not_Allow_Duplicate()
        {
            var settings = new LeaveSettings { ID = Guid.NewGuid() };
            var addResult = await _leaveSettingsService.AddAsync(settings);

            var updateResult = await _leaveSettingsService.UpdateAsync(settings);

            var addDuplicateResult = await _leaveSettingsService.AddAsync(new LeaveSettings { ID = Guid.NewGuid() });

            Assert.True(addResult.IsSuccess);
            Assert.True(addResult.Messages.Count == 0);

            Assert.True(updateResult.IsSuccess);
            Assert.True(updateResult.Messages.Count == 0);

            Assert.False(addDuplicateResult.IsSuccess);
            Assert.True(dbContext.Leave_LeaveSettings.Count() == 1);
        }
    }
}
