using FluentValidation;
using System;

namespace TranSmart.Domain.Models.PayRoll.Request
{
    public class IncentivesPayCutRequest : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? PayMonthId { get; set; }
		public int FaxFilesAndArrears { get; set; }
		public int SundayInc { get; set; }
		public int ProductionInc { get; set; }
		public int SpotInc { get; set; }
		public int PunctualityInc { get; set; }
		public int CentumClub { get; set; }
		public int FirstMinuteInc { get; set; }
		public int OtherInc { get; set; }
		public int NightShift { get; set; }
		public int WeeklyStarInc { get; set; }
		public int TTeamInc { get; set; }
		public int DoublePay { get; set; }
		public int InternalQualityFeedbackDed { get; set; }
		public int ExternalQualityFeedbackDed { get; set; }
		public int LateComingDed { get; set; }
		public int UnauthorizedLeaveDed { get; set; }
		public int OtherDed { get; set; }


	}
	public class IncentivesPayCutRequestModelValidator : AbstractValidator<IncentivesPayCutRequest>
    {
        public IncentivesPayCutRequestModelValidator()
        {
            RuleFor(m => m.EmployeeId).NotNull().When(x => x.EmployeeId == Guid.Empty).WithName("EmployeeId").WithMessage(Resource.Employee_No_Is_Required);
            RuleFor(c => c.PayMonthId).NotNull().When(x => x.ID == Guid.Empty).WithMessage(Resource.PayMonth_Is_Required);
			RuleFor(c => c.FaxFilesAndArrears).GreaterThanOrEqualTo(0).WithMessage(Resource.Fax_Files_And_Arrears_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.SundayInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Sunday_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.ProductionInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Production_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.SpotInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Spot_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.PunctualityInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Punctuality_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.CentumClub).GreaterThanOrEqualTo(0).WithMessage(Resource.Centum_Club_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.FirstMinuteInc).GreaterThanOrEqualTo(0).WithMessage(Resource.First_Minute_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.OtherInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Other_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.NightShift).GreaterThanOrEqualTo(0).WithMessage(Resource.Night_Shift_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.WeeklyStarInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Weekly_Star_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.TTeamInc).GreaterThanOrEqualTo(0).WithMessage(Resource.Transition_Team_Incentives_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.DoublePay).GreaterThanOrEqualTo(0).WithMessage(Resource.Double_Pay_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.InternalQualityFeedbackDed).GreaterThanOrEqualTo(0).WithMessage(Resource.Internal_Quality_Feedback_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.ExternalQualityFeedbackDed).GreaterThanOrEqualTo(0).WithMessage(Resource.External_Quality_Feedback_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.LateComingDed).GreaterThanOrEqualTo(0).WithMessage(Resource.Late_Coming_Deduction_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.UnauthorizedLeaveDed).GreaterThanOrEqualTo(0).WithMessage(Resource.Unauthorized_Leave_Deduction_Must_Be_Greater_Than_Or_Equal_To_0);
			RuleFor(c => c.OtherDed).GreaterThanOrEqualTo(0).WithMessage(Resource.Other_Deduction_Must_Be_Greater_Than_Or_Equal_To_0);
		}
	}
}
