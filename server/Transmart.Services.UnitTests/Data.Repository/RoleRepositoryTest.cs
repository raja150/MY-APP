using System;
using System.Linq;
using TranSmart.Data.Repository.Reports;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Data.Repository
{
	public class RoleRepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly RoleRepository _repository;
		private readonly Guid roleId = Guid.NewGuid();
		private readonly Guid pageId = Guid.NewGuid();
		private readonly Guid departmentId = Guid.NewGuid();
		private readonly Guid empId = Guid.NewGuid();
		private readonly Guid designationId = Guid.NewGuid();

		public RoleRepositoryTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_repository = new RoleRepository(inMemory.DbContext);

		}
		[Fact]
		public async void GetPageEmployees()
		{
			await inMemory.DbContext.RolePrivileges.AddRangeAsync(new RolePrivilege
			{
				ID = Guid.NewGuid(),
				RoleId = roleId,
				PageId = pageId,
				Privilege = 12,
				Page = new Page { ID = pageId },
				Role = new Role { ID = roleId, Name = "Test", },
			});
			await inMemory.DbContext.SaveChangesAsync();
			inMemory.DbContext.ChangeTracker.Clear();

			await inMemory.DbContext.Roles.AddRangeAsync(new Role
			{
				ID = roleId,
				Name = "Test",
			});
			inMemory.DbContext.ChangeTracker.Clear();
			await inMemory.DbContext.SaveChangesAsync();

			await inMemory.DbContext.Users.AddRangeAsync(new User
			{
				ID = Guid.NewGuid(),
				RoleId = roleId,
				Name = "AVONTIX2063",
				EmployeeID = empId,
				Employee = new Employee
				{
					ID = empId,
					Name = "Vamshi",
					No = "AVONTIX2063",
					DepartmentId = departmentId,
					Department = new Department { ID = departmentId, Name = "IT" },
					DesignationId = designationId,
					Designation = new Designation { ID = designationId, Name = "Developer" },
					AadhaarNumber = "123412341234",
					MobileNumber ="6786786786",
					FirstName = "Vamshi"
				},
			});
			await inMemory.DbContext.SaveChangesAsync();
			inMemory.DbContext.ChangeTracker.Clear();

			await inMemory.DbContext.Organization_Employee.AddRangeAsync(new Employee
			{
				ID = empId,
				Name = "Vamshi",
				No = "AVONTIX2063",
				DepartmentId = departmentId,
				Department = new Department { ID = departmentId, Name = "IT" },
				DesignationId = designationId,
				Designation = new Designation { ID = designationId, Name = "Developer" },
				AadhaarNumber = "123412341234",
				MobileNumber = "6786786786",
				FirstName = "Vamshi"
			});
			inMemory.DbContext.ChangeTracker.Clear();
			await inMemory.DbContext.SaveChangesAsync();

			await inMemory.DbContext.Organization_Department.AddRangeAsync(new Department
			{
				ID = departmentId,
				Name = "IT",
			});
			inMemory.DbContext.ChangeTracker.Clear();
			await inMemory.DbContext.SaveChangesAsync();

			await inMemory.DbContext.Organization_Designation.AddRangeAsync(new Designation
			{
				ID = designationId,
				Name = "Developer"
			});
			inMemory.DbContext.ChangeTracker.Clear();
			await inMemory.DbContext.SaveChangesAsync();

			var res = await _repository.PageEmployees(pageId);
			Assert.Equal("Vamshi", res.FirstOrDefault().EmployeeName);
			Assert.Equal("IT", res.FirstOrDefault().Department);
			Assert.Equal("AVONTIX2063", res.FirstOrDefault().EmployeeCode);
		}
	}
}
