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
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class DesignationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IDesignationService _service;
		private readonly Mock<DbContext> _context;
		public DesignationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new DesignationService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task GetAllWeekOffDepts_SpecificWeekOffSetupAndReference_ReturnValidDesignationList()
		{
			// Arrange & Mock
			var id = Guid.Parse("2D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<Designation>
			{
				new Designation
				{
					WeekOffSetupId = id
				},
				new Designation
				{
					WeekOffSetupId = id
				},
				new Designation
				{
					WeekOffSetupId = Guid.NewGuid()
				}
			};

			var mockDesignation = data.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var baseSearch = new BaseSearch()
			{
				RefId = id
			};

			//Assert
			var dd = await _service.GetAllWeekOffDesign(baseSearch);

			//Act
			Assert.Equal(2, dd.Count);
		}






		[Fact]
		public async Task UpdateWeekoff_SpecificDesignation_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var designationList = new List<Designation>
			{
				new Designation
				{
					ID = id,
				}
			};

			var mockDesignation = designationList.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var designationAllocationModel = new DesignationAllocationModel()
			{
				DesignationId = id,
				WeekOffSetupId = Guid.NewGuid()
			};

			//Assert
			var dd = await _service.UpdateWeekoff(designationAllocationModel);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}





		[Fact]
		public async Task UpdateWeekoffTest_ExistWeekOffSetup_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var designationList = new List<Designation>
			{
				new Designation
				{
					ID = id,
					WeekOffSetupId= Guid.NewGuid()
				}
			};

			var mockDesignation = designationList.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var designationAllocationModel = new DesignationAllocationModel()
			{
				DesignationId = id
			};

			//Assert
			var dd = await _service.UpdateWeekoff(designationAllocationModel);

			//Act
			Assert.True(dd.HasError);
		}


		[Fact]
		public async Task DeleteWeekOffSetup_SpecificDesignation_NoException()
		{
			// Arrange 
			var id = Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var designationList = new List<Designation>
			{
				new Designation
				{
				   ID = id,
				   WeekOffSetupId = Guid.NewGuid()
				}
			};

			// Mock
			var mockDesignation = designationList.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var designation = new Designation()
			{
				ID = id,
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(designation);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}


		[Fact]
		public async Task DeleteWeekOffSetup_NewDesignation_ThrowException()
		{
			// Arrange 
			var designationList = new List<Designation>
			{
				 new Designation
				{
				   ID = Guid.NewGuid()
				}
			};

			// Mock
			var mockDesignation = designationList.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var designation = new Designation()
			{
				ID = Guid.NewGuid()
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(designation);

			//Act
			Assert.True(dd.HasError);
		}
		[Fact]
		public async Task OnAfterAdd_Test()
		{
			//Arrange
			var designation = new List<Designation> { new Designation
			{
				ID = Guid.NewGuid(),
				Name  = "Designation",
				Status = true
			} };
			var replication = new List<Replication>
			{
				new Replication
				{
					Type = 1,
					Category = (byte)ReplicationType.Add,
					RefId = Guid.NewGuid()
				}
			};
			//Mock
			var mockDesignation = designation.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			var mockReplication = replication.AsQueryable().BuildMockDbSet();
			SetData.MockReplication(uow, _context, mockReplication);
			//Act
			var response = await _service.AddAsync(
				new Designation
				{
					Name = "IT Development",
					DepartmentId = Guid.NewGuid(),
					Status = true
				});
			//Assert
			Assert.True(response.IsSuccess);
		}
		[Fact]
		public async Task OnAfterUpdate_Test()
		{
			//Arrange
			var designationId = Guid.NewGuid();
			var designation = new List<Designation> { new Designation
			{
				ID = designationId,
				Name = "Designation",
				DepartmentId = Guid.NewGuid()
			} };
			var replication = new List<Replication> { new Replication
			{
				Type = (byte)ReplicationType.Update,
				Category =  (byte)ReplicationCategory.Designation,
				RefId = designationId
			} };
			//Mock
			var mocksetDesignation = designation.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mocksetDesignation);

			var mockReplication = replication.AsQueryable().BuildMockDbSet();
			SetData.MockReplication(uow, _context, mockReplication);
			//Act
			var response = await _service.UpdateAsync(
				new Designation
				{
					ID = designationId,
					Name = "Designation Updated",
					DepartmentId = Guid.NewGuid()
				});
			//Assert
			Assert.True(response.IsSuccess);
		}

		[Fact]
		public async Task OnAfterUpdate_Replication_Null_Test()
		{
			//Arrange
			var designationId = Guid.NewGuid();
			var designation = new List<Designation> { new Designation
			{
				ID = designationId,
				Name = "Designation",
				DepartmentId = Guid.NewGuid()
			} };
			var replication = new List<Replication> { new Replication
			{
				Type = (byte)ReplicationType.Update,
				Category =  (byte)ReplicationCategory.Designation,
				RefId = Guid.NewGuid()
			} };
			//Mock
			var mocksetDesignation = designation.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mocksetDesignation);

			var mockReplication = replication.AsQueryable().BuildMockDbSet();
			SetData.MockReplication(uow, _context, mockReplication);
			//Act
			var response = await _service.UpdateAsync(
				new Designation
				{
					ID = designationId,
					Name = "Designation Updated",
					DepartmentId = Guid.NewGuid()
				});
			//Assert
			Assert.True(response.IsSuccess);
		}
	}
}
