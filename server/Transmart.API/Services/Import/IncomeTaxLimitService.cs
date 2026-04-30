using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
    public interface IIncomeTaxLimitService : IImportBaseService<IncomeTaxLimitModel>
    {
        MemoryStream IncomeTaxLimit(Dictionary<string, Dictionary<string, string>> dictionary);
    }
    public class IncomeTaxLimitService : ImportBaseService<IncomeTaxLimitModel>, IIncomeTaxLimitService
    {
        public IncomeTaxLimitService()
        {

        }
        public MemoryStream IncomeTaxLimit(Dictionary<string, Dictionary<string, string>> dictionary)
        {

            return ClosedXmlGeneric.DataExport("IncomeTaxLimit", dictionary);
        }
        public override IEnumerable<IncomeTaxLimitModel> ToModel(string path, int sheetNo = 1)
        {
           var data = new Dictionary<string, IList<IncomeTaxLimitModel>>();
            if (System.IO.File.Exists(path))
            {
                data = ClosedXmlGeneric.Import<IncomeTaxLimitModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<IncomeTaxLimitModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeId != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<IncomeTaxLimitModel>();
        }
        public override IEnumerable<IncomeTaxLimitModel> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<IncomeTaxLimitModel>>();
            try
            {
                data = ClosedXmlGeneric.Import<IncomeTaxLimitModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<IncomeTaxLimitModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeId != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<IncomeTaxLimitModel>();
        } 
        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<IncomeTaxLimitModel>("IncomeTaxLimit", new List<IncomeTaxLimitModel>());
        }
        public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(IncomeTaxLimitModel));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<IncomeTaxLimitModel>(stream);

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
