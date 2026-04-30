using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Data.Repository.Attendance
{
	public interface IAttendanceRepository
	{
		IEnumerable<AttendanceSum> Summary(DateTime fromDate, DateTime toDate, byte month, short year);
		Task UpdateEmpInAttLogs();
	}
	public class AttendanceRepository : IAttendanceRepository
	{
		private readonly TranSmartContext _context;
		public AttendanceRepository(TranSmartContext context)
		{
			_context = context;
		}
		public IEnumerable<AttendanceSum> Summary(DateTime fromDate, DateTime toDate, byte month, short year)
		{
			return _context.HR_Attendance.Where(a => a.AttendanceDate.Date >= fromDate.Date && a.AttendanceDate.Date <= toDate.Date)
				.GroupBy(g => new { g.EmployeeId })
				.Select(s => new AttendanceSum
				{
					Month = month,
					Year = year,
					EmployeeId = s.Key.EmployeeId,
					Present = (decimal)s.Sum(x => x.Present),
					LOP = (decimal)s.Sum(x => x.Absent),
					Unauthorized = (decimal)s.Sum(x => x.UADays),
					NoOfLeaves = (decimal)s.Sum(x => x.Leave)
				}).ToList();
		}
		public async Task UpdateEmpInAttLogs()
		{
			await _context.Connection.QueryAsync<dynamic>("UPDATE B SET B.EmployeeId = E.ID FROM LM_BiometricAttLogs B inner JOIN (select ID,CONCAT('AVONTIX',EmpCode) EmpCode  from LM_BiometricAttLogs) S ON S.ID = B.ID inner JOIN Org_Employee E  ON S.EmpCode=E.No WHERE B.EmployeeId is null;");
		}
	}
}
