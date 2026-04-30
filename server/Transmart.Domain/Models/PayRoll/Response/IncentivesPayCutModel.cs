using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Response
{
    public class IncentivesPayCutModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
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
	}
}
