using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class AllocationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IAllocationService _service;
		private readonly Mock<DbContext> _context;
		public AllocationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new AllocationService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task GetEmployee_SpecificAllocation_ReturnAllocationData()
		{
			// Arrange & Mock
			var allocationId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<Allocation>
			{
				new Allocation
				{
					ID = allocationId,
				},
				new Allocation
				{
					ID = Guid.NewGuid()
				}
			};
			var mockAllocation = data.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			//Act
			var dd = await _service.GetEmployee(allocationId);

			//Assert
			Assert.Equal(allocationId, dd.ID);
		}



		[Fact]   
		public async Task GetAllWeekOffEmployees_SpecificReferenceAndWeekOffSetup_ReturnValidAllocationsList()
		{
			// Arrange &  Mock
			var id = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<Allocation>
			{
			   new Allocation 
			   {
				   WeekOffSetupId = id
			   },
			   new Allocation 
			   { 
				   WeekOffSetupId = id
			   },
			   new Allocation
			   {
				   WeekOffSetupId = Guid.NewGuid()
			   }
			};

			var mockAllocation = data.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var baseSearch = new BaseSearch(){ RefId = id };

			//Act
			var dd = await _service.GetAllWeekOffEmployees(baseSearch);

			//Assert
			Assert.Equal(2, dd.Count);
		}




		[Fact]
		public async Task UpdateAllocation_SpecificEmployee_NoException()
		{
			// Arrange 
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var mockAllocationToDB = new List<Allocation>
			{
				new Allocation
				{
					EmployeeId = id
				}
			};

			// Mock
			var mockAllocation = mockAllocationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var allocation = new Allocation
			{
				EmployeeId = id
			};

			//Act
			var dd = await _service.UpdateAllocation(allocation);

			//Assert
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}





		[Fact]
		public async Task UpdateAllocation_NewEmployee_ThrowException()
		{
			// Arrange 
			var mockAllocationToDB = new List<Allocation>
			{
				new Allocation 
				{
					EmployeeId = Guid.NewGuid()
				}
			};

			// Mock
			var mockAllocation = mockAllocationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var allocation = new Allocation
			{
				EmployeeId = Guid.NewGuid()
			};

			//Act
			var dd = await _service.UpdateAllocation(allocation);

			//Assert
			Assert.True(dd.HasError);
		}



		[Fact]
		public async Task UpdateAllocation_ExistWeekOffSetup_ThrowException()
		{
			// Arrange 
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
            var mockAllocationToDB = new List<Allocation>
			{
				new Allocation
				{
					WeekOffSetupId = Guid.NewGuid(),
					EmployeeId = id
				}
			};

			// Mock
			var mockAllocation = mockAllocationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var allocation = new Allocation
			{
				EmployeeId = id
			};

			//Act
			var dd = await _service.UpdateAllocation(allocation);

			//Assert
			Assert.True(dd.HasError);
		}




		[Fact]
		public async Task DeleteWeekOffSetup_SpecificAllocation_NoException()
		{
			// Arrange
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var mockAllocationToDB = new List<Allocation>
			{
				new Allocation  
				{
					ID = id,
				}
			};

			// Mock
			var mockAllocation = mockAllocationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var allocation = new Allocation
			{
				ID = id
			};

			//Act
			var dd = await _service.DeleteWeekOffSetup(allocation);

			//Assert
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}





		[Fact]
		public async Task DeleteWeekOffSetupTest_NewAllocation_ThrowException()
		{
			// Arrange
			var mockAllocationToDB = new List<Allocation>
			{
				new Allocation
				{
					ID = Guid.NewGuid(),
				}
			};

			// Mock
			var mockAllocation = mockAllocationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockAllocation(uow, _context, mockAllocation);

			var allocation = new Allocation
			{
				ID = Guid.NewGuid()
			};

			//Act
			var dd = await _service.DeleteWeekOffSetup(allocation);

			//Assert
			Assert.True(dd.HasError);
		}
	}
}
