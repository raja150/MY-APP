using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Repository;
using TranSmart.Data.Specifications;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Organization;
using TranSmart.Services.UnitTests;
using Xunit;
using Xunit.Abstractions;

namespace Transmart.Services.UnitTests.Repository
{
	[CollectionDefinition("Collection #1")]
	[TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class RepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly Mock<DbContext> _context;
		private readonly InMemoryFixture inMemory;
		private readonly Repository<EmployeeEducation> _employeeEduRepo;
		private readonly Repository<LookUpValues> _lookUpDataRepo;
		private readonly EmployeeEducationService _service;
		private readonly Repository<Attendance> _attendanceRepo;
		private readonly Repository<Department> _departmentRepo;
		private readonly Repository<Designation> _designationRepo;
		private readonly Repository<Employee> _employeeRepo;
		private readonly Repository<AppForm> _appformRepository;


		public RepositoryTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_context = new Mock<DbContext>();
			_employeeEduRepo = new Repository<EmployeeEducation>(inMemory.DbContext);
			_lookUpDataRepo = new Repository<LookUpValues>(inMemory.DbContext);
			_attendanceRepo = new Repository<Attendance>(inMemory.DbContext);
			_departmentRepo = new Repository<Department>(inMemory.DbContext);
			_designationRepo = new Repository<Designation>(inMemory.DbContext);
			_employeeRepo = new Repository<Employee>(inMemory.DbContext);
			_service = new EmployeeEducationService(inMemory.UnitOfWork);
			_appformRepository = new Repository<AppForm>(inMemory.DbContext);
		}

		[Fact, TestPriority(1)]
		public void Add_SaveData_ShouldPost()
		{
			var entity = new Attendance
			{
				ID = Guid.NewGuid(),
				Breaks = 1
			};
			_attendanceRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			//var response = inMemory.DbContext.HR_Attendance.FirstOrDefault();
			//Assert.Equal(entity.ID, response.ID);
		}

		[Fact, TestPriority(2)]
		public void Add_Params_ShouldSave()
		{

			var entity1 = new Department
			{
				ID = Guid.NewGuid(),
				Name = "ITDepartment"
			};

			var entity2 = new Department
			{
				ID = Guid.NewGuid(),
				Name = "Accounts"
			};

			Department[] entity = { entity1, entity2 };

			_departmentRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_Department.ToList();
			Assert.True(response.Count == 2);
		}

		[Fact, TestPriority(3)]
		public void Add_IEnumerable_ShouldSave()
		{
			var entity = new List<EmployeeEducation>
			{
				new EmployeeEducation
				{
					ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
					Institute = "TKR",
					Percentage = 55,
					Degree ="B.Tech",
					Medium = "English",
					Qualification="Graduate"
				},
				new EmployeeEducation
				{
					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
					Institute = "Princeton",
					Percentage = 45,
					Degree ="B.Tech",
					Medium = "English",
					Qualification="Graduate"
				}
			};
			_employeeEduRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.True(response.Count == 2);
		}

		[Fact, TestPriority(4)]
		public void Add_WithGuid_ShouldSave()
		{
			var entity = new Designation
			{
				Name = "Developer"
			};
			_designationRepo.Add(entity, Guid.Parse("13bfe615-f467-42f0-ae3c-d560e7568d48"));
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_Designation.ToList();
			Assert.Equal(entity.Name, response[0].Name);
		} 

		[Fact, TestPriority(7)]
		public void Update_Edit_ShouldUpdate()
		{
			var entity = new EmployeeEducation
			{
				ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
				Institute = "TKR1++"
			};

			var item = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault(x => x.ID == entity.ID);
			inMemory.DbContext.ChangeTracker.Clear();
			item.Institute = entity.Institute;
			_employeeEduRepo.Update(item);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault(x => x.ID == entity.ID);
			Assert.Equal(entity.Institute, response.Institute);
		}

