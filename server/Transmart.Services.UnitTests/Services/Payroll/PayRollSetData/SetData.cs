using Microsoft.EntityFrameworkCore;
using Moq;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;

namespace Transmart.Services.UnitTests.Services.Payroll.PayRollSetData
{
	public static class SetData
	{
		public static void MockArrear(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Arrear>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Arrear>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Arrear>()).Returns(repository);
		}
		public static void MockEmployee(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}
		public static void MockEmployeeSalary(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Salary>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Salary>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Salary>()).Returns(repository);
		}
		public static void MockEmpStatutory(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpStatutory>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpStatutory>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpStatutory>()).Returns(repository);
		}
		public static void MockEmployeePayInfo(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePayInfo>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePayInfo>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePayInfo>()).Returns(repository);
		}
		public static void MockPaysheetEarning(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheetEarning>()).Returns(mockSet.Object);
			var repository = new Repository<PaySheetEarning>(context.Object);
			uow.Setup(m => m.GetRepository<PaySheetEarning>()).Returns(repository);
		}
		public static void MockPaysheetEarningAsync(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheetEarning>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySheetEarning>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySheetEarning>()).Returns(repository);
		}

		public static void MockIncentivePayCut(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<IncentivesPayCut>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<IncentivesPayCut>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<IncentivesPayCut>()).Returns(repository);
		}

		public static void MockIncomeTaxLimit(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<IncomeTaxLimit>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<IncomeTaxLimit>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<IncomeTaxLimit>()).Returns(repository);
		}

		public static void MockLoanAsync(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Loan>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Loan>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Loan>()).Returns(repository);
		}
		public static void MockLoan(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Loan>()).Returns(mockSet.Object);
			var repository = new Repository<Loan>(context.Object);
			uow.Setup(m => m.GetRepository<Loan>()).Returns(repository);
		}
		public static void MockLoanDeduction(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LoanDeduction>()).Returns(mockSet.Object);
			var repository = new Repository<LoanDeduction>(context.Object);
			uow.Setup(m => m.GetRepository<LoanDeduction>()).Returns(repository);
		}
		public static void MockLoanDeductionAsync(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LoanDeduction>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LoanDeduction>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LoanDeduction>()).Returns(repository);
		}
		public static void MockPayMonth(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PayMonth>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PayMonth>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PayMonth>()).Returns(repository);
		}

		public static void MockProfessionalTaxSlab(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ProfessionalTaxSlab>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ProfessionalTaxSlab>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ProfessionalTaxSlab>()).Returns(repository);
		}
		public static void MockDeclaration(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Declaration>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Declaration>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Declaration>()).Returns(repository);
		}
		public static void MockDeclarationSetting(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeclarationSetting>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeclarationSetting>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeclarationSetting>()).Returns(repository);
		}

		public static void MockESI(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ESI>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ESI>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ESI>()).Returns(repository);
		}
		public static void MockEPF(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EPF>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EPF>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EPF>()).Returns(repository);
		}

		public static void MockFinancial(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<FinancialYear>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<FinancialYear>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<FinancialYear>()).Returns(repository);
		}

		public static void MockPaySheet(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheet>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySheet>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repository);
		}
		public static void MockPaySetting(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySettings>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySettings>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySettings>()).Returns(repository);
		}

		public static void MockPaySheetDeduction(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheetDeduction>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySheetDeduction>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySheetDeduction>()).Returns(repository);
		}

		public static void MockSequenceNum(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<SequenceNo>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<SequenceNo>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<SequenceNo>()).Returns(repository);
		}
		public static void MockEarningComponents(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EarningComponent>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EarningComponent>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EarningComponent>()).Returns(repository);
		}

		public static void MockDeductionComponents(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeductionComponent>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeductionComponent>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeductionComponent>()).Returns(repository);
		}

		public static void MockAttendanceSum(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<AttendanceSum>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<AttendanceSum>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AttendanceSum>()).Returns(repository);
		}

		public static void MockEmployeeDataGenerator(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}
		public static void MockPrevEmployment(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PrevEmployment>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PrevEmployment>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PrevEmployment>()).Returns(repository);
		}
		public static void MockNewRegimeSlab(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<NewRegimeSlab>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<NewRegimeSlab>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<NewRegimeSlab>()).Returns(repository);
		}
		public static void MockOldRegimeSlab(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<OldRegimeSlab>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<OldRegimeSlab>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<OldRegimeSlab>()).Returns(repository);
		}
		public static void MockEmpBonus(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpBonus>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpBonus>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpBonus>()).Returns(repository);
		}
		public static void MockHRADeclaration(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HraDeclaration>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HraDeclaration>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HraDeclaration>()).Returns(repository);
		}
		public static void MockDeclarationAllowance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeclarationAllowance>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeclarationAllowance>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeclarationAllowance>()).Returns(repository);
		}
		public static void MockLateComers(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Latecomers>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Latecomers>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Latecomers>()).Returns(repository);
		}
		public static void MockHomeLoanPay(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HomeLoanPay>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HomeLoanPay>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HomeLoanPay>()).Returns(repository);
		}
		public static void MockLetOutProperty(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LetOutProperty>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LetOutProperty>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LetOutProperty>()).Returns(repository);
		}
		public static void MockOtherIncomeSource(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<OtherIncomeSources>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<OtherIncomeSources>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<OtherIncomeSources>()).Returns(repository);
		}
		public static void MockSection6A80CWages(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80CWages>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80CWages>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80CWages>()).Returns(repository);
		}
		public static void MockSection6A80C(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80C>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80C>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80C>()).Returns(repository);
		}
		public static void MockSection6A80D(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80D>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80D>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80D>()).Returns(repository);
		}
		public static void MockSection6AOther(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6AOther>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6AOther>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6AOther>()).Returns(repository);
		}
	}
}
