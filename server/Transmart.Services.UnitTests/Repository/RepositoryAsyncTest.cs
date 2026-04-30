//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using TranSmart.Data;
//using TranSmart.Domain.Entities;
//using TranSmart.Domain.Entities.Leave;
//using TranSmart.Domain.Entities.Organization;
//using TranSmart.Services.UnitTests;
//using Xunit;

//namespace Transmart.Services.UnitTests.Repository
//{

//	[CollectionDefinition("Collection #1")]
//	[TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
//	public class RepositoryAsyncTest : IClassFixture<InMemoryFixture>
//	{
//		private readonly Mock<DbContext> _context;
//		private readonly InMemoryFixture inMemory;
//		private readonly RepositoryAsync<EmployeeEducation> _repo;
//		private readonly RepositoryAsync<Attendance> _attendanceRepo;
//		private readonly RepositoryAsync<LeaveBalance> _leaveBalanceRepo;
//		private readonly RepositoryAsync<ApplyWfh> _applyWFH;
//		private readonly RepositoryAsync<ApplyClientVisits> _applyClientVisits;
//		private readonly RepositoryAsync<User> _userRepo;

//		public RepositoryAsyncTest(InMemoryFixture fixture)
//		{
//			inMemory = fixture;
//			_context = new Mock<DbContext>();
//			_repo = new RepositoryAsync<EmployeeEducation>(inMemory.DbContext);
//			_attendanceRepo = new RepositoryAsync<Attendance>(inMemory.DbContext);
//			_leaveBalanceRepo = new RepositoryAsync<LeaveBalance>(inMemory.DbContext);
//			_applyWFH = new RepositoryAsync<ApplyWfh>(inMemory.DbContext);
//			_applyClientVisits = new RepositoryAsync<ApplyClientVisits>(inMemory.DbContext);
//			_userRepo = new RepositoryAsync<User>(inMemory.DbContext);
//		}

//		[Fact, TestPriority(1)]
//		public async Task AddAsync_SaveData_ShouldPost()
//		{
//			var employee = new Employee
//			{
//				ID = Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
//				No="Avontix2065",
//				Name = "Vijay",
//				FirstName= "Sangu",
//				AadhaarNumber = "123412341234",
//				MobileNumber ="9897949596"

//			};

//			var employee1 = new Employee
//			{
//				ID = Guid.Parse("3d2a0761-15c7-4fdf-abee-7920881f4598"),
//				No = "Avontix2061",
//				Name = "Vishnu",
//				FirstName = "Kulari",
//				AadhaarNumber = "123412341224",
//				MobileNumber = "9897949592"
//			};

//			var entity = new List<EmployeeEducation>
//			{
//				new EmployeeEducation
//				{
//					ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//					Institute = "TKR",
//					Percentage = 55,
//					Employee = employee,
//					EmployeeId =Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
//					YearOfPassing = 2021,
//					Degree ="B.Tech",
//					Medium = "English",
//					Qualification="Graduate"
//				},
//				new EmployeeEducation
//				{
//					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
//					Institute = "Princeton",
//					Percentage = 45,
//					Employee = employee1,
//					EmployeeId = Guid.Parse("3d2a0761-15c7-4fdf-abee-7920881f4598"),
//					YearOfPassing = 2021,
//					Degree ="No",
//					Medium = "English",
//					Qualification="Polytechnic"
//				}
//			};

//			await _repo.AddAsync(entity);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
//			Assert.True(response.Count == 2);
//		}

//		[Fact, TestPriority(2)]
//		public async Task SingleAsync_WithPredicate_ShouldGetSingleData()
//		{
//			var response = await _repo.SingleAsync(x => x.Institute == "TKR");
//			Assert.Equal("TKR", response.Institute);
//		}

//		[Fact, TestPriority(3)]
//		public async Task SingleAsync_WithInclude_ShouldGetIncludedTable()
//		{
//			var response = await _repo.SingleAsync(x => x.Institute == "TKR", include: i => i.Include(x => x.Employee));
//			Assert.Equal("Vijay", response.Employee.Name);
//		}

//		[Fact, TestPriority(4)]
//		public async Task SingleAsync_OrderBy_ShouldBeOrder()
//		{
//			var response = await _repo.SingleAsync(x => x.Institute == "TKR", include: i => i.Include(x => x.Employee), orderBy: o => o.OrderByDescending(x => x.Percentage));
//			Assert.Equal("Vijay", response.Employee.Name);
//		}

//		[Fact, TestPriority(5)]
//		public async Task GetAsync_WithPredicate_ShouldGetList()
//		{
//			var response = await _repo.GetAsync(x => x.Institute == "TKR", include: i => i.Include(x => x.Employee), orderBy: null);
//			Assert.Single(response);
//		}

