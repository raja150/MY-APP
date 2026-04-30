using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
	public class LetOutPropertyRequest : BaseModel
	{
		public string No { get; set; }
		public Guid DeclarationId { get; set; }
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
	public class LetOutPropertyRequestValidator : AbstractValidator<LetOutPropertyRequest>
	{
		public LetOutPropertyRequestValidator()
		{ 
			RuleFor(m => m.InterestPaid).GreaterThanOrEqualTo(1).When(x => x.RepayingHomeLoan).WithMessage("Interest Paid is Required");
			RuleFor(m => m.Principle).GreaterThanOrEqualTo(1).When(x => x.RepayingHomeLoan).WithMessage("Principle is Required");
			RuleFor(m => m.NameOfLender).NotEmpty().NotNull().When(x => x.RepayingHomeLoan).WithMessage("Name Of Lender is Required");
			RuleFor(m => m.LenderPAN).NotEmpty().NotNull().When(x => x.RepayingHomeLoan).WithMessage("Lender PAN is Required");
			RuleFor(m => m.NetIncome).NotEmpty().GreaterThanOrEqualTo(0).When(x => x.RepayingHomeLoan).WithMessage("Net income should be more then zero.");

		}
	}
}
