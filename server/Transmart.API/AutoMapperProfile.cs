using System;
using AutoMapper;
using TranSmart.API.Converter;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Models.Settings;

namespace TranSmart.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TranSmart.Domain.Entities.LookUpValues, TranSmart.Domain.Models.LookUpValues>();
            CreateMap<TranSmart.Domain.Models.LookUpValues, TranSmart.Domain.Entities.LookUpValues>();


            CreateMap<TranSmart.Domain.Entities.AppSettings.Role, TranSmart.Domain.Models.AppSettings.RoleModel>()
                .ForMember(d => d.Pages, s => s.MapFrom(x => x.Pages));
            CreateMap<TranSmart.Domain.Models.AppSettings.RoleModel, TranSmart.Domain.Entities.AppSettings.Role>()
                .ForMember(d => d.Pages, s => s.MapFrom(x => x.Pages));

            CreateMap<TranSmart.Domain.Entities.AppSettings.RolePrivilege, Domain.Models.RolePrivilegeModel>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Page.Name))
                .ForMember(d => d.DisplayName, s => s.MapFrom(x => x.Page.DisplayName))
                .ForMember(d => d.Module, s => s.MapFrom(x => x.Page.Group.Name));
            CreateMap<Domain.Models.RolePrivilegeModel, TranSmart.Domain.Entities.AppSettings.RolePrivilege>();

            CreateMap<TranSmart.Domain.Entities.Page, TranSmart.Domain.Models.Page>()
                .ForMember(d => d.Module, s => s.MapFrom(x => x.Group.Name));

            CreateMap<TranSmart.Domain.Entities.User, TranSmart.Domain.Models.UserModel>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name));

            CreateMap<TranSmart.Domain.Models.UserModel, TranSmart.Domain.Entities.User>();
            CreateMap<TranSmart.Domain.Entities.User, TranSmart.Domain.Models.UsersList>()
                .ForMember(dest => dest.EmpName, opt => opt.MapFrom(x => x.Employee.Name))
                .ForMember(dest => dest.EmpCode, opt => opt.MapFrom(x => x.Employee.No))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(x => x.Role.Name));

            CreateMap<TranSmart.Domain.Models.SequenceNoModel, TranSmart.Domain.Entities.SequenceNo>();
            CreateMap<TranSmart.Domain.Entities.SequenceNo, TranSmart.Domain.Models.SequenceNoModel>();

            CreateMap(typeof(Data.Paging.Paginate<>), typeof(Models.Paginate<>)).ConvertUsing(typeof(TranSmart.API.Converter.Converter<,>));

            CreateMap<Core.Attributes.ExcelHelperAttribute, Core.HeaderModel>()
                .ForMember(d => d.PropertyName, s => s.MapFrom(x => Core.Util.StringUtil.ToCamelCase(x.PropertyName)))
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Attribute.Name))
                .ForMember(d => d.Order, s => s.MapFrom(x => x.Attribute.Order));

            CreateMap<Report, TranSmart.Domain.Models.Settings.RoleReportPrivilegeModel>()
               .ForMember(d => d.ReportId, s => s.MapFrom(x => x.ID))
               .ForMember(d => d.Name, s => s.MapFrom(x => x.Name))
               .ForMember(d => d.Label, s => s.MapFrom(x => x.Label))
               .ForMember(d => d.Module, s => s.MapFrom(x => x.Module.Name));

            CreateMap<RoleReportPrivilege,RoleReportPrivilegeModel>()
               .ForMember(d => d.Name, s => s.MapFrom(x => x.Report.Name))
			   .ForMember(d => d.ModuleId, s => s.MapFrom(x => x.Report.ModuleId))
			   .ForMember(d => d.Label, s => s.MapFrom(x => x.Report.Label))
               .ForMember(d => d.Privilege, s => s.MapFrom(x => x.Privilege))
               .ForMember(d => d.Module, s => s.MapFrom(x => x.Report.Module.Name));
            CreateMap<RoleReportPrivilegeModel, RoleReportPrivilege>();

        }


    }
}