//		[Fact, TestPriority(6)]
//		public async Task GetWithSelectAsync_WithPredicate_ShouldGetSelectedList()
//		{
//			var response = await _repo.GetWithSelectAsync<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
//				predicate: x => x.Percentage == 45,
//				include: x => x.Include(x => x.Employee));

//			Assert.Single(response);
//		}

//		[Fact, TestPriority(7)]
//		public async Task GetWithSelectAsync_OrderBy_ShouldGetSelectedList()
//		{
//			var response = await _repo.GetWithSelectAsync<EmployeeEducation>(selector: d => new EmployeeEducation { Percentage = d.Percentage },
//				predicate: x => x.Percentage == 45,
//				include: x => x.Include(x => x.Employee),
//				orderBy: o => o.OrderBy(x => x.Percentage));

//			Assert.Single(response);
//		}

//		[Fact, TestPriority(8)]
//		public async Task GetCountAsync_WithPredicate_ShouldGetCount()
//		{
//			var response = await _repo.GetCountAsync(x => x.YearOfPassing == 2021);
//			Assert.Equal(2, response);
//		}

//		[Fact, TestPriority(9)]
//		public async Task GetListAsync_WithPredicate_ShouldGetList()
//		{
//			string orderBy = "Percentage";
//			var response = await _repo.GetListAsync(orderBy, x => x.YearOfPassing == 2021);
//			Assert.Equal(2, response.Count());
//		}

//		[Fact, TestPriority(10)]
//		public async Task GetListAsync_OrderByNull_ShouldGetList()
//		{
//			var response = await _repo.GetListAsync(null, x => x.YearOfPassing == 2021);
//			Assert.Equal(2, response.Count());
//		}

//		[Fact, TestPriority(11)]
//		public async Task GetRefListAsync_WithPredicate_GetList()
//		{

//			var item = inMemory.DbContext.Organization_EmployeeEducation.ToList();
//			var response = await _repo.GetRefListAsync(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"), null);
//			Assert.Equal(1, response.Count);
//		}

//		[Fact, TestPriority(12)]
//		public async Task GetRefListAsync_NullAttribute_GetList()
//		{
//			var item = inMemory.DbContext.Organization_EmployeeEducation.ToList();

//			try
//			{
//				var response = await _attendanceRepo.GetRefListAsync(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
//					include: i => i.Include(x => x.Employee),
//					orderBy: o => o.OrderByDescending(x => x.AddedAt));
//				Assert.Equal(1, response.Count);
//			}
//			catch
//			{
//				_ = Assert.ThrowsAsync<ArgumentException>(async () => await _attendanceRepo.GetRefListAsync(item[0].EmployeeId, x => x.EmployeeId == Guid.Parse("cc3489fb-6e2f-4a0e-abcb-654ac6179331"),
//				  include: i => i.Include(x => x.Employee),
//				  orderBy: o => o.OrderByDescending(x => x.AddedAt)));
//			}
//		}

//		[Fact, TestPriority(13)]
//		public async Task GetPaginateAsync_GetItems_ShouldBePaginate()
//		{
//			var response = await _repo.GetPaginateAsync(predicate: x => x.ID == Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//				include: i => i.Include(x => x.Employee));

//			Assert.Equal(1, response.Count);
//		}

//		[Fact, TestPriority(14)]
//		public async Task GetPaginateAsync_OrderBy_ShouldBePaginate()
//		{
//			var response = await _repo.GetPaginateAsync(predicate: x => x.ID == Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//				orderBy: o => o.OrderBy(x => x.Percentage),
//				include: i => i.Include(x => x.Employee));

//			Assert.Equal(1, response.Count);
//		}

//		[Fact, TestPriority(15)]
//		public async Task GetPageListAsync_WithPredicate_ShouldBeList()
//		{
//			var response = await _repo.GetPaginateAsync(predicate: x => x.ID == Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//				orderBy: null,
//				include: i => i.Include(x => x.Employee));

//			Assert.Equal(1, response.Count);
//		}

//		[Fact, TestPriority(16)]
//		public async Task GetPageListAsync_OrderBy_ShouldBeList()
//		{
//			var response = await _repo.GetPaginateAsync(predicate: x => x.ID == Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//				include: i => i.Include(x => x.Employee),
//				orderBy: o => o.OrderBy(x => x.Percentage));

//			Assert.Equal(1, response.Count);


//		}

//		[Fact, TestPriority(17)]
//		public async Task AddAsync_SingleEntity_ShouldSave()
//		{
//			var leaveBalance = new LeaveBalance
//			{
//				ID = Guid.Parse("5e41d78b-51c1-4869-98db-93758654cc3b"),
//				Leaves = 10
//			};

//			await _leaveBalanceRepo.AddAsync(leaveBalance);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Leave_LeaveBalance.ToList();
//			Assert.Single(response);
//		}

