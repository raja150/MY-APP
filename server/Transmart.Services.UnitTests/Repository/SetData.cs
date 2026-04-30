using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService.Reports;

namespace Transmart.Services.UnitTests.Repository
{
	public static class SetData
	{
		public static IEnumerable<Attendance> DbAttendance()
		{

			var Employee = new Employee
			{
				ID = Guid.Parse("1f971b90-1cce-4007-b19f-d6a04bfa4b0b"),
				Name = "Vijay",
				No = "Avn8001",
			};

			var Employee1 = new Employee
			{
				ID = Guid.Parse("f2a13d76-d74d-4879-b2d5-b210d431cf9e"),
				Name = "Vishnu",
				No = "Avn8002"
			};

			var Attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"),
					SchIntime = 10,
					SchOutTime = 22,
					EmployeeId = Employee.ID,
					Employee = Employee,
					AddedAt = DateTime.Now.AddDays(-10)
				},
				new Attendance
				{
					ID = Guid.Parse("ac2a04cf-b4a0-4baa-8c4b-88c4a7605e70"),
					SchIntime = 11,
					SchOutTime = 22,
					EmployeeId = Employee1.ID,
					AddedAt = DateTime.Now
				}
			}.AsQueryable();
			return Attendance;
		}
		
	}
}
