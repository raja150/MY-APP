using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;

namespace Transmart.Services.UnitTests.Services.Payroll.PayRollData
{
	public static class SetData
	{
		public static void MockArrear(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Arrear>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Arrear>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Arrear>()).Returns(repository);
		}
		public static void MockFinancialYear(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<FinancialYear>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<FinancialYear>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<FinancialYear>()).Returns(repository);
		}
		public static void MockDeclaration(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Declaration>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Declaration>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Declaration>()).Returns(repository);
		}
		public static void MockDeclarationSettings (Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeclarationSetting>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeclarationSetting>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeclarationSetting>()).Returns(repository);
		}
		public static void MockProfessionalTaxSlab(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ProfessionalTaxSlab>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ProfessionalTaxSlab>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ProfessionalTaxSlab>()).Returns(repository);
		}
		public static void MockDeductionComponent(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeductionComponent>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeductionComponent>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeductionComponent>()).Returns(repository);
		}
		public static void MockEarningComponent(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EarningComponent>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EarningComponent>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EarningComponent>()).Returns(repository);
		}
		public static void MockEmpBonus(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpBonus>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpBonus>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpBonus>()).Returns(repository);
		}
		public static void MockEmployeeDataGenerator(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}
		public static void MockEmployeePayInfoList(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePayInfo>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePayInfo>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePayInfo>()).Returns(repository);
		}
		public static void MockEmployeePayInfoAuditList(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePayInfoAudit>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePayInfoAudit>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePayInfoAudit>()).Returns(repository);
		}
		public static void MockBank(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Bank>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Bank>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Bank>()).Returns(repository);
		}
		public static void MockEmployeePayInfoAudit(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePayInfoAudit>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePayInfoAudit>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePayInfoAudit>()).Returns(repository);
		}
		public static void MockEmpStatutory(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpStatutory>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpStatutory>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpStatutory>()).Returns(repository);
		}
		public static void MockESI(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ESI>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ESI>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ESI>()).Returns(repository);
		}
		public static void MockPayMonth(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PayMonth>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PayMonth>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PayMonth>()).Returns(repository);
		}
		public static void MockPaySettings(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySettings>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySettings>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySettings>()).Returns(repository);
		}
		public static void MockOrganizations(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Organizations>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Organizations>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Organizations>()).Returns(repository);
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
		public static void MockEPF(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic epf)
		{
			context.Setup(x => x.Set<EPF>()).Returns(epf.Object);
			var repository = new RepositoryAsync<EPF>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EPF>()).Returns(repository);
		}
		public static void MockHRADeclaration(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HraDeclaration>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HraDeclaration>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HraDeclaration>()).Returns(repository);
		}
		public static void MockSection80C(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section80C>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section80C>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section80C>()).Returns(repository);
		}
		public static void MockSection80D(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section80D>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section80D>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section80D>()).Returns(repository);
		}
		public static void MockSection6AOther(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6AOther>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6AOther>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6AOther>()).Returns(repository);
		}
		public static void MockPrevEmployment(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PrevEmployment>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PrevEmployment>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PrevEmployment>()).Returns(repository);
		}
		public static void MockOtherIncomeSources(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<OtherIncomeSources>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<OtherIncomeSources>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<OtherIncomeSources>()).Returns(repository);
		}
		public static void MockHomeLoanPay(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HomeLoanPay>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HomeLoanPay>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HomeLoanPay>()).Returns(repository);
		}
		public static void MockOtherSections(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<OtherSections>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<OtherSections>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<OtherSections>()).Returns(repository);
		}
		public static void MockSection6A80D(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80D>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80D>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80D>()).Returns(repository);
		}
		public static void MockSection6A80C(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80C>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80C>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80C>()).Returns(repository);
		}
		public static void MockLetOutProperty(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LetOutProperty>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LetOutProperty>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LetOutProperty>()).Returns(repository);
		}
		public static void MockPaySheet(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheet>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySheet>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repository);
		}
		public static void MockSalary(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Salary>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Salary>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Salary>()).Returns(repository);
		}
		public static void MockDeclarationAllowance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeclarationAllowance>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeclarationAllowance>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeclarationAllowance>()).Returns(repository);
		}
		public static void MockSalaryDeduction(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<SalaryDeduction>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<SalaryDeduction>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<SalaryDeduction>()).Returns(repository);
		}
		public static void MockSalaryEarning(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<SalaryEarning>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<SalaryEarning>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<SalaryEarning>()).Returns(repository);
		}
		
		public static void MockSection6A80CWages(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Section6A80CWages>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Section6A80CWages>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Section6A80CWages>()).Returns(repository);
		}
		public static void MockLatecomers(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Latecomers>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Latecomers>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Latecomers>()).Returns(repository);
		}



	}
}
