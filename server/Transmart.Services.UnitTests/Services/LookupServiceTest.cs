using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Service;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{
	public class LookupServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private ILookupService _service;
		private readonly Mock<DbContext> _context;
		public LookupServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LookupService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public void OnBeforeAdd_SpecificLookUpValue_ValueCountIncreases()
		{
			// Arrange & Mock
			var code = "Ar_jun";
			var mockToDatabase = new List<LookUpValues>
			{
				new LookUpValues
				{
					Code = code,
				},
				new LookUpValues
				{
					Code = code,
				}
			};

			var mockLookUpValuesNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLookUpValuesNo(uow, _context, mockLookUpValuesNoNonAsync);

			var item = new LookUpValues { Code = code };
			 

			//Act
			_service.AddAsync(item);

			//Assert
			Assert.Equal(3, item.Value);
		}


		[Fact]
		public async void Search_SpecificLookUpValue_ReturnLookUpValueList()
		{
			// Arrange & Mock
			var code = "Ar_jun";
			var mockToDatabase = new List<LookUpValues>
			{
				new LookUpValues
				{
					Code = code,
				},
				new LookUpValues
				{
					Code = code,
				},
				new LookUpValues
				{
					Code = "Basanth",
				}
			};

			var mockLookUpValuesNoNonAsync = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLookUpValuesNo(uow, _context, mockLookUpValuesNoNonAsync);

			//Act
			var src = await _service.Search(code);

			//Assert
			Assert.Equal(2, src.Count());
		}
	}
}
