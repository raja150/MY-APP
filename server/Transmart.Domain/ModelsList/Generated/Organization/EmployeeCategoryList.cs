using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeCategoryList : BaseModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
