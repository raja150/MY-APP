using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll
{
    public class PaySheetDeductionModel
    {
        public Guid PaySheetId { get; set; } 
        public string HeaderName { get; set; } 
        public int Salary { get; set; } 
        public int Deduction { get; set; }
        public Guid ComponentId { get; set; } 

    }
}
