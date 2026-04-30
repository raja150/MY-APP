//using Microsoft.EntityFrameworkCore;
//using MockQueryable.Moq;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using Transmart.Services.UnitTests.Services.Setup;
//using TranSmart.Core;
//using TranSmart.Data;
//using TranSmart.Domain.Entities;
//using TranSmart.Domain.Entities.Organization;
//using TranSmart.Domain.Models;
//using TranSmart.Service;
//using Xunit;

//namespace Transmart.Services.UnitTests.Services
//{
//	public class BaseServiceTest
//	{
//		private readonly Mock<TranSmartContext> _dbContext;
//		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
//		private IBaseService<Employee> _service;
//		private readonly Mock<DbContext> _context;
//		public BaseServiceTest()
//		{
//			var builder = new DbContextOptionsBuilder<TranSmartContext>();
//			var app = new Mock<IApplicationUser>();
//			var dbContextOptions = builder.Options;
//			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
//			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
//			_service = new BaseService<Employee>(uow.Object);
//			_context = new Mock<DbContext>();
//		}

//		[Fact]
//		public async Task  GetList_AnyName_ReturnValidEmployeeList()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee>
//			{
//				new Employee
//			    {
//					Name = "Arjun",
//				},
//				new Employee
//				{
//					Name = "Basanth",
//				}
//			};

//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			//Act
//			var src = await  _service.GetList("AnyName");

//			//Assert
//			Assert.Equal(2,src.ToList().Count);
//		}


//		[Fact]
//		public async Task GetPaginate_ReturnEntityList()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee>
//			{
//				new Employee
//				{
//					Name = "Arjun",
//				},
//				new Employee
//				{
//					Name = "Basanth",
//				}
//			};
//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			var baseSearch = new BaseSearch(){	};

//			//Act
//			var src = await _service.GetPaginate(baseSearch);

//			//Assert
//			Assert.Equal(2,src.Count);
//		}
		

//        [Fact]
//		public async Task AddOnlyAsync_ThrowException()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee>{};

//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			var item = new  Employee{ };

//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = await _service.AddOnlyAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}


//		[Fact]
//		public async Task AddAsync_ThrowException()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee> { };

//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			var item = new Employee { };

//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = await _service.AddAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}


//		[Fact]
//		public async Task UpdateOnlyAsync_ThrowException()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee> { };

//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			var item = new Employee { };

//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = await _service.UpdateOnlyAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}

//		[Fact]
//		public async Task UpdateAsync_ThrowException()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<Employee> { };

//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			var item = new Employee { };

//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = await _service.UpdateAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}


//		[Fact]
//		public async Task Get_SpecificEntity_ReturnEntity()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("245848d7-c9d5-4c85-baea-68726002283b");
//			var mockToDatabase = new List<Employee>
//			{
//				new Employee
//				{
//					ID = id
//				},
//				new Employee
//				{
//					ID = Guid.NewGuid()
//				}
//			};
//			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockEmployee(uow, _context, mockEmployee);

//			//Act
//			var src = await _service.GetById(id);

//			//Assert
//			Assert.Equal(id, src.ID);
//		}



//		[Fact]
//		public async Task GetNextSequenceNo_SpecificEntityNameAttribute_ReturnValidData()
//		{
//			// Arrange & Mock
//			var mockToDatabase = new List<SequenceNo>
//			{
//				new SequenceNo
//				{
//					EntityName = "Entity Name",
//					Attribute = "Attribute"
//				}
//			};

//			var mockSequenceNo = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNo(uow, _context, mockSequenceNo);

//			var mockSequenceNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
//			SetData.MockSequenceNoNonAsync(uow, _context, mockSequenceNoNonAsync);

//			var src = new BaseService<Employee>(uow.Object);
//			//Act
//			var dd = await src.GetNextSequenceNo("Entity Name", "Attribute");

//			Assert.Equal("A00000", dd.Item2);
//		}
//	}
//}
