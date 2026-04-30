using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Repository.Reports;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models.Reports;

namespace TranSmart.Service.Reports
{
	public interface IPayRollReportService
	{
		Task<IEnumerable<EmployeeEsiModel>> EmployeeESI(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<EmployeeEpfModel>> EmployeeEPF(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<IncentivesPayCutReportModel>> IncentivesPayCuts(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<ArrearsReportModel>> Arrears(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<ProffesionalTaxReportModel>> GetProfessionalTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<IncomeTaxReportModel>> GetIncomeTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth);
		Task<IEnumerable<EmployeePayInfoRptModel>> GetPaymentInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId);
		Task<IEnumerable<EmployeeEpfModel>> GetProvidentFundInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type);
		Task<IEnumerable<EmployeeEsiModel>> GetESIInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type);
	}
	public class PayRollReportService : IPayRollReportService
	{
		private readonly IUnitOfWork _UOW;
		private readonly IEmployeeStatutoryRepository _empStatutoryReportRepository;
		public PayRollReportService(IUnitOfWork uow, IEmployeeStatutoryRepository empStatutoryReportRepository)
		{
			_UOW = uow;
			_empStatutoryReportRepository = empStatutoryReportRepository;
		}
		public async Task<IEnumerable<EmployeeEsiModel>> EmployeeESI(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{
			IEnumerable<PaySheet> paySheet = (await _UOW.GetRepositoryAsync<PaySheet>().GetAsync(
				predicate: p => p.PayMonth.Month == payMonth && p.PayMonth.Year == payYear)).ToList();

			IEnumerable<EmpStatutory> empStat = await _UOW.GetRepositoryAsync<EmpStatutory>().GetAsync(
				predicate: x => x.EnableESI == 1
								&& ((!departmentId.HasValue || x.Emp.DepartmentId == departmentId)
								&& (!designationId.HasValue || x.Emp.DesignationId == designationId)
								&& (!employeeId.HasValue || x.EmpId == employeeId)
								&& (!teamId.HasValue || x.Emp.TeamId == teamId)),
				include: i => i.Include(x => x.Emp).ThenInclude(x => x.Designation)
				.Include(x => x.Emp).ThenInclude(x => x.Department)
				.Include(x => x.Emp).ThenInclude(x => x.WorkLocation));

			return from EmpStatutory item in empStat
				   let sheet = paySheet.FirstOrDefault(x => x.EmployeeID == item.EmpId)
				   select new EmployeeEsiModel
				   {
					   Branch = item.Emp.WorkLocation.Name,
					   EmployeeName = item.Emp.Name,
					   ESINo = item.ESINo,
					   EmployeeCode = item.Emp.No,
					   Designation = item.Emp.Designation.Name,
					   ESIDeduction = sheet != null ? sheet.ESI : 0,
					   GrossWage = sheet != null ? sheet.Gross : 0,
					   LOPDays = sheet != null ? sheet.LOPDays : 0,
					   DateOfJoining = item.Emp.DateOfJoining
				   };
		}
		public async Task<IEnumerable<EmployeeEpfModel>> EmployeeEPF(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{
			var models = new List<EmployeeEpfModel>();
			IEnumerable<PaySheet> paySheet = (await _UOW.GetRepositoryAsync<PaySheet>().GetAsync(
		predicate: p => p.PayMonth.Month == payMonth && p.PayMonth.Year == payYear)).ToList();

			IEnumerable<EmpStatutory> empStat = await _UOW.GetRepositoryAsync<EmpStatutory>().GetAsync(
				predicate: x => x.EnablePF == 1
								&& ((departmentId == null || x.Emp.DepartmentId == departmentId)
								&& (designationId == null || x.Emp.DesignationId == designationId)
								&& (employeeId == null || x.EmpId == employeeId)
								&& (teamId == null || x.Emp.TeamId == teamId)),
				include: i => i.Include(x => x.Emp).ThenInclude(x => x.Designation)
				.Include(x => x.Emp).ThenInclude(x => x.Department)
				.Include(x => x.Emp).ThenInclude(x => x.WorkLocation));

			models.AddRange(from EmpStatutory item in empStat
							let sheet = paySheet.FirstOrDefault(x => x.EmployeeID == item.EmpId)
							select new EmployeeEpfModel
							{
								UANNo = item.UAN,
								MemberName = item.Emp.Name,
								GrossWages = sheet != null ? sheet.Gross : 0,
								NCPDays = sheet != null ? sheet.LOPDays : 0,
								EPFCont = item.EmployeeContrib,
								EmployeeName = item.Emp.Name,
							});
			return models;
		}
		public async Task<IEnumerable<IncentivesPayCutReportModel>> IncentivesPayCuts(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{

			return await _UOW.GetRepositoryAsync<IncentivesPayCut>().GetWithSelectAsync(
			   selector: e => new IncentivesPayCutReportModel
			   {

				   EmpCode = e.Employee.No,
				   Name = e.Employee.Name,
				   Designation = e.Employee.Designation.Name,
				   DateOfJoining = e.Employee.DateOfJoining,
				   Department = e.Employee.Department.Name,
				   Month = e.Month,
				   TaxAmount = e.PayCut,
				   PAN = e.Employee.PanNumber,
				   FaxFilesAndArrears = e.FaxFilesAndArrears,
				   SundayInc = e.SundayInc,
				   SpotInc = e.SpotInc,
				   PunctualityInc = e.PunctualityInc,
				   OtherInc = e.OtherInc,
				   ProductionInc = e.ProductionInc,
				   FirstMinuteInc = e.FirstMinuteInc,
				   TTeamInc = e.TTeamInc,
				   WeeklyStarInc = e.WeeklyStarInc,
				   ExternalQualityFeedbackDed = e.ExternalQualityFeedbackDed,
				   CentumClub = e.CentumClub,
				   DoublePay = e.DoublePay,
				   InternalQualityFeedbackDed = e.InternalQualityFeedbackDed,
				   LateComingDed = e.LateComingDed,
				   NightShift = e.NightShift,
				   OtherDed = e.OtherDed,
				   UnauthorizedLeaveDed = e.UnauthorizedLeaveDed,
			   },
			   predicate: p => p.Year == payYear && p.Month == payMonth
								&& ((departmentId == null || p.Employee.DepartmentId == departmentId)
								&& (designationId == null || p.Employee.DesignationId == designationId)
								&& (employeeId == null || p.EmployeeId == employeeId)
								&& (teamId == null || p.Employee.TeamId == teamId)),
				   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
								.Include(x => x.Employee).ThenInclude(x => x.Department));

		}
		public async Task<IEnumerable<ArrearsReportModel>> Arrears(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{
			return await _UOW.GetRepositoryAsync<Arrear>().GetWithSelectAsync(
			  selector: e => new ArrearsReportModel
			  {

				  EmpCode = e.Employee.No,
				  Name = e.Employee.Name,
				  Designation = e.Employee.Designation.Name,
				  DOJ = e.Employee.DateOfJoining,
				  Month = e.Month,
				  TaxAmount = e.Pay,
			  },
			   predicate: p => p.Year == payYear && p.Month == payMonth
								&& ((departmentId == null || p.Employee.DepartmentId == departmentId)
								&& (designationId == null || p.Employee.DesignationId == designationId)
								&& (employeeId == null || p.EmployeeID == employeeId)
								&& (teamId == null || p.Employee.TeamId == teamId)),
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
								.Include(x => x.Employee).ThenInclude(x => x.Department));
		}
		public async Task<IEnumerable<ProffesionalTaxReportModel>> GetProfessionalTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{
			return await _UOW.GetRepositoryAsync<PaySheet>().GetWithSelectAsync(
				selector: d => new ProffesionalTaxReportModel
				{
					EmpCode = d.Employee.No,
					Name = d.Employee.Name,
					Designation = d.Employee.Designation.Name,
					Department = d.Employee.Department.Name,
					TaxAmount = d.Tax,
					PAN = d.Employee.PanNumber,
					Month = d.PayMonth.Month,
					Year = d.PayMonth.Year
				},
				 predicate: p => p.PTax > 0 && p.PayMonth.Year == payYear && p.PayMonth.Month == payMonth
								&& ((departmentId == null || p.Employee.DepartmentId == departmentId)
								&& (designationId == null || p.Employee.DesignationId == designationId)
								&& (employeeId == null || p.EmployeeID == employeeId)
								&& (teamId == null || p.Employee.TeamId == teamId)),
				 include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
								.Include(x => x.Employee).ThenInclude(x => x.Department)
								.Include(x => x.PayMonth));
		}
		public async Task<IEnumerable<IncomeTaxReportModel>> GetIncomeTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int payYear, int? payMonth)
		{
			return await _UOW.GetRepositoryAsync<PaySheet>().GetWithSelectAsync(
				selector: d => new IncomeTaxReportModel
				{
					EmpCode = d.Employee.No,
					Name = d.Employee.Name,
					Designation = d.Employee.Designation.Name,
					Department = d.Employee.Department.Name,
					TaxAmount = d.Tax,
					PAN = d.Employee.PanNumber,
					Month = d.PayMonth.Month,
					Year = d.PayMonth.Year
				},
				predicate: p => p.Tax > 0 && p.PayMonth.Year == payYear && p.PayMonth.Month == payMonth
								&& ((departmentId == null || p.Employee.DepartmentId == departmentId)
								&& (designationId == null || p.Employee.DesignationId == designationId)
								&& (employeeId == null || p.EmployeeID == employeeId)
								&& (teamId == null || p.Employee.TeamId == teamId)),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
								.Include(x => x.Employee).ThenInclude(x => x.Department));
		}



		public async Task<IEnumerable<EmployeePayInfoRptModel>> GetPaymentInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId)
		{
			return await _UOW.GetRepositoryAsync<EmployeePayInfo>().GetWithSelectAsync(
			   selector: d => new EmployeePayInfoRptModel
			   {
				   EmployeeName = d.Employee.Name,
				   EmployeeCode = d.Employee.No,
				   Bank = d.Bank.Name,
				   BankName = d.BankName,
				   IFSCCode = d.IFSCCode,
				   AccountNo = d.AccountNo,
				   PayMode = d.PayMode,
				   DateOfJoining = d.Employee.DateOfJoining,
				   Department = d.Employee.Department.Name,
				   Designation = d.Employee.Designation.Name,
			   },
			   predicate: p => (employeeId == null || p.EmployeeId == employeeId)
								&& ((departmentId == null || p.Employee.DepartmentId == departmentId)
								&& (designationId == null || p.Employee.DesignationId == designationId)
								&& (teamId == null || p.Employee.TeamId == teamId)),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
								.Include(x => x.Employee).ThenInclude(x => x.Department));
		}

		public async Task<IEnumerable<EmployeeEpfModel>> GetProvidentFundInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return await _empStatutoryReportRepository.GetProvidentFundInfo(departmentId, designationId, teamId, employeeId, type);
		}

		public async Task<IEnumerable<EmployeeEsiModel>> GetESIInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return await _empStatutoryReportRepository.GetESIInfo(departmentId, designationId, teamId, employeeId, type);
		}
	}
}
