using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;
using TranSmart.API.Services.Import;

namespace TranSmart.API.Services.Import
{
    public interface IArrearService : IImportBaseService<ArrearModel>
    {
        MemoryStream Arrear(Dictionary<string, Dictionary<string, string>> DictionaryList);
    }
    public class ArrearService : ImportBaseService<ArrearModel>, IArrearService
    {
        public ArrearService()
        {

        }
        public MemoryStream Arrear(Dictionary<string, Dictionary<string, string>> DictionaryList)
        {

            return ClosedXmlGeneric.DataExport("Arrear", DictionaryList);
        }
        public override IEnumerable<ArrearModel> ToModel(string path, int sheetNo = 1)
        { 
            if (System.IO.File.Exists(path))
            {
                var data = ClosedXmlGeneric.Import<ArrearModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<ArrearModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<ArrearModel>();
        }
        public override IEnumerable<ArrearModel> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<ArrearModel>>();
            try
            {
                data = ClosedXmlGeneric.Import<ArrearModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<ArrearModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<ArrearModel>();
        }
        
        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<ArrearModel>("Arrear", new List<ArrearModel>());
        }
        public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(ArrearModel));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<ArrearModel>(stream);

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
