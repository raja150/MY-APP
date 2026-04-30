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
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Service.AppSettings;
using Xunit;

namespace Transmart.Services.UnitTests.Services
{
	public class RolesServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IRolesService _service;
		private readonly Mock<DbContext> _context;
		public RolesServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new RolesService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public void Get_SpecificRole_ReturnRoleData()
		{
			// Arrange & Mock
			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
			var mockRoleToDatabase = new List<Role>
			{
				new Role
				{
					ID = id
				 }
			};

			var mockRolePrivilegeToDB = new List<RolePrivilege>
			{
				new RolePrivilege
				{
					ID = Guid.NewGuid(),
					RoleId = id,
					Page = new Page
					{
						Group = new Group
						{}
					}
				}
			};

			var mockRoleReportPrivilegeToDB = new List<RoleReportPrivilege>
			{
				new RoleReportPrivilege
				{
					RoleId = id,
					Report = new Report
					{
						Module = new ReportModule
						{}
					}
				}
			};

			var mockRole = mockRoleToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockRole);

			var mockRolePrivilege = mockRolePrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRolePrivilege(uow, _context, mockRolePrivilege);

			var mockRoleReportPrivilege = mockRoleReportPrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRoleReportPrivilege(uow, _context, mockRoleReportPrivilege);

			//Act
			var src = _service.GetById(id);

