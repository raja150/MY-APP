using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class StateList : BaseModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }
    }
}
