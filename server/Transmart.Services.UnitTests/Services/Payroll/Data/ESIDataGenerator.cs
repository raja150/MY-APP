using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll.Data
{
	public class EsiDataGenerator : TheoryData<int, decimal?, int, bool, int, bool, Tuple<int, int, int>, decimal, int>
	{
		public EsiDataGenerator()
		{
			//ESISalaryLimit, EmployeesCont,PayMonth, ESIApplied, ESIAmount, Applicable, salary, LOPDays, ESIWages
			this.Add(21000, 0.75m, 4, false, 0, false, new Tuple<int, int, int>(22000, 6000, 3001), 0m, 28000);
			this.Add(21000, 0.75m, 4, true, 135, true, new Tuple<int, int, int>(12000, 6000, 3000), 0m, 18000);
			this.Add(0, null, 4, true, 0, false, new Tuple<int, int, int>(12000, 6000, 3000), 0m, 0);
		}
		public class DeclarationDataGeneratorForAdd : TheoryData<DeclarationData>
		{
			public DeclarationDataGeneratorForAdd()
			{
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = true,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 1000,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AC8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C475B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 1000,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 1110,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 1110,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 1000,
					fromDate = DateTime.Parse("2021-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				//[Mohan]
				//Add(new DeclarationData
				//{
				//	result = true,
				//	employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
				//	financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				//	closed = false,
				//	isDeclaration = false,
				//	section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
				//	section80CAmount = 0,
				//	section80DAmount = 0,
				//	otherSectionAmount = 0,
				//	hraLinesAmount = 120,
				//	fromDate = DateTime.Parse("2018-12-12"),
				//	toDate = DateTime.Parse("2019-12-12"),
				//	rentalFrom = DateTime.Parse("2021-10-21"),
				//	rentalTo = DateTime.Parse("2022-10-28")
				//});
				//Add(new DeclarationData
				//{
				//	result = true,
				//	employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
				//	financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				//	closed = false,
				//	isDeclaration = false,
				//	section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
				//	section80CAmount = 0,
				//	section80DAmount = 0,
				//	otherSectionAmount = 0,
				//	hraLinesAmount = 0,
				//	fromDate = DateTime.Parse("2018-12-12"),
				//	toDate = DateTime.Parse("2019-12-12"),
				//	rentalFrom = DateTime.Parse("2020-10-21"),
				//	rentalTo = DateTime.Parse("2020-10-28")
				//});
			}
		}

		public class DeclarationDataForCustomValidation : TheoryData<DeclarationData>
		{
			public DeclarationDataForCustomValidation()
			{
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = true,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 1000,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AC8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C475B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 1000,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 1110,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 1110,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 1000,
					fromDate = DateTime.Parse("2021-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2020-10-21"),
					rentalTo = DateTime.Parse("2020-10-28")
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 120,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2021-10-21"),
					rentalTo = DateTime.Parse("2022-10-28")
				});
				Add(new DeclarationData
				{
					result = true,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					rentalFrom = DateTime.Parse("2021-10-21"),
					rentalTo = DateTime.Parse("2022-10-28")
				});
			}
		}

		public class DeclarationDataGeneratorForUpdate : TheoryData<DeclarationData>
		{
			public DeclarationDataGeneratorForUpdate()
			{
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = true,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2D595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 1000,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AC8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C475B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					closed = true,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 1000,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 1110,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 1110,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2021-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C585B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 120,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4218-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 0,
					maxLimitEightyD = 0,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4218-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					isCatch = false
				});
				Add(new DeclarationData
				{
					result = false,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("3E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					isCatch = true
				});
				Add(new DeclarationData
				{
					result = true,
					employeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					financialId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					closed = false,
					isDeclaration = false,
					section80CId = Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					section80CAmount = 0,
					section80DAmount = 0,
					otherSectionAmount = 0,
					hraLinesAmount = 0,
					fromDate = DateTime.Parse("2018-12-12"),
					toDate = DateTime.Parse("2019-12-12"),
					maxLimitEightyC = 1,
					maxLimitEightyD = 1,
					declarationId = Guid.Parse("3E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					isCatch = false
				});
			}
		}

		public class DeclarationData
		{
			public bool result { get; set; }
			public Guid employeeId { get; set; }
			public Guid financialId { get; set; }
			public bool closed { get; set; }
			public bool isDeclaration { get; set; }
			public Guid section80CId { get; set; }
			public int section80CAmount { get; set; }
			public int section80DAmount { get; set; }
			public int otherSectionAmount { get; set; }
			public int hraLinesAmount { get; set; }
			public DateTime fromDate { get; set; }
			public DateTime toDate { get; set; }
			public DateTime rentalFrom { get; set; }
			public DateTime rentalTo { get; set; }
			public int maxLimitEightyC { get; set; }
			public int maxLimitEightyD { get; set; }
			public Guid declarationId { get; set; }
			public bool isCatch { get; set; }
		}
	}
}
