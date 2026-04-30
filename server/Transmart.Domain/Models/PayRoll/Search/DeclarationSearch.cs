using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Search
{
	public class DeclarationSearch : BaseSearch
	{
		public byte? TaxPayers { get; set; }
	}
}
