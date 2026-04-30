using System;

namespace TranSmart.Domain.Models.Reports
{
	public partial class IncentivesPayCutReportModel : BaseModel
	{
		public string EmpCode { get; set; }
		public string Name { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public string Department { get; set; }
		public int Month { get; set; }
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
		public decimal TaxAmount { get; set; }
		public string PAN { get; set; }


	}
}
