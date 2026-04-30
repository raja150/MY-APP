using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Payroll;

namespace Transmart.Services.UnitTests.Services.Payroll.IntigrationTest
{
	public class LetOutPropertySheetModel
	{
		public Guid DeclarationId { get; set; }
		public Guid FinancialYearId { get; set; }
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int AnnualRentReceived { get; set; }
		public int MunicipalTaxPaid { get; set; }
		public int NetAnnualValue { get; set; }
		public int StandardDeduction { get; set; }
		public bool RepayingHomeLoan { get; set; }
		public int InterestPaid { get; set; }
		public int Principle { get; set; }
		public string NameOfLender { get; set; }
		public string LenderPAN { get; set; }
		public int NetIncome { get; set; }
	}
	public class Section6A80CSheetModel
	{
		public Guid DeclarationId { get; set; }
		public Guid Section80CId { get; set; }
		public string Section80CName { get; set; }
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int Amount { get; set; }
	}
	public class Section6A80DSheetModel
	{
		public Guid DeclarationId { get; set; }
		public Guid Section80DId { get; set; }
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public string Section80DName { get; set; }
		public int Amount { get; set; } 
	}
	
	public class HRADeclarationSheetModel
	{
		public Guid DeclarationId { get; set; }
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public DateTime RentalFrom{ get; set; }
		public DateTime RentalTo { get; set; }
		public int Amount { get; set; } 
		public string Address { get; set; }
		public string City { get; set; }
		public string Pan { get; set; }
		public string LandLord { get; set; }
	}
	public class HomeLoanPayPropertySheetModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public Guid DeclarationId { get; set; }
		public int InterestPaid { get; set; }
		public int Principle { get; set; }
		public string NameOfLender { get; set; }
		public string LenderPAN { get; set; }

	}

	public class PreviousEmploymentSheetModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int IncomeAfterException { get; set; }
		public int IncomeTax { get; set; }
		public int ProfessionalTax { get; set; }
		public int ProvisionalFund { get; set; }
		public int EncashmentExceptions { get; set; }

	}

	public class OtherIncomeSourcesSheetModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int OtherSources { get; set; }
		public int InterestOnSaving { get; set; }
		public int InterestOnFD { get; set; }

	}
	public class Section6AOtherSheetModel
	{
		public Guid DeclarationId { get; set; }
		public Guid OtherSectionsId { get; set; }
		public string OtherSectionName { get; set; }
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int Amount { get; set; } 
	}
	public class BonusSheetModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public DateTime AddedOn { get; set; }
		public int Amount { get; set; }
	}
}
