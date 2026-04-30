using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Core.Exceptions
{
	public class TranSmartException : Exception
	{
		public string Code { get; }
		public TranSmartException(string code, string message) : base(message)
		{
			Code = code;
		}
	}
}
