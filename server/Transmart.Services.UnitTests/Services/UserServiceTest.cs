//using DocumentFormat.OpenXml.Vml.Spreadsheet;
//using Microsoft.EntityFrameworkCore;
//using MockQueryable.Moq;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Transmart.Services.UnitTests.Services.Setup;
//using TranSmart.Core;
//using TranSmart.Core.Result;
//using TranSmart.Core.Util;
//using TranSmart.Data;
//using TranSmart.Domain.Entities;
//using TranSmart.Domain.Entities.AppSettings;
//using TranSmart.Domain.Models;
//using TranSmart.Service;
//using Xunit;

//namespace Transmart.Services.UnitTests.Services
//{
//	public class UserServiceTest
//	{
//		private readonly Mock<TranSmartContext> _dbContext;
//		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
//		private IUserService _service;
//		private readonly Mock<DbContext> _context;
//		public UserServiceTest()
//		{
//			var builder = new DbContextOptionsBuilder<TranSmartContext>();
//			var app = new Mock<IApplicationUser>();
//			var dbContextOptions = builder.Options;
//			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
//			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
//			_service = new UserService(uow.Object);
//			_context = new Mock<DbContext>();
//		}

//		[Fact]
//		public async Task AddUserLoginLog_NoException()
//		{
//			// Arrange & Mock
//			var mockUserLoginLogToDB = new List<UserLoginLog>{};

//			var mockUserLoginLog = mockUserLoginLogToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUserLoginLog(uow, _context, mockUserLoginLog);

//			var item = new UserLoginLog{};

//			//Act
//			var src = await _service.AddUserLoginLog(item);

//			//Assert
//			Assert.True(src.IsSuccess);
//			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
//		}


//		[Fact]
//		public async Task AddUserLoginLog_ThrowException()
//		{
//			var item = new UserLoginLog{ };
//			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

//			//Act
//			var src = await _service.AddUserLoginLog(item);

//			//Assert
//			Assert.True(src.HasError);
//		}




//		[Fact]
//		public void  AddUserLoginFail()
//		{
//			// Arrange & Mock
//			var mockUserLoginFailToDB = new List<UserLoginFail>{};

//			var mockUserLoginFail = mockUserLoginFailToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUserLoginFail(uow, _context, mockUserLoginFail);

//			var item = new UserLoginFail{ ID = Guid.NewGuid(), IPAddress = "IPAddress", UserName = "UserName" };

//			//Act
//			var src =  _service.AddUserLoginFail(item);

//			//Assert
//			Assert.True(src.IsCompleted);
//		}


//		[Fact]
//		public async Task UpdateAsync_SpecificUser_NoException()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("d42bba7f-6691-4c34-b5d5-07548ded6065");
//			var roleId = Guid.Parse("4b336286-7639-40ee-9692-fe0e4f3b3c32");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = id,
//					RoleId = roleId
//				 }
//			};

//			var mockRoleToDB = new List<Role>
//			{
//				new Role
//				{
//					ID = roleId,
//					Name = "Development"
//				 },
//				new Role
//				{
//					ID = id,
//					Name = "Development"
//				 }
//			};

//			var mockUserAuditToDB = new List<UserAudit>
//			{
//				new UserAudit
//				{
//					ID = id
//				}
//			};

//			var userPasswords = new List<UserPasswords> { new UserPasswords {
//				UserId= id,Password = "$2a$12$2FRNdjxNadukHAdkQywSgObkyvoBHKIha/t2weZ3oOpivcRdIo6Ya",
//			} };

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var mockRole = mockRoleToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockRole(uow, _context, mockRole);

//			var mockUserAudit = mockUserAuditToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUserAudit(uow, _context, mockUserAudit);

//			var mockUserPassword = userPasswords.AsQueryable().BuildMockDbSet();
//			SetData.MockUserPassword(uow, _context, mockUserPassword);

//			var item = new User
//			{
//				ID = id,
//				RoleId = roleId,
//				Password = "password",
//				ExpireOn = DateTime.Parse("2022-10-10")
//			};

//			//Act
//			var src = await _service.UpdateAsync(item);

//			//Assert
//			Assert.True(src.IsSuccess);
//			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
//		}





//		[Fact]
//		public async Task UpdateAsync_SpecificUserRoleNotEqualToRole_NoException()
//		{
//			// Arrange & Mock
//			var userId = Guid.Parse("d42bba7f-6691-4c34-b5d5-07548ded6065");
//			var roleId = Guid.Parse("2af7565c-d748-4cb4-b557-eb298656093f");
//			var userRoleId = Guid.Parse("141b78c9-fb52-45d8-a65e-140c50c79b9d");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = userId,
//					RoleId = userRoleId
//				}
//			};

//			var mockRoleToDB = new List<Role>
//			{
//				new Role
//				{
//					ID = userRoleId,
//					Name = "Development"
//				 },
//				new Role
//				{
//					ID = roleId,
//					Name = "Tesing"
//				 }
//			};

