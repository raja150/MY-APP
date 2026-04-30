using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class OtherIncomeModel : BaseModel
    {
        public Guid DeclarationId { get; set; }
        public int OtherSources { get; set; }
        public int InterestOnSaving { get; set; }
        public int InterestOnFD { get; set; }
    }
}
