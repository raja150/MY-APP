using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface IEmpStatutoryImportService : IImportBaseService<EmpStatutoryModel>
	{
		MemoryStream Sample(Dictionary<int, string> dictionary);
		Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1);
	}
	public class EmpStatutoryImportService : ImportBaseService<EmpStatutoryModel>, IEmpStatutoryImportService
	{
		public EmpStatutoryImportService()
		{

		}
		public Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1)
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
			return ClosedXmlGeneric.Export<EmpStatutoryModel>("EPF ESI Details", new List<EmpStatutoryModel>());
		}
		public MemoryStream Sample(Dictionary<int, string> dictionary)
		{

			return ClosedXmlGeneric.Export("Emp_StatutoryDetails", dictionary);
		}
	}
}
