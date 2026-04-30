using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Organization;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Repository
{
	[CollectionDefinition("Collection #1")]
	[TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class BaseRepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly Mock<DbContext> _context;
		private readonly InMemoryFixture inMemory;
		private readonly Repository<EmployeeEducation> _employeeEduRepo;
		private readonly Repository<Attendance> _attendanceRepo;
		private readonly EmployeeEducationService _service;
		private readonly EmployeeEducation _employeeEducation;


		public BaseRepositoryTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_context = new Mock<DbContext>();
			_employeeEduRepo = new Repository<EmployeeEducation>(inMemory.DbContext);
			_service = new EmployeeEducationService(inMemory.UnitOfWork);
			_attendanceRepo = new Repository<Attendance>(inMemory.DbContext);
			_employeeEducation = new()
			{
				ID = Guid.Parse("47abf273-48a8-4601-97a9-512fa398da64"),
				Institute = "TKR"
			};
		}

		[Fact, TestPriority(1)]
		public void Add_EmpEducationData_data()
		{
			var employee = new Employee
			{
				ID = Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
				Name = "Vijay",
				AadhaarNumber = "123412341234",
				FirstName="Sangu",
				MobileNumber="9686766656",
				No = "Avontix2065"
			};

			var employee1 = new Employee
			{
				ID = Guid.Parse("3d2a0761-15c7-4fdf-abee-7920881f4598"),
				Name = "Vishnu",
				AadhaarNumber = "123412341232",
				FirstName = "Lulari",
				MobileNumber = "9686766652",
				No = "Avontix2061"
			};

			var entity = new List<EmployeeEducation>
			{
				new EmployeeEducation
				{
					ID = Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"),
					Institute = "TKR",
					Percentage = 55,
					Employee = employee,
					EmployeeId =Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
					YearOfPassing = 2021,
					Degree = "B.Tech",
					Medium = "English",
					Qualification = "Graduate"
				},
				new EmployeeEducation
				{
					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
					Institute = "Princeton",
					Percentage = 45,
					Employee = employee1,
					EmployeeId = Guid.Parse("3d2a0761-15c7-4fdf-abee-7920881f4598"),
					YearOfPassing = 2021,
					Degree = "No",
					Medium = "English",
					Qualification = "Intermediate"
				}
			};

			_employeeEduRepo.Add(entity);
			inMemory.DbContext.SaveChanges();

			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			Assert.True(response.Count == 2);
		}

		[Fact, TestPriority(2)]
		public void Get_EmpEducation_data()
		{
			var dd = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault();
			var response = _employeeEduRepo.GetSingle(x => x.ID == dd.ID);
			Assert.Equal(dd.ID, response.ID);
		}


		[Fact, TestPriority(3)]
		public void Single_WithPredicate_SingleData()
		{
			var response = _employeeEduRepo.GetSingle(x => x.ID == Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"));
			Assert.Equal(response.ID, Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"));
		}


		[Fact, TestPriority(4)]
		public void Single_Include_IsSuccess()
		{
			var response = _employeeEduRepo.GetSingle(x => x.ID == Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"), include: i => i.Include(x => x.Employee));
			Assert.Equal("Vijay", response.Employee.Name);
		}

		[Fact, TestPriority(5)]
		public void Single_OrderBy_IsSuccess()
		{
			var response = _employeeEduRepo.GetSingle(x => x.ID == Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"),
				include: i => i.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.Percentage));

			Assert.Equal(55, response.Percentage);
		}		

		[Fact, TestPriority(7)]
		public void GetWithSelect_GetSelectedData()
		{

			var response = _employeeEduRepo.GetWithSelect<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
				predicate: x => x.Percentage == 45,
				include: x => x.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.Percentage));
			Assert.Single(response);
		}

		[Fact, TestPriority(8)]
		public void GetWithSelect_OrderByNull_GetData()
		{

			var response = _employeeEduRepo.GetWithSelect<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
				predicate: x => x.Percentage == 55,
				include: x => x.Include(x => x.Employee));

			Assert.Single(response);
		}


		[Fact, TestPriority(9)]
		public void GetCount_ItemsInList_Number()
		{
			var response = _employeeEduRepo.GetCount(x => x.YearOfPassing == 2021);
			Assert.Equal(2, response);
		}

		[Fact, TestPriority(10)]
		public void GetList_GetAll_List()
		{
			var response = _employeeEduRepo.GetList(x => x.YearOfPassing == 2021);
			Assert.Equal(2, response.Count);
		}

		[Fact, TestPriority(11)]
		public void GetList_Include_GetIncludedTableData()
		{
			var response = _employeeEduRepo.GetList(x => x.YearOfPassing == 2021,
				include: i => i.Include(x => x.Employee)
				);

			Assert.Equal("Vijay", response.Items.ToList()[0].Employee.Name);
		}

		[Fact, TestPriority(12)]
		public void GetList_OrderBy_SortedData()
		{
			var response = _employeeEduRepo.GetList(x => x.YearOfPassing == 2021,
				orderBy: o => o.OrderByDescending(x => x.Percentage));

			Assert.Equal(55, response.Items.ToList()[0].Percentage);
			Assert.Equal(45, response.Items.ToList()[1].Percentage);

		}

		[Fact, TestPriority(13)]
		public void GetPageList_PageList()
		{
			var response = _employeeEduRepo.GetPageList(x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"));
			Assert.Equal(1, response.Count);
		}

		[Fact, TestPriority(14)]
		public void GetPageList_Include_PageList()
		{

			var response = _employeeEduRepo.GetPageList(x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
				include: i => i.Include(x => x.Employee),
				sortBy: "AddedAt",
				ascending: false);
			Assert.Equal(1, response.Count);
		}

		[Fact, TestPriority(15)]
		public void GetList_GetAllData_List()
		{

			var response = _employeeEduRepo.GetList<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
				predicate: x => x.Percentage == 55,
				include: x => x.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.Percentage));

			Assert.Equal(1, response.Count);
		}

		[Fact, TestPriority(16)]
		public void GetList_WithoutOrderBy_List()
		{

			var response = _employeeEduRepo.GetList<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
				predicate: x => x.Percentage == 55,
				include: x => x.Include(x => x.Employee));

			Assert.Equal(1, response.Count);
		}

		[Fact, TestPriority(17)]
		public void GetRefList_GetAll_List()
		{
			var item = inMemory.DbContext.Organization_EmployeeEducation.ToList();

			var response = _employeeEduRepo.GetRefList(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"), null);
			Assert.Equal(1, response.Count);
		}

		[Fact, TestPriority(18)]
		public void GetRefList_Include_List()
		{
			var item = inMemory.DbContext.Organization_EmployeeEducation.ToList();

			var response = _employeeEduRepo.GetRefList(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
				include: i => i.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));

			Assert.Equal(item.ToList()[0].Institute, response.Items.ToList()[0].Institute);
		}

		[Fact, TestPriority(19)]
		public void GetRefList_NoAttribute_ThrowNullException()
		{
			var item = inMemory.DbContext.Organization_EmployeeEducation.ToList();
			try
			{
				var response = _attendanceRepo.GetRefList(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
					include: i => i.Include(x => x.Employee),
					orderBy: o => o.OrderByDescending(x => x.AddedAt));
				Assert.Equal(1, response.Count);
			}
			catch
			{
				Assert.Throws<ArgumentException>(() => _attendanceRepo.GetRefList(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
				include: i => i.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.AddedAt)));
			}

		}


		[Fact, TestPriority(20)]
		public void GetAllList_WithOrderByGetAll_GetList()
		{
			string OrderBy = "SchIntime";
			var response = _employeeEduRepo.GetAllList(OrderBy, x => x.YearOfPassing == 2021);
			Assert.Equal(2, response.Count());
		}

		[Fact, TestPriority(21)]
		public void Query_queryOutput()
		{
			var response = _employeeEduRepo.Query("select * from Org_EmployeeEducation ");
			Assert.Equal(2, response.Count());
		}

		[Fact, TestPriority(22)]
		public void ExecuteStoreQuery_Test()
		{
			var response = _employeeEduRepo.ExecuteStoreQuery<EmployeeEducation>("select * from Org_EmployeeEducation");
			Assert.Equal(2, response.Count());
		}


		[Fact, TestPriority(23)]
		public void Search_FindDataWithInMemory_IsSuccess()
		{
			var response = _employeeEduRepo.Search(Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"));
			Assert.Equal(Guid.Parse("ef9bc9ae-99ad-4e37-9d33-a6ad2339d3c1"), response.ID);
		}

	}
}
