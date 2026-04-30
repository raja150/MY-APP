using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities
{
	[Table("_Tokens")]
	public class Token : DataGroupEntity
	{
		public string Key { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
	}
}
