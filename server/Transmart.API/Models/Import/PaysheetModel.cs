using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class PaysheetModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmpCode { get; set; }

        [DataImport(Name = "Month", Order = 2)]
        public int Month { get; set; }

        [DataImport(Name = "Year", Order = 3)]
        public int Year { get; set; }

        [DataImport(Name = "Present Days", Order = 5)]
        public decimal PresentDays { get; set; }

        [DataImport(Name = "LOP Days", Order = 6)]
        public decimal LOPDays { get; set; }

        [DataImport(Name = "UA Days", Order = 7)]
        public decimal UADays { get; set; }

        [DataImport(Name = "Salary", Order = 8)]
        public int Salary { get; set; }

        [DataImport(Name = "Gross", Order = 9)]
        public int Gross { get; set; }

        [DataImport(Name = "Gross Taxable", Order = 10)]
        public int GrossTaxable { get { return Gross - PayCut - LOP; } }

        [DataImport(Name = "Deduction", Order = 11)]
        public int Deduction { get; set; }

        [DataImport(Name = "Net", Order = 12)]
        public int Net { get; set; }

        [DataImport(Name = "Incentive", Order = 13)]
        public int Incentive { get; set; }

        [DataImport(Name = "Arrears", Order = 14)]
        public int Arrears { get; set; }

        [DataImport(Name = "LOP", Order = 15)]
        public int LOP { get; set; }

        [DataImport(Name = "Pay cut", Order = 16)]
        public int PayCut { get; set; }

        [DataImport(Name = "EPF", Order = 17)]
        public int EPF { get; set; }

        [DataImport(Name = "ESI", Order = 18)]
        public int ESI { get; set; }

        [DataImport(Name = "ESI Applied", Order = 19)]
        public bool ESIApplied { get; set; }

        [DataImport(Name = "PTax", Order = 20)]
        public int PTax { get; set; }

        [DataImport(Name = "Tax", Order = 21)]
        public int Tax { get; set; }

        [DataImport(Name = "Hold", Order = 22)]
        public bool Hold { get; set; }

        [DataImport(Name = "EPF No", Order = 23)]
        public string EPFNo { get; set; }

        [DataImport(Name = "ESI No", Order = 24)]
        public string ESINo { get; set; }

        [DataImport(Name = "Bank Name", Order = 25)]
        public string BankName { get; set; }

        [DataImport(Name = "Bank IFSC", Order = 26)]
        public string BankIFSC { get; set; }

        [DataImport(Name = "Bank AcNo", Order = 27)]
        public string BankACNo { get; set; }

        [DataImport(Name = "Loan", Order = 29)]
        public int Loan { get; set; }

    }
}
