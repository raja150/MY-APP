using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Services.Import
{
    public interface IIncentivesPayCutService : IImportBaseService<IncentivesPayCutModel>
    {

    }
    public class IncentivesPayCutService : ImportBaseService<IncentivesPayCutModel>
    {
        public IncentivesPayCutService()
        {

        }
        public override IEnumerable<IncentivesPayCutModel> ToModel(string path, int sheetNo = 1)
        {
            if (System.IO.File.Exists(path))
            { 
                var data = ClosedXmlGeneric.Import<IncentivesPayCutModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<IncentivesPayCutModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<IncentivesPayCutModel>();
        }
        public override IEnumerable<IncentivesPayCutModel> ToModel(Stream stream, int sheetNo = 1)
        {
            try
            {
                var data = ClosedXmlGeneric.Import<IncentivesPayCutModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<IncentivesPayCutModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<IncentivesPayCutModel>();
        } 

        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<IncentivesPayCutModel>("Incentives and Pay cut", new List<IncentivesPayCutModel>());
        }
        public override bool ValidateHeaders(Stream stream)
        {
            var columnList = ClosedXmlGeneric.GetColomnList(typeof(IncentivesPayCutModel));
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<IncentivesPayCutModel>(stream);

            return columnList.All(column => headers.ContainsValue(column.Attribute.GetName()));
        }
    }
}
