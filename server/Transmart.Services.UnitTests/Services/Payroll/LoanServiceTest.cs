using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.PayRollSetData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class LoanServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILoanService _loanService;
		private readonly ISequenceNoService sequenceNoService;
		private readonly Mock<DbContext> _context;


		public LoanServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			sequenceNoService = new SequenceNoService(uow.Object);
			_loanService = new LoanService(uow.Object, sequenceNoService);
			_context = new Mock<DbContext>();
		}

		private void LoanDeductionMockData()
		{
			PayMonth payMonth = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				Status = 4
			};

			var loanDeductions = new List<LoanDeduction>()
			{
				
				new LoanDeduction()
				{
					ID=Guid.Parse("6A73C8BB-4B04-4C34-5157-08DA64AB8559"),
					LoanID=Guid.Parse("6F73C8BB-4B04-4C34-5157-08DA64AB8559"),
					PayMonth=payMonth
				},
				new LoanDeduction
				{
					ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					LoanID=Guid.NewGuid(),
				}
			};

			//Mock
			var mockSetLoanDeductions = loanDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, mockSetLoanDeductions);
		}

		private void LoanMockData()
		{
			var loans = new List<Loan>()
			{
				new Loan()
				{
                    ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					LoanNo ="L1",
					LoanAmount=12345,
				},
				new Loan
				{
					ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
					LoanNo ="L1",
					LoanAmount=12345
				}
			};

			var mockSet = loans.AsQueryable().BuildMockDbSet();
			SetData.MockLoanAsync(uow, _context, mockSet);
		}
			
		
		[Fact]
		public async Task Update()
		{
			var LoanId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562");

			LoanDeductionMockData();

			LoanMockData();

			// Act
			var src = await _loanService.UpdateAsync(new Loan()
			{
				ID = LoanId,
				EmployeeId = employeeId,
				LoanNo = "L1",
				LoanAmount = 1234
			});

			//Assert
			Assert.True(src.IsSuccess);
		}

		[Fact]
		public void  CustomValidation_DataExist_Returns_Error()
		{
			var LoanId = Guid.Parse("6F73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562");
			LoanDeductionMockData();

			//Act
			var _service = new LoanService(uow.Object, sequenceNoService);
			Result<Loan> result = new();

			_=  _service.CustomValidation(new Loan()
			{
				ID = LoanId,
				EmployeeId = employeeId,
				LoanNo = "L3",
				LoanAmount = 10000
			}, result);

			//Assert
			Assert.False(result.HasNoError);
		}

		[Fact]
		public void CustomValidation_NotExist_Returns_NoError()
		{
			var LoanId = Guid.Parse("9E73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562");
			LoanDeductionMockData();

			//Act
			var _service = new LoanService(uow.Object, sequenceNoService);
			Result<Loan> result = new();

			_ = _service.CustomValidation(new Loan()
			{
				ID = LoanId,
				EmployeeId = employeeId,
				LoanNo = "L3",
				LoanAmount = 10000
			}, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public void OnBeforeAdd()
		{
			#region Arrange

			var ded = new List<LoanDeduction>()
			{
				new LoanDeduction()
				{
					ID= Guid.NewGuid(),
				    LoanID= Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				    PayMonth = new PayMonth{Status=4}
				}
			};

			var seqno = new List<SequenceNo>()
			{
				new SequenceNo()
				{
					EntityName="PayRoll_Loan",
					Attribute= "No"
				},
			};
			Result<Loan> result = new();

			Loan loan = new()
			{
				ID=Guid.NewGuid(),
				LoanNo= "A00000",
				Status=true,
				LoanAmount=1200
			};
			var loans = new List<Loan>() 
			{
				 new Loan()
			     {
				  ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				  EmployeeId = Guid.Parse("5E2C595B-DD5E-4328-FC26-08DA64AB8562"),
				  Due= 1000,
				  LoanAmount = 1000,
				  LoanNo="Q1234",
				  Status=true
				 },

				 new Loan()
				 {
				  ID = Guid.NewGuid(),
				  EmployeeId = Guid.NewGuid(),
				  Due= 11000,
				  LoanAmount = 20000,
				  LoanNo="L023",
				  Status=false
				 }
			};
			
			#endregion

			//Mock
			var mockSetLoan = loans.AsQueryable().BuildMockDbSet();
			SetData.MockLoanAsync(uow, _context, mockSetLoan);

			var mockSetLoanDed = ded.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, mockSetLoanDed);

			var mockSequenceNum = seqno.AsQueryable().BuildMockDbSet();
			SetData.MockSequenceNum(uow, _context, mockSequenceNum);

			//Act
			var src = new LoanService(uow.Object, sequenceNoService).OnBeforeAdd(loan, result);

			//Assert
			Assert.True(src.IsCompleted);
		}
	}
}
