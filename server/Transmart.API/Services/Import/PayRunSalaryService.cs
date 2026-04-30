using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;
using TranSmart.Core.Attributes;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.API.Services.Import
{
    public interface IPayRunSalaryService : IImportBaseService<PaySheet>
    {
        MemoryStream PayrunMonthSlip(Dictionary<string, Dictionary<string, string>> dictionary);

    }
    public class PayRunSalaryService: ImportBaseService<PaySheet>, IPayRunSalaryService
    {
        public PayRunSalaryService()
        {

        }
        public MemoryStream PayrunMonthSlip(Dictionary<string,Dictionary<string, string>> dictionary) 
        {

            return ClosedXmlGeneric.DataExport("PayRuns", dictionary);
        }
    }
}
