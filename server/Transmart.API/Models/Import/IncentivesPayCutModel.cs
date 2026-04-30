using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.API.Models.Import
{
    public class IncentivesPayCutModel
    {
        [DataImportAttribute(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

		[DataImportAttribute(Name = "Fax Files and Arrears", Order = 2)]
		public int FaxFilesAndArrears { get; set; }
		[DataImportAttribute(Name = "Sunday Incentives", Order = 3)]
		public int SundayInc { get; set; }
		[DataImportAttribute(Name = "Production Incentives", Order = 4)]
		public int ProductionInc { get; set; }
		[DataImportAttribute(Name = "Spot Incentives", Order = 5)]
		public int SpotInc { get; set; }
		[DataImportAttribute(Name = "Punctuality Incentive", Order = 6)]
		public int PunctualityInc { get; set; }
		[DataImportAttribute(Name = "Centum Club/Double Centum Club", Order = 7)]
		public int CentumClub { get; set; }
		[DataImportAttribute(Name = "First Minute Incentive", Order = 8)]
		public int FirstMinuteInc { get; set; }
		[DataImportAttribute(Name = "Other Incentives", Order = 9)]
		public int OtherInc { get; set; }
		[DataImportAttribute(Name = "Night Shift", Order = 10)]
		public int NightShift { get; set; }
		[DataImportAttribute(Name = "Weekly Star Incentives", Order = 11)]
		public int WeeklyStarInc { get; set; }
		[DataImportAttribute(Name = "Transition Team Incentives", Order = 12)]
		public int TTeamInc { get; set; }
		[DataImportAttribute(Name = "Double Pay", Order = 13)]
		public int DoublePay { get; set; }
		[DataImportAttribute(Name = "Internal Quality Feedback", Order = 14)]
		public int InternalQualityFeedbackDed { get; set; }
		[DataImportAttribute(Name = "External Quality Feedback", Order = 15)]
		public int ExternalQualityFeedbackDed { get; set; }
		[DataImportAttribute(Name = "Late Coming Deductions", Order = 16)]
		public int LateComingDed { get; set; }
		[DataImportAttribute(Name = "Unauthorized Leave Deduction", Order = 17)]
		public int UnauthorizedLeaveDed { get; set; }
		[DataImportAttribute(Name = "Other Deductions", Order = 18)]
		public int OtherDed { get; set; }
		[DataImport(Name = "Error", Order = 19, Required = false, ForError = true)]
        public string Error { get; set; }   
    }
}
