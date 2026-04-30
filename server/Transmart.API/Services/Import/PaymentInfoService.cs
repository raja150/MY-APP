using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Models.Import;

namespace TranSmart.API.Services.Import
{
    public interface IPaymentInfoService : IImportBaseService<PaymentInfoModel>
    {
		Dictionary<int, Dictionary<string, string>> ToDictionary(Stream stream, int sheetNo = 1);
    }
    public class PaymentInfoService : ImportBaseService<PaymentInfoModel>, IPaymentInfoService
    {
        public PaymentInfoService()
        {

        }

        public override IEnumerable<PaymentInfoModel> ToModel(string path, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<PaymentInfoModel>>();
            if (System.IO.File.Exists(path))
            {
                data = ClosedXmlGeneric.Import<PaymentInfoModel>(path);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<PaymentInfoModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmpCode != null);
                }
            }
            else { throw new FileNotFoundException("File not found"); }
            return new List<PaymentInfoModel>();
        }
        public override IEnumerable<PaymentInfoModel> ToModel(Stream stream, int sheetNo = 1)
        {
            var data = new Dictionary<string, IList<PaymentInfoModel>>();
            try
            {
                data = ClosedXmlGeneric.Import<PaymentInfoModel>(stream);
                if (data.Count >= sheetNo)
                {
                    IEnumerable<PaymentInfoModel> values = data.ElementAtOrDefault(sheetNo - 1).Value;
                    return values.Where(x => x.EmpCode != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new List<PaymentInfoModel>();
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
			return ClosedXmlGeneric.Export<PaymentInfoModel>("Payment Info", new List<PaymentInfoModel>());
		}

		public override bool ValidateHeaders(Stream stream)
        {
            var colimnsList = ClosedXmlGeneric.GetColomnList(typeof(PaymentInfoModel));
            bool valid = true;
            Dictionary<int, string> headers = ClosedXmlGeneric.Header<PaymentInfoModel>(stream);

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
