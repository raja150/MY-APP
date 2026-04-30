using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DepartmentGroupModel : BaseModel
    {
        public Guid DeskDepartmentId { get; set; }
        public Guid GroupsId { get; set; }
    }
    public class DepartmentGroupModelValidator : AbstractValidator<DepartmentGroupModel>
    {
        public DepartmentGroupModelValidator()
        {
        }
    }
}
