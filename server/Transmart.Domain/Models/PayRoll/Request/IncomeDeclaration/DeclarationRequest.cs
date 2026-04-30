using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class DeclarationRequest : BaseModel
    {
        public string No { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid FinancialYearId { get; set; }
        public bool IsNewRegime { get; set; }
        public List<HraDeclarationRequest> HRALines { get; set; }
        public HomeLoanPayRequest HomeLoanPay { get; set; }
        public ICollection<LetOutPropertyRequest> LetOutPropertyLines { get; set; }
        public ICollection<Section6A80CRequest> Section80CLines { get; set; }
        public ICollection<Section6A80DRequest> Section80DLines { get; set; }
        public ICollection<Section6AOtherRequest> SectionOtherLines { get; set; }
        public PrevEmploymentRequest PrevEmployment { get; set; }
        public OtherIncomeRequest IncomeSource { get; set; }
    }
    public class DeclarationRequestValidator : AbstractValidator<DeclarationRequest>
    {
        public DeclarationRequestValidator()
        {
            RuleForEach(m => m.HRALines).SetValidator(new HraDeclarationRequestValidator());
            RuleFor(m => m.HomeLoanPay).SetValidator(new HomeLoanPayRequestValidator());
            RuleForEach(m => m.LetOutPropertyLines).SetValidator(new LetOutPropertyRequestValidator());
            RuleForEach(m => m.Section80CLines).SetValidator(new Section6A80CRequestValidator());
            RuleForEach(m => m.Section80DLines).SetValidator(new Section6A80DRequestValidator());
            RuleForEach(m => m.SectionOtherLines).SetValidator(new SectionOtherRequestValidator()); 
        } 
    }

}
