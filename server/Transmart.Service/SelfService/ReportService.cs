using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.SelfService.Reports;

namespace TranSmart.Service.SelfService
{
    public partial interface IReportService
    {
        IEnumerable<LeaveBalancesQry> Leaves();
        IEnumerable<EmployeeLeaveBalanceQry> EmployeeLeaveBalance(Guid EmpId);
        IEnumerable<EmployeeProfileQry> EmployeeProfile(Guid EmpId);
    }
    public partial class ReportService : IReportService
    {
        private readonly IUnitOfWork _UOW;
        public ReportService(IUnitOfWork uow)
        {
            _UOW = uow;
        }
        public IEnumerable<LeaveBalancesQry> Leaves()
        {
            return _UOW.GetRepository<LeaveBalancesQry>().Query($"select L.ID, L.Name, T.Leaves from LM_LeaveType L inner join(select LeaveTypeId, sum(Leaves) Leaves from LM_LeaveBalance group by LeaveTypeId) AS T on L.Id = T.LeaveTypeId").ToList();
        }
        public IEnumerable<EmployeeLeaveBalanceQry> EmployeeLeaveBalance(Guid EmpId)
        {
            return _UOW.GetRepository<EmployeeLeaveBalanceQry>().Query(string.Format("select E.ID,E.Name,L.Leaves from Org_Employee E inner join(select EmployeeId, sum(Leaves) Leaves from LM_LeaveBalance group by EmployeeId)L on L.EmployeeID=E.ID Where L.EmployeeID='{0}'", EmpId)).ToList();
        }
        public IEnumerable<EmployeeProfileQry> EmployeeProfile(Guid EmpId)
            {
                return _UOW.GetRepository<EmployeeProfileQry>().Query(string.Format("select E.Id, E.No,E.Name,E.MobileNumber,E.AadharNumber,E.DateOfBirth,E.DateOfJoining,E.Gender,e.MaritalStatus,E.PanNumber,E.PersonalEmail,D.Name as Designation,R.Name as ReportingTo,De.Name as Department,W.Name as WorkType from Org_Employee E inner join Org_Designation D On E.DesignationId=D.ID left join Org_Employee R on R.ID=E.ReportingToId inner join Org_Department De on De.ID=E.DepartmentId left join Org_WorkType W on W.ID=E.WorkTypeId Where E.ID='{0}'", EmpId)).ToList();
            }
        }
    }
