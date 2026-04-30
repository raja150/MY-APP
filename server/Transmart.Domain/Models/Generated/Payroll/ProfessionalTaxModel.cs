using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class ProfessionalTaxModel : BaseModel
    {
        public Guid StateId { get; set; }
    }
    public class ProfessionalTaxModelValidator : AbstractValidator<ProfessionalTaxModel>
    {
        public ProfessionalTaxModelValidator()
        {
        }
    }
}
