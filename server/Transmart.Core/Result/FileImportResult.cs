using System.Collections.Generic;
using TranSmart.Core.Attributes;

namespace TranSmart.Core.Result
{
    public class FileImportResult<T> : BaseResult
    {
        public FileImportResult()
        {
        }

        public FileImportResult(bool isSuccess) : base(isSuccess)
        {
        }

        public FileImportResult(T returnValue, bool isSuccess = false) : base(isSuccess)
        {
            ReturnValue = returnValue;
        }

        public T ReturnValue { get; set; }

        public List<HeaderModel> Headers { get; set; }
    }
}