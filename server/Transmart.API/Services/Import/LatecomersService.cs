using System.Collections.Generic;
using System;
using TranSmart.API.Models.Import;
using System.Linq;
using System.IO;

namespace TranSmart.API.Services.Import
{
	public interface ILatecomersService : IImportBaseService<LatecomersModel>
	{
		MemoryStream Latecomers(Dictionary<string, Dictionary<string, string>> DictionaryList);
	}
	public class LatecomersService : ImportBaseService<LatecomersModel> , ILatecomersService
	{
		public LatecomersService()
		{

		}
		public MemoryStream Latecomers(Dictionary<string, Dictionary<string, string>> DictionaryList)
		{

			return ClosedXmlGeneric.DataExport("Arrear", DictionaryList);
		}
		public override IEnumerable<LatecomersModel> ToModel(string path, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<LatecomersModel>>();
			if (System.IO.File.Exists(path))
			{
				data = ClosedXmlGeneric.Import<LatecomersModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<LatecomersModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			else { throw new FileNotFoundException("File not found"); }
			return new List<LatecomersModel>();
		}
		public override IEnumerable<LatecomersModel> ToModel(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<LatecomersModel>>();
			try
			{
				data = ClosedXmlGeneric.Import<LatecomersModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<LatecomersModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<LatecomersModel>();
		}
		public override MemoryStream Sample()
		{
			return ClosedXmlGeneric.Export<LatecomersModel>("Latecomers", new List<LatecomersModel>());
		}
		public override bool ValidateHeaders(Stream stream)
		{
			var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(LatecomersModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<LatecomersModel>(stream);

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
