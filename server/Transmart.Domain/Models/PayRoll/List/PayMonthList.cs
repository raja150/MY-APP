using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Enums;

namespace TranSmart.Domain.Models.Payroll
{
    public class PayMonthList : BaseModel
    {
        public Guid FinancialYearId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; } 
        public string Name { get; set; } 
        public decimal Days { get; set; }
        public int Status { get; set; }
        public string StatusTxt
        {
            get
            {
                return Status switch
                {
                    (int)PayMonthStatus.InActive => "In Active",
                    (int)PayMonthStatus.Active => "Active",
                    (int)PayMonthStatus.Open => "Open",
                    (int)PayMonthStatus.InProcess => "In process",
                    (int)PayMonthStatus.Released => "Released",
                    (int)PayMonthStatus.Hold=>"Hold",
                    _ => "",
                };
            }
        }
    }
}
