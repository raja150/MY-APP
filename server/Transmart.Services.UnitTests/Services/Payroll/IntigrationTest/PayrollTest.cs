using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;
using TranSmart.Service;
using TranSmart.Service.Organization;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll.IntigrationTest
{
	[CollectionDefinition("Collection #1")]
	[Xunit.TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class PayrollTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly AttendanceSumService _attendanceSumService;
		private readonly ArrearService _arrearService;
		private readonly IncentivesPayCutService _incentivesPayCutService;
		private readonly SequenceNoService _sequenceNoService;
		private readonly EmployeeService _empService;
		private readonly DeclarationService _declarationService;
		private readonly PayRollService _payRollService;
		private readonly FinancialYearService _financialYearService;
		private readonly EarningComponentService _earningComponent;
		private readonly SalaryService _salaryService;
		private readonly LoanService _loanService;
		private readonly IncomeTaxLimitService _incomeTaxLimitService;
		private readonly LatecomersService _latecomersService;
		private readonly Guid departmentId = Guid.NewGuid();
		private readonly Guid designationId = Guid.NewGuid();
		private readonly Guid workLocationId = Guid.NewGuid();
		private readonly Guid _financialYrId = Guid.NewGuid();
		private readonly Guid _stateId = Guid.NewGuid();
		private readonly Guid _organizationId = Guid.NewGuid();
		private readonly Guid _paySettingId = Guid.NewGuid();
		private readonly Guid _declarationSettingId = Guid.NewGuid();
		private readonly Guid _esiId = Guid.NewGuid();
		private readonly Guid _epfId = Guid.NewGuid();
		private readonly Guid _basicId = Guid.NewGuid();
		private readonly Guid _hraId = Guid.NewGuid();
		private readonly Guid _medicalId = Guid.NewGuid();
		private readonly Guid _foodCouponId = Guid.NewGuid();
		private readonly Guid _specialId = Guid.NewGuid();
		private readonly Guid _section80CId = Guid.NewGuid();
		private readonly Guid _section80DId = Guid.NewGuid();
		private readonly Guid _otherSectionId = Guid.NewGuid();
		private readonly string fileName = @"..\..\..\Services\Payroll\IntigrationTest\Salary Sheet.xlsx";

		public PayrollTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_sequenceNoService = new SequenceNoService(inMemory.UnitOfWork);
			_financialYearService = new FinancialYearService(inMemory.UnitOfWork);
			_attendanceSumService = new AttendanceSumService(inMemory.UnitOfWork);
			_arrearService = new ArrearService(inMemory.UnitOfWork);
			_incentivesPayCutService = new IncentivesPayCutService(inMemory.UnitOfWork);
			_empService = new EmployeeService(inMemory.UnitOfWork);
			_declarationService = new DeclarationService(inMemory.UnitOfWork);
			_payRollService = new PayRollService(inMemory.UnitOfWork);
			_earningComponent = new EarningComponentService(inMemory.UnitOfWork);
			_salaryService = new SalaryService(inMemory.UnitOfWork);
			_loanService = new LoanService(inMemory.UnitOfWork, _sequenceNoService);
			_incomeTaxLimitService = new IncomeTaxLimitService(inMemory.UnitOfWork);
			_latecomersService = new LatecomersService(inMemory.UnitOfWork);
		}

		private async Task AddEarningComponent()
		{
			await _earningComponent.AddAsync(new EarningComponent
			{
				ID = _basicId,
				EarningType = (int)EarningType.Basic,
				Name = "Basic",
				ProrataBasis = true,
				EPFContribution = 1,
				ESIContribution = true,
			});
			await _earningComponent.AddAsync(new EarningComponent
			{
				ID = _hraId,
				EarningType = (int)EarningType.HRA,
				Name = "HRA",
				ProrataBasis = true,
				ESIContribution = true
			});
			await _earningComponent.AddAsync(new EarningComponent
			{
				ID = _foodCouponId,
				EarningType = (int)EarningType.FoodCoupon,
				Name = "Food Coupon",
				ProrataBasis = true,
				ESIContribution = true
			});
			await _earningComponent.AddAsync(new EarningComponent
			{
				ID = _medicalId,
				EarningType = (int)EarningType.MedicalAllowance,
				Name = "Medical Allowance",
				ProrataBasis = true,
				ESIContribution = true
			});

			await _earningComponent.AddAsync(new EarningComponent
			{
				ID = _specialId,
				EarningType = 0,
				Name = "Special Allowance",
				ProrataBasis = true,
				ESIContribution = true
			});
		}

		private async Task<Guid> AddEmployeeToDB(string empCode, DateTime doj, DateTime? dor, int number, int salary)
		{
			var id = Guid.NewGuid();
			var employee = new Employee
			{
				ID = id,
				No = empCode,
				Name = empCode,
				FirstName = empCode,
				DateOfJoining = doj,
				DepartmentId = departmentId,
				DesignationId = designationId,
				WorkLocationId = workLocationId,
				PanNumber = "AKGBY83" + number.ToString().PadLeft(2, '0') + "N",
				MobileNumber = "78945612" + number.ToString().PadLeft(2, '0'),
				AadhaarNumber = "3438393389" + number.ToString().PadLeft(2, '0'),
				WorkEmail = "abc@gmail.com",
				PersonalEmail = "abc@gmail.com",
				PassportNumber = "",
				DateOfBirth = DateTime.Now.AddYears(-30),
				Status = 1
			};
			if (dor.HasValue) // && dor.Value.Year > 1990
			{
				employee.LastWorkingDate = dor;
			}
			_ = await _empService.AddAsync(employee);

			inMemory.DbContext.Payroll_EmpStatutory.Add(new EmpStatutory
			{
				ID = Guid.NewGuid(),
				EmpId = id,
				EnablePF = 1,
				EmployeeContrib = 2,
				EnableESI = salary < 21000 ? 1 : 0
			});
			inMemory.DbContext.Payroll_EmployeePayInfo.Add(new EmployeePayInfo
			{
				ID = Guid.NewGuid(),
				EmployeeId = id,
				PayMode = 2
			});
			inMemory.DbContext.SaveChanges();
			return id;
		}

		private List<BonusSheetModel> GetBonusSheetData()
		{
			XLWorkbook workbook = new(fileName);
			var ws = workbook.Worksheet("Bonus");
			var totalRows = ws.LastRowUsed().RowNumber();
			List<BonusSheetModel> bonusSheetModels = new();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				bonusSheetModels.Add(new BonusSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					AddedOn = DateTime.Parse(row.Cell(2).Value.ToString()).Date,
					Amount = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
				});
			}
			return bonusSheetModels;
		}
		private List<HRADeclarationSheetModel> GetHRASheetData()
		{
			XLWorkbook workbook = new(fileName);
			var ws = workbook.Worksheet("HRADeclaration");
			var totalRows = ws.LastRowUsed().RowNumber();
			List<HRADeclarationSheetModel> HRASheetModels = new();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				HRASheetModels.Add(new HRADeclarationSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					LandLord = row.Cell(2).Value.ToString(),
					Pan = row.Cell(3).Value.ToString(),
					City = row.Cell(4).Value.ToString(),
					Address = row.Cell(5).Value.ToString(),
					Amount = Convert.ToInt32(row.Cell(6).Value.GetNumber()),
					RentalFrom = DateTime.Parse(row.Cell(7).Value.ToString()).Date,
					RentalTo = DateTime.Parse(row.Cell(8).Value.ToString()).Date
				});

			}
			return HRASheetModels;
		}
		private List<Section6A80CSheetModel> Get80CSheetData()
		{
			XLWorkbook workbook = new(fileName);
			var ws = workbook.Worksheet("Section6A80C");
			var totalRows = ws.LastRowUsed().RowNumber();
			List<Section6A80CSheetModel> cSheetModels = new();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				cSheetModels.Add(new Section6A80CSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					Amount = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
					Section80CName = row.Cell(2).Value.ToString(),
					//Section80CId = inMemory.DbContext.Payroll_Section80C.FirstOrDefault(x => x.Name == row.Cell(2).Value.ToString()).ID
				});
			}
			return cSheetModels;
		}
		private List<Section6A80DSheetModel> Get80DSheetData()
		{
			XLWorkbook workbook = new(fileName);
			var ws = workbook.Worksheet("Section6A80D");
			var totalRows = ws.LastRowUsed().RowNumber();
			List<Section6A80DSheetModel> dSheetModels = new();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				dSheetModels.Add(new Section6A80DSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					Section80DName = row.Cell(2).Value.ToString(),
					Amount = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
					Section80DId = Guid.NewGuid()
				});
			}
			return dSheetModels;
		}
		private List<LetOutPropertySheetModel> GetLetOutPropertyData()
		{
			XLWorkbook workbook = new(fileName);
			var ws = workbook.Worksheet("LetOutProperty");
			var totalRows = ws.LastRowUsed().RowNumber();
			List<LetOutPropertySheetModel> letOutsModel = new();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				letOutsModel.Add(new LetOutPropertySheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					AnnualRentReceived = Convert.ToInt32(row.Cell(2).Value.GetNumber()),
					MunicipalTaxPaid = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
					NetAnnualValue = Convert.ToInt32(row.Cell(4).Value.GetNumber()),
					StandardDeduction = Convert.ToInt32(row.Cell(5).Value.GetNumber()),
					RepayingHomeLoan = (bool)row.Cell(6).Value,
					InterestPaid = Convert.ToInt32(row.Cell(7).Value.GetNumber()),
					Principle = Convert.ToInt32(row.Cell(8).Value.GetNumber()),
					NameOfLender = row.Cell(9).Value.ToString(),
					LenderPAN = row.Cell(10).Value.ToString(),
					NetIncome = Convert.ToInt32(row.Cell(11).Value.GetNumber())
				});
			}
			return letOutsModel;
		}
		private List<HomeLoanPayPropertySheetModel> GetHomeLoanPaySheetData()
		{
			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("HomeLoanPay");
			var totalRows = ws.LastRowUsed().RowNumber();
			var homeLoanPayModel = new List<HomeLoanPayPropertySheetModel>();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				homeLoanPayModel.Add(new HomeLoanPayPropertySheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					LenderPAN = row.Cell(2).Value.ToString(),
					NameOfLender = row.Cell(3).Value.ToString(),
					Principle = Convert.ToInt32(row.Cell(4).Value.GetNumber()),
					InterestPaid = Convert.ToInt32(row.Cell(5).Value.GetNumber()),
				});
			}
			return homeLoanPayModel;
		}

		private List<PreviousEmploymentSheetModel> GetPreviousEmploymentSheetData()
		{
			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("PrevEmployment");
			var totalRows = ws.LastRowUsed().RowNumber();
			var previousEmploymentModel = new List<PreviousEmploymentSheetModel>();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				previousEmploymentModel.Add(new PreviousEmploymentSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					EncashmentExceptions = Convert.ToInt32(row.Cell(2).Value.GetNumber()),
					ProvisionalFund = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
					ProfessionalTax = Convert.ToInt32(row.Cell(4).Value.GetNumber()),
					IncomeTax = Convert.ToInt32(row.Cell(5).Value.GetNumber()),
					IncomeAfterException = Convert.ToInt32(row.Cell(6).Value.GetNumber()),
				});
			}
			return previousEmploymentModel;
		}

		private List<OtherIncomeSourcesSheetModel> GetOtherIncomeSourcesSheetData()
		{
			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("OtherIncomeSources");
			var totalRows = ws.LastRowUsed().RowNumber();
			var otherIncomeSourcesModel = new List<OtherIncomeSourcesSheetModel>();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				otherIncomeSourcesModel.Add(new OtherIncomeSourcesSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					InterestOnFD = Convert.ToInt32(row.Cell(2).Value.GetNumber()),
					InterestOnSaving = Convert.ToInt32(row.Cell(3).Value.GetNumber()),
					OtherSources = Convert.ToInt32(row.Cell(4).Value.GetNumber()),
				});
			}
			return otherIncomeSourcesModel;
		}

		private List<Section6AOtherSheetModel> GetSection6AOtherSheetData()
		{
			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("Section6AOther");
			var totalRows = ws.LastRowUsed().RowNumber();
			var section6AOtherModel = new List<Section6AOtherSheetModel>();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				section6AOtherModel.Add(new Section6AOtherSheetModel
				{
					Code = row.Cell(1).Value.ToString(),
					OtherSectionName = row.Cell(2).Value.ToString(),
					Amount = Convert.ToInt32(row.Cell(3).Value.GetNumber())
				});
			}
			return section6AOtherModel;
		}
		private List<EmployeeLoanModel> GetLoanSheetData()
		{
			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("EmployeeLoan");
			var totalRows = ws.LastRowUsed().RowNumber();
			var loanModel = new List<EmployeeLoanModel>();
			for (int rNo = 2; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				loanModel.Add(new EmployeeLoanModel
				{
					Code = row.Cell(1).Value.ToString(),
					LoanAmount = Convert.ToInt32(row.Cell(2).Value.GetNumber()),
					//LoanAmount = Convert.ToInt32(row.Cell(2).Value),
					//LoanReleasedOn = DateTime.FromOADate(Convert.ToDouble(row.Cell(3).Value)).Date,
					LoanDeductFrom = DateTime.Parse(row.Cell(4).Value.ToString()).Date,
					MonthlyAmount = Convert.ToInt32(row.Cell(5).Value.GetNumber()),
					//MonthlyAmount = Convert.ToInt32(row.Cell(5).Value),
				});
			}
			return loanModel;
		}
		private List<PaySheetDataModel> GetSheetData()
		{
			//Issue while reading excel sheet when formula's.
			//So values are copied into separate sheet. 

			var workbook = new XLWorkbook(fileName);
			var ws = workbook.Worksheet("Salary");
			var totalRows = ws.LastRowUsed().RowNumber();
			var model = new List<PaySheetDataModel>();
			for (int rNo = 4; rNo <= totalRows; rNo++)
			{
				var row = ws.Row(rNo);
				if (row.Cell(2).Value.ToString().Trim() == "")
				{
					continue;
				}
				//Push all rows into model
				var data = new PaySheetDataModel
				{
					Code = row.Cell((int)Header.Code).Value.ToString(),
					DOJ = DateTime.Parse(row.Cell((int)Header.DOJ).Value.ToString()),
					Month = DateTime.Parse(row.Cell((int)Header.Month).Value.ToString()),
					Salary = GetCellValue(row, (int)Header.Salary),
					ActualBasic = GetCellValue(row, (int)Header.ActualBasic),
					ActualHRA = GetCellValue(row, (int)Header.ActualHRA),
					ActualMedicalTransport = GetCellValue(row, (int)Header.ActualMedicalTransport),
					ActualFoodCoupons = GetCellValue(row, (int)Header.ActualFoodCoupons),
					ActualSpecialAllowance = GetCellValue(row, (int)Header.ActualSpecialAllowance),
					MonthDays = GetCellValue(row, (int)Header.MonthDays),
					EmployeeWorkingDays = GetCellValue(row, (int)Header.EmployeeWorkingDays),
					DaysPresent = GetCellValue(row, (int)Header.DaysPresent),
					LOPDays = GetCellValue(row, (int)Header.LOPDays),
					LateComingDays = GetCellValue(row, (int)Header.LateComingDays),
					UnauthorizedLeaves = GetCellValue(row, (int)Header.UnauthorizedLeaves),
					EarnedBasic = GetCellValue(row, (int)Header.EarnedBasic),
					EarnedHRA = GetCellValue(row, (int)Header.EarnedHRA),
					EarnedMedicalTransport = GetCellValue(row, (int)Header.EarnedMedicalTransport),
					EarnedFoodCoupons = GetCellValue(row, (int)Header.EarnedFoodCoupons),
					EarnedSpecialAllowance = GetCellValue(row, (int)Header.EarnedSpecialAllowance),
					Arrears = GetCellValue(row, (int)Header.Arrears),
					Incentives = GetCellValue(row, (int)Header.Incentives),
					GrossEarningtotal = GetCellValue(row, (int)Header.GrossEarningtotal),
					LOPAmount = GetCellValue(row, (int)Header.LOPAmount),
					UnauthorizedAmount = GetCellValue(row, (int)Header.UnauthorizedAmount),
					LateComingAmount = GetCellValue(row, (int)Header.LateComingAmount),
					StaffSalaryAdvances = GetCellValue(row, (int)Header.StaffSalaryAdvances),
					Paycut = GetCellValue(row, (int)Header.Paycut),
					NetSalaryEarned = GetCellValue(row, (int)Header.NetSalaryEarned),
					PT = GetCellValue(row, (int)Header.PT),
					IncomeTax = GetCellValue(row, (int)Header.IncomeTax),
					PFContribution = GetCellValue(row, (int)Header.PFContribution),
					ESIContribution = GetCellValue(row, (int)Header.ESIContribution),
					ESIFinal = GetCellValue(row, (int)Header.ESIFinal),
					GrossEarnings = GetCellValue(row, (int)Header.GrossEarnings),
					GrossDeduction = GetCellValue(row, (int)Header.GrossDeduction),
					NetTakeHome = GetCellValue(row, (int)Header.NetTakeHome),
					IncomeTaxCalc = GetCellValue(row, (int)Header.IncomeTaxCalc),
				};

				if (row.Cell((int)Header.DOR).Value.ToString() != "")
				{
					data.DOR = DateTime.Parse(row.Cell((int)Header.DOR).Value.ToString());
				}
				model.Add(data);
			}
			return model;
		}

		private int GetCellValue(IXLRow row, int column)
		{
			if (int.TryParse(row.Cell(column).Value.ToString(), out int value))
			{
				return value;
			}
			else
			{
				return 0;
			}
		}

		private async Task EmployeeSalary(PaySheetDataModel model)
		{
			var employeeSalary = await inMemory.UnitOfWork.GetRepositoryAsync<Salary>()
				.SingleAsync(x => x.EmployeeId == model.EmployeeId);
			if (employeeSalary == null)
			{
				employeeSalary = UpdateSalary(model);
				await _salaryService.AddAsync(employeeSalary);
			}
			else
			{
				var employeeUpdated = UpdateSalary(model);
				employeeUpdated.ID = employeeSalary.ID;
				await _salaryService.UpdateAsync(employeeUpdated);
			}
		}

		private Salary UpdateSalary(PaySheetDataModel model)
		{
			return new Salary
			{
				EmployeeId = model.EmployeeId,
				Annually = model.Salary * 12,
				Monthly = model.Salary,
				CTC = model.Salary * 12,
				Earnings = new List<SalaryEarning>()
					{
						new SalaryEarning
						{
							ComponentId = _basicId,
							Monthly = model.ActualBasic,
							Annually = model.ActualBasic *12,
						},
						new SalaryEarning
						{
							ComponentId = _hraId,
							Monthly = model.ActualHRA,
							Annually = model.ActualHRA *12,
						},
						new SalaryEarning
						{
							ComponentId = _medicalId,
							Monthly = model.ActualMedicalTransport,
							Annually = model.ActualMedicalTransport *12,
						},
						new SalaryEarning
						{
							ComponentId = _foodCouponId,
							Monthly = model.ActualFoodCoupons,
							Annually = model.ActualFoodCoupons *12,
						},
						new SalaryEarning
						{
							ComponentId = _specialId,
							Monthly = model.ActualSpecialAllowance,
							Annually = model.ActualSpecialAllowance * 12,
						}
					}
			};
		}

		[Fact]
		public async Task SalaryProcessTest()
		{
			inMemory.DbContext.ChangeTracker.Clear();
			if (!await SetupData())
			{
				return;
			}

			//Get sheet data
			var sheetData = GetSheetData();
			var loanSheetData = GetLoanSheetData();

			var HRASheetData = GetHRASheetData();
			var homeLoanPaySheetData = GetHomeLoanPaySheetData();
			var letOutsData = GetLetOutPropertyData();
			var cSheetData = Get80CSheetData();
			var dSheetData = Get80DSheetData();

			var previousEmploymentSheetData = GetPreviousEmploymentSheetData();
			var otherIncomeSourcesSheetData = GetOtherIncomeSourcesSheetData();
			var section6AOtherSheetData = GetSection6AOtherSheetData();
			var bonusSheetData = GetBonusSheetData();

			await _sequenceNoService.AddAsync(new SequenceNo
			{
				EntityName = "PayRoll_Loan",
				Attribute = "No",
				NextDisplayNo = "1",
				NextNo = 1,
				Prefix = "L",
				ID = Guid.NewGuid()
			});
			inMemory.UnitOfWork.SaveChanges();

			var months = sheetData.Select(x => x.Month).Distinct();

			using StreamWriter salaryFile = new("salary.csv", append: false);
			await salaryFile.WriteLineAsync($"Employee Code,Month,Header,Sheet Value,Application Values,Differences");

			using StreamWriter declarationFile = new("declaration.csv", append: false);
			await declarationFile.WriteLineAsync("Code,Month,No,Salary,Perquisites,PreviousEmployment,TotalSalary,Allowance,Balance,StandardDeduction,TaxOnEmployment,Deductions,IncomeChargeable,HouseIncome,OtherIncome,GrossTotal,EightyC,EPF,EightyD,OtherSections,Taxable,Tax,Relief,Cess,TaxPayable,TaxPaid,Due");

			int number = 0;

			//Distinct months from sheet data
			//For each month wise 
			foreach (var month in months)
			{
				inMemory.DbContext.ChangeTracker.Clear();
				var attSum = new List<AttendanceSum>();
				var arrears = new List<Arrear>();
				var incentivePaycut = new List<IncentivesPayCut>();
				var incomeTaxLimit = new List<IncomeTaxLimit>();
				var latecomers = new List<Latecomers>();

				//Get the records specific to for loop month
				var employeesData = sheetData.Where(x => x.Month == month);
				var employees = await inMemory.UnitOfWork.GetRepositoryAsync<Employee>().GetAsync();
				//Get distinct of employee code and create employees.
				//Create a function to add employee to DB.Designation Dept are predefined

				var employeesDataForSalary = employeesData;
				foreach (var employeeItem in employeesDataForSalary)
				{
					var existingEmployee = employees.FirstOrDefault(x => x.No.Equals(employeeItem.Code, StringComparison.OrdinalIgnoreCase));
					if (existingEmployee == null)
					{
						number++;
						employeeItem.EmployeeId = await AddEmployeeToDB(employeeItem.Code, employeeItem.DOJ, employeeItem.DOR, number, employeeItem.Salary);
					}
					else
					{
						employeeItem.EmployeeId = existingEmployee.ID;
						if (employeeItem.DOR.HasValue)
						{
							existingEmployee.LastWorkingDate = employeeItem.DOR.Value;
							await _empService.UpdateAsync(existingEmployee);
						}
					}

					await EmployeeSalary(employeeItem);

					//import Bonus
					foreach (var bonus in bonusSheetData.Where(x => x.Code == employeeItem.Code))
					{
						inMemory.DbContext.Payroll_EmpBonus.Add(new EmpBonus
						{
							EmployeeId = employeeItem.EmployeeId,
							Amount = bonus.Amount,
							ReleasedOn = bonus.AddedOn,
						});
					}
					await inMemory.DbContext.SaveChangesAsync();

					var declaration = new Declaration
					{
						EmployeeId = employeeItem.EmployeeId,
						FinancialYearId = _financialYrId,
						HRALines = new List<HraDeclaration>()
					};
					//import HRA
					foreach (var hra in HRASheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.HRALines.Add(new HraDeclaration
						{
							RentalFrom = hra.RentalFrom,
							RentalTo = hra.RentalTo,
							Amount = hra.Amount,
							Address = hra.Address,
							Pan = hra.Pan,
							City = hra.City,
							LandLord = hra.LandLord,
						});
					}

					//import Section6A80C
					declaration.Section80CLines = new List<Section6A80C>();
					foreach (var C in cSheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.Section80CLines.Add(new Section6A80C
						{
							Amount = C.Amount,
							Section80CId = inMemory.DbContext.Payroll_Section80C.FirstOrDefault(x => x.Name.Equals(C.Section80CName, StringComparison.OrdinalIgnoreCase)).ID
						});
					}

					//import Section6A80D
					declaration.Section80DLines = new List<Section6A80D>();
					foreach (var D in dSheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.Section80DLines.Add(new Section6A80D
						{
							Amount = D.Amount,
							Section80DId = inMemory.DbContext.Payroll_Section80D.FirstOrDefault(x => x.Name == D.Section80DName).ID,
						});
					}

					//import LetOutProperty
					declaration.LetOutPropertyLines = new List<LetOutProperty>();
					foreach (var x in letOutsData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.LetOutPropertyLines.Add(new LetOutProperty
						{
							AnnualRentReceived = x.AnnualRentReceived,
							MunicipalTaxPaid = x.MunicipalTaxPaid,
							NetAnnualValue = x.NetAnnualValue,
							StandardDeduction = x.StandardDeduction,
							RepayingHomeLoan = x.RepayingHomeLoan,
							InterestPaid = x.InterestPaid,
							Principle = x.Principle,
							NameOfLender = x.NameOfLender,
							LenderPAN = x.LenderPAN,
							NetIncome = x.NetIncome,
						});
					}
					//import HomeLoanPay
					foreach (var x in homeLoanPaySheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.HomeLoanPay = new HomeLoanPay
						{
							InterestPaid = x.InterestPaid,
							Principle = x.Principle,
							NameOfLender = x.NameOfLender,
							LenderPAN = x.LenderPAN,
						};
					}
					//import PrevEmployment
					foreach (var x in previousEmploymentSheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.PrevEmployment = new PrevEmployment
						{
							EncashmentExceptions = x.EncashmentExceptions,
							ProfessionalTax = x.ProfessionalTax,
							IncomeAfterException = x.IncomeAfterException,
							ProvisionalFund = x.ProvisionalFund,
							IncomeTax = x.IncomeTax,
						};
					}
					//import other income Source
					foreach (var x in otherIncomeSourcesSheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.IncomeSource = new OtherIncomeSources
						{
							InterestOnFD = x.InterestOnFD,
							InterestOnSaving = x.InterestOnSaving,
							OtherSources = x.OtherSources,
						};
					}
					//import section6AOther
					declaration.SectionOtherLines = new List<Section6AOther>();
					foreach (var x in section6AOtherSheetData.Where(x => x.Code == employeeItem.Code))
					{
						declaration.SectionOtherLines.Add(new Section6AOther
						{
							Amount = x.Amount,
							OtherSectionsId = inMemory.DbContext.Payroll_OtherSections.FirstOrDefault(y => y.Name == x.OtherSectionName).ID,
						});
					}

					await _declarationService.AddAsync(declaration);
				}
				inMemory.DbContext.ChangeTracker.Clear();
				employees = await inMemory.UnitOfWork.GetRepositoryAsync<Employee>().GetAsync();

				foreach (var loan in loanSheetData.Where(x => x.LoanDeductFrom.Month == month.Month
									&& x.LoanDeductFrom.Year == month.Year))
				{
					await _loanService.AddAsync(new Loan
					{
						EmployeeId = employees.FirstOrDefault(y => y.No.Equals(loan.Code)).ID,
						LoanAmount = loan.LoanAmount,
						LoanReleasedOn = loan.LoanReleasedOn,
						DeductFrom = loan.LoanDeductFrom,
						MonthlyAmount = loan.MonthlyAmount,
						Status = loan.Status,
						Notes = "Notes"
					});
				}
				//Import attendance data into DB
				attSum.AddRange(employeesDataForSalary.Select(x => new AttendanceSum
				{
					ID = Guid.NewGuid(),
					EmployeeId = x.EmployeeId,
					Month = Convert.ToByte(month.Month),
					Year = Convert.ToInt16(month.Year),
					Present = x.DaysPresent,
					LOP = x.LOPDays,
					Unauthorized = x.UnauthorizedLeaves,
				}));
				await _attendanceSumService.AddBulk(attSum, month.Year, month.Month);

				//import Latecomers Data into DB
				latecomers.AddRange(employeesDataForSalary.Select(x => new Latecomers
				{
					ID = Guid.NewGuid(),
					EmployeeID = x.EmployeeId,
					Month = Convert.ToByte(month.Month),
					Year = Convert.ToInt16(month.Year),
					NumberOfDays = x.LateComingDays
				}));
				await _latecomersService.AddBulk(latecomers, month.Year, month.Month);

				//Import arrears data into DB
				arrears.AddRange(employeesDataForSalary.Select(x => new Arrear
				{
					ID = Guid.NewGuid(),
					EmployeeID = x.EmployeeId,
					Month = month.Month,
					Year = month.Year,
					Pay = x.Arrears,
				}));
				await _arrearService.AddBulk(arrears, month.Month, month.Year);

				//import incentivePaycut data into DB
				incentivePaycut.AddRange(employeesDataForSalary.Select(x => new IncentivesPayCut
				{
					ID = Guid.NewGuid(),
					EmployeeId = x.EmployeeId,
					Month = month.Month,
					Year = month.Year,
					Incentives = x.Incentives,
					PayCut = x.Paycut
				}));
				await _incentivesPayCutService.AddBulk(incentivePaycut);

				//import incomeTaxLimit
				incomeTaxLimit.AddRange(employeesDataForSalary.Select(x => new IncomeTaxLimit
				{
					ID = Guid.NewGuid(),
					Amount = x.IncomeTax,
					Month = month.Month,
					Year = month.Year,
					EmployeeId = x.EmployeeId,
				}));
				await _incomeTaxLimitService.AddBulk(incomeTaxLimit, month.Month, month.Year);

				//import income tax
				incentivePaycut.AddRange(employeesDataForSalary.Select(x => new IncentivesPayCut
				{
					ID = Guid.NewGuid(),
					EmployeeId = x.EmployeeId,
					Month = month.Month,
					Year = month.Year,
					Incentives = x.Incentives,
					PayCut = x.Paycut
				}));
				await _incentivesPayCutService.AddBulk(incentivePaycut);


				await inMemory.DbContext.SaveChangesAsync();
				inMemory.DbContext.ChangeTracker.Clear();

				var _payMonth = inMemory.DbContext.Payroll_PayMonth.FirstOrDefault(x => x.Month == month.Month && x.Year == month.Year);

				var response = await _payRollService.Process(_payMonth.ID);
				if (response.IsSuccess)
				{
					inMemory.DbContext.ChangeTracker.Clear();
					response = await _payRollService.Process(_payMonth.ID);
					if (response.IsSuccess)
					{
						inMemory.DbContext.ChangeTracker.Clear();
						response = await _payRollService.Release(_payMonth.ID);
					}
				}

				foreach (var item in employeesDataForSalary)
				{
					var declaration = await inMemory.UnitOfWork.GetRepositoryAsync<Declaration>()
						.SingleAsync(x => x.EmployeeId == item.EmployeeId && x.FinancialYearId == _payMonth.FinancialYearId);

					if (declaration != null)
					{
						await declarationFile.WriteAsync($"{item.Code},{item.Month},");
						await declarationFile.WriteAsync($"{declaration.No},{declaration.Salary},{declaration.Perquisites},");
						await declarationFile.WriteAsync($"{declaration.PreviousEmployment},{declaration.TotalSalary},{declaration.Allowance},");
						await declarationFile.WriteAsync($"{declaration.Balance},{declaration.StandardDeduction},{declaration.TaxOnEmployment},");
						await declarationFile.WriteAsync($"{declaration.Deductions},{declaration.IncomeChargeable},{declaration.HouseIncome},");
						await declarationFile.WriteAsync($"{declaration.OtherIncome},{declaration.GrossTotal},{declaration.EightyC},");
						await declarationFile.WriteAsync($"{declaration.EPF},{declaration.EightyD},{declaration.OtherSections},");
						await declarationFile.WriteAsync($"{declaration.Taxable},{declaration.Tax},{declaration.Relief},");
						await declarationFile.WriteAsync($"{declaration.Cess},{declaration.TaxPayable},{declaration.TaxPaid},");
						await declarationFile.WriteLineAsync($"{declaration.Due}");

					}

					var paySlip = await inMemory.UnitOfWork.GetRepositoryAsync<PaySheet>()
						.SingleAsync(x => x.EmployeeID == item.EmployeeId && x.PayMonthId == _payMonth.ID,
						include: o => o.Include(x => x.Employee).Include(x => x.Earnings).Include(x => x.Deductions).Include(x => x.PayMonth).Include(x => x.Employee));
					if (paySlip == null)
					{
						continue;
					}
					var basicValue = paySlip.Earnings.FirstOrDefault(x => x.ComponentId == _basicId);
					var HRAValue = paySlip.Earnings.FirstOrDefault(x => x.ComponentId == _hraId);
					var medicalTransValue = paySlip.Earnings.FirstOrDefault(x => x.ComponentId == _medicalId);
					var foodCouponsValue = paySlip.Earnings.FirstOrDefault(x => x.ComponentId == _foodCouponId);
					var splAllowanceValue = paySlip.Earnings.FirstOrDefault(x => x.ComponentId == _specialId);

					//for few rows difference 1 is showing.
					//But it will clear when sheet is provided without excel formulas.

					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},MonthDays,{item.MonthDays},{paySlip.PayMonth.Days},{item.MonthDays - paySlip.PayMonth.Days}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},EmployeeWorkingDays,{item.EmployeeWorkingDays},{paySlip.WorkDays},{item.EmployeeWorkingDays - paySlip.WorkDays}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},DaysPresent,{item.DaysPresent},{paySlip.PresentDays},{item.DaysPresent - paySlip.PresentDays}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},LOPDays,{item.LOPDays},{paySlip.LOPDays},{item.LOPDays - paySlip.LOPDays}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},LateComingDays,{item.LateComingDays},{paySlip.LCDays},{item.LateComingDays - paySlip.LCDays}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},UnauthorizedLeaves,{item.UnauthorizedLeaves},{paySlip.UADays},{item.UnauthorizedLeaves - paySlip.UADays}");

					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Salary,{item.Salary},{paySlip.Salary},{item.Salary - paySlip.Salary}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Basic,{item.EarnedBasic},{basicValue.Earning},{item.EarnedBasic - basicValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},HRA,{item.EarnedHRA},{HRAValue.Earning},{item.EarnedHRA - HRAValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Medical,{item.EarnedMedicalTransport},{medicalTransValue.Earning},{item.EarnedMedicalTransport - medicalTransValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Food,{item.EarnedFoodCoupons},{foodCouponsValue.Earning},{item.EarnedFoodCoupons - foodCouponsValue.Salary}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},SPLAllowance,{item.EarnedSpecialAllowance},{splAllowanceValue.Earning},{item.EarnedSpecialAllowance - splAllowanceValue.Earning}");


					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Earned Basic,{item.EarnedBasic},{basicValue.Earning},{item.EarnedBasic - basicValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Earned HRA,{item.EarnedHRA},{HRAValue.Earning},{item.EarnedHRA - HRAValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Earned Medical,{item.EarnedMedicalTransport},{medicalTransValue.Earning},{item.EarnedMedicalTransport - medicalTransValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Earned Food,{item.EarnedFoodCoupons},{foodCouponsValue.Earning},{item.EarnedFoodCoupons - foodCouponsValue.Salary}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Earned SPLAllowance,{item.EarnedSpecialAllowance},{splAllowanceValue.Earning},{item.EarnedSpecialAllowance - splAllowanceValue.Earning}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Arrears,{item.Arrears},{paySlip.Arrears},{item.Arrears - paySlip.Arrears}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Incentives,{item.Incentives},{paySlip.Incentive},{item.Incentives - paySlip.Incentive}");

					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},LOPAmount,{item.LOPAmount},{paySlip.LOP},{item.LOPAmount - paySlip.LOP}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},UnauthorizedAmount,{item.UnauthorizedAmount},{paySlip.UA},{item.UnauthorizedAmount - paySlip.UA}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},LateComingAmount,{item.LateComingAmount},{paySlip.LC},{item.LateComingAmount - paySlip.LC}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Salary Advance,{item.StaffSalaryAdvances},{paySlip.Loan},{item.StaffSalaryAdvances - paySlip.Loan}");

					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Pay cut,{item.Paycut},{paySlip.PayCut},{item.Paycut - paySlip.PayCut}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},PT,{item.PT},{paySlip.PTax},{item.PT - paySlip.PTax}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Income Tax,{item.IncomeTax + item.IncomeTaxCalc},{paySlip.Tax},{item.IncomeTax + item.IncomeTaxCalc - paySlip.Tax}");

					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},PF,{item.PFContribution},{paySlip.EPF},{item.PFContribution - paySlip.EPF}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},ESI Final,{item.ESIFinal},{paySlip.ESI},{item.ESIFinal - paySlip.ESI}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Gross Earning,{item.GrossEarnings},{paySlip.Gross},{item.GrossEarnings - paySlip.Gross}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},Gross Deduction,{item.GrossDeduction + item.IncomeTaxCalc}," +
						$"{paySlip.Deduction},{item.GrossDeduction + item.IncomeTaxCalc - paySlip.Deduction}");
					await salaryFile.WriteLineAsync($"{item.Code},{item.Month},NetTakeHome,{item.NetTakeHome},{paySlip.Net},{item.NetTakeHome - paySlip.Net}");

				}

				Assert.True(response.HasNoError);
			}
		}

		//Here we setup Data for , State, Organization, PaySettings, DeclarationSetting, EPF, ESI, FinancialYear, 
		//OldRegimSlab, NewRegimSlab, ProfessionalTaxSlab
		private async Task<bool> SetupData()
		{
			inMemory.DbContext.Organization_State.Add(new State
			{
				ID = _stateId,
				Name = "TS",
				Country = "India"
			});

			inMemory.DbContext.Organization_Organizations.Add(new Organizations
			{
				ID = _organizationId,
				Name = "Avontix",
				PAN = "KMOKG3808L",
				TAN = " AAAA99999A",
				MonthStartDay = 26
			});

			inMemory.DbContext.Organization_Location.Add(new Location
			{
				ID = workLocationId,
				StateId = _stateId,
				Name = "Test",
				Status = true,
			});

			inMemory.DbContext.Payroll_PaySettings.Add(new PaySettings
			{
				ID = _paySettingId,
				OrganizationId = _organizationId,
				FYFromMonth = 4,
				TaxDeductor = "Anil",
				TaxDeductorFNam = "Alla"
			});

			await inMemory.DbContext.SaveChangesAsync();

			inMemory.DbContext.Payroll_DeclarationSetting.Add(new DeclarationSetting
			{
				ID = _declarationSettingId,
				PaySettingsId = _paySettingId,
				MaxLimitEightyC = 150000,
				MaxLimitEightyD = 55000,
				TaxDedLastMonth = 12,
				EducationCess = 4.0m,
				HouseLoanInt = 200000
			});
			await inMemory.DbContext.SaveChangesAsync();

			inMemory.DbContext.Payroll_EPF.Add(new EPF
			{
				ID = _epfId,
				PaySettingsId = _paySettingId,
				EnableEPF = 1,
				EmployeeContrib = 1,
				IncludeinCTC = true,
			});

			await inMemory.UnitOfWork.GetRepositoryAsync<EPF>()
			   .AddAsync(new EPF
			   {
				   EmployeeContrib = 3,
				   IncludeinCTC = true,
				   PaySettingsId = _paySettingId,
				   PFConfiguration = "1",
				   RestrictedWage = 15000,
				   EnableEPF = 1
			   });
			await inMemory.DbContext.SaveChangesAsync();

			inMemory.DbContext.Payroll_ESI.Add(new ESI
			{
				ID = _esiId,
				PaySettingsId = _paySettingId,
				EnableESI = 1,
				ESISalaryLimit = 21000,
				ESINo = "1234A2103B",
				EmployeesCont = 1.75m,
				EmployerCont = 3.5m,
			});
			await inMemory.DbContext.SaveChangesAsync();

			var result = await _financialYearService.AddAsync(new FinancialYear
			{
				ID = _financialYrId,
				Name = "FY_2022_2023",
				FromYear = new DateTime(2022, 04, 26).Year,
				ToYear = new DateTime(2023, 03, 26).Year,
				PaySettingsId = _paySettingId
			});

			if (result.HasError) return false;

			inMemory.DbContext.Payroll_OldRegimeSlab.Add(new OldRegimeSlab
			{
				ID = Guid.NewGuid(),
				PaySettingsId = _paySettingId,
				IncomeFrom = 0,
				IncomeTo = 250000,
				TaxRate = 0,
			});
			inMemory.DbContext.Payroll_OldRegimeSlab.Add(new OldRegimeSlab
			{
				ID = Guid.NewGuid(),
				PaySettingsId = _paySettingId,
				IncomeFrom = 250000,
				IncomeTo = 500000,
				TaxRate = 5,
			});
			inMemory.DbContext.Payroll_OldRegimeSlab.Add(new OldRegimeSlab
			{
				ID = Guid.NewGuid(),
				PaySettingsId = _paySettingId,
				IncomeFrom = 500000,
				IncomeTo = 1000000,
				TaxRate = 20,
			});
			inMemory.DbContext.Payroll_OldRegimeSlab.Add(new OldRegimeSlab
			{
				ID = Guid.NewGuid(),
				PaySettingsId = _paySettingId,
				IncomeFrom = 1000000,
				IncomeTo = 999999999,
				TaxRate = 30,
			});
			await inMemory.DbContext.SaveChangesAsync();

			inMemory.DbContext.Payroll_NewRegimeSlab.Add(new NewRegimeSlab
			{
				ID = Guid.NewGuid(),
				PaySettingsId = _paySettingId,
				IncomeFrom = 0,
				IncomeTo = 100000,
				TaxRate = 5,
			});
			await inMemory.DbContext.SaveChangesAsync();
			var pt = new ProfessionalTax()
			{
				StateId = _stateId
			};
			var ptSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab
				{
					ID = Guid.NewGuid(),
					From = 0,
					To = 15000,
					Amount = 0,
					ProfessionalTax = pt,
				},
				new ProfessionalTaxSlab
				{
					ID = Guid.NewGuid(),
					From = 15000,
					To = 20000,
					Amount = 150,
					ProfessionalTax = pt,
				},
				new ProfessionalTaxSlab
				{
					ID = Guid.NewGuid(),
					From = 20000,
					To = 9999999,
					Amount = 200,
					ProfessionalTax = pt,
				}
			};
			inMemory.DbContext.Payroll_ProfessionalTaxSlab.AddRange(ptSlabs);

			inMemory.DbContext.Payroll_Section80C.Add(new Section80C
			{
				ID = _section80CId,
				Name = "Contribution to ULIP of LIC",
				Status = true
			});
			inMemory.DbContext.Payroll_Section80D.Add(new Section80D
			{
				ID = _section80DId,
				Name = "Medical Bills",
				Limit = 25000,
				Status = true
			});
			inMemory.DbContext.Payroll_OtherSections.Add(new OtherSections
			{
				ID = _otherSectionId,
				Name = "Donation to Chief Minister’s Relief Fund",
				Limit = 50000,
				Status = true
			});
			await inMemory.DbContext.SaveChangesAsync();

			await AddEarningComponent();

			return true;
		}
	}
}
