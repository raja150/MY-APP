using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
    public interface IAttendanceStatusService : IImportBaseService<AttendanceStatusModel>
    {
        Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo);
    }
    public class AttendanceStatusService: ImportBaseService<AttendanceStatusModel>, IAttendanceStatusService
    {
        public AttendanceStatusService()
        {

        }

        public override IEnumerable<AttendanceStatusModel> ToModel(string path, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<AttendanceStatusModel>>();
            if (System.IO.File.Exists(path))
            {
                data = ClosedXmlGeneric.Import<AttendanceStatusModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<AttendanceStatusModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmpCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<AttendanceStatusModel>();
        }
        public override IEnumerable<AttendanceStatusModel> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<AttendanceStatusModel>>();
            try
            {
                data = ClosedXmlGeneric.Import<AttendanceStatusModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<AttendanceStatusModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmpCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<AttendanceStatusModel>();
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
			return ClosedXmlGeneric.Export<AttendanceStatusModel>("UpdateAttendance", new List<AttendanceStatusModel>());
		}

		public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(AttendanceStatusModel));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<AttendanceStatusModel>(stream);

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
    
