using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.PayRoll
{
	public static class ProvidentFundCalc
	{
		public static Tuple<int, int, string> ForSalary(EPF EPFSettings,
			IEnumerable<EarningComponent> Earnings,
			ICollection<PaySheetEarning> paySheetEarnings, EmpStatutory empStatutory)
		{
			//Checking for organization and employee PF is enabled  
			if (EPFSettings == null || EPFSettings?.EnableEPF == 0
				|| empStatutory == null || empStatutory.EnablePF == 0)
			{
				return new Tuple<int, int, string>(0, 0, "-");
			}

			int pfAmount = 0; int pfWageForCalc = 0;
			//PF Configuration When LOP Applied (PFConfiguration)
			//1 - PF contribution pro-rated based
			//2 - Consider all applicable PF components if PF wage is less than 15k after Loss of Pay 
			int pfAlwaysWages = 0;
			int pfConsiderWages = 0;
			//PF Configuration When LOP Applied (PFConfiguration)
			//1 - PF contribution pro-rated based
			//2 - Consider all applicable PF components if PF wage is less than 15k after Loss of Pay
			if (EPFSettings.PFConfiguration.Equals("1", StringComparison.OrdinalIgnoreCase))
			{
				pfAlwaysWages = Earnings.SelectMany(x => paySheetEarnings
					.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(1))).Sum(x => x.Earning);
				pfConsiderWages = Earnings.SelectMany(x => paySheetEarnings
					.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(2))).Sum(x => x.Earning);
			}
			else
			{
				pfAlwaysWages = Earnings.SelectMany(x => paySheetEarnings
					.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(1))).Sum(x => x.Salary);
				pfConsiderWages = Earnings.SelectMany(x => paySheetEarnings
					.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(2))).Sum(x => x.Salary);
			}

			pfWageForCalc = pfAlwaysWages;
			if (pfAlwaysWages <= EPFSettings.RestrictedWage &&
				EPFSettings.PFConfiguration.Contains("2"))
			{
				pfWageForCalc = pfAlwaysWages + pfConsiderWages;
			}

			//Employee Contribution
			//As per the organization at employee wise
			if (empStatutory.EmployeeContrib != null && empStatutory.EmployeeContrib.Value == 3)
			{
				pfAmount = EpfCalculation(EPFSettings.RestrictedWage.Value, EPFSettings.EmployeeContrib.Value, pfWageForCalc);
			}
			else
			{
				pfAmount = EpfCalculation(EPFSettings.RestrictedWage.Value, empStatutory.EmployeeContrib.Value, pfWageForCalc);
			}
			return new Tuple<int, int, string>(pfAmount, pfWageForCalc, empStatutory.EmployeesProvid);
		}

		public static Tuple<int, int, string> ForFormSixteen(EPF EPFSettings,
		IEnumerable<EarningComponent> Earnings,
		ICollection<SalaryEarning> salaryEarnings, EmpStatutory empStatutory)
		{
			//Checking for organization and employee PF is enabled  
			if (EPFSettings == null || EPFSettings?.EnableEPF == 0
				|| empStatutory == null || empStatutory.EnablePF == 0)
			{
				return new Tuple<int, int, string>(0, 0, "-");
			}

			int pfAmount = 0; int pfWageForCalc = 0;

			//PF Configuration When LOP Applied (PFConfiguration)
			//1 - PF contribution pro-rated based
			//2 - Consider all applicable PF components if PF wage is less than 15k after Loss of Pay 
			//For form 16 calculation consider full salary of an employee

			int pfAlwaysWages = Earnings.SelectMany(x => salaryEarnings
				.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(1))).Sum(x => x.Monthly);
			int pfConsiderWages = Earnings.SelectMany(x => salaryEarnings
				.Where(y => y.ComponentId == x.ID && x.EPFContribution.Equals(2))).Sum(x => x.Monthly);

			//Assume always wages as default for calculation
			pfWageForCalc = pfAlwaysWages;
			if (pfAlwaysWages <= EPFSettings.RestrictedWage &&
				EPFSettings.PFConfiguration.Contains("2"))
			{
				pfWageForCalc = pfAlwaysWages + pfConsiderWages;
			}

			//Employee Contribution
			//As per the organization at employee wise
			if (empStatutory.EmployeeContrib != null && empStatutory.EmployeeContrib.Value == 3)
			{
				pfAmount = EpfCalculation(EPFSettings.RestrictedWage.Value, EPFSettings.EmployeeContrib.Value, pfWageForCalc);
			}
			else
			{
				pfAmount = EpfCalculation(EPFSettings.RestrictedWage.Value, empStatutory.EmployeeContrib.Value, pfWageForCalc);
			}
			return new Tuple<int, int, string>(pfAmount, pfWageForCalc, empStatutory.EmployeesProvid);
		}
		public static int EpfCalculation(int restrictedWage, int contribType, int pfWages)
		{
			int amount = 0;
			switch (contribType)
			{
				//12% actual PF wages
				case 1:
					amount = (int)Math.Round(pfWages * 0.12, 0);
					break;
				//Restrict to 15k of PF wages
				case 2:
					//PF Wages
					amount = (int)Math.Round(pfWages > restrictedWage ? (restrictedWage * 0.12) : pfWages * 0.12, 0);
					break;
				default:
					break;
			}
			return amount;
		}

	}
}
