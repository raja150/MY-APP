using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_IncentivesPayCut")]
	public sealed class IncentivesPayCut : DataGroupEntity, IEquatable<IncentivesPayCut>
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Incentives { get; set; } 
		public int PayCut { get; set; }
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

		public bool Equals(IncentivesPayCut other)
		{
			if (other == null) return false;
			return EmployeeId.Equals(other.EmployeeId) &&
			  FaxFilesAndArrears.Equals(other.FaxFilesAndArrears) && SundayInc.Equals(other.SundayInc)
			  && ProductionInc.Equals(other.ProductionInc) && SpotInc.Equals(other.SpotInc)
			  && PunctualityInc.Equals(other.PunctualityInc) && CentumClub.Equals(other.CentumClub)
			  && FirstMinuteInc.Equals(other.FirstMinuteInc) && OtherInc.Equals(other.OtherInc)
			  && NightShift.Equals(other.NightShift) && WeeklyStarInc.Equals(other.WeeklyStarInc)
			  && TTeamInc.Equals(other.TTeamInc) && DoublePay.Equals(other.DoublePay)
			  && InternalQualityFeedbackDed.Equals(other.InternalQualityFeedbackDed) && ExternalQualityFeedbackDed.Equals(other.ExternalQualityFeedbackDed)
			  && LateComingDed.Equals(other.LateComingDed) && UnauthorizedLeaveDed.Equals(other.UnauthorizedLeaveDed)
			  && OtherDed.Equals(other.OtherDed);
		}
		public void Update(IncentivesPayCut other)
		{
			FaxFilesAndArrears = other.FaxFilesAndArrears;
			SundayInc = other.SundayInc;
			ProductionInc = other.ProductionInc;
			SpotInc = other.SpotInc;
			PunctualityInc = other.PunctualityInc;
			CentumClub = other.CentumClub;
			FirstMinuteInc = other.FirstMinuteInc;
			OtherInc = other.OtherInc;
			NightShift = other.NightShift;
			WeeklyStarInc = other.WeeklyStarInc;
			TTeamInc = other.TTeamInc;
			DoublePay = other.DoublePay;
			InternalQualityFeedbackDed = other.InternalQualityFeedbackDed;
			ExternalQualityFeedbackDed = other.ExternalQualityFeedbackDed;
			LateComingDed = other.LateComingDed;
			UnauthorizedLeaveDed = other.UnauthorizedLeaveDed;
			OtherDed = other.OtherDed;
			Incentives = SumOfIncentives();
			PayCut = SumOfPayCut();
		}

		public int SumOfIncentives()
		{
			  return FaxFilesAndArrears + SundayInc + ProductionInc + SpotInc +
					PunctualityInc + CentumClub + FirstMinuteInc + OtherInc + NightShift +
					WeeklyStarInc + TTeamInc + DoublePay;
		}
		public int SumOfPayCut()
		{
			return InternalQualityFeedbackDed + ExternalQualityFeedbackDed
					+ UnauthorizedLeaveDed + LateComingDed + OtherDed;
		}
	}
}
