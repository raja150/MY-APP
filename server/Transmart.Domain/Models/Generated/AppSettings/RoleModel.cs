using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.AppSettings
{
    public partial class RoleModel : BaseModel
    {
        public string Name { get; set; }
    }
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
