using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Enums;

namespace TranSmart.Domain.Models.Payroll
{
    public class PayMonthModel : BaseModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
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
                    (int)PayMonthStatus.Hold => "Hold",
                    _ => "",
                };
            }
        }
        public decimal Days { get; set; }
        public int Cost { get; set; }
        public int Net { get; set; }
        public int Employees { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
