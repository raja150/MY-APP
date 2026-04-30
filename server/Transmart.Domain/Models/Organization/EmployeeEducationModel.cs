using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
   public partial class EmployeeEducationModel
    {

    }
    public partial class EmployeeEducationModalValidator:AbstractValidator<EmployeeEducationModel>
    {
        public EmployeeEducationModalValidator()
        {
            RuleFor(m => m.YearOfPassing).LessThanOrEqualTo(DateTime.Now.Year).WithMessage(Resource.Year_of_Passing_Should_be_Less_than_Current_Date);
        }
    }
}
