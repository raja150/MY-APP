using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TranSmart.API.Services;
using TranSmart.API.Services.Import;

namespace Transmart.Controller.UnitTests.ClosedXml
{
	public interface ITestService : IImportBaseService<TestModel>
	{
		MemoryStream Arrear(Dictionary<string, Dictionary<string, string>> DictionaryList);
	}
	public class TestService : ImportBaseService<TestModel>, ITestService
	{
		public TestService()
		{

		}
		public MemoryStream Arrear(Dictionary<string, Dictionary<string, string>> DictionaryList)
		{

			return ClosedXmlGeneric.DataExport("Test", DictionaryList);
		}
		public override IEnumerable<TestModel> ToModel(string path, int sheetNo = 1)
		{
			if (File.Exists(path))
			{
				var data = ClosedXmlGeneric.Import<TestModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<TestModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			else
			{
				throw new FileNotFoundException("File not found");
			}
			return new List<TestModel>();
		}
		public override IEnumerable<TestModel> ToModel(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<TestModel>>();
			try
			{
				data = ClosedXmlGeneric.Import<TestModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<TestModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<TestModel>();
		}

		public override MemoryStream Sample()
		{
			return ClosedXmlGeneric.Export("Test", new List<TestModel>());
		}
		public override bool ValidateHeaders(Stream stream)
		{
			var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(TestModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<TestModel>(stream);

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
