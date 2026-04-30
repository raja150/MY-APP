using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ShiftList : BaseModel
    {
        public string Name { get; set; }
        public int StartFrom { get; set; }
        public int EndsOn { get; set; }
        public int? loginGraceTime { get; set; }
        public int? logoutGraceTime { get; set; }
        public int? Allowance { get; set; }
        public int BreakTime { get; set; }
        public string Desciption { get; set; }
        public bool Status { get; set; }
    }
}
