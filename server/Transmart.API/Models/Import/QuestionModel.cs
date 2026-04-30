using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
	public class QuestionModel
	{
		[DataImport(Name = "Question", Order = 1)]
		public string Question { get; set; }

		[DataImport(Name = "Type", Order = 2)]
		public string Type { get; set; }

		[DataImport(Name = "Key", Order = 3)]
		public string Key { get; set; }

		[DataImport(Name = "Option A", Order = 4)]
		public string OptionA { get; set; }

		[DataImport(Name = "Option B", Order = 5)]
		public string OptionB { get; set; }

		[DataImport(Name = "Option C", Order = 6, Required = false)]
		public string OptionC { get; set; }

		[DataImport(Name = "Option D", Order = 7, Required = false)]
		public string OptionD { get; set; }

		[DataImport(Name = "Option E", Order = 8, Required = false)]
		public string OptionE { get; set; }

		[DataImport(Name = "Option F", Order = 9, Required = false)]
		public string OptionF { get; set; }

		[DataImport(Name = "Error", Order = 10, Required = false, ForError = true)]
		public string Error { get; set; }

	}
}
