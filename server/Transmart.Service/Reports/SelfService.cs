using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Entities.Payroll;
using Microsoft.EntityFrameworkCore;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Domain.Models.Reports.Generated;
using TranSmart.Data.Repository.Leave;

namespace TranSmart.Service.Reports
{
    public interface ISelfService
    {
        Task<IEnumerable<dynamic>> LeaveBalance(Guid employeeId);
    }
    public class SelfService : ISelfService
    {
        private readonly ILeaveBalanceRepository _repository;
        public SelfService(ILeaveBalanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<dynamic>> LeaveBalance(Guid employeeId)
        {
            return await _repository.GetByEmployee(employeeId);
        }
    }
}



