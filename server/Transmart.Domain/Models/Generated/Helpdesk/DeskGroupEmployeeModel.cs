using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DeskGroupEmployeeModel : BaseModel
    {
        public Guid DeskGroupId { get; set; }
        public Guid EmployeeId { get; set; }
    }
    public class DeskGroupEmployeeModelValidator : AbstractValidator<DeskGroupEmployeeModel>
    {
        public DeskGroupEmployeeModelValidator()
        {
        }
    }
}
