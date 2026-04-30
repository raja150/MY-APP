using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_Question")]
	public class Question : DataGroupEntity
	{
		public Int32 SNo { get; set; }
		public string Text { get; set; }
		public byte Type { get; set; }
		public ICollection<Choice> Choices { get; set; }
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public bool IsDelete { get; set; }

	}

	[Table("OT_Choice")]
	public class Choice : DataGroupEntity
	{
		public Guid QuestionId { get; set; }
		public Question Question { get; set; }
		public byte SNo { get; set; }
		public string Text { get; set; }
	}
}
