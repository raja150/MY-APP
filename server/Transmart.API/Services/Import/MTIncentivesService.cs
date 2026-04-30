using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Services.Import
{
    public interface IMTIncentivesService : IImportBaseService<MTIncentivesModel>
    {

    }
    public class MTIncentivesService : ImportBaseService<MTIncentivesModel>
    {
        public MTIncentivesService()
        {

        }
        public override IEnumerable<MTIncentivesModel> ToModel(string path, int sheetNo = 1)
        {
            if (System.IO.File.Exists(path))
            { 
                var data = ClosedXmlGeneric.Import<MTIncentivesModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<MTIncentivesModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<MTIncentivesModel>();
        }
        public override IEnumerable<MTIncentivesModel> ToModel(Stream stream, int sheetNo = 1)
        {
            try
            {
                var data = ClosedXmlGeneric.Import<MTIncentivesModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<MTIncentivesModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmployeeCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<MTIncentivesModel>();
        } 

        public override MemoryStream Sample()
        {
            return ClosedXmlGeneric.Export<MTIncentivesModel>("Leave Balance", new List<MTIncentivesModel>());
        }
        public override bool ValidateHeaders(Stream stream)
        {
            var columnList = ClosedXmlGeneric.GetColomnList(typeof(MTIncentivesModel));
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<MTIncentivesModel>(stream);

            return columnList.All(column => headers.ContainsValue(column.Attribute.GetName()));
        }
    }
}
