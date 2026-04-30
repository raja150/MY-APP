using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{

    public interface ILeaveBalanceService : IImportBaseService<LeaveBalance>
    {

    }
    public class LeaveBalanceService : ImportBaseService<LeaveBalance>, ILeaveBalanceService
    {
        public LeaveBalanceService()
        {
            
        }

        public override IEnumerable<LeaveBalance> ToModel(string path, int sheetNo = 1)
        { 
            if (System.IO.File.Exists(path))
            {
                var data = ClosedXmlGeneric.Import<LeaveBalance>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<LeaveBalance> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<LeaveBalance>();
        }
        public override IEnumerable<LeaveBalance> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<LeaveBalance>>();
            try
            {
                data = ClosedXmlGeneric.Import<LeaveBalance>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<LeaveBalance> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<LeaveBalance>();
        } 

        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<LeaveBalance>("Leave Balance", new List<LeaveBalance>());
        }

        public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(LeaveBalance));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<LeaveBalance>(stream);

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


