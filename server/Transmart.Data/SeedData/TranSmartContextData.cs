
using System;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities;

namespace TranSmart.Data.SeedData
{
	public static class TranSmartContextData
	{
		static TranSmartContext _context;
		public static async Task SeedAsync(TranSmartContext context, int? retry = 0)
		{
			_context = context;
			await addSequenceAttribute("SelfService_Ticket", "No");
			await addSequenceAttribute("PayRoll_Loan", "No");
			await addSequenceAttribute("Online_Questions", "No");
			await addSequenceAttribute("Questions_Options", "No");

		}

		private static async Task addSequenceAttribute(string EntityName, string Attribute)
		{
			if (!_context.SequenceNo.Any(x => x.EntityName == EntityName && x.Attribute == Attribute))
			{
				_context.SequenceNo.Add(new SequenceNo
				{
					ID = Guid.NewGuid(),
					EntityName = EntityName,
					Attribute = Attribute,
					NextNo = 1,
					Prefix = string.Empty,
					NextDisplayNo = string.Empty,
				});
				await _context.SaveChangesAsync();
			}
		}
		public static Tuple<int, string> GetSeqNo(string EntityName, string Attribute)
		{
			SequenceNo sequenceNo = _context.SequenceNo.Single(x => x.EntityName == EntityName && x.Attribute == Attribute);
			int seq = sequenceNo.NextNo++;

			return new Tuple<int, string>(seq, $"{(string.IsNullOrEmpty(sequenceNo.Prefix) ? "A" : sequenceNo.Prefix)}{seq.ToString().PadLeft(4, '0')}");
		}
	}
}
