using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class RaiseTicketRequest : BaseModel
    {
        public Guid? EmployeeId { get; set; }
        public string TicketTitle { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public string Description { get; set; }

        public string Upload { get; set; }
#nullable enable
		public IFormFile? File { get; set; }
#nullable disable
	}
	public class RiaseTicketRequestValidator : AbstractValidator<RaiseTicketRequest>
    {
        public RiaseTicketRequestValidator()
        {
            RuleFor(x => x.TicketTitle).NotEmpty().NotNull().WithMessage("Required");
            RuleFor(x => x.Category).NotEqual(0).WithMessage("Required");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Required");
        }
    }
    public partial class RaiseTicketModel : BaseModel   
    {
        public Guid EmployeeId { get; set; }
        public string TicketTitle { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public string Upload { get; set; }  


    }
}
