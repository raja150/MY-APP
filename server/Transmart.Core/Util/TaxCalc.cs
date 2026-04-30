using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranSmart.Core.Util
{
    public class TaxBracket
    {
        public int Low { get; set; }
        public int High { get; set; }
        public decimal Rate { get; set; }
    }

    public class TaxCalculator
    {
        private readonly int _taxableIncome;
        private readonly TaxBracket[] _taxBrackets;

        public TaxCalculator(int taxableIncome, TaxBracket[] taxBrackets)
        {
            _taxableIncome = taxableIncome;
            _taxBrackets = taxBrackets;
        }

        public int Calculate()
        {
            if (_taxableIncome == 0 || _taxBrackets.Length == 0) return 0;
            var fullPayTax =
                _taxBrackets.Where(t => t.High < _taxableIncome)
                    .Select(t => t)
                    .Sum(taxBracket => (taxBracket.High - taxBracket.Low) * (taxBracket.Rate/100));

            var partialTax =
                _taxBrackets.Where(t => t.Low < _taxableIncome && t.High >= _taxableIncome)
                    .Select(t => (_taxableIncome - t.Low) * (t.Rate/100))
                    .Single();

            return (int)Math.Round(fullPayTax + partialTax, 0);
        }
    }
}
