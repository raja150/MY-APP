using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Leave.Reports;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService.Reports;

namespace TranSmart.Service.Reports
{
    public interface IEmpReportService
    {
        Task<IEnumerable<EmployeeProfileQry>> EmployeeProfile();
        Task<IEnumerable<EmployeeLeaveBalance>> EmployeeLeaveBalance();
    }
    public class EmpReportService : IEmpReportService
    {
		readonly IUnitOfWork _UOW;
        public EmpReportService(IUnitOfWork uow)
        {
            _UOW = uow;
        }
        public async Task<IEnumerable<EmployeeProfileQry>> EmployeeProfile()
        {
            return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
                selector: a => new EmployeeProfileQry
                {
                    ID = a.ID,
                    AadhaarNumber = a.AadhaarNumber,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    Department = a.Department.Name,
                    Name = a.Name,
                    Designation = a.Designation.Name,
                    Gender = a.Gender,
                    MaritalStatus = a.MaritalStatus,
                    MobileNumber = a.MobileNumber,
                    No = a.No,
                    PanNumber = a.PanNumber,
                    PersonalEmail = a.PersonalEmail,
                    ReportingTo = a.ReportingTo.Name,
                    WorkType = a.WorkType.Name,
                }, include: i => i.Include(x => x.Department).Include(x => x.Designation).Include(x => x.WorkType).Include(x => x.ReportingTo));
        }
        public async Task<IEnumerable<EmployeeLeaveBalance>> EmployeeLeaveBalance()
        {
            return await _UOW.GetRepositoryAsync<LeaveBalance>().GetWithSelectAsync(
                  selector: a => new EmployeeLeaveBalance
                  {
                      ID = a.ID,
                      Name = a.Employee.Name,
                      Leaves = a.Leaves,
                      No = a.Employee.No,
                  },
                 include: i => i.Include(x => x.Employee));
        }
    }
}
