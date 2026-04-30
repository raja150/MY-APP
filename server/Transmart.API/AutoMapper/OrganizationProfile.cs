using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Cache;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.Reports;

namespace TranSmart.API.AutoMapper
{
    public class OrganizationProfile:Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Employee, EmployeeInfoModel>()
                .ForMember(dest => dest.EmployeeNo, opt => opt.MapFrom(x => x.No))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Department.Name))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Designation.Name));
            CreateMap<Employee, EmpProfileModel>()
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Designation.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(x => x.Department.Name))
                .ForMember(dest => dest.AadhaarNumber, opt => opt.MapFrom(x => string.Format("XXXX XXXX {0}", x.AadhaarNumber.Trim().Substring(8, 4))))
                .ForMember(dest => dest.WorkLocation, opt => opt.MapFrom(x => x.WorkLocation.Name))
                .ForMember(dest => dest.EmployeeTeam, opt => opt.MapFrom(x => x.Team.Name));

			CreateMap<Employee , EmployeeImageModel>()
				.ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(x => x.Department.Name))
				.ForMember(dest => dest.WorkLocation, opt => opt.MapFrom(x => x.WorkLocation.Name))
				.ForMember(dest => dest.EmployeeTeam, opt => opt.MapFrom(x => x.Team.Name));

			CreateMap<EmployeeContact, EmpProfileModel>();
            CreateMap<EmpProfileModel, EmployeeContact>();

            CreateMap<AllocationModel, Allocation>();
            CreateMap<Allocation, AllocationModel>()
                .ForMember(dest => dest.Name , opt => opt.MapFrom(x => x.Employee.Name));
            CreateMap<Allocation, AllocationList>()
               .ForMember(d => d.Shift, s => s.MapFrom(x => x.Shift.Name))
               .ForMember(d => d.No, s => s.MapFrom(x => x.Employee.No))
               .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
               .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
               .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));

            CreateMap<Employee, EmployeeSearchCache>()
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Designation.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Department.Name))
                .ForMember(dest => dest.No, opt => opt.MapFrom(x => x.No));

            CreateMap<EmployeeFamily, EmployeeContactReportModel>()
                .ForMember(dest => dest.No, opt => opt.MapFrom(x => x.Employee.No))
                .ForMember(dest => dest.DateOfJoining , opt => opt.MapFrom(x => x.Employee.DateOfJoining))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Employee.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name));

			CreateMap<EmployeeDevice, EmployeeDeviceModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name));
			CreateMap<EmployeeDeviceModel, EmployeeDevice>();

			
			CreateMap<PerformanceModel, Performance>();
			CreateMap<Performance, PerformanceModel>()
				.ForMember(d => d.EmployeeNo , opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));
		}
    }
}