//		[Fact, TestPriority(18)]
//		public async Task AddAsync_AddWithCancellationToken_ShouldSave()
//		{
//			var applyWFH = new ApplyWfh
//			{
//				ID = Guid.NewGuid(),
//				ReasonForWFH ="Illness"
//			};

//			await _applyWFH.AddAsync(applyWFH, CancellationToken.None);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.LM_ApplyWFH.ToList();
//			Assert.Single(response);
//		}

//		[Fact, TestPriority(19)]
//		public async Task AddAsync_AddWithGuid_ShouldSave()
//		{
//			var applyClientVisits = new ApplyClientVisits
//			{
//				AdminReason = "Client Visit",
//				PlaceOfVisit = "Hyderabad",
//				PurposeOfVisit = "Project Work"
//			};

//			await _applyClientVisits.AddAsync(applyClientVisits, Guid.Parse("03b85d42-d9af-4bc5-9c4a-270027654d79"));
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.SelfService_ApplyClientVisits.ToList();
//			Assert.Single(response);
//		}

//		[Fact, TestPriority(20)]
//		public async Task AddAsync_Params_ShouldSave()
//		{
//			var user = new User
//			{
//				Name = "AVN8002"
//			};

//			var user1 = new User
//			{
//				Name = "AVN8001"
//			};

//			User[] paramEntity = { user, user1 };
//			await _userRepo.AddAsync(paramEntity);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Users.ToList();
//			Assert.Equal(2, response.Count);
//		}

//		[Fact, TestPriority(21)]
//		public void UpdateAsync_SingleEntity_ShouldUpdate()
//		{
//			inMemory.DbContext.ChangeTracker.Clear();
//			var item = inMemory.DbContext.Leave_LeaveBalance.FirstOrDefault(x => x.ID == Guid.Parse("5e41d78b-51c1-4869-98db-93758654cc3b"));
//			item.Leaves = 12;

//			_leaveBalanceRepo.UpdateAsync(item);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Leave_LeaveBalance.FirstOrDefault();
//			Assert.Equal(12, response.Leaves);
//		}

//		[Fact, TestPriority(22)]
//		public void UpdateAsync_IEnumerable_ListShouldUpdate()
//		{
//			var empEdulist = new List<EmployeeEducation>
//			{
//				new EmployeeEducation
//				{
//					ID = Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"),
//					Institute = "TKR1++"
//				},
//				new EmployeeEducation
//				{
//					ID = Guid.Parse("d66d4f6b-93b1-4acf-906f-7e21f6bd12e0"),
//					Institute = "Princeton++"
//				}
//			};
			
//			var list = inMemory.DbContext.Organization_EmployeeEducation.ToList();

//			foreach (var empEdu in empEdulist)
//			{
//				foreach (var item in list)
//				{
//					if (empEdu.ID == item.ID)
//					{
//						item.Institute = empEdu.Institute;
//					}
//				}
//			}

//			_repo.UpdateAsync(list);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
//			Assert.Equal(empEdulist[0].Institute, response[0].Institute);
//			Assert.Equal(empEdulist[1].Institute, response[1].Institute);
//		}

//		[Fact, TestPriority(23)]
//		public void DeleteAsync_SingleEntity_ShouldDelete()
//		{
//			var entity = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault(x => x.ID == Guid.Parse("ffe8ac2a-ae1a-4361-9381-d01630f208cc"));

//			_repo.DeleteAsync(entity);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
//			Assert.True(response.Count == 1);
//		}

//		[Fact, TestPriority(24)]
//		public void DeleteAsync_WithObject_ShouldDelete() // Not Deleting
//		{
//			var entity = inMemory.DbContext.Organization_EmployeeEducation.FirstOrDefault();

//			_repo.DeleteAsync(entity.ID);
//			inMemory.DbContext.SaveChanges();

//			var response = inMemory.DbContext.Organization_EmployeeEducation.ToList();
//			Assert.True(response.Count == 1);
//		}

//		[Fact, TestPriority(25)]
//		public async Task HasRecord_CheckRecords_True()
//		{
//			var response = await _repo.HasRecordsAsync(x => x.Percentage == 45);
//			Assert.True(response);
//		}

//		[Fact, TestPriority(26)]
//		public async Task SumOfDecimalAsync_FindSum_Decimal()
//		{
//			var response = await _repo.SumOfDecimalAsync(x => x.Percentage == 45, x => x.Percentage);
//			Assert.Equal(45, response);
//		}

//		[Fact, TestPriority(27)]
//		public async Task SumOfIntAsync_FindSum_Integer()
//		{
//			var response = await _repo.SumOfIntAsync(x => x.Percentage == 45, x => x.YearOfPassing);
//			Assert.Equal(2021, response);
//		}

//	}
//}
