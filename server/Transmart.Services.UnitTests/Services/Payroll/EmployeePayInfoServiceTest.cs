using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class EmployeePayInfoServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IEmployeePayInfoService _employeePayInfoService;
		private readonly Mock<DbContext> _context;


		public EmployeePayInfoServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_employeePayInfoService = new EmployeePayInfoService(uow.Object);
			_context = new Mock<DbContext>();
		}
		[Fact]
		public async Task GetById_GetValidRecords()
		{
			//Arrange
			var id = Guid.NewGuid();

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID = id,
					EmployeeId=Guid.NewGuid(),
					PayMode=1,
					BankId=Guid.NewGuid(),
					AccountNo="111111111"
				},
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
				}
			};

			// Mock
			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			// Act
			var payInfo = await _employeePayInfoService.GetById(id);
			// Assert
			Assert.Equal(id, payInfo.ID);
		}

		[Fact]
		public async Task GetPaginate_GetValidRecords()
		{
			//Arrange
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					PayMode=1,
					BankId=Guid.NewGuid(),
					AccountNo="111111111"
				},
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
				}
			};

			// Mock
			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			// Act
			var payInfo = await _employeePayInfoService.GetPaginate(new BaseSearch());
			// Assert
			Assert.Equal(2, payInfo.Count);
		}

		[Fact]
		public async Task AddPaymentInfo_UpdatePayment_IsSuccess()
		{
			var employeeId = Guid.NewGuid();
			var employeePatInfoId = Guid.NewGuid();
			var accountNo = "4444444444";
			#region Arrange
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
				ID = Guid.NewGuid(),
				EmployeeId=employeeId,
				PayMode=1,
				BankId=Guid.NewGuid(),
				AccountNo="111111111"
				},
				new EmployeePayInfo
				{
				ID =employeePatInfoId,
				EmployeeId=Guid.NewGuid(),
				}
			};
			var employeePayInfoList = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
				ID = employeePatInfoId,
				EmployeeId=employeeId,
				PayMode=2,
				BankId=Guid.NewGuid(),
				AccountNo=accountNo
				}
			};

			#endregion

			//Mock
			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			//Asset
			var dd = await _employeePayInfoService.AddPaymentInfo(employeePayInfoList);
			var list = await uow.Object.GetRepositoryAsync<EmployeePayInfo>().GetAsync();
			var empPayInfoValues = list.FirstOrDefault(x => x.EmployeeId == employeeId);

			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(2, empPayInfoValues.PayMode);
			Assert.Equal(accountNo, empPayInfoValues.AccountNo);
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task AddPaymentInfo_Addpayment_IsSuccess()
		{
			#region Arrange
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
				},
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
				}
			};
			var employeePayInfoList = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
				ID = Guid.NewGuid(),
				EmployeeId=Guid.NewGuid(),
				}
			};
			var _repository = _context.GetRepositoryAsyncDbSet(uow, employeePayInfos);
			_repository.Setup(m => m.SingleAsync(It.IsAny<Expression<Func<EmployeePayInfo, bool>>>(),
				  It.IsAny<Func<IQueryable<EmployeePayInfo>, IOrderedQueryable<EmployeePayInfo>>>(),
				  It.IsAny<Func<IQueryable<EmployeePayInfo>, IIncludableQueryable<EmployeePayInfo, object>>>(), true)).ReturnsAsync(() => { return null; });
			#endregion

			//Act
			var dd = await _employeePayInfoService.AddPaymentInfo(employeePayInfoList);
			var list = await uow.Object.GetRepositoryAsync<EmployeePayInfo>().GetAsync();

			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(3, list.Count());
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task AddPaymentInfo_IsCatch_ThrowException()
		{
			var employeePayInfoId = Guid.NewGuid();
			#region Arrange
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
				ID = Guid.NewGuid(),
				EmployeeId=Guid.NewGuid(),
				}
			};
			var employeePayInfoList = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
				ID = employeePayInfoId,
				EmployeeId=Guid.NewGuid(),
				}
			};

			#endregion

			//Mock
			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);
			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

			//Asset
			var dd = await _employeePayInfoService.AddPaymentInfo(employeePayInfoList);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task OnBeforeAdd_AddEmployeepayInfo_SaveData()
		{
			#region Arrange
			var bankId = Guid.NewGuid();
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=Guid.NewGuid()},
					BankId=Guid.NewGuid()
				}
			};
			var employeePayInfoStatus = new List<EmployeePayInfoStatusAudit>();

			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9,
					 Name = "Bank New"
				}
			};
			var employeePayInfoAudits = new List<EmployeePayInfoAudit>
			{
				new EmployeePayInfoAudit
				{
					ID=Guid.NewGuid()
				}
			};
			var employeePayInfo = new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 3,
				IFSCCode = "SBIN0007654",
				AccountNo = "62376357882",
				ModifiedBy = "Mahesh",
			};
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfoAudits);
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfos);
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfoStatus);
			//Bank
			var mockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, mockSet);
			

			//Asset
			var src = new EmployeePayInfoService(uow.Object);
			var excutionResult = new Result<EmployeePayInfo>();
			//Act
			await src.OnBeforeAdd(employeePayInfo, excutionResult);
			var payInfoStatus = await uow.Object.GetRepositoryAsync<EmployeePayInfoStatusAudit>().GetAsync();
			var list = await uow.Object.GetRepositoryAsync<EmployeePayInfoAudit>().GetAsync();

			//Assert
			Assert.True(list.Count() == 2);
			Assert.True(payInfoStatus.LastOrDefault().Status == "Created");
		}

		[Fact]
		public async Task OnBeforeUpdate_UpdateEmployeepayInfo_UpdateData()
		{
			var bankId = Guid.NewGuid();
			var empPayInfoId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();

			#region Arrange
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=empPayInfoId,
					EmployeeId=employeeId,
					AccountNo="121323123",
					Bank=new Bank{ID=Guid.NewGuid()},
					BankId=bankId
				}
			};

			var employeePayInfoAudits = new List<EmployeePayInfoAudit>
			{
				new EmployeePayInfoAudit
				{
					ID=Guid.NewGuid(),
					AccountNo="111111111",
					PayType="Cheque",
					EmployeeId=employeeId,
					ModifiedBy="Vishnu"
				}
			};
			var employeePayInfoStatus = new List<EmployeePayInfoStatusAudit>
			{
				new EmployeePayInfoStatusAudit
				{
					ID=Guid.NewGuid(),
					EmployeeId=employeeId,
					Status="Created"
				}
			};


			var empPayInfo = new EmployeePayInfo
			{
				ID = empPayInfoId,
				BankId = bankId,
				EmployeeId = employeeId,
				PayMode = 1,
				AccountNo = "623763578",
				ModifiedBy = "Mahesh",
				IFSCCode = "SBIN0070988"
			};
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfos);
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfoAudits);
			_ = _context.GetRepositoryAsyncDbSet(uow, employeePayInfoStatus);

			var employeePayInfoAuditsMockSet = employeePayInfoAudits.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoAuditList(uow, _context, employeePayInfoAuditsMockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);


			//Asset
			var src = new EmployeePayInfoService(uow.Object);
			var excutionResult = new Result<EmployeePayInfo>();
			await src.OnBeforeUpdate(empPayInfo, excutionResult);
			var payInfoStatus = await uow.Object.GetRepositoryAsync<EmployeePayInfoStatusAudit>().GetAsync();
			//Assert
			Assert.True(excutionResult.HasNoError);
			Assert.True(payInfoStatus.LastOrDefault().EmployeeId == employeeId);
			Assert.True(payInfoStatus.LastOrDefault().Status == "Modified");

		}

		[Fact]
		public async Task CustomValidation_EmployeePayInfoExists_ThrowError()
		{
			#region Arrange
			var bankId = Guid.Parse("6D23A8BB-4B07-4C34-5157-08DA64AB8559");
			var employeeId = Guid.Parse("6D23A8BB-4B05-4C34-5157-08DA64AB8559");
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=employeeId,
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			//Assert
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = employeeId,
				PayMode = 0,
				AccountNo = "121321",
				Bank = new Bank { ID = bankId, BankNoLength = 9 }

			}, excutionResult);
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_AccountNoIsNull_ThrowError()
		{
			var bankId = Guid.NewGuid();

			//Arrange
			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};

			// Mock
			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			//Act
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "",
				Bank = new Bank { ID = bankId, BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_InvalidAccountNoFormat_ThrowError()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "121Addv323",
				Bank = new Bank { ID = bankId, BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_InvalidAccountNoLength_ThrowError()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "121323",
				Bank = new Bank { ID = bankId, BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_InvalidBankId_ThrowError()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = Guid.Empty,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "121323",
				Bank = new Bank { ID = Guid.NewGuid(), BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_AccountNoMisMatch_ThrowError()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "121323123",
				Bank = new Bank { ID = Guid.NewGuid(), BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_AccountNoAlreadyExists_ThrowError()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					AccountNo="121323123",
					Bank=new Bank{ID=bankId},
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				EmployeeId = Guid.NewGuid(),
				PayMode = 1,
				AccountNo = "121323123",
				Bank = new Bank { ID = Guid.NewGuid(), BankNoLength = 9 }

			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_PayModeIsCheque_PayWithCheque()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Act
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				PayMode = 2,
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}

		[Fact]
		public async Task CustomValidation_PayModeIsOnline_AccountNoAlreadyExists_And_InvalidIFSCCode()
		{
			var bankId = Guid.NewGuid();
			#region Arrange

			var employeePayInfos = new List<EmployeePayInfo>
			{
				new EmployeePayInfo
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					BankId=bankId
				}
			};
			var banks = new List<Bank>
			{
				new Bank
				{
					ID=bankId,
					 BankNoLength=9
				}
			};
			#endregion

			#region Mock

			var mockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfoList(uow, _context, mockSet);

			var bankMockSet = banks.AsQueryable().BuildMockDbSet();
			SetData.MockBank(uow, _context, bankMockSet);

			#endregion

			//Act
			var excutionResult = new Result<EmployeePayInfo>();
			await _employeePayInfoService.CustomValidation(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				BankId = bankId,
				PayMode = 3,
				IFSCCode = "SBI00000012"
			}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}
	}
}
