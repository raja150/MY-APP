using System;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DeskDepartmentList : BaseModel
    {
        public string Department { get; set; }
        public string Manager { get; set; }
        public bool Status { get; set; }
    }
}
