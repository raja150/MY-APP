using TranSmart.Core.Util;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.Payroll.List;
using TranSmart.Domain.Models.Payroll.Request;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;

namespace TranSmart.API.AutoMapper
{
	public class PayrollProfile : Profile
	{
		public PayrollProfile()
		{
			CreateMap<SalaryRequest, Salary>();
			CreateMap<Salary, SalaryModel>()
			   .ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
			   .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
			   .ForMember(d => d.Gender, opt => opt.MapFrom(x => x.Employee.Gender))
			   .ForMember(d => d.MobileNumber, opt => opt.MapFrom(x => x.Employee.MobileNumber));

			CreateMap<PaySheet, SalaryModel>()
			   .ForMember(d => d.Name, opt => opt.MapFrom(x => x.PayMonth.Name))
			   .ForMember(d => d.Monthly, opt => opt.MapFrom(x => x.Earnings.Sum(x => x.Earning)));

			CreateMap<PaySheet, SalarySummaryModel>()
			 .ForMember(d => d.Month, opt => opt.MapFrom(x => x.PayMonth.Name))
			 .ForMember(d => d.Salary, opt => opt.MapFrom(x => x.Net));
			CreateMap<PaySheetEarning, SalaryHeaderSummaryModel>()
			 .ForMember(d => d.SalaryId, opt => opt.MapFrom(x => x.PaySheetId))
			 .ForMember(d => d.Amount, opt => opt.MapFrom(x => x.Earning))
			 .ForMember(d => d.Name, opt => opt.MapFrom(x => x.HeaderName))
			 .ForMember(d => d.Order, opt => opt.MapFrom(x => x.Component.DisplayOrder));
			CreateMap<PaySheetDeduction, SalaryHeaderSummaryModel>()
			 .ForMember(d => d.SalaryId, opt => opt.MapFrom(x => x.PaySheetId))
			 .ForMember(d => d.Amount, opt => opt.MapFrom(x => x.Deduction))
			 .ForMember(d => d.Name, opt => opt.MapFrom(x => x.HeaderName));

			CreateMap<PaySheetEarning, SalaryEarningModel>()
				.ForMember(d => d.Component, opt => opt.MapFrom(x => x.Component.Name))
				.ForMember(d => d.Amount, opt => opt.MapFrom(x => x.Earning));


			CreateMap<PaySheetDeduction, SalaryDeductionModel>();

			CreateMap<SalaryEarningRequest, SalaryEarning>();
			CreateMap<SalaryEarning, SalaryEarningModel>()
			   .ForMember(d => d.Component, opt => opt.MapFrom(x => x.Component.Name))
			   .ForMember(d => d.PercentOnComp, opt => opt.MapFrom(x => x.PercentOnComp.Name))
			   .ForMember(d => d.DisplayOrder, opt => opt.MapFrom(x => x.Component.DisplayOrder));
			CreateMap<SalaryDeductionRequest, SalaryDeduction>();
			CreateMap<SalaryDeduction, SalaryDeductionModel>();


			CreateMap<Salary, SalaryList>()
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.DateOfJoining, opt => opt.MapFrom(x => x.Employee.DateOfJoining))
				.ForMember(d => d.Salary, opt => opt.MapFrom(x => x.Monthly))
				.ForMember(d => d.MobileNo, opt => opt.MapFrom(x => x.Employee.MobileNumber))
				.ForMember(d => d.EmpName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.EmpNo, opt => opt.MapFrom(x => x.Employee.No));

			CreateMap<TemplateEarning, SalaryEarningModel>()
			   .ForMember(d => d.Component, opt => opt.MapFrom(x => x.Component.Name))
			   .ForMember(d => d.PercentOnComp, opt => opt.MapFrom(x => x.PercentOnComp.Name))
			   .ForMember(d => d.DisplayOrder, opt => opt.MapFrom(x => x.Component.DisplayOrder))
			   .ForMember(d => d.Monthly, opt => opt.MapFrom(x => x.Amount))
			   .ForMember(d => d.Annually, opt => opt.MapFrom(x => x.Amount * 12));

			CreateMap<DeclarationRequest, Declaration>();
			CreateMap<Declaration, DeclarationModel>()
					 .ForMember(d => d.PaySettingId, opt => opt.MapFrom(x => x.FinancialYear.PaySettingsId))
					 .ForMember(d => d.No, opt => opt.MapFrom(x => x.Employee.No))
					 .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<LetOutPropertyRequest, LetOutProperty>();
			CreateMap<LetOutProperty, LetOutPropertyModel>();

			CreateMap<HraDeclarationRequest, HraDeclaration>();
			CreateMap<HraDeclaration, HraDeclarationModel>();

