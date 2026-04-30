using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class PaySettingsList : BaseModel
    {
        public string Organization { get; set; }
        public int FYFromMonth { get; set; }
    }
}
