using AutoMapper;

namespace TranSmart.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //https://dotnettutorials.net/lesson/automapper-in-c-sharp/

            CreateMap<TranSmart.Domain.Models.AppSettings.UserSettingsModel, TranSmart.Domain.Entities.AppSettings.UserSettings>();
            CreateMap<TranSmart.Domain.Entities.AppSettings.UserSettings, TranSmart.Domain.Models.AppSettings.UserSettingsModel>();

            CreateMap<TranSmart.Domain.Entities.AppSettings.UserSettings, TranSmart.Domain.Models.AppSettings.UserSettingsList>();

            CreateMap<TranSmart.Domain.Models.AppSettings.RoleModel, TranSmart.Domain.Entities.AppSettings.Role>();
            CreateMap<TranSmart.Domain.Entities.AppSettings.Role, TranSmart.Domain.Models.AppSettings.RoleModel>();

            CreateMap<TranSmart.Domain.Entities.AppSettings.Role, TranSmart.Domain.Models.AppSettings.RoleList>();

            CreateMap<TranSmart.Domain.Models.Organization.StateModel, TranSmart.Domain.Entities.Organization.State>();
            CreateMap<TranSmart.Domain.Entities.Organization.State, TranSmart.Domain.Models.Organization.StateModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.State, TranSmart.Domain.Models.Organization.StateList>();

            CreateMap<TranSmart.Domain.Models.Organization.OrganizationsModel, TranSmart.Domain.Entities.Organization.Organizations>();
            CreateMap<TranSmart.Domain.Entities.Organization.Organizations, TranSmart.Domain.Models.Organization.OrganizationsModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Organizations, TranSmart.Domain.Models.Organization.OrganizationsList>();

            CreateMap<TranSmart.Domain.Models.Organization.ProbationPeriodModel, TranSmart.Domain.Entities.Organization.ProbationPeriod>();
            CreateMap<TranSmart.Domain.Entities.Organization.ProbationPeriod, TranSmart.Domain.Models.Organization.ProbationPeriodModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.ProbationPeriod, TranSmart.Domain.Models.Organization.ProbationPeriodList>();

            CreateMap<TranSmart.Domain.Models.Organization.LocationModel, TranSmart.Domain.Entities.Organization.Location>();
            CreateMap<TranSmart.Domain.Entities.Organization.Location, TranSmart.Domain.Models.Organization.LocationModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Location, TranSmart.Domain.Models.Organization.LocationList>()
                .ForMember(d => d.State, s => s.MapFrom(x => x.State.Name))
                .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
                .ForMember(d => d.WeekOffSetup, s => s.MapFrom(x => x.WeekOffSetup.Name))
                .ForMember(d => d.WorkHoursSetting, s => s.MapFrom(x => x.WorkHoursSetting.Name));

            CreateMap<TranSmart.Domain.Models.Organization.DepartmentModel, TranSmart.Domain.Entities.Organization.Department>();
            CreateMap<TranSmart.Domain.Entities.Organization.Department, TranSmart.Domain.Models.Organization.DepartmentModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Department, TranSmart.Domain.Models.Organization.DepartmentList>()
                .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
                .ForMember(d => d.WeekOffSetup, s => s.MapFrom(x => x.WeekOffSetup.Name))
                .ForMember(d => d.WorkHoursSetting, s => s.MapFrom(x => x.WorkHoursSetting.Name));

            CreateMap<TranSmart.Domain.Models.Organization.DesignationModel, TranSmart.Domain.Entities.Organization.Designation>();
            CreateMap<TranSmart.Domain.Entities.Organization.Designation, TranSmart.Domain.Models.Organization.DesignationModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Designation, TranSmart.Domain.Models.Organization.DesignationList>()
                .ForMember(d => d.Department, s => s.MapFrom(x => x.Department.Name))
                .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
                .ForMember(d => d.WeekOffSetup, s => s.MapFrom(x => x.WeekOffSetup.Name))
                .ForMember(d => d.WorkHoursSetting, s => s.MapFrom(x => x.WorkHoursSetting.Name));

            CreateMap<TranSmart.Domain.Models.Organization.LOBModel, TranSmart.Domain.Entities.Organization.LOB>();
            CreateMap<TranSmart.Domain.Entities.Organization.LOB, TranSmart.Domain.Models.Organization.LOBModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.LOB, TranSmart.Domain.Models.Organization.LOBList>();

            CreateMap<TranSmart.Domain.Models.Organization.TeamModel, TranSmart.Domain.Entities.Organization.Team>();
            CreateMap<TranSmart.Domain.Entities.Organization.Team, TranSmart.Domain.Models.Organization.TeamModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Team, TranSmart.Domain.Models.Organization.TeamList>()
                .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
                .ForMember(d => d.WeekOffSetup, s => s.MapFrom(x => x.WeekOffSetup.Name))
                .ForMember(d => d.WorkHoursSetting, s => s.MapFrom(x => x.WorkHoursSetting.Name));

            CreateMap<TranSmart.Domain.Models.Organization.FunctionalAreaModel, TranSmart.Domain.Entities.Organization.FunctionalArea>();
            CreateMap<TranSmart.Domain.Entities.Organization.FunctionalArea, TranSmart.Domain.Models.Organization.FunctionalAreaModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.FunctionalArea, TranSmart.Domain.Models.Organization.FunctionalAreaList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeModel, TranSmart.Domain.Entities.Organization.Employee>();
            CreateMap<TranSmart.Domain.Entities.Organization.Employee, TranSmart.Domain.Models.Organization.EmployeeModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Employee, TranSmart.Domain.Models.Organization.EmployeeList>()
                .ForMember(d => d.Department, s => s.MapFrom(x => x.Department.Name))
                .ForMember(d => d.Designation, s => s.MapFrom(x => x.Designation.Name))
                .ForMember(d => d.WorkLocation, s => s.MapFrom(x => x.WorkLocation.Name))
                .ForMember(d => d.WorkType, s => s.MapFrom(x => x.WorkType.Name))
                .ForMember(d => d.Team, s => s.MapFrom(x => x.Team.Name));

            CreateMap<TranSmart.Domain.Models.Organization.AllocationModel, TranSmart.Domain.Entities.Organization.Allocation>();
            CreateMap<TranSmart.Domain.Entities.Organization.Allocation, TranSmart.Domain.Models.Organization.AllocationModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.Allocation, TranSmart.Domain.Models.Organization.AllocationList>()
                .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
                .ForMember(d => d.WeekOffSetup, s => s.MapFrom(x => x.WeekOffSetup.Name))
                .ForMember(d => d.WorkHoursSetting, s => s.MapFrom(x => x.WorkHoursSetting.Name));

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeFamilyModel, TranSmart.Domain.Entities.Organization.EmployeeFamily>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeFamily, TranSmart.Domain.Models.Organization.EmployeeFamilyModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeFamily, TranSmart.Domain.Models.Organization.EmployeeFamilyList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeePresentAdModel, TranSmart.Domain.Entities.Organization.EmployeePresentAd>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeePresentAd, TranSmart.Domain.Models.Organization.EmployeePresentAdModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeePresentAd, TranSmart.Domain.Models.Organization.EmployeePresentAdList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeePermanentAdModel, TranSmart.Domain.Entities.Organization.EmployeePermanentAd>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeePermanentAd, TranSmart.Domain.Models.Organization.EmployeePermanentAdModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeePermanentAd, TranSmart.Domain.Models.Organization.EmployeePermanentAdList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeEmergencyAdModel, TranSmart.Domain.Entities.Organization.EmployeeEmergencyAd>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeEmergencyAd, TranSmart.Domain.Models.Organization.EmployeeEmergencyAdModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeEmergencyAd, TranSmart.Domain.Models.Organization.EmployeeEmergencyAdList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeResignationModel, TranSmart.Domain.Entities.Organization.EmployeeResignation>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeResignation, TranSmart.Domain.Models.Organization.EmployeeResignationModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeResignation, TranSmart.Domain.Models.Organization.EmployeeResignationList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeWorkExpModel, TranSmart.Domain.Entities.Organization.EmployeeWorkExp>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeWorkExp, TranSmart.Domain.Models.Organization.EmployeeWorkExpModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeWorkExp, TranSmart.Domain.Models.Organization.EmployeeWorkExpList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeEducationModel, TranSmart.Domain.Entities.Organization.EmployeeEducation>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeEducation, TranSmart.Domain.Models.Organization.EmployeeEducationModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeEducation, TranSmart.Domain.Models.Organization.EmployeeEducationList>();

            CreateMap<TranSmart.Domain.Models.Organization.WorkTypeModel, TranSmart.Domain.Entities.Organization.WorkType>();
            CreateMap<TranSmart.Domain.Entities.Organization.WorkType, TranSmart.Domain.Models.Organization.WorkTypeModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.WorkType, TranSmart.Domain.Models.Organization.WorkTypeList>();

            CreateMap<TranSmart.Domain.Models.Organization.EmployeeCategoryModel, TranSmart.Domain.Entities.Organization.EmployeeCategory>();
            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeCategory, TranSmart.Domain.Models.Organization.EmployeeCategoryModel>();

            CreateMap<TranSmart.Domain.Entities.Organization.EmployeeCategory, TranSmart.Domain.Models.Organization.EmployeeCategoryList>();

            CreateMap<TranSmart.Domain.Models.Leave.ApprovedLeavesModel, TranSmart.Domain.Entities.Leave.ApprovedLeaves>();
            CreateMap<TranSmart.Domain.Entities.Leave.ApprovedLeaves, TranSmart.Domain.Models.Leave.ApprovedLeavesModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.ApprovedLeaves, TranSmart.Domain.Models.Leave.ApprovedLeavesList>()
                .ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name));

            CreateMap<TranSmart.Domain.Models.Leave.HolidaysModel, TranSmart.Domain.Entities.Leave.Holidays>();
            CreateMap<TranSmart.Domain.Entities.Leave.Holidays, TranSmart.Domain.Models.Leave.HolidaysModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.Holidays, TranSmart.Domain.Models.Leave.HolidaysList>();

            CreateMap<TranSmart.Domain.Models.Leave.ExemptionsModel, TranSmart.Domain.Entities.Leave.Exemptions>();
            CreateMap<TranSmart.Domain.Entities.Leave.Exemptions, TranSmart.Domain.Models.Leave.ExemptionsModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.Exemptions, TranSmart.Domain.Models.Leave.ExemptionsList>();

            CreateMap<TranSmart.Domain.Models.Leave.AdjustLeaveModel, TranSmart.Domain.Entities.Leave.AdjustLeave>();
            CreateMap<TranSmart.Domain.Entities.Leave.AdjustLeave, TranSmart.Domain.Models.Leave.AdjustLeaveModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.AdjustLeave, TranSmart.Domain.Models.Leave.AdjustLeaveList>()
                .ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name))
                .ForMember(d => d.LeaveType, s => s.MapFrom(x => x.LeaveType.Name));

            CreateMap<TranSmart.Domain.Models.Leave.LeaveTypeModel, TranSmart.Domain.Entities.Leave.LeaveType>();
            CreateMap<TranSmart.Domain.Entities.Leave.LeaveType, TranSmart.Domain.Models.Leave.LeaveTypeModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.LeaveType, TranSmart.Domain.Models.Leave.LeaveTypeList>();

            CreateMap<TranSmart.Domain.Models.Leave.LeaveTypeScheduleModel, TranSmart.Domain.Entities.Leave.LeaveTypeSchedule>();
            CreateMap<TranSmart.Domain.Entities.Leave.LeaveTypeSchedule, TranSmart.Domain.Models.Leave.LeaveTypeScheduleModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.LeaveTypeSchedule, TranSmart.Domain.Models.Leave.LeaveTypeScheduleList>();

            CreateMap<TranSmart.Domain.Models.Leave.LeaveSettingsModel, TranSmart.Domain.Entities.Leave.LeaveSettings>();
            CreateMap<TranSmart.Domain.Entities.Leave.LeaveSettings, TranSmart.Domain.Models.Leave.LeaveSettingsModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.LeaveSettings, TranSmart.Domain.Models.Leave.LeaveSettingsList>()
                .ForMember(d => d.CompCreditTo, s => s.MapFrom(x => x.CompCreditTo.Name))
                .ForMember(d => d.CompoLeaveType, s => s.MapFrom(x => x.CompoLeaveType.Name));

            CreateMap<TranSmart.Domain.Models.Leave.ShiftModel, TranSmart.Domain.Entities.Leave.Shift>();
            CreateMap<TranSmart.Domain.Entities.Leave.Shift, TranSmart.Domain.Models.Leave.ShiftModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.Shift, TranSmart.Domain.Models.Leave.ShiftList>();

            CreateMap<TranSmart.Domain.Models.Leave.WeekOffSetupModel, TranSmart.Domain.Entities.Leave.WeekOffSetup>();
            CreateMap<TranSmart.Domain.Entities.Leave.WeekOffSetup, TranSmart.Domain.Models.Leave.WeekOffSetupModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.WeekOffSetup, TranSmart.Domain.Models.Leave.WeekOffSetupList>();

            CreateMap<TranSmart.Domain.Models.Leave.WeekOffDaysModel, TranSmart.Domain.Entities.Leave.WeekOffDays>();
            CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.WeekOffDaysModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.WeekOffDaysList>();

            CreateMap<TranSmart.Domain.Models.Leave.WorkHoursSettingModel, TranSmart.Domain.Entities.Leave.WorkHoursSetting>();
            CreateMap<TranSmart.Domain.Entities.Leave.WorkHoursSetting, TranSmart.Domain.Models.Leave.WorkHoursSettingModel>();

            CreateMap<TranSmart.Domain.Entities.Leave.WorkHoursSetting, TranSmart.Domain.Models.Leave.WorkHoursSettingList>();

            CreateMap<TranSmart.Domain.Models.Payroll.BankModel, TranSmart.Domain.Entities.Payroll.Bank>();
            CreateMap<TranSmart.Domain.Entities.Payroll.Bank, TranSmart.Domain.Models.Payroll.BankModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.Bank, TranSmart.Domain.Models.Payroll.BankList>();

            CreateMap<TranSmart.Domain.Models.Payroll.EmpBonusModel, TranSmart.Domain.Entities.Payroll.EmpBonus>();
            CreateMap<TranSmart.Domain.Entities.Payroll.EmpBonus, TranSmart.Domain.Models.Payroll.EmpBonusModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.EmpBonus, TranSmart.Domain.Models.Payroll.EmpBonusList>()
                .ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.DeductionComponentModel, TranSmart.Domain.Entities.Payroll.DeductionComponent>();
            CreateMap<TranSmart.Domain.Entities.Payroll.DeductionComponent, TranSmart.Domain.Models.Payroll.DeductionComponentModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.DeductionComponent, TranSmart.Domain.Models.Payroll.DeductionComponentList>();

            CreateMap<TranSmart.Domain.Models.Payroll.EarningComponentModel, TranSmart.Domain.Entities.Payroll.EarningComponent>();
            CreateMap<TranSmart.Domain.Entities.Payroll.EarningComponent, TranSmart.Domain.Models.Payroll.EarningComponentModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.EarningComponent, TranSmart.Domain.Models.Payroll.EarningComponentList>();

            CreateMap<TranSmart.Domain.Models.Payroll.LoanModel, TranSmart.Domain.Entities.Payroll.Loan>();
            CreateMap<TranSmart.Domain.Entities.Payroll.Loan, TranSmart.Domain.Models.Payroll.LoanModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.Loan, TranSmart.Domain.Models.Payroll.LoanList>()
                .ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.ProfessionalTaxModel, TranSmart.Domain.Entities.Payroll.ProfessionalTax>();
            CreateMap<TranSmart.Domain.Entities.Payroll.ProfessionalTax, TranSmart.Domain.Models.Payroll.ProfessionalTaxModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.ProfessionalTax, TranSmart.Domain.Models.Payroll.ProfessionalTaxList>()
                .ForMember(d => d.State, s => s.MapFrom(x => x.State.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.ProfessionalTaxSlabModel, TranSmart.Domain.Entities.Payroll.ProfessionalTaxSlab>();
            CreateMap<TranSmart.Domain.Entities.Payroll.ProfessionalTaxSlab, TranSmart.Domain.Models.Payroll.ProfessionalTaxSlabModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.ProfessionalTaxSlab, TranSmart.Domain.Models.Payroll.ProfessionalTaxSlabList>();

            CreateMap<TranSmart.Domain.Models.Payroll.Section80CModel, TranSmart.Domain.Entities.Payroll.Section80C>();
            CreateMap<TranSmart.Domain.Entities.Payroll.Section80C, TranSmart.Domain.Models.Payroll.Section80CModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.Section80C, TranSmart.Domain.Models.Payroll.Section80CList>();

            CreateMap<TranSmart.Domain.Models.Payroll.Section80DModel, TranSmart.Domain.Entities.Payroll.Section80D>();
            CreateMap<TranSmart.Domain.Entities.Payroll.Section80D, TranSmart.Domain.Models.Payroll.Section80DModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.Section80D, TranSmart.Domain.Models.Payroll.Section80DList>();

            CreateMap<TranSmart.Domain.Models.Payroll.OtherSectionsModel, TranSmart.Domain.Entities.Payroll.OtherSections>();
            CreateMap<TranSmart.Domain.Entities.Payroll.OtherSections, TranSmart.Domain.Models.Payroll.OtherSectionsModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.OtherSections, TranSmart.Domain.Models.Payroll.OtherSectionsList>();

            CreateMap<TranSmart.Domain.Models.Payroll.PaySettingsModel, TranSmart.Domain.Entities.Payroll.PaySettings>();
            CreateMap<TranSmart.Domain.Entities.Payroll.PaySettings, TranSmart.Domain.Models.Payroll.PaySettingsModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.PaySettings, TranSmart.Domain.Models.Payroll.PaySettingsList>()
                .ForMember(d => d.Organization, s => s.MapFrom(x => x.Organization.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.DeclarationSettingModel, TranSmart.Domain.Entities.Payroll.DeclarationSetting>();
            CreateMap<TranSmart.Domain.Entities.Payroll.DeclarationSetting, TranSmart.Domain.Models.Payroll.DeclarationSettingModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.DeclarationSetting, TranSmart.Domain.Models.Payroll.DeclarationSettingList>();

            CreateMap<TranSmart.Domain.Models.Payroll.EPFModel, TranSmart.Domain.Entities.Payroll.EPF>();
            CreateMap<TranSmart.Domain.Entities.Payroll.EPF, TranSmart.Domain.Models.Payroll.EPFModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.EPF, TranSmart.Domain.Models.Payroll.EPFList>();

            CreateMap<TranSmart.Domain.Models.Payroll.ESIModel, TranSmart.Domain.Entities.Payroll.ESI>();
            CreateMap<TranSmart.Domain.Entities.Payroll.ESI, TranSmart.Domain.Models.Payroll.ESIModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.ESI, TranSmart.Domain.Models.Payroll.ESIList>();

            CreateMap<TranSmart.Domain.Models.Payroll.PaySetupModel, TranSmart.Domain.Entities.Payroll.PaySetup>();
            CreateMap<TranSmart.Domain.Entities.Payroll.PaySetup, TranSmart.Domain.Models.Payroll.PaySetupModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.PaySetup, TranSmart.Domain.Models.Payroll.PaySetupList>();

            CreateMap<TranSmart.Domain.Models.Payroll.FinancialYearModel, TranSmart.Domain.Entities.Payroll.FinancialYear>();
            CreateMap<TranSmart.Domain.Entities.Payroll.FinancialYear, TranSmart.Domain.Models.Payroll.FinancialYearModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.FinancialYear, TranSmart.Domain.Models.Payroll.FinancialYearList>();

            CreateMap<TranSmart.Domain.Models.Payroll.OldRegimeSlabModel, TranSmart.Domain.Entities.Payroll.OldRegimeSlab>();
            CreateMap<TranSmart.Domain.Entities.Payroll.OldRegimeSlab, TranSmart.Domain.Models.Payroll.OldRegimeSlabModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.OldRegimeSlab, TranSmart.Domain.Models.Payroll.OldRegimeSlabList>();

            CreateMap<TranSmart.Domain.Models.Payroll.NewRegimeSlabModel, TranSmart.Domain.Entities.Payroll.NewRegimeSlab>();
            CreateMap<TranSmart.Domain.Entities.Payroll.NewRegimeSlab, TranSmart.Domain.Models.Payroll.NewRegimeSlabModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.NewRegimeSlab, TranSmart.Domain.Models.Payroll.NewRegimeSlabList>();

            CreateMap<TranSmart.Domain.Models.Payroll.EmpStatutoryModel, TranSmart.Domain.Entities.Payroll.EmpStatutory>();
            CreateMap<TranSmart.Domain.Entities.Payroll.EmpStatutory, TranSmart.Domain.Models.Payroll.EmpStatutoryModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.EmpStatutory, TranSmart.Domain.Models.Payroll.EmpStatutoryList>()
                .ForMember(d => d.Emp, s => s.MapFrom(x => x.Emp.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.TemplateModel, TranSmart.Domain.Entities.Payroll.Template>();
            CreateMap<TranSmart.Domain.Entities.Payroll.Template, TranSmart.Domain.Models.Payroll.TemplateModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.Template, TranSmart.Domain.Models.Payroll.TemplateList>();

            CreateMap<TranSmart.Domain.Models.Payroll.TemplateEarningModel, TranSmart.Domain.Entities.Payroll.TemplateEarning>();
            CreateMap<TranSmart.Domain.Entities.Payroll.TemplateEarning, TranSmart.Domain.Models.Payroll.TemplateEarningModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.TemplateEarning, TranSmart.Domain.Models.Payroll.TemplateEarningList>()
                .ForMember(d => d.Component, s => s.MapFrom(x => x.Component.Name))
                .ForMember(d => d.PercentOnComp, s => s.MapFrom(x => x.PercentOnComp.Name));

            CreateMap<TranSmart.Domain.Models.Payroll.TemplateDeductionModel, TranSmart.Domain.Entities.Payroll.TemplateDeduction>();
            CreateMap<TranSmart.Domain.Entities.Payroll.TemplateDeduction, TranSmart.Domain.Models.Payroll.TemplateDeductionModel>();

            CreateMap<TranSmart.Domain.Entities.Payroll.TemplateDeduction, TranSmart.Domain.Models.Payroll.TemplateDeductionList>()
                .ForMember(d => d.Component, s => s.MapFrom(x => x.Component.Name));

            CreateMap<TranSmart.Domain.Models.Helpdesk.TicketStatusModel, TranSmart.Domain.Entities.Helpdesk.TicketStatus>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.TicketStatus, TranSmart.Domain.Models.Helpdesk.TicketStatusModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.TicketStatus, TranSmart.Domain.Models.Helpdesk.TicketStatusList>();

            CreateMap<TranSmart.Domain.Models.Helpdesk.DeskGroupModel, TranSmart.Domain.Entities.Helpdesk.DeskGroup>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskGroup, TranSmart.Domain.Models.Helpdesk.DeskGroupModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskGroup, TranSmart.Domain.Models.Helpdesk.DeskGroupList>();

            CreateMap<TranSmart.Domain.Models.Helpdesk.DeskGroupEmployeeModel, TranSmart.Domain.Entities.Helpdesk.DeskGroupEmployee>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskGroupEmployee, TranSmart.Domain.Models.Helpdesk.DeskGroupEmployeeModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskGroupEmployee, TranSmart.Domain.Models.Helpdesk.DeskGroupEmployeeList>()
                .ForMember(d => d.Employee, s => s.MapFrom(x => x.Employee.Name));

            CreateMap<TranSmart.Domain.Models.Helpdesk.DeskDepartmentModel, TranSmart.Domain.Entities.Helpdesk.DeskDepartment>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskDepartment, TranSmart.Domain.Models.Helpdesk.DeskDepartmentModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.DeskDepartment, TranSmart.Domain.Models.Helpdesk.DeskDepartmentList>()
                .ForMember(d => d.Manager, s => s.MapFrom(x => x.Manager.Name));

            CreateMap<TranSmart.Domain.Models.Helpdesk.DepartmentGroupModel, TranSmart.Domain.Entities.Helpdesk.DepartmentGroup>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.DepartmentGroup, TranSmart.Domain.Models.Helpdesk.DepartmentGroupModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.DepartmentGroup, TranSmart.Domain.Models.Helpdesk.DepartmentGroupList>()
                .ForMember(d => d.Groups, s => s.MapFrom(x => x.Groups.Name));

            CreateMap<TranSmart.Domain.Models.Helpdesk.HelpTopicModel, TranSmart.Domain.Entities.Helpdesk.HelpTopic>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.HelpTopic, TranSmart.Domain.Models.Helpdesk.HelpTopicModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.HelpTopic, TranSmart.Domain.Models.Helpdesk.HelpTopicList>()
                .ForMember(d => d.Department, s => s.MapFrom(x => x.Department.Department))
                .ForMember(d => d.TicketStatus, s => s.MapFrom(x => x.TicketStatus.Name));

            CreateMap<TranSmart.Domain.Models.Helpdesk.HelpTopicSubModel, TranSmart.Domain.Entities.Helpdesk.HelpTopicSub>();
            CreateMap<TranSmart.Domain.Entities.Helpdesk.HelpTopicSub, TranSmart.Domain.Models.Helpdesk.HelpTopicSubModel>();

            CreateMap<TranSmart.Domain.Entities.Helpdesk.HelpTopicSub, TranSmart.Domain.Models.Helpdesk.HelpTopicSubList>();

        }
    }
}
