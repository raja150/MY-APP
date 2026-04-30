using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
	public interface IAddAttendanceService : IImportBaseService<AddAttendanceModel>
	{

	}
	public class AddAttendanceService : ImportBaseService<AddAttendanceModel>, IAddAttendanceService
	{
		public AddAttendanceService()
		{
		}
		public override IEnumerable<AddAttendanceModel> ToModel(string path, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<AddAttendanceModel>>();
			if (System.IO.File.Exists(path))
			{
				data = ClosedXmlGeneric.Import<AddAttendanceModel>(path);
				if (data.Count >= sheetNo)
				{
					IEnumerable<AddAttendanceModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmpCode != null);
				}
			}
			else { throw new FileNotFoundException("File not found"); }
			return new List<AddAttendanceModel>();
		}
		public override IEnumerable<AddAttendanceModel> ToModel(Stream stream, int sheetNo = 1)
		{
			var data = new Dictionary<string, IList<AddAttendanceModel>>();
			try
			{
				data = ClosedXmlGeneric.Import<AddAttendanceModel>(stream);
				if (data.Count >= sheetNo)
				{
					IEnumerable<AddAttendanceModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
					return values.Where(x => x.EmpCode != null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return new List<AddAttendanceModel>();
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
			return ClosedXmlGeneric.Export<AddAttendanceModel>("AddAttendance", new List<AddAttendanceModel>());
		}

		public override bool ValidateHeaders(Stream stream)
		{
			var columnsList = ClosedXmlGeneric.GetColomnList(typeof(AddAttendanceModel));
			bool valid = true;
			Dictionary<int, string> headers = ClosedXmlGeneric.Header<AddAttendanceModel>(stream);

			for (int i = 0; i < columnsList.Count; i++)
			{
				if (!headers.ContainsValue(columnsList[i].Attribute.GetName()))
				{
					valid = false;
					break;
				}
			}

			return valid;
		}
	}
}
