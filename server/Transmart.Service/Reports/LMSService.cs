using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Repository.Leave;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Leave.Reports;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Reports.LMS;
using TranSmart.Service.Reports.LMS;

namespace TranSmart.Service.Reports.LMS
{
    public partial interface ILmsService
    {
        Task<IEnumerable<LeaveBalancesModel>> LeaveBalances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? leaveTypeId);
        Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalances(Guid empId, Guid leavetypaId);
    }
    public class LmsService : ILmsService
    {
        private readonly IUnitOfWork _UOW;
        private readonly ILeaveBalanceRepository _repository;
        public LmsService(IUnitOfWork uow, ILeaveBalanceRepository repository)
        {
            _UOW = uow;
            _repository = repository;
        }
        public async Task<IEnumerable<LeaveBalancesModel>> LeaveBalances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? leaveTypeId)
        {
            return await _repository.BalanceReport(departmentId, designationId, teamId,employeeId, leaveTypeId);
        }
        public async Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalances(Guid empId, Guid leavetypaId)
        {
            return await _UOW.GetRepositoryAsync<LeaveBalance>().GetWithSelectAsync(
                selector: d => new LeaveBalancesModel
                {
                    No = d.Employee.No,
                    Name = d.Employee.Name,
                    Leaves = d.Leaves,
                    Designation = d.Employee.Designation.Name,
                    LeaveType = d.LeaveType.Name,
                    Balance = d.CustomizedBal.NewBalance
                },
                predicate: x => x.LeaveTypeId == leavetypaId && x.EmployeeId == empId);
        }
    }
}
