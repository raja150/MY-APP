using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface IEmployeeService : IImportBaseService<EmployeeModel>
	{
		Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo);
	}
	public class EmployeeService : ImportBaseService<EmployeeModel>, IEmployeeService
	{
		public EmployeeService()
		{

		}

		public override IEnumerable<EmployeeModel> ToModel(string path, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<EmployeeModel>>();
			if (System.IO.File.Exists(path))
			{
				data = ClosedXmlGeneric.Import<EmployeeModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<EmployeeModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmpCode != null);
				}
			}
			else { throw new FileNotFoundException("File not found"); }
			return new List<EmployeeModel>();
		}
		public override IEnumerable<EmployeeModel> ToModel(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<EmployeeModel>>();
			try
			{
				data = ClosedXmlGeneric.Import<EmployeeModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<EmployeeModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmpCode != null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<EmployeeModel>();
		}
		public Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo)
		{
			var data = new Dictionary<int, Dictionary<string, string>>();
			try
			{
				var returndata = ClosedXmlGeneric.Import(stream).ToList();
				foreach (KeyValuePair<int, Dictionary<string, string>> item in returndata)
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
			return ClosedXmlGeneric.Export<EmployeeModel>("Employee", new List<EmployeeModel>());
		}

		public override bool ValidateHeaders(Stream stream)
		{
			var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(EmployeeModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<EmployeeModel>(stream);

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
