using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface ISalaryImportService : IImportBaseService<SalaryDetailsModel>
	{
		Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1);
	}
	public class SalaryImportService : ImportBaseService<SalaryDetailsModel>, ISalaryImportService
	{
		public SalaryImportService()
		{

		}

		public override IEnumerable<SalaryDetailsModel> ToModel(string path, int sheetNo = 1)
		{
			if (System.IO.File.Exists(path))
			{
				var data = ClosedXmlGeneric.Import<SalaryDetailsModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<SalaryDetailsModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			else { throw new FileNotFoundException("File not found"); }
			return new List<SalaryDetailsModel>();
		}
		public override IEnumerable<SalaryDetailsModel> ToModel(Stream stream, int sheetNo = 1)
		{
			try
			{
				var data = ClosedXmlGeneric.Import<SalaryDetailsModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<SalaryDetailsModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmployeeCode != null);
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return new List<SalaryDetailsModel>();
		}

		public Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<int, Dictionary<string, string>>();
			try
			{
				var returnData = ClosedXmlGeneric.Import(stream).ToList();
				foreach (KeyValuePair<int, Dictionary<string, string>> item in returnData)
				{
					data.Add(item.Key, item.Value);
				}
				if (data.Count >= sheetNo)
				{ 
					return data;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new Dictionary<int, Dictionary<string, string>>();
		}

		public override MemoryStream Sample()
		{
			return ClosedXmlGeneric.Export<SalaryDetailsModel>("Salary", new List<SalaryDetailsModel>());
		}
		public MemoryStream Sample(Dictionary<int, string> dictionary)
		{
			return ClosedXmlGeneric.Export("Salary", dictionary);
		}
	}
}
