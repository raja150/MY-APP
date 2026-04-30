using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Setup;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{
	public class SearchServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private ISearchService _service;
		private readonly Mock<DbContext> _context;
		public SearchServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new SearchService(uow.Object);
			_context = new Mock<DbContext>();
		}




		[Fact]
		public async Task Get_ReturnRolePrivilegeList()
		{
			// Arrange & Mock
			var mockRolePrivilegeToDB = new List<RolePrivilege>
			{
				new RolePrivilege
				{
					ID = Guid.NewGuid(),
					Page = new TranSmart.Domain.Entities.Page
					{
						Group = new TranSmart.Domain.Entities.Group {}
					}
				},
				new RolePrivilege
				{
					ID = Guid.NewGuid(),
					Page = new TranSmart.Domain.Entities.Page
					{
						Group = new TranSmart.Domain.Entities.Group {}
					}
				}
			};
			var mockRolePrivilege = mockRolePrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRolePrivilege(uow, _context, mockRolePrivilege);

			//Act
			var src = await _service.GetRolePrivilige();

			//Assert
			Assert.Equal(2, src.Count());
		}




		[Fact]
		public async Task GetRoleReportPrivilige_ReturnRoleReportPrivilegeList()
		{
			// Arrange & Mock
			var mockRoleReportPrivilegeToDB = new List<RoleReportPrivilege>
			{
				new RoleReportPrivilege
				{
					Report = new TranSmart.Domain.Entities.Report
					{
						Module = new TranSmart.Domain.Entities.ReportModule
						{}
					}
				 },
				new RoleReportPrivilege
				{
					Report = new TranSmart.Domain.Entities.Report
					{
						Module = new TranSmart.Domain.Entities.ReportModule
						{}
					}
				 }
			};

			var mockRoleReportPrivilege = mockRoleReportPrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRoleReportPrivilege(uow, _context, mockRoleReportPrivilege);

			//Act
			var src = await _service.GetRoleReportPrivilige();

			//Assert
			Assert.Equal(2, src.Count());
		}




		[Fact]
		public async void GetPayMonths_SpecificName_ReturnPayMonthList()
		{
			// Arrange & Mock
			var name = "Arjun";
			var mockPayMonthToDB = new List<PayMonth>
			{
				new PayMonth
				{
					Name = name
				},
				new PayMonth
				{
					Name = name
				},
				new PayMonth
				{
					Name = "Micheal"
				}
			};

			var mockPayMonthNonAsync = mockPayMonthToDB.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockPayMonthNonAsync);

			//Act
			var src = await _service.GetPayMonths(name);

			//Assert
			Assert.Equal(2, src.Count());
		}


		[Fact]
		public async void GetEmployee_SpecificName_ReturnEmployeeList()
		{
			// Arrange & Mock
			var name = "Arjun";
			var mockEmployeeToDB = new List<Employee>
			{
				new Employee
				{
					Name = name
				},
				new Employee
				{
					Name = name
				},
				new Employee
				{
					Name = "Micheal"
				}
			};

			var mockEmployeeNonAsync = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployeeNonAsync);

			//Act
			var src = await _service.GetEmployee(name);

			//Assert
			Assert.Equal(2, src.Count());
		}


		[Fact]
		public async void GetEmployeeDetails_SpecificNameOrMobile_ReturnEmployeeList()
		{
			// Arrange & Mock
			var name = "Arjun";
			var mockEmployeeToDB = new List<Employee>
			{
				new Employee
				{
					Name = name,
					Department = new Department{},
					Designation = new Designation{}
				},
				new Employee
				{
					Name = name,
					Department = new Department{},
					Designation = new Designation{}
				}
			};

			var mockEmployeeNonAsync = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployeeNonAsync);

			//Act
			var src = await _service.GetEmployeeDetails(name);

			//Assert
			Assert.Equal(2, src.Count());
		}





		[Fact]
		public async Task GetEmployeeBySearch_ReturnEmployeeList()
		{
			// Arrange & Mock
			var name = "Arjun";
			var mockEmployeeToDB = new List<Employee>
			{
				new Employee
				{
					Name = name,
					Department = new Department{},
					Designation = new Designation{}
				},
				new Employee
				{
					Name = name,
					Department = new Department{},
					Designation = new Designation{}
				}
			};

			var mockEmployee = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = await _service.GetEmployeeBySearch();

			//Assert
			Assert.Equal(2, src.Count());
		}

		[Fact]
		public async Task GetTeamEmployeeBySearch_ReturnEmployeeTeamList()
		{
			// Arrange & Mock
			var loginUserId = Guid.NewGuid();
			var teamId = Guid.NewGuid();
			var mockEmployeeToDB = new List<Employee>
			{
				new Employee
				{
					ID = loginUserId,
					Name = "vamshi",
					Department = new Department{},
					Designation = new Designation{},
					TeamId=teamId,
					Team = new Team{ID=teamId},
					
				},
				new Employee
				{
					ID=Guid.NewGuid(),
					Name = "Shiva",
					Department = new Department{},
					Designation = new Designation{},
					TeamId=teamId,
					Team = new Team{ID=teamId},
				},
				new Employee
				{
					ID=Guid.NewGuid(),
					Name = "mahesh",
					Department = new Department{},
					Designation = new Designation{},
					TeamId=Guid.NewGuid(),
					Team = new Team{ID=Guid.NewGuid()},
				}
			};

			var mockEmployee = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = await _service.GetTeamEmployeeBySearch(loginUserId);

			//Assert
			Assert.Equal(2, src.ReturnValue.Count());
		}

		[Fact]
		public async Task GetDesignations_SpecificDesignation_ReturnDesignationList()
		{
			// Arrange & Mock
			var payMonthId = Guid.Parse("cc917398-79a2-467e-b34b-a48402584b64");
			var designationId = Guid.Parse("9919d559-0712-4bcc-81ba-000fdeea44bc");
			var mockPaySheetToDB = new List<PaySheet>
			{
				new PaySheet
				{
					PayMonthId = payMonthId,
					Employee = new Employee
					{
						DesignationId = designationId,
						Designation = new Designation
						{}
					}
				},
				new PaySheet
				{
					PayMonthId = payMonthId,
					Employee = new Employee
					{
						DesignationId = designationId,
						Designation = new Designation
						{}
					}
				}
			};
			var mockDesignationDB = new List<Designation>
			{
				new Designation
				{
					ID =  designationId,
					Status = true,
					Name = "Developer"
				}
			};

			var mockPaySheet = mockPaySheetToDB.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockPaySheet);

			var mockDesignation = mockDesignationDB.AsQueryable().BuildMockDbSet();
			SetData.MockDesignation(uow, _context, mockDesignation);

			//Act
			var src = await _service.GetDesignations(payMonthId);

			//Assert
			Assert.Equal(2, src.Count());
		}
	}
}