//			var mockUserAuditToDB = new List<UserAudit>
//			{
//				new UserAudit
//				{
//					ID = userId
//				}
//			};

//			var userPasswords = new List<UserPasswords> { new UserPasswords
//			{
//				UserId= userId,
//				Password = "$2a$12$2FRNdjxNadukHAdkQywSgObkyvoBHKIha/t2weZ3oOpivcRdIo6Ya",
//			} };

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var mockRole = mockRoleToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockRole(uow, _context, mockRole);

//			var mockUserAudit = mockUserAuditToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUserAudit(uow, _context, mockUserAudit);

//			var mockUserPassword = userPasswords.AsQueryable().BuildMockDbSet();
//			SetData.MockUserPassword(uow, _context, mockUserPassword);
//			var item = new User
//			{
//				ID = userId,
//				RoleId = roleId,
//				Password = "password",
//				ExpireOn = DateTime.Parse("2022-10-10")
//			};

//			//Act
//			var src = await _service.UpdateAsync(item);

//			//Assert
//			Assert.True(src.IsSuccess);
//			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
//		}





//		[Fact]
//		public async Task UpdateAsync_NewUser_ThrowException()
//		{
//			// Arrange & Mock
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = Guid.NewGuid()
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);		

//			var item = new User
//			{
//				ID = Guid.NewGuid(),
//			};

//			//Act
//			var src = await _service.UpdateAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}




//		[Fact]
//		public void Validate_ExistUser()
//		{
//			// Arrange & Mock
//			var name = "Arjun";
//			var password = "Password";
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					Name = name,
//					Password = Encrypt.HashPassword(password),
//					LastLogin = DateTime.Parse("2022-07-07"),
//					RefreshToken = "vini"
//				}
//			};

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = _service.Validate(name, password);

//			//Assert
//			Assert.True(src.IsCompleted);
//		}




//		[Fact]
//		public void Validate_NewUser()
//		{
//			// Arrange & Mock
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					Name = "Arjun",
//					Password = Encrypt.HashPassword("Password"),
//					LastLogin = DateTime.Parse("2022-07-07"),
//					RefreshToken = "vini"
//				 }
//			};

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = _service.Validate("Arun", "Pssrd");

//			//Assert
//			Assert.True(src.IsCompleted);
//		}




//		[Fact]
//		public async Task Refresh_ExistUser_NoException()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("39f2a528-9c8f-4ed4-b673-cff8171a3fc4");
//            var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = id,
//					RefreshToken = "Hello",
//					RefreshTokenExpiryAt = DateTime.Now.AddHours(1)
//				 }
//			};

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = await _service.Refresh(id, "Hello");

//			//Assert
//			Assert.True(src.HasNoError);
//		}




//		[Fact]
//		public async Task Refresh_NewUser_ThrowException()
//		{
//			// Arrange & Mock
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = Guid.NewGuid()
//				 }
//			};

//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = await _service.Refresh(Guid.NewGuid(), "DFGR");

//			//Assert
//			Assert.True(src.HasError);
//		}




//		[Fact]
//		public async Task  ChangePassword_SpecificUserId_ReturnUser()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("39f2a528-9c8f-4ed4-b673-cff8171a3fc4");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = id
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src =  await _service.ChangePassword(id);

//			//Assert
//			Assert.Equal(id, src.ID);
//		}




//		[Fact]
//		public async Task ChangePassword_SpecificUserName_ReturnUser()
//		{
//			// Arrange & Mock
//			var username = "Arjun";
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					Name = username
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = await _service.ChangePassword(username);

//			//Assert
//			Assert.Equal(username, src.Name);
//		}



//		[Fact]
//		public async Task CheckUser_SpecificUserName_ReturnUser()
//		{
//			// Arrange & Mock
//			var user = "Arjun";
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					Name = user,
//					Employee=new TranSmart.Domain.Entities.Organization.Employee
//					{}
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = await _service.CheckUser(user);

//			//Assert
//			Assert.Equal(user, src.Name);
//		}


//		[Fact]
//		public async Task GetUserById_SpecificEmployee_ReturnUserData()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("59a4a8f5-294e-47a0-9c2f-f6ccce784109");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{	
//					EmployeeID = id,
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						ID = id
//					}
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			//Act
//			var src = await _service.GetUserById(id);

//			//Assert
//			Assert.Equal(id, src.Employee.ID);
//		}


