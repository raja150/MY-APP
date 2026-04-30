using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Data.Repository.Leave;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Data.Repository
{
	[CollectionDefinition("Collection #1")]
	[Xunit.TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class LeaveBalanceRepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly LeaveBalanceRepository _leaveBalanceRepository;
		private readonly EmployeeDataGenerator _employeeData;
		public LeaveBalanceRepositoryTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_leaveBalanceRepository = new LeaveBalanceRepository(inMemory.DbContext);
			_employeeData = new EmployeeDataGenerator();
		}
		[Fact]
		public async Task BalanceReport_Test()
		{
			inMemory.DbContext.Leave_LeaveBalance.AddRange(
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						Leaves = 5,
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						Employee = new TranSmart.Domain.Entities.Organization.Employee
						{
							ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
							No = "AVONTIX1822",
							Name = "Anudeep",
							Gender = 1,
							MobileNumber = "9639639632",
							DateOfBirth = new DateTime(1989, 02, 02),
							DateOfJoining = new DateTime(2020, 08, 12),
							AadhaarNumber = "561250752388",
							PanNumber = "BLMPJ2797L",
							DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
							Designation = new Designation
							{
								ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
								Name = "Jr Software Developer",
							},
							WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
							TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
							FirstName = "Alla",

						},
						LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
					}
			);
			inMemory.DbContext.SaveChanges();
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.Leave_LeaveType.AddRange(new LeaveType()
			{
				ID = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
				Name = "Casual Leave",
				Code = "CL",
				MinLeaves = 0.5m,
				MaxLeaves = 22,
				DefaultPayoff = false,
				Status = true
			});
			inMemory.DbContext.SaveChanges();
			var leaveBalance = new LeaveBalance()
			{
				ID = Guid.NewGuid(),
				Leaves = 5,
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
				Employee = new Employee
				{
					ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
				},
				LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
			};


			var balance = await _leaveBalanceRepository.BalanceReport(leaveBalance.Employee.DepartmentId, leaveBalance.Employee.DesignationId,
							leaveBalance.Employee.TeamId, leaveBalance.EmployeeId, leaveBalance.LeaveTypeId);
			Assert.True(balance.Count() == 1);
			Assert.True(balance.FirstOrDefault().Balance == 5);
		}
		[Fact]
		public async Task LeaveBalance_GetBy_EmployeeId_Test()
		{
			var employeeID = Guid.NewGuid();
			inMemory.DbContext.Leave_LeaveBalance.AddRange(
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						Leaves = 5,
						Employee = new Employee
						{
							ID = employeeID,
							No = "AVONTIX1822",
							Name = "Anudeep",
							Gender = 1,
							MobileNumber = "9639639632",
							DateOfBirth = new DateTime(1989, 02, 02),
							DateOfJoining = new DateTime(2020, 08, 12),
							AadhaarNumber = "561250752388",
							PanNumber = "BLMPJ2797L",
							DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
							Designation = new Designation
							{
								ID = Guid.Parse("E7D81382-4D0A-491C-88EF-51EA7FF044D4"),
								Name = "Jr Software Developer",
							},
							WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
							TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
							FirstName = "Alla"
						},
						LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
						LeaveType = new LeaveType
						{
							Name = "Test Leave",
							Code = "TL",
							DefaultPayoff = false,
							MinLeaves = 1,
							MaxLeaves = 4,
							Status = true
						}
					}
			);
			inMemory.DbContext.SaveChanges();
			inMemory.DbContext.ChangeTracker.Clear();
			var dd = await _leaveBalanceRepository.GetByEmployee(employeeID);

			Assert.True(dd.Count() == 1);
		}
		[Fact]
		public async Task MyTeamLeaveBalance_Report_Test()
		{
			var employee= _employeeData.GetAllEmployeesData().LastOrDefault();
			employee.FirstName = "Kulari";
			await inMemory.DbContext.Leave_LeaveBalance.AddRangeAsync(
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						Leaves = 5,
						EmployeeId = employee.ID,
						Employee = employee,
						LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
					},
					new LeaveBalance
					{
						ID = Guid.NewGuid(),
						Leaves = -2,
						EmployeeId = employee.ID,
						Employee = employee,
						LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
					}
			);
			await inMemory.DbContext.SaveChangesAsync();
			inMemory.DbContext.ChangeTracker.Clear();
			await inMemory.DbContext.Leave_LeaveType.AddRangeAsync(new LeaveType()
			{
				ID = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
				Name = "Casual Leave",
				Code = "CL",
				MinLeaves = 0.5m,
				MaxLeaves = 22,
				DefaultPayoff = false,
				Status = true
			});
			await inMemory.DbContext.SaveChangesAsync();
			inMemory.DbContext.ChangeTracker.Clear();
			var leaveBalance = new LeaveBalance()
			{
				ID = Guid.NewGuid(),
				Leaves = 5,
				EmployeeId = Guid.Parse("4c48a4cd-6933-4ba0-9e01-600c4124800c"),
				Employee = new Employee
				{
					ID = Guid.Parse("4c48a4cd-6933-4ba0-9e01-600c4124800c"),
					DepartmentId = Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),
					DesignationId = Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"),
					TeamId = Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"),
					ReportingToId = Guid.Parse("77934662-9896-44d5-bc74-5cce64150fba"),
				},
				LeaveTypeId = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
			};


			var balance = await _leaveBalanceRepository.MyTeamLeaveBalanceReport(leaveBalance.Employee.DesignationId,
							leaveBalance.EmployeeId, leaveBalance.LeaveTypeId, (Guid)leaveBalance.Employee.ReportingToId);
			Assert.True(balance.Count() == 1);
			Assert.True(balance.LastOrDefault().Balance == 3);
		}
	}
}
