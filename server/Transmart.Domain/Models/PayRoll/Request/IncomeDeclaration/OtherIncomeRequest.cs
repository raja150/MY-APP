using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class OtherIncomeRequest : BaseModel
    {
        public Guid DeclarationId { get; set; }
        public decimal OtherSources { get; set; }
        public decimal InterestOnSaving { get; set; }
        public decimal InterestOnFD { get; set; }
    } 
}
