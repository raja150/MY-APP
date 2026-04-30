using Microsoft.EntityFrameworkCore;

namespace TranSmart.Data
{
    public partial class TranSmartContext
    {
        public DbSet<TranSmart.Domain.Entities.AppSettings.UserSettings> AppSettings_UserSettings { get; set; }
        public DbSet<TranSmart.Domain.Entities.AppSettings.Role> AppSettings_Role { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.State> Organization_State { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Organizations> Organization_Organizations { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.ProbationPeriod> Organization_ProbationPeriod { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Location> Organization_Location { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Department> Organization_Department { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Designation> Organization_Designation { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.LOB> Organization_LOB { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Team> Organization_Team { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.FunctionalArea> Organization_FunctionalArea { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Employee> Organization_Employee { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.Allocation> Organization_Allocation { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeFamily> Organization_EmployeeFamily { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeePresentAd> Organization_EmployeePresentAd { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeePermanentAd> Organization_EmployeePermanentAd { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeEmergencyAd> Organization_EmployeeEmergencyAd { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeResignation> Organization_EmployeeResignation { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeWorkExp> Organization_EmployeeWorkExp { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeEducation> Organization_EmployeeEducation { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.WorkType> Organization_WorkType { get; set; }
        public DbSet<TranSmart.Domain.Entities.Organization.EmployeeCategory> Organization_EmployeeCategory { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.ApprovedLeaves> Leave_ApprovedLeaves { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.Holidays> Leave_Holidays { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.Exemptions> Leave_Exemptions { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.AdjustLeave> Leave_AdjustLeave { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.LeaveType> Leave_LeaveType { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.LeaveTypeSchedule> Leave_LeaveTypeSchedule { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.LeaveSettings> Leave_LeaveSettings { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.Shift> Leave_Shift { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.WeekOffSetup> Leave_WeekOffSetup { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.WeekOffDays> Leave_WeekOffDays { get; set; }
        public DbSet<TranSmart.Domain.Entities.Leave.WorkHoursSetting> Leave_WorkHoursSetting { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.Bank> Payroll_Bank { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.EmpBonus> Payroll_EmpBonus { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.DeductionComponent> Payroll_DeductionComponent { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.EarningComponent> Payroll_EarningComponent { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.Loan> Payroll_Loan { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.ProfessionalTax> Payroll_ProfessionalTax { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.ProfessionalTaxSlab> Payroll_ProfessionalTaxSlab { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.Section80C> Payroll_Section80C { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.Section80D> Payroll_Section80D { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.OtherSections> Payroll_OtherSections { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.PaySettings> Payroll_PaySettings { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.DeclarationSetting> Payroll_DeclarationSetting { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.EPF> Payroll_EPF { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.ESI> Payroll_ESI { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.PaySetup> Payroll_PaySetup { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.FinancialYear> Payroll_FinancialYear { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.OldRegimeSlab> Payroll_OldRegimeSlab { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.NewRegimeSlab> Payroll_NewRegimeSlab { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.EmpStatutory> Payroll_EmpStatutory { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.Template> Payroll_Template { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.TemplateEarning> Payroll_TemplateEarning { get; set; }
        public DbSet<TranSmart.Domain.Entities.Payroll.TemplateDeduction> Payroll_TemplateDeduction { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.TicketStatus> Helpdesk_TicketStatus { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.DeskGroup> Helpdesk_DeskGroup { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.DeskGroupEmployee> Helpdesk_DeskGroupEmployee { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.DeskDepartment> Helpdesk_DeskDepartment { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.DepartmentGroup> Helpdesk_DepartmentGroup { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.HelpTopic> Helpdesk_HelpTopic { get; set; }
        public DbSet<TranSmart.Domain.Entities.Helpdesk.HelpTopicSub> Helpdesk_HelpTopicSub { get; set; }

    }
}
