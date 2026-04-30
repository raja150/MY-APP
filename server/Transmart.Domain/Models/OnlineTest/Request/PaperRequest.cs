using FluentValidation;
using System;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class PaperRequest : BaseModel
	{
		public string Name { get; set; }
		public Guid OrganiserId { get; set; }
		public Int16 Duration { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public bool IsJumbled { get; set; }
		public bool MoveToLive { get; set; }
		public bool Status { get; set; }
		public bool ShowResult { get; set; }
	}

	public class OnlineTestRequestValidator : AbstractValidator<PaperRequest>
	{
		public OnlineTestRequestValidator()
		{
			RuleFor(m => m.Name).NotNull().MaximumLength(128).WithName("Test Name");

			RuleFor(m => m.Duration).GreaterThanOrEqualTo((short)15)
				.WithName("Duration").WithMessage("Duration Greater than or equal to 15 min");

			RuleFor(m => m.StartAt).GreaterThanOrEqualTo(DateTime.Today).When(x => x.ID == Guid.Empty)
				.WithName("Start Date")
				.WithMessage("Start date greater than or equal to Present date");
			RuleFor(m => m.StartAt).GreaterThanOrEqualTo(DateTime.Today).When(x => x.MoveToLive);
			RuleFor(m => m.StartAt).NotEmpty().NotNull().When(x => !x.MoveToLive);

			RuleFor(m => m.EndAt).GreaterThanOrEqualTo(DateTime.Today).When(x => x.ID == Guid.Empty)
				.WithName("End Date").WithMessage("End date greater than or equal to start date");

			RuleFor(m => m.Status).NotEmpty().When(x => x.MoveToLive).WithMessage("Status must be enable when Paper is moving to live");
		}
	}
}
