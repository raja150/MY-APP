using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeContactList : BaseModel
    {
        public string PersonName { get; set; }
        public int HumanRelation { get; set; }
        public string ContactNo { get; set; }
    }
}
