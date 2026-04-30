//using Microsoft.EntityFrameworkCore;
//using MockQueryable.Moq;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Transmart.Services.UnitTests.Services.Helpdesk.Setup;
//using TranSmart.Core;
//using TranSmart.Data;
//using TranSmart.Domain.Entities;
//using TranSmart.Service;
//using Xunit;

//namespace Transmart.Services.UnitTests.Services
//{
//	public class ImageServiceTest
//	{
//		private readonly Mock<TranSmartContext> _dbContext;
//		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
//		private IImageService _service;
//		private readonly Mock<DbContext> _context;
//		private readonly ISequenceNoService sequenceNoService;
//		public ImageServiceTest()
//		{
//			var builder = new DbContextOptionsBuilder<TranSmartContext>();
//			var app = new Mock<IApplicationUser>();
//			var dbContextOptions = builder.Options;
//			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
//			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
//			sequenceNoService = new SequenceNoService(uow.Object);
//			_service = new ImageService(uow.Object);
//			_context = new Mock<DbContext>();
//		}

//		[Fact]
//		public async void GetImg_ReturnSpecificEmpImageStatusList()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<EmpImage>
//			{
//				new EmpImage
//				{
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Status = 1
//					}
//				},
//				new EmpImage
//				{
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Status = 1
//					}
//				},
//				new EmpImage
//				{
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Status = 0
//					}
//				}
//			};
//			var mockEmpImageNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmpImageNo(uow, _context, mockEmpImageNoNonAsync);

//			//Act
//			var src = await _service.GetImg();

//			//Assert
//			Assert.Equal(2, src.ToList().Count);
//		}



//		[Fact]
//		public async Task AddandUpdate_NewEmployeeImage_AddingImage()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<EmpImage>
//			{
//				new EmpImage
//				{
//					EmployeeId = Guid.NewGuid()
//				}
//			};

//			var mockEmpImageNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmpImage(uow, _context, mockEmpImageNoNonAsync);

//			var item = new EmpImage
//			{
//				EmployeeId = Guid.NewGuid()
//			};


//			//Act
//			var src = await _service.AddandUpdate(item);

//			//Assert
//			Assert.True(src.IsSuccess);
//			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
//		}




//		[Fact]
//		public async Task AddandUpdate_SpecificEmployeeImage_UpdatingImage()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("690de6c9-90ee-4d9a-b287-2a1c4821d2e6");
//			var mockToDatabase = new List<EmpImage>
//			{
//				new EmpImage
//				{
//					EmployeeId = id
//				}
//			};
//			var mockEmpImageNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmpImage(uow, _context, mockEmpImageNoNonAsync);

//			var item = new EmpImage
//			{
//				EmployeeId = id
//			};

//			//Act
//			var src = await _service.AddandUpdate(item);

//			//Assert
//			Assert.True(src.HasNoError);
//		}
//	}
//}