			//Assert
			Assert.Equal(id, src.Result.ID);
		}





		[Fact]
		public void Validate()
		{
			// Arrange & Mock
			string name = "Arjun";

			//Act
			var src = _service.Validate(name);

			//Assert
			Assert.True(src);
		}


		[Fact]
		public async void RoleMenu_SpecificRole_ReturnRolePrivilegeList()
		{
			// Arrange & Mock
			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
			var mockRolePrivilegeToDB = new List<RolePrivilege>
			{
				new RolePrivilege
				{
					RoleId = id,
					Page = new Page
					{
						Group = new Group{}
					},
					Privilege = 2
				},
				new RolePrivilege
				{
					RoleId = id,
					Page = new Page
					{
						Group = new Group{}
					},
					Privilege = 2,
				},
				new RolePrivilege
				{
					RoleId = Guid.NewGuid(),
					Page = new Page
					{
						Group = new Group{}
					},
					Privilege = 2,
				}
			};
			var mockRolePrivilege = mockRolePrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRolePrivilege(uow, _context, mockRolePrivilege);

			//Act
			var src = await _service.RoleMenu(id);

			//Assert
			Assert.Equal(2, src.Count());
		}



		[Fact]
		public async void Pages_IFExistGroup_ReturnPage()
		{
			// Arrange & Mock
			var mockPageToDB = new List<Page>
			{
				new Page
				{
					Group = new Group{}
				},
				new Page
				{
					Group = new Group{}
				}
			};

			var mockPageNoNonAsync = mockPageToDB.AsQueryable().BuildMockDbSet();
			SetData.MockPageNo(uow, _context, mockPageNoNonAsync);

			//Act
			var src = await _service.Pages();

			//Assert
			Assert.Equal(2, src.Count());
		}


		[Fact]
		public void  CustomValidation_CanEditRole_NoException()
		{
			// Arrange & Mock
			var mockRoleToDB = new List<Role>
			{
				new Role
				{
					ID = Guid.NewGuid(),
					CanEdit = true
				}
			};

			var mockPageNoNonAsync = mockRoleToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockPageNoNonAsync);

			var item = new Role
			{
				ID = Guid.NewGuid(),
				Name = "Developer"
			};

			//Act
			var src = new RolesService(uow.Object); 
			var result = new Result<Role>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasNoError);
		}



		[Fact]
		public void  CustomValidation_CanNotEditRole_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("6083f1dc-4e62-42c8-9505-ea38b86f7fa6");
			var mockRoleToDB = new List<Role>
			{
				new Role
				{
					ID = id,
					CanEdit = false
				}
			};

			var mockPageNoNonAsync = mockRoleToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockPageNoNonAsync);

			var item = new Role
			{
				ID = id
			};

			//Act
			var src = new RolesService(uow.Object);
			var result = new Result<Role>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}


		[Fact]
		public async void GetModules_ReturnGroupList()
		{
			// Arrange & Mock
			var mockGroupToDB = new List<Group>
			{
				new Group
				{
					ID = Guid.NewGuid()
				},
				new Group
				{
					ID = Guid.NewGuid()
				}
			};

			var mockPageNoNonAsync = mockGroupToDB.AsQueryable().BuildMockDbSet();
			SetData.MockGroup(uow, _context, mockPageNoNonAsync);

			//Act
			var dd = await _service.GetModules();

			//Assert
			Assert.Equal(2, dd.Count());
		}




		[Fact]
		public async void GetReport_SpecificReport_ReturnReportList()
		{
			// Arrange & Mock
			var id = Guid.Parse("e4e7d114-d279-4372-961e-aad35a8e6ce4");
			var mockGroupToDB = new List<Report>
			{
				new Report
				{
					ModuleId = id,
					Module = new ReportModule {}
				},
				new Report
				{
					ModuleId = id,
					Module = new ReportModule {}
				},
				new Report
				{
					ModuleId = Guid.NewGuid(),
					Module = new ReportModule {}
				}
			};
			var mockPageNoNonAsync = mockGroupToDB.AsQueryable().BuildMockDbSet();
			SetData.MockReport(uow, _context, mockPageNoNonAsync);

			//Act
			var dd = await _service.GetReport(id);

			//Assert
			Assert.Equal(2, dd.Count());
		}



		[Fact]  
		public async Task UpdateAsync_SpecificRole_ThrowException()
		{
			// Arrange & Mock
			var pageId = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
			var reportId = Guid.Parse("5c6bef3c-b182-488c-812e-b5f6e3b00049");
			var mockRoleToDatabase = new List<Role>
			{
				new Role
				{
					ID = pageId,
					Pages = new List<RolePrivilege>
					{
						new RolePrivilege
						{
							PageId = pageId,
						}
					},
					Reports= new List<RoleReportPrivilege>
					{
					   new RoleReportPrivilege
					   {
							ID = pageId,
							ReportId = reportId,
							Privilege = true,
							RoleId = pageId
					   }
					}
				}
			};

			var mockRolePrivilegeToDB = new List<RolePrivilege>
			{
				new RolePrivilege
				{
					ID = Guid.NewGuid(),
					PageId = pageId,
					RoleId = pageId,
					Page = new Page
					{
						ID = pageId,
						Group = new Group
						{}
					},
					Privilege = 1
				}
			};

			var mockRoleReportPrivilegeToDB = new List<RoleReportPrivilege>
			{
				new RoleReportPrivilege
				{
					RoleId = pageId,
					Report = new Report
					{
						Module = new ReportModule
						{}
					},
					ReportId = pageId
				}
			};
			var mockRole = mockRoleToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockRole);

			var mockRolePrivilege = mockRolePrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRolePrivilege(uow, _context, mockRolePrivilege);

			var mockRoleReportPrivilege = mockRoleReportPrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRoleReportPrivilege(uow, _context, mockRoleReportPrivilege);

			var item = new Role
			{
				ID = pageId,
				Name = "Arjun",
				Pages = new List<RolePrivilege>
				{
					new RolePrivilege
					{
						PageId = pageId,
						Privilege = 1,
						Page = new Page
						{
							ID = pageId,
							Privilege = 1
						}
					}
				},
				Reports = new List<RoleReportPrivilege>
				{
					new RoleReportPrivilege
					{
						ID = pageId,
						ReportId = reportId,
						Privilege = false,
						RoleId = pageId
					}
				}
			};

			//Act
			var dd = await _service.UpdateAsync(item);

			//Assert
			Assert.True(dd.HasError);
		}





		[Fact]
		public async Task UpdateAsyncTest_ThrowException()
		{
			// Arrange & Mock
			var roleId = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
			var pageId = Guid.Parse("7be319dd-79e2-4eef-b937-bc4c2ee6b670");
			var reportId = Guid.Parse("8150aa77-05bf-4354-9bdc-58d14a531a25");
			var mockRoleToDatabase = new List<Role>
			{
				new Role
				{
					ID = roleId,
					Pages = new List<RolePrivilege>
					{
						new RolePrivilege
						{
							PageId = pageId,
						 }
					 },
					Reports= new List<RoleReportPrivilege>
					{
					   new RoleReportPrivilege
					   {
							ID = roleId,
							ReportId = Guid.NewGuid(),
							Privilege = true,
							RoleId = roleId
					   }
					}
				}
			};

			var mockRolePrivilegeToDB = new List<RolePrivilege>
			{
				new RolePrivilege
				{
					ID = Guid.NewGuid(),
					PageId = pageId,
					RoleId = roleId,
					Page = new Page
					{
						ID = pageId,
						Group = new Group
						{}
					}
				}
			};

			var mockRoleReportPrivilegeToDB = new List<RoleReportPrivilege>
			{
				new RoleReportPrivilege
				{
					RoleId = roleId,
					Report= new Report
					{
						Module = new ReportModule
						{}
					},
					ReportId = roleId
				}
			};

			var mockRole = mockRoleToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockRole);

			var mockRolePrivilege = mockRolePrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRolePrivilege(uow, _context, mockRolePrivilege);

			var mockRoleReportPrivilege = mockRoleReportPrivilegeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRoleReportPrivilege(uow, _context, mockRoleReportPrivilege);

			var item = new Role
			{
				ID = roleId,
				Name = "Arjun",
				Pages = new List<RolePrivilege>
				{
					new RolePrivilege
					{
						PageId = Guid.NewGuid(),
						Privilege = 1,
						Page = new Page
						{
							ID = roleId,
							Privilege = 1
						}
					}
				},
				Reports = new List<RoleReportPrivilege>
				{
					new RoleReportPrivilege
					{
						ID = roleId,
						ReportId = reportId,
						Privilege = true,
						RoleId = roleId
					}
				}
			};

			//Act
			var dd = await _service.UpdateAsync(item);

			//Assert
			Assert.True(dd.HasError);
		}




		[Fact]
		public async void Roles_ReturnRoleList()
		{
			// Arrange & Mock
			var mockRoleToDB = new List<Role>
			{
				new Role
				{
					ID = Guid.NewGuid()
				},
				new Role
				{
					ID = Guid.NewGuid()
				}
			};

			var mockRole = mockRoleToDB.AsQueryable().BuildMockDbSet();
			SetData.MockRole(uow, _context, mockRole);

			//Act
			var src = await _service.Roles();

			//Assert
			Assert.Equal(2, src.Count());
		}
	}
}
