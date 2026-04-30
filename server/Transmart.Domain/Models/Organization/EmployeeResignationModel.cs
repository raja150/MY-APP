using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeResignationModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
		public DateTime? LeavingOn { get; set; }
    }
    public class EmployeeResignationManual : AbstractValidator<EmployeeResignationModel>
    {
        public EmployeeResignationManual()
        {
            RuleFor(x => x.ApprovedOn).GreaterThanOrEqualTo(x => x.ResignationOn).WithMessage("Aprroval date should be greater than resignation date");
        }
    }
}
