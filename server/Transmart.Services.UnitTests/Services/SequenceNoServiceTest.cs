//using Microsoft.EntityFrameworkCore;
//using MockQueryable.Moq;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Transmart.Services.UnitTests.Services.Setup;
//using TranSmart.Core;
//using TranSmart.Data;
//using TranSmart.Domain.Entities;
//using TranSmart.Service;
//using Xunit;

//namespace Transmart.Services.UnitTests.Services
//{
//	public class SequenceNoServiceTest
//	{
//		private readonly Mock<TranSmartContext> _dbContext;
//		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
//		private readonly ISequenceNoService _service;
//		private readonly Mock<DbContext> _context;
//		public SequenceNoServiceTest()
//		{
//			var builder = new DbContextOptionsBuilder<TranSmartContext>();
//			var app = new Mock<IApplicationUser>();
//			var dbContextOptions = builder.Options;
//			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
//			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
//			_service = new SequenceNoService(uow.Object);
//			_context = new Mock<DbContext>();
//		}



//		[Fact]
//		public async Task UpdateRange_SpecificSequenceNo_ReturnUpdatedSequenceNo()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					ID = id
//				},
//				new SequenceNo
//				{
//					ID =  Guid.NewGuid()
//				}
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			var mockSequenceNoNonAsync = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNoNonAsync(uow, _context, mockSequenceNoNonAsync);

//			var items = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					ID = id,
//					NextNo = 1,
//					Prefix = "Arjun"
//				}
//			};

//			//Act
//			var src = await _service.UpdateRange(items);

//			//Assert
//			Assert.Equal(2, src.ToList().Count);
//		}




//		[Fact]
//		public async Task NextSequenceNo_SpecificEntityNameAndAttribute_AddsNextNoToPrefix()
//		{
//			// Arrange & Mock
//			var entityName = "Entity";
//		    var attribute = "Attribute";
//			var nextNo = 10000;
//			var prefix = "Arjun";
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					EntityName = entityName,
//					Attribute = attribute,
//					NextNo = nextNo,
//					Prefix = prefix,
//				}
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			//Act
//			var src = await _service.NextSequenceNo(entityName, attribute);

//			//Assert
//			Assert.Equal(prefix+nextNo, src.Item2);
//		}



//		[Fact]
//		public void NextSequenceNo_NewEntityNameAndAttribute_ThrowException()
//		{
//			// Arrange & Mock
//			var mockSequenceNoToDB = new List<SequenceNo>{};

//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = _service.NextSequenceNo("EntityName", "Attribute");

//			//Assert
//			Assert.True(src.IsFaulted);
//		}




//		[Fact]
//		public async Task DisplaySeqNo_SpecificEntityNameAndAttribute_ReturnUpdatedSequenceNoWithPassedData()
//		{
//			// Arrange & Mock
//			var entityName = "Entity";
//			var attribute = "Attribute";
//			var nextNo = 10000;
//			var prefix = "Arjun"; 
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					EntityName = entityName,
//					Attribute = attribute,
//				    Prefix = prefix,
//					NextNo = nextNo
//				 }
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			//Act
//			var src = await _service.DisplaySeqNo(entityName, attribute);


//			//Assert
//			Assert.Equal(prefix+nextNo, src);
//		}



//		[Fact]
//		public async Task DisplaySeqNo_NewEntityNameAndAttribute_ReturnConstantData()
//		{
//			// Arrange & Mock
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					EntityName = "Entity",
//					Attribute = "Attribute"
//				 }
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			//Act
//			var src = await _service.DisplaySeqNo("entity", "attribute");

//			//Assert
//			Assert.Equal("00000", src);
//		}




//		[Fact]
//		public void AddSequenceAttributeTest()
//		{
//			// Arrange & Mock
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{}
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			//Act
//			var src = _service.AddSequenceAttribute("Entity", "Attribute");

//			//Assert
//			Assert.True(src.IsCompleted);
//		}





//		[Fact]
//		public  void GetNoTest_ReturnSequenceNo()
//		{
//			// Arrange & Mock
//			var mockSequenceNoToDB = new List<SequenceNo>
//			{
//				new SequenceNo
//				{}
//			};
//			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			//Act
//			var src = new SequenceNoService(uow.Object);
//			var dd =  src.GetNo("Employee", 12);

//			//Assert
//			Assert.Equal("Employee00012", dd);
//		}
//	}
//}