		[Fact, TestPriority(8)]
		public void Update_Params_ShouldUpdate()
		{
			inMemory.DbContext.ChangeTracker.Clear();
			var entity = new EmployeeEducation
			{
				ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
				Institute = "TKR1++"
			};

			var entity1 = new EmployeeEducation
			{
				ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
				Institute = "Princeton++"
			};

			var item = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault(x => x.ID == entity.ID);
			var item1 = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault(x => x.ID == entity1.ID);

			item.Institute = entity.Institute;
			item1.Institute = entity1.Institute;

			EmployeeEducation[] empEducation = { item, item1 };
			_employeeEduRepo.Update(empEducation);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();

			Assert.Equal(entity1.Institute, response[0].Institute);
			Assert.Equal(entity.Institute, response[1].Institute);
		}

		[Fact, TestPriority(9)]
		public void Update_IEnumberable_ShouldUpdate()
		{
			inMemory.DbContext.ChangeTracker.Clear();
			var empEdulist = new List<EmployeeEducation>
			{
				new EmployeeEducation
				{
					ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
					Institute = "TKR1++"
				},
				new EmployeeEducation
				{
					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
					Institute = "Princeton++"
				}
			};

			var list = inMemory.DbContext.Organization_EmployeeEducation.ToList();

			foreach (var empEdu in empEdulist)
			{
				foreach (var item in list)
				{
					if (empEdu.ID == item.ID)
					{
						item.Institute = empEdu.Institute;
					}
				}
			}

			_employeeEduRepo.Update(list);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.Equal(empEdulist[1].Institute, response[0].Institute);
			Assert.Equal(empEdulist[0].Institute, response[1].Institute);
		}

		[Fact, TestPriority(10)]
		public void SumOfDecimal_AddDecimal_DecimalsSum()
		{
			var response = _employeeEduRepo.SumOfDecimal(x => x.Percentage == 45, x => x.Percentage);
			Assert.Equal(45, response);
		}


		[Fact, TestPriority(11)]
		public void HasRecords_CheckRecords_TrueOrFalse()
		{
			var response = _employeeEduRepo.HasRecords(x => x.Institute == "TKR1++");
			Assert.True(response);
		}

		[Fact, TestPriority(12)]
		public void Delete_Params_ShouldRemove()
		{
			inMemory.DbContext.ChangeTracker.Clear();
			var entity = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			EmployeeEducation[] item = { entity[0], entity[1] };
			_employeeEduRepo.Delete(item);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.True(response.Count == 0);
		}

		[Fact, TestPriority(13)]
		public void Delete_IEnumerable_ShouldRemove()
		{
			var entity = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			_employeeEduRepo.Delete(entity);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.True(response.Count == 0);
		}

		[Fact, TestPriority(14)]
		public void Delete_WithObject_ShouldRemove()
		{
			var entity = new List<EmployeeEducation>
			{
				new EmployeeEducation
				{
					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
					Institute = "Princeton",
					Percentage = 45,
					Degree ="B.Tech",
					Medium = "English",
					Qualification="Graduate"
				},
				new EmployeeEducation
				{
					ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
					Institute = "TKR",
					Percentage = 55,
					Degree ="B.Tech",
					Medium = "English",
					Qualification="Graduate"
				},
				
			};
			_employeeEduRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			var item = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault();

			var entityToDelete = new EmployeeEducation
			{
				ID = item.ID,
				Institute = item.Institute
			};

			_employeeEduRepo.Delete(entityToDelete);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.Equal(0, response.Count(x => x.ID == entityToDelete.ID));
		}


		[Fact, TestPriority(15)]
		public void GetAll_Specifications_SpecificationsList()  
		{
			var appforms = new AppForm
			{
				ID = Guid.NewGuid(),
				Name = "AppForms",
				DisplayName= "Settings",
				Header= "PaySettings",
			};
			_appformRepository.Add(appforms);
			inMemory.DbContext.SaveChanges();

			var response = _appformRepository.GetAll(new AppFormSpecification(appforms.ID));
			Assert.Single(response);
		} 

		[Fact, TestPriority(18)]
		public void Dispose_Test_ShouldDisponse()
		{
			var entity = new Employee
			{
				ID = Guid.NewGuid(),
				Name = "Vijay",
				AadhaarNumber = "123412341212",
				FirstName ="Sangu",
				MobileNumber="9897949596",
				No = "Avontix2065"
			};
			_employeeRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			_employeeRepo.Dispose();
			Assert.Throws<ObjectDisposedException>(() => inMemory.DbContext.Organization_Employee.ToList());
		}

	}
}
