using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EmpStatutoryModel : BaseModel
    {
        public Guid EmpId { get; set; }
        [Required]
        public int EnablePF { get; set; }
        public string EmployeesProvid { get; set; }
        public string UAN { get; set; }
        public int? EmployeeContrib { get; set; }
        [Required]
        public bool EPS { get; set; }
        [Required]
        public int EnableESI { get; set; }
        public string ESINo { get; set; }
    }
    public class EmpStatutoryModelValidator : AbstractValidator<EmpStatutoryModel>
    {
        public EmpStatutoryModelValidator()
        {
            RuleFor(c => c.EmployeesProvid).MaximumLength(1024).WithName("Employees Provident Fund A/C No");
            RuleFor(c => c.UAN).MaximumLength(1024).WithName("UAN");
            RuleFor(c => c.ESINo).MaximumLength(1024).WithName("Employees State Insurance");
        }
    }
}
