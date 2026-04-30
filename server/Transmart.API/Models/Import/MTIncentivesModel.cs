using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class MTIncentivesModel
    {
        [DataImportAttribute(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImportAttribute(Name = "Late Coming", Order = 2)]
        public int LateComing { get; set; }

        [DataImportAttribute(Name = "Internal FeedBack", Order = 3)]
        public int InternalFeedBack { get; set; }

        [DataImportAttribute(Name = "External FeedBack", Order = 4)]
        public int ExternalFeedBack { get; set; }

        [DataImportAttribute(Name = "Pay cut Total", Order = 5)]
        public decimal PayCutTotal { get; set; }

        [DataImportAttribute(Name = "Production", Order = 6)]
        public int Production { get; set; }

        [DataImportAttribute(Name = "Spot Incentives", Order = 7)]
        public int SpotIncentives { get; set; }

        [DataImportAttribute(Name = "Punctuality Incentive", Order = 8)]
        public int PunctualityIncentive { get; set; }

        [DataImportAttribute(Name = "Star Incentives", Order = 9)]
        public int StarIncentives { get; set; }

        [DataImportAttribute(Name = "CentumClub", Order = 10)]
        public int CentumClub { get; set; }

        [DataImportAttribute(Name = "FirstMin Incentive", Order = 11)]
        public int FirstMinIncentive { get; set; }

        [DataImportAttribute(Name = "Team Incentives", Order = 12)]
        public int TeamIncentives { get; set; }

        [DataImportAttribute(Name = "Fax Fail And Special Incentives", Order = 13)]
        public int FaxFilesAndSpecialIncentives { get; set; }

        [DataImportAttribute(Name = "Arrears", Order = 14)]
        public int Arrears { get; set; }

        [DataImportAttribute(Name = "Sunday", Order = 15)]
        public int Sunday { get; set; }

        [DataImportAttribute(Name = "Incentives Grand Total", Order = 16)]
        public decimal IncentivesGrandTotal { get; set; }
        [DataImportAttribute(Name = "Year", Order = 17)]
        public int Year { get; set; }

        [DataImportAttribute(Name = "Month", Order = 18)]
        public int Month { get; set; }
        [DataImport(Name = "Error", Order = 99, Required = false, ForError = true)]
        public string Error { get; set; }   
    }
}
