using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
    public interface IBonusService : IImportBaseService<BonusModel>
    {
        MemoryStream Bonus(Dictionary<string, Dictionary<string, string>> dictionary);
    }
    public class BonusService : ImportBaseService<BonusModel>, IBonusService
    {
        public BonusService()
        {

        }
        public MemoryStream Bonus(Dictionary<string, Dictionary<string, string>> dictionary)
        {

            return ClosedXmlGeneric.DataExport("Bonus", dictionary);
        }
        public override IEnumerable<BonusModel> ToModel(string path, int sheetNo = 1)
        { 
            if (System.IO.File.Exists(path))
            {
                var data = ClosedXmlGeneric.Import<BonusModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<BonusModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<BonusModel>();
        }
        public override IEnumerable<BonusModel> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<BonusModel>>();
            try
            {
                data = ClosedXmlGeneric.Import<BonusModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<BonusModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<BonusModel>();
        } 
        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<BonusModel>("Bonus", new List<BonusModel>());
        }
        public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(BonusModel));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<BonusModel>(stream);

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