			CreateMap<Section6A80CRequest, Section6A80C>();
			CreateMap<Section6A80C, Section6A80CModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Section80C.Name));

			CreateMap<Section6A80DRequest, Section6A80D>();
			CreateMap<Section6A80D, Section6A80DModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Section80D.Name))
				.ForMember(d => d.Limit, opt => opt.MapFrom(x => x.Section80D.Limit));

			CreateMap<HomeLoanPayRequest, HomeLoanPay>();
			CreateMap<HomeLoanPay, HomeLoanPayModel>();

			CreateMap<Section6AOtherRequest, Section6AOther>();
			CreateMap<Section6AOther, Section6AOtherModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.OtherSections.Name));

			CreateMap<PrevEmploymentRequest, PrevEmployment>();
			CreateMap<PrevEmployment, PrevEmploymentModel>();

			CreateMap<OtherIncomeRequest, OtherIncomeSources>();
			CreateMap<OtherIncomeSources, OtherIncomeModel>();

			CreateMap<Declaration, DeclarationList>()
				.ForMember(d => d.Year, opt => opt.MapFrom(x => x.FinancialYear.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.DateOfJoining, opt => opt.MapFrom(x => x.Employee.DateOfJoining));

			CreateMap<PayMonth, PayMonthList>();
			CreateMap<PayMonth, PayMonthModel>();
			CreateMap<PaySheet, PaySheetModel>()
				.ForMember(d => d.No, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Location, opt => opt.MapFrom(x => x.Employee.WorkLocation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Days, opt => opt.MapFrom(x => x.PayMonth.Days));
			CreateMap<PaySheetEarning, PaySheetEarningModel>();
			CreateMap<PaySheetDeduction, PaySheetDeductionModel>();

			CreateMap<PaySheet, PaySlipModel>()
				.ForMember(d => d.No, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Location, opt => opt.MapFrom(x => x.Employee.WorkLocation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Days, opt => opt.MapFrom(x => x.PayMonth.Days));

			CreateMap<DeclarationAllowance, DeclarationAllowanceModel>();

			CreateMap<PaySheet, EmployeePayMonthModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.PayMonth.Name))
				.ForMember(d => d.Year, opt => opt.MapFrom(x => x.PayMonth.Year))
				.ForMember(d => d.Month, opt => opt.MapFrom(x => x.PayMonth.Month));


			CreateMap<FinancialYear, OpenFinancialYearList>()
				.ForMember(d => d.From, opt => opt.MapFrom(x => DateUtil.FYFromDate(x.FromYear, x.PaySettings.FYFromMonth, 1)))
				.ForMember(d => d.To, opt => opt.MapFrom(x => DateUtil.FYToDate(x.FromYear, x.PaySettings.FYFromMonth, 1)));

			CreateMap<IncentivesPayCutRequest, IncentivesPayCut>();
			CreateMap<IncentivesPayCut, IncentivesPayCutModel>()
				.ForMember(d => d.EmployeeNo, so => so.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, so => so.MapFrom(x => x.Employee.Name));

			CreateMap<IncentivesPayCut, IncentivesPayCutList>()
				.ForMember(d => d.Year, opt => opt.MapFrom(x => string.Concat(x.Month, "-", x.Year)))
		  .ForMember(d => d.EmployeeNo, o => o.MapFrom(x => x.Employee.No))
		   .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
		   .ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
		   .ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));

			CreateMap<PaySheet, PaySlipPdfModel>()
				 .ForMember(d => d.No, opt => opt.MapFrom(x => x.Employee.No))
				 .ForMember(d => d.PAN, opt => opt.MapFrom(x => x.Employee.PanNumber))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Location, opt => opt.MapFrom(x => x.Employee.WorkLocation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Month, opt => opt.MapFrom(x => x.PayMonth.Name))
				.ForMember(d => d.Days, opt => opt.MapFrom(x => x.PayMonth.Days));

			CreateMap<ArrearsRequest, Arrear>();
			CreateMap<Arrear, ArrearsModel>()
			.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
			.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<Arrear, ArrearsList>()
				.ForMember(d => d.Year, opt => opt.MapFrom(x => string.Concat(x.Month, "-", x.Year)))
		  .ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
		  .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
		  .ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
		  .ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));

			CreateMap<IncomeTaxLimitRequest, IncomeTaxLimit>();
			CreateMap<IncomeTaxLimit, IncomeTaxLimitModel>()
			.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
			.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<IncomeTaxLimit, IncomeTaxLimitList>()
				.ForMember(d => d.Year, opt => opt.MapFrom(x => string.Concat(x.Month, "-", x.Year)))
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));


			CreateMap<EmployeePayInfoModel, EmployeePayInfoAudit>()
			   .ForMember(d => d.PayType, opt => opt.MapFrom(x => x.PayMode));

			CreateMap<EmployeePayInfoAudit, EmployeePayInfoModel>();

			CreateMap<EmployeePayInfoModel, EmployeePayInfo>();
			CreateMap<EmployeePayInfo, EmployeePayInfoModel>()
			   .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
			   .ForMember(d => d.No, s => s.MapFrom(x => x.Employee.No))
			   .ForMember(d => d.AccountNoVerify, opt => opt.MapFrom(x => x.AccountNo));


			CreateMap<EmployeePayInfo, EmployeePayInfoList>()
				.ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.No, s => s.MapFrom(x => x.Employee.No));

			CreateMap<LatecomersRequest, Latecomers>();
			CreateMap<Latecomers, LatecomersModel>()
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<Latecomers, LatecomersList>()
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Year, opt => opt.MapFrom(x => string.Concat(x.Month, "-", x.Year)))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name));

		}
	}
}
