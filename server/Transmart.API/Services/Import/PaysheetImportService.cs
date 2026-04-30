using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
    public interface IPaysheetImportService : IImportBaseService<PaysheetModel>
    {
        MemoryStream Sample(Dictionary<int, string> DicList);
        Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1);
    }
    public class PaysheetImportService : ImportBaseService<PaysheetModel>, IPaysheetImportService
    {
        public PaysheetImportService()
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
            return ClosedXmlGeneric.Export<PaysheetModel>("PaySheet", new List<PaysheetModel>());
        }
        public MemoryStream Sample(Dictionary<int, string> DicList)
        { 
            return ClosedXmlGeneric.Export("PaySheet", DicList);
        }
    }
}
