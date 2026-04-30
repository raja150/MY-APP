using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
	[Table("_UserPasswords")]
	public class UserPasswords : BaseEntity
	{
		public Guid UserId { get; set; }
		public User User { get; set; }
		public string Password { get; set; }
		public DateTime ChangedAt { get; set; }
	}
}
