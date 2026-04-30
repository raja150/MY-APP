using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.SelfService.Request
{
    public partial class TicketRequest : BaseModel
    {
        public Guid RaiseById { get; set; }
        public DateTime RaisedOn { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid HelpTopicId { get; set; }
        public Guid SubTopicId { get; set; }
        public Guid? AssignedToId { get; set; }
#nullable enable
		public IFormFile? File { get; set; }
#nullable disable

	}
	public class UserRespnoseModel
	{
		public Guid TicketId { get; set; }
		public string Response { get; set; }
	}
}
