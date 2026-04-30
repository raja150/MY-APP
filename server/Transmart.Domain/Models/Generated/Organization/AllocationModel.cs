using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class AllocationModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? WeekOffSetupId { get; set; }
        public Guid? WorkHoursSettingId { get; set; }
    }
    public class AllocationModelValidator : AbstractValidator<AllocationModel>
    {
        public AllocationModelValidator()
        {
        }
    }
}
