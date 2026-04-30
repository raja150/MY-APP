using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace TranSmart.Domain.Models.PayRoll.Response
{
	public partial class EmployeePayInfoModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; }
		public string No { get; set; }
		[Required]
		public int PayMode { get; set; }
		public Guid BankId { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		public string AccountNo { get; set; }
		public string AccountNoVerify { get; set; }
	}
	public class EmployeePayInfoModelValidator : AbstractValidator<EmployeePayInfoModel>
	{
		public EmployeePayInfoModelValidator()
		{
			RuleFor(c => c.No).MaximumLength(1024).WithName("Employee Code");
			RuleFor(c => c.AccountNo).MaximumLength(20).WithName("Account Number");
			RuleFor(c => c.AccountNoVerify).NotNull().NotEmpty().Matches(c => c.AccountNo).When(p => p.PayMode != 2).WithMessage(Resource.Both_Account_Numbers_Need_To_Be_Same);
		}
	}
}
