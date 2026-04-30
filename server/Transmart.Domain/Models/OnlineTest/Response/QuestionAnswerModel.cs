using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class QuestionAnswerModel : BaseModel
	{
		public string Text { get; set; }
		public byte Type { get; set; }
		public ICollection<ChoiceModel> Choices { get; set; }
		public string UserAnswer { get; set; }
		public string CorrectAnswer { get; set; }
	}
}
