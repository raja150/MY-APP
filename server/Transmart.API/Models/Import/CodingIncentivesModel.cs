using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class CodingIncentivesModel
    {
        [DataImportAttribute(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImportAttribute(Name = "Production Incentive", Order = 2)]
        public int ProductionIncentive { get; set; }

        [DataImportAttribute(Name = "Total Incentive", Order = 3)]
        public int TotalIncentive { get; set; }

        [DataImportAttribute(Name = "Internal Pay cut", Order = 4)]
        public int InternalPaycut { get; set; }
        [DataImportAttribute(Name = "External Pay cut", Order = 5)]
        public int ExternalPaycut { get; set; }

        [DataImportAttribute(Name = "Total Deduction", Order = 6)]
        public int TotalDeduction { get; set; }

        [DataImportAttribute(Name = "Night Shift Incentive", Order = 7)]
        public int NightShiftIncentive { get; set; }

        [DataImportAttribute(Name = "Spot Incentive", Order = 8)]
        public int SpotIncentive { get; set; }

        [DataImportAttribute(Name = "Star Employee Incentive", Order = 9)]
        public int StarEmployeeIncentive { get; set; }

        [DataImportAttribute(Name = "Other Incentive", Order = 10)]
        public int OtherIncentive { get; set; }
        [DataImportAttribute(Name = "Arrears", Order = 11)]
        public int Arrears { get; set; }
        [Display(Name = "Unauthorized Leaves", Order = 12)]
        public int UnauthorizedLeaves { get; set; }

        [Display(Name = "LateComing Deduction", Order = 13)]
        public int LateComingDeduction { get; set; }

        [DataImportAttribute(Name = "Total Deduction", Order = 14)]
        public int AmountNotConsideredForInternalPaycut { get; set; }

        [DataImportAttribute(Name = "Comments", Order = 15)]
        public string Comments { get; set; }

        [DataImportAttribute(Name = "Year", Order = 16)]
        public int Year { get; set; }

        [DataImportAttribute(Name = "Month", Order = 17)]
        public int Month { get; set; }

        [DataImport(Name = "Error", Order = 99, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
