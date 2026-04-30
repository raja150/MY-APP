using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_QuestionAnswer")]
	public class QuestionAnswer : DataGroupEntity
	{
		public Guid QuestionId { get; set; }
		public Question Question { get; set; }
		public Guid? ChoiceId { get; set; }
		public Choice Choice { get; set; }
		public string AnswerTxt { get; set; }
	}
}
