using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.PayRoll
{
	public abstract class IncomeTax
	{
		private readonly IUnitOfWork _uow;
		protected IUnitOfWork UOW { get { return _uow; } }

		protected IEnumerable<TaxBracket> Slabs;
		protected IncomeTax(IUnitOfWork uow)
		{
			_uow = uow;
		}
		public abstract Task Calculate(Declaration declaration);
		public virtual int TaxCalculator(int taxableIncome, TaxBracket[] taxBrackets)
		{
			if (taxableIncome == 0 || taxBrackets.Length == 0) return 0;
			var fullPayTax =
				 taxBrackets.Where(t => t.High < taxableIncome)
					.Select(t => t)
					.Sum(taxBracket => (taxBracket.High - taxBracket.Low) * (taxBracket.Rate / 100));

			var partialTax =
				 taxBrackets.Where(t => t.Low < taxableIncome && t.High >= taxableIncome)
					.Select(t => (taxableIncome - t.Low) * (t.Rate / 100))
					.SingleOrDefault();

			return (int)Math.Round(fullPayTax + partialTax, 0);
		}
		public async Task<int> OtherIncome(Guid declarationId)
		{
			var otherIncomeSource = await UOW.GetRepositoryAsync<OtherIncomeSources>().SingleAsync(x => x.DeclarationId == declarationId);

			if (otherIncomeSource != null)
			{
				return otherIncomeSource.Qualified();
			}
			return 0;
		}
	}

	public class TaxBracket
	{
		public int Low { get; set; }
		public int High { get; set; }
		public decimal Rate { get; set; }
	}

}
