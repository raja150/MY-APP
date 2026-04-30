using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class SalarySummaryModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string Month { get; set; }
        public decimal Salary { get; set; }
        public int Incentive { get; set; }
        public int Arrears { get; set; }
        public int LOP { get; set; }
        public int UA { get; set; }
        public int LC { get; set; }
        public int PayCut { get; set; }
        public int EPF { get; set; }
        public int ESI { get; set; }
        public int PTax { get; set; }
        public int Tax { get; set; }
        public List<SalaryHeaderSummaryModel> Earnings { get; set; }
        public List<SalaryHeaderSummaryModel> Deductions { get; set; }
    }
    public class SalaryHeaderSummaryModel : BaseModel
    {
        public Guid SalaryId { get; set; }
        public Guid ComponentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Order { get; set; }
    }
}
