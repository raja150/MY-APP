using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveTypeList : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int PayType { get; set; }
        public int Gender { get; set; }
        public bool Status { get; set; }
    }
}
