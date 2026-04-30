using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface ICodingIncentivesService : IImportBaseService<CodingIncentivesModel>
	{

	}
	public class CodingIncentivesService : ImportBaseService<CodingIncentivesModel>, ICodingIncentivesService
	{
		public CodingIncentivesService()
		{

		}
		public override IEnumerable<CodingIncentivesModel> ToModel(string path, int sheetNo = 1)
		{
			if (System.IO.File.Exists(path))
			{
				var data = ClosedXmlGeneric.Import<CodingIncentivesModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<CodingIncentivesModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			else { throw new FileNotFoundException("File not found"); }
			return new List<CodingIncentivesModel>();
		}
		public override IEnumerable<CodingIncentivesModel> ToModel(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<CodingIncentivesModel>>();
			try
			{
				data = ClosedXmlGeneric.Import<CodingIncentivesModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<CodingIncentivesModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<CodingIncentivesModel>();
		}

		public override MemoryStream Sample()
		{
			return ClosedXmlGeneric.Export<CodingIncentivesModel>("Leave Balance", new List<CodingIncentivesModel>());
		}
		public override bool ValidateHeaders(Stream stream)
		{
			var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(CodingIncentivesModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<CodingIncentivesModel>(stream);

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
	}
}