//		[Fact]
//		public async Task GetPaginate_SpecificUserName_ReturnUserList()
//		{
//			// Arrange & Mock
//			var name = "Arjun";
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					EmployeeID = Guid.NewGuid(),
//					Name = name,
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Name = "Reddy",
//						Designation = new TranSmart.Domain.Entities.Organization.Designation{},
//						Department = new TranSmart.Domain.Entities.Organization.Department{},
//						Status = 1
//					},
//					Role = new Role{}
//				},
//				new User
//				{
//					EmployeeID = Guid.NewGuid(),
//					Name = name,
//					Employee = new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Name = "Prasad",
//						Designation = new TranSmart.Domain.Entities.Organization.Designation{},
//						Department = new TranSmart.Domain.Entities.Organization.Department{},
//						Status = 1
//					},
//					Role = new Role{}
//				},
//				new User
//				{
//					EmployeeID = Guid.NewGuid(),
//					Name = "Basanth",
//					Employee=new TranSmart.Domain.Entities.Organization.Employee
//					{
//						Name = "Kumari",
//						Designation = new TranSmart.Domain.Entities.Organization.Designation{},
//						Department = new TranSmart.Domain.Entities.Organization.Department{},
//						Status = 1
//					},
//					Role = new Role{}
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var baseSearch = new BaseSearch
//			{
//				Name = name
//			};

//			//Act
//			var src = await _service.GetPaginate(baseSearch);

//			//Assert
//			Assert.Equal(2, src.Count);
//		}
		 




//		[Fact]   
//		public async Task AddAsync_NewUser_NoException()
//		{
//			// Arrange & Mock
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					EmployeeID = Guid.NewGuid(),
//					Name = "Arjun"
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var item = new User
//			{
//				Name= "Barach",
//				EmployeeID = Guid.NewGuid(),
//				AddedAt = DateTime.Parse("2022-06-06"),
//				Password = "Password",
//				ExpireOn = DateTime.Parse("2022-10-10"),
//				Type=1
//			};

//			//Act
//			var src = await _service.AddAsync(item);

//			//Assert
//			Assert.True(src.IsSuccess);
//			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
//		}



//		[Fact]   
//		public async Task AddAsync_ExistUserName_ThrowException()
//		{
//			// Arrange & Mock
//			var name = "Arjun";
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					EmployeeID = Guid.NewGuid(),
//					Name = name
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var item = new User
//			{
//				Name = name,
//				EmployeeID = Guid.NewGuid(),
//				AddedAt = DateTime.Parse("2022-06-06"),
//				Password = "Password",
//				ExpireOn = DateTime.Parse("2022-10-10"),
//				Type = 1
//			};

//			//Act
//			var src = await _service.AddAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}





//		[Fact]  
//		public async Task AddAsync_ExistEmployee_ThrowException()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("59a4a8f5-294e-47a0-9c2f-f6ccce784109");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					EmployeeID = id,
//					Name = "Arjun"
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);

//			var item = new User
//			{
//				Name = "Barach",
//				EmployeeID = id,
//				AddedAt = DateTime.Parse("2022-06-06"),
//				Password = "Password",
//				ExpireOn = DateTime.Parse("2022-10-10"),
//				Type = 1
//			};

//			//Act
//			var src = await _service.AddAsync(item);

//			//Assert
//			Assert.True(src.HasError);
//		}



//		[Fact]
//		public async Task UpdatePassword_WithPasswordMatching_NoException()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = id,
//					Password=Encrypt.HashPassword("oldPassword")
//				 }
//			};
//			var userPassword = new List<UserPasswords> { new UserPasswords
//			{
//				Password = Encrypt.HashPassword("oldPassword"),
//				UserId= id,
//			} };
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);
			
//			var mockUserPassword = userPassword.AsQueryable().BuildMockDbSet();
//			SetData.MockUserPassword(uow, _context, mockUserPassword);
//			//Act
//			var src = await _service.UpdatePassword(id, "oldPassword", "newPassword");

//			//Assert
//			Assert.True(src.HasNoError);
//		}





//		[Fact]   
//		public async Task UpdatePassword_InvalidPassword_ThrowException()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
//			var mockUserToDB = new List<User>
//			{
//				new User
//				{
//					ID = id,
//					Password = Encrypt.HashPassword("oldPassword")
//				}
//			};
//			var mockUser = mockUserToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUser(uow, _context, mockUser);


//			//Act
//			var src = await _service.UpdatePassword(id, "InvalidPassword", "newPassword");

//			//Assert
//			Assert.True(src.HasError);
//		}



//		[Fact]
//		public void GetAuditHistory_SpecificUserAudit_ReturnUserAuditList()
//		{
//			// Arrange & Mock
//			var id = Guid.Parse("99d43d69-508d-42a1-bd44-fba7a70e6245");
//			var mockUserAuditToDB = new List<UserAudit>
//			{
//				new UserAudit
//				{
//					UserId = id
//				},
//				new UserAudit
//				{
//					UserId = id
//				},
//				new UserAudit
//				{
//					UserId = Guid.NewGuid()
//				}
//			};
//			var mockUserAudit = mockUserAuditToDB.AsQueryable().BuildMockDbSet();
//			SetData.MockUserAudit(uow, _context, mockUserAudit);

//			//Act
//			var src =  _service.GetAuditHistory(id).Result;

//			//Assert
//			Assert.Equal(2, src.ToList().Count);
//		}
//	}
//}
