using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Services.Import
{
    public interface IImportBaseService<T> where T : class
    {
        void AddHeader(HeaderModel h);
        IEnumerable<T> ToModel(string path, int sheetNo = 1);
        IEnumerable<T> ToModel(Stream stream, int sheetNo = 1);
        List<ExcelHelperAttribute> GetColumns();
        List<ExcelHelperAttribute> GetAllColumns();
    }
    public class ImportBaseService<T> : IImportBaseService<T> where T : class
    {
        public List<HeaderModel> Headers { get; set; }

        public virtual void AddHeader(HeaderModel h)
        {
            Headers.Add(h);
        }

        public virtual void HeaderValidation()
        {

        }
        public virtual MemoryStream Sample()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> ToModel(string path, int sheetNo = 1)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> ToModel(Stream stream, int sheetNo = 1)
        {
            throw new NotImplementedException();
        }

        public virtual List<ExcelHelperAttribute> GetColumns()
        {
            return ClosedXmlGeneric.GetColomnList(typeof(T));
        }

        public virtual List<ExcelHelperAttribute> GetAllColumns()
        {
            return ClosedXmlGeneric.GetColomnList(typeof(T), true);
        }

        public virtual bool ValidateHeaders(Stream stream)
        {
            throw new NotImplementedException();
        }
    }

}
