using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class FunctionalAreaList : BaseModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
