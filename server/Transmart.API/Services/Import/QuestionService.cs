using System.Linq;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface IQuestionService : IImportBaseService<QuestionModel>
	{

	}
	public class QuestionService : ImportBaseService<QuestionModel>, IQuestionService
	{
		public QuestionService()
		{

		}

		public override MemoryStream Sample()
		{
			return ClosedXmlGeneric.Export<QuestionModel>("Question", new List<QuestionModel>());
		}

		public override bool ValidateHeaders(Stream stream)
		{
			var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(QuestionModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<QuestionModel>(stream);

			for (int i = 0; i < colimnsList.Count; i++)
			{
				if (!headers.ContainsValue(colimnsList[i].Attribute.GetName()))
				{
					valid = false;
					break;
				}
			}

			return valid;
		}

		public override IEnumerable<QuestionModel> ToModel(Stream stream, int sheetNo = 1)
		{
			//var data = new Dictionary<string, IList<QuestionModel>>();
			try
			{
				var data = ClosedXmlGeneric.Import<QuestionModel>(stream);
				if (data.Count >= sheetNo)
				{
					return data.ElementAtOrDefault(sheetNo - 1).Value.Where(x => !string.IsNullOrEmpty(x.Question));
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<QuestionModel>();
		}
	}
}
