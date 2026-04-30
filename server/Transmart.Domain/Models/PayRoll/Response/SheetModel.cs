using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Response
{
    public class SheetModel
    {
        public string Employee_Code { get; set; }
        public string EmployeeName { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime DOj { get; set; }
        public string PAN { get; set; }
        public int Salary { get; set; }
        public int SalaryEarned { get; set; }
        public int Basic { get; set; }
        public int HRA { get; set; }
        public int Medical { get; set; }
        public int FoodCoupons { get; set; }
        public int SpecialAllowance { get; set; }
        public int Incentives { get; set; }
        public int Arrears { get; set; }
        public int GrossEarnings { get; set; }
        public int Lop { get; set; }
        public int PayCut { get; set; }
        public int ESI { get; set; }
        public string EPFNo { get; set; }
        public int PTax { get; set; }
        public int Tax { get; set; }
        public int GrossDeductions { get; set; }
        public int Net { get; set; }    

    }
}
