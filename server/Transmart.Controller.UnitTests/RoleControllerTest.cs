//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TranSmart.API.Controllers;
//using TranSmart.API.Domain.Models;
//using TranSmart.API.Extensions;
//using TranSmart.API.Services;
//using TranSmart.AR.Controller.UnitTest.Data;
//using TranSmart.Core.Result;
//using TranSmart.Data.Paging;
//using TranSmart.Domain.Entities.AppSettings;
//using TranSmart.Domain.Models;
//using TranSmart.Domain.Models.AppSettings;
//using TranSmart.Domain.Models.Cache;
//using TranSmart.Domain.Models.Settings;
//using TranSmart.Service.AppSettings;
//using TranSmart.Service.Reports;
//using Xunit;

//namespace Transmart.Controller.UnitTests
//{
//	public class RoleControllerTest : ControllerTestBase
//	{
//		private readonly Mock<IRolesService> _roleService;
//		private readonly Mock<ICacheService> _cacheService;
//		private readonly Mock<IAppReportService> _appReportService;
//		private readonly RoleController _controller;
//		public RoleControllerTest() : base()
//		{
//			_roleService = new Mock<IRolesService>();
//			_cacheService = new Mock<ICacheService>();
//			_appReportService = new Mock<IAppReportService>();

//			_controller = new RoleController(Mapper, _roleService.Object, _cacheService.Object, _appReportService.Object)
//			{
//				ControllerContext = new ControllerContext()
//				{
//					HttpContext = new DefaultHttpContext() { User = Claim }
//				}
//			};
//		}



//		[Theory]
//		[InlineData(2)]
//		public async Task PAGINATE_TEST(int count)
//		{
//			var roles = SetData.RolesData();
//			_ = _roleService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>()))
//				.ReturnsAsync(roles.ToPaginate(0, 10));

//			// Act
//			var resposne = await _controller.Paginate(new BaseSearch());
//			var okResult = resposne;

//			// Assert
//			Assert.NotNull(okResult);
//			Assert.Equal(count, okResult.Items.Count());
//		}


//		[Fact]
//		public async Task GET_ALL_ROLES_TEST()
//		{
//			_ = _roleService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(
//				new Role
//				{
//					ID = EmployeeId,
//					Pages = new List<RolePrivilege>
//					{
//					  new RolePrivilege
//					  {
//						  ID=EmployeeId
//					  }
//					}
//				}));

//			var resposne = await _controller.Get(It.IsAny<Guid>());

//			var okResult = resposne as OkObjectResult;

//			// Assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}


//		[Theory]
//		[InlineData(false, false)]
//		[InlineData(true, true)]
//		public async Task POST_ROLES_TEST(bool badRequest, bool hasError)
//		{
//			var result = new Result<Role>();

//			_ = !hasError
//				? _roleService.Setup(x => x.AddAsync(It.IsAny<Role>()))
//			 .Callback(() =>
//			 {
//				 result.IsSuccess = true;
//				 result.ReturnValue = new Role { ID = EmployeeId };
//			 }).ReturnsAsync(result)
//				:
//				 _roleService.Setup(x => x.AddAsync(It.IsAny<Role>()))
//			   .Callback(() =>
//			   {
//				   result.AddMessageItem(new MessageItem("Error"));
//				   result.ReturnValue = null;
//			   }).ReturnsAsync(result);

//			// Act
//			var resposne = await _controller.Post(SetData.RoleRequest());

//			ADD_CODE(resposne, badRequest);
//		}



//		[Theory]
//		[InlineData(false, false)]
//		[InlineData(true, true)]
//		public async Task PUT_ROLES_TEST(bool badRequest, bool hasError)
//		{

//			var result = new Result<Role>();

//			_ = !hasError
//				? _roleService.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
//			 .Callback(() =>
//			 {
//				 result.IsSuccess = true;
//				 result.ReturnValue = new Role { ID = EmployeeId };
//			 }).ReturnsAsync(result)
//				:
//				_roleService.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
//			   .Callback(() =>
//			   {
//				   result.AddMessageItem(new MessageItem("hello"));
//				   result.ReturnValue = null;
//			   }).ReturnsAsync(result);


//			// Act
//			var resposne = await _controller.Put(SetData.RoleRequest());

//			ADD_CODE(resposne, badRequest);
//		}



//		private void ADD_CODE(dynamic resposne, bool hasBadRequest)
//		{
//			var okResult = resposne as OkObjectResult;
//			if (hasBadRequest)
//			{
//				Assert.Null(okResult);
//				Assert.Equal(400, resposne.StatusCode);
//			}
//			else
//			{
//				Assert.NotNull(okResult);
//				Assert.Equal(200, okResult.StatusCode);
//			}

//		}

//		[Fact]
//		public async void GET_PAGES_TEST()
//		{
//			var pages = new List<TranSmart.Domain.Entities.Page>
//			{
//				new TranSmart.Domain.Entities.Page
//				{
//					DisplayName =" My Claims", DisplayOrder=1,Module ="Claims",Name ="MyClaims",Privilege=15
//				},
//				 new TranSmart.Domain.Entities.Page
//				{
//					DisplayName ="Audit", DisplayOrder=1,Module ="Claims",Name ="Audit",Privilege=15
//				}
//			};
//			_ = _roleService.Setup(x => x.Pages()).ReturnsAsync(pages);

//			var response = await _controller.GetPages();
//			var okResult = response as OkObjectResult;

//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(2, ((List<Page>)okResult.Value).Count);
//		}



//		[Fact]
//		public async Task GET_REPORTS_TEST()
//		{
//			var reports = new List<TranSmart.Domain.Entities.Report>
//			{
//				new TranSmart.Domain.Entities.Report
//				{
//					Label =" My Claims", DisplayOrder=1,Name ="MyClaims",
//				},
//				 new TranSmart.Domain.Entities.Report
//				{
//					Label ="Audit", DisplayOrder=1,Name ="Audit",
//				}
//			};

//			_ = _appReportService.Setup(x => x.GetReports()).ReturnsAsync(reports);

//			var response = await _controller.GetReports();
//			var okResult = response as OkObjectResult;

//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(2, ((List<RoleReportPrivilegeModel>)okResult.Value).Count);
//		}





//		[Fact]
//		public async Task GET_MENU_TEST()
//		{
//			var menu = new List<RolePrivilegeCache>
//			{
//				new RolePrivilegeCache {GroupName ="Settings",PageName ="Carrirer",Privilege=15,RoleId = EmployeeId},
//				new RolePrivilegeCache {GroupName ="Settings",PageName ="Client",Privilege=15,RoleId = EmployeeId},
//				new RolePrivilegeCache {GroupName ="Reports",PageName ="Summary",Privilege=15,RoleId = EmployeeId}
//			};

//			_ = _cacheService.Setup(x => x.GetRoleMenu(It.IsAny<Guid>())).ReturnsAsync(menu);
//			_ = _controller.GetType().GetMethod("GetMenu");
//			var response = await _controller.GetMenu(Guid.NewGuid());

//			var okResult = response as OkObjectResult;
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(3, ((List<RolePrivilegeCache>)okResult.Value).Count);
//		}

//		[Fact]
//		public async Task GET_MENU_CONTENT__TEST()
//		{
//			var menu = new Role
//			{
//				ID = EmployeeId,
//				Menu = "Settings"
//			};

//			_ = _roleService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(menu);
//			var response = await _controller.GetMenuContent(Guid.NewGuid());

//			var okResult = response as OkObjectResult;
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal("Settings", (okResult.Value));
//		}



//		[Fact]
//		public async void GetModulesTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.GetModules());

//			// Act
//			var response = await _controller.GetModules();
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}

//		[Fact]
//		public async void GetReportTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.GetReport(It.IsAny<Guid>()));

//			// Act
//			var response = await _controller.GetReport(It.IsAny<Guid>());
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}



//		[Fact]
//		public async void GetAllRollesTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.Roles());

//			// Act
//			var response = await _controller.GetAllRolles();
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}
//	}
//}//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TranSmart.API.Controllers;
//using TranSmart.API.Domain.Models;
//using TranSmart.API.Extensions;
//using TranSmart.API.Services;
//using TranSmart.AR.Controller.UnitTest.Data;
//using TranSmart.Core.Result;
//using TranSmart.Data.Paging;
//using TranSmart.Domain.Entities.AppSettings;
//using TranSmart.Domain.Models;
//using TranSmart.Domain.Models.AppSettings;
//using TranSmart.Domain.Models.Cache;
//using TranSmart.Domain.Models.Settings;
//using TranSmart.Service.AppSettings;
//using TranSmart.Service.Reports;
//using Xunit;

//namespace Transmart.Controller.UnitTests
//{
//	public class RoleControllerTest : ControllerTestBase
//	{
//		private readonly Mock<IRolesService> _roleService;
//		private readonly Mock<ICacheService> _cacheService;
//		private readonly Mock<IAppReportService> _appReportService;
//		private readonly RoleController _controller;
//		public RoleControllerTest() : base()
//		{
//			_roleService = new Mock<IRolesService>();
//			_cacheService = new Mock<ICacheService>();
//			_appReportService = new Mock<IAppReportService>();

//			_controller = new RoleController(Mapper, _roleService.Object, _cacheService.Object, _appReportService.Object)
//			{
//				ControllerContext = new ControllerContext()
//				{
//					HttpContext = new DefaultHttpContext() { User = Claim }
//				}
//			};
//		}



//		[Theory]
//		[InlineData(2)]
//		public async Task PAGINATE_TEST(int count)
//		{
//			var roles = SetData.RolesData();
//			_ = _roleService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>()))
//				.ReturnsAsync(roles.ToPaginate(0, 10));

//			// Act
//			var resposne = await _controller.Paginate(new BaseSearch());
//			var okResult = resposne;

//			// Assert
//			Assert.NotNull(okResult);
//			Assert.Equal(count, okResult.Items.Count());
//		}


//		[Fact]
//		public async Task GET_ALL_ROLES_TEST()
//		{
//			_ = _roleService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(
//				new Role
//				{
//					ID = EmployeeId,
//					Pages = new List<RolePrivilege>
//					{
//					  new RolePrivilege
//					  {
//						  ID=EmployeeId
//					  }
//					}
//				}));

//			var resposne = await _controller.Get(It.IsAny<Guid>());

//			var okResult = resposne as OkObjectResult;

//			// Assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}


//		[Theory]
//		[InlineData(false, false)]
//		[InlineData(true, true)]
//		public async Task POST_ROLES_TEST(bool badRequest, bool hasError)
//		{
//			var result = new Result<Role>();

//			_ = !hasError
//				? _roleService.Setup(x => x.AddAsync(It.IsAny<Role>()))
//			 .Callback(() =>
//			 {
//				 result.IsSuccess = true;
//				 result.ReturnValue = new Role { ID = EmployeeId };
//			 }).ReturnsAsync(result)
//				:
//				 _roleService.Setup(x => x.AddAsync(It.IsAny<Role>()))
//			   .Callback(() =>
//			   {
//				   result.AddMessageItem(new MessageItem("Error"));
//				   result.ReturnValue = null;
//			   }).ReturnsAsync(result);

//			// Act
//			var resposne = await _controller.Post(SetData.RoleRequest());

//			ADD_CODE(resposne, badRequest);
//		}



//		[Theory]
//		[InlineData(false, false)]
//		[InlineData(true, true)]
//		public async Task PUT_ROLES_TEST(bool badRequest, bool hasError)
//		{

//			var result = new Result<Role>();

//			_ = !hasError
//				? _roleService.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
//			 .Callback(() =>
//			 {
//				 result.IsSuccess = true;
//				 result.ReturnValue = new Role { ID = EmployeeId };
//			 }).ReturnsAsync(result)
//				:
//				_roleService.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
//			   .Callback(() =>
//			   {
//				   result.AddMessageItem(new MessageItem("hello"));
//				   result.ReturnValue = null;
//			   }).ReturnsAsync(result);


//			// Act
//			var resposne = await _controller.Put(SetData.RoleRequest());

//			ADD_CODE(resposne, badRequest);
//		}



//		private void ADD_CODE(dynamic resposne, bool hasBadRequest)
//		{
//			var okResult = resposne as OkObjectResult;
//			if (hasBadRequest)
//			{
//				Assert.Null(okResult);
//				Assert.Equal(400, resposne.StatusCode);
//			}
//			else
//			{
//				Assert.NotNull(okResult);
//				Assert.Equal(200, okResult.StatusCode);
//			}

//		}

//		[Fact]
//		public async void GET_PAGES_TEST()
//		{
//			var pages = new List<TranSmart.Domain.Entities.Page>
//			{
//				new TranSmart.Domain.Entities.Page
//				{
//					DisplayName =" My Claims", DisplayOrder=1,Module ="Claims",Name ="MyClaims",Privilege=15
//				},
//				 new TranSmart.Domain.Entities.Page
//				{
//					DisplayName ="Audit", DisplayOrder=1,Module ="Claims",Name ="Audit",Privilege=15
//				}
//			};
//			_ = _roleService.Setup(x => x.Pages()).ReturnsAsync(pages);

//			var response = await _controller.GetPages();
//			var okResult = response as OkObjectResult;

//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(2, ((List<Page>)okResult.Value).Count);
//		}



//		[Fact]
//		public async Task GET_REPORTS_TEST()
//		{
//			var reports = new List<TranSmart.Domain.Entities.Report>
//			{
//				new TranSmart.Domain.Entities.Report
//				{
//					Label =" My Claims", DisplayOrder=1,Name ="MyClaims",
//				},
//				 new TranSmart.Domain.Entities.Report
//				{
//					Label ="Audit", DisplayOrder=1,Name ="Audit",
//				}
//			};

//			_ = _appReportService.Setup(x => x.GetReports()).ReturnsAsync(reports);

//			var response = await _controller.GetReports();
//			var okResult = response as OkObjectResult;

//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(2, ((List<RoleReportPrivilegeModel>)okResult.Value).Count);
//		}





//		[Fact]
//		public async Task GET_MENU_TEST()
//		{
//			var menu = new List<RolePrivilegeCache>
//			{
//				new RolePrivilegeCache {GroupName ="Settings",PageName ="Carrirer",Privilege=15,RoleId = EmployeeId},
//				new RolePrivilegeCache {GroupName ="Settings",PageName ="Client",Privilege=15,RoleId = EmployeeId},
//				new RolePrivilegeCache {GroupName ="Reports",PageName ="Summary",Privilege=15,RoleId = EmployeeId}
//			};

//			_ = _cacheService.Setup(x => x.GetRoleMenu(It.IsAny<Guid>())).ReturnsAsync(menu);
//			_ = _controller.GetType().GetMethod("GetMenu");
//			var response = await _controller.GetMenu(Guid.NewGuid());

//			var okResult = response as OkObjectResult;
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal(3, ((List<RolePrivilegeCache>)okResult.Value).Count);
//		}

//		[Fact]
//		public async Task GET_MENU_CONTENT__TEST()
//		{
//			var menu = new Role
//			{
//				ID = EmployeeId,
//				Menu = "Settings"
//			};

//			_ = _roleService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(menu);
//			var response = await _controller.GetMenuContent(Guid.NewGuid());

//			var okResult = response as OkObjectResult;
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//			Assert.Equal("Settings", (okResult.Value));
//		}



//		[Fact]
//		public async void GetModulesTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.GetModules());

//			// Act
//			var response = await _controller.GetModules();
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}

//		[Fact]
//		public async void GetReportTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.GetReport(It.IsAny<Guid>()));

//			// Act
//			var response = await _controller.GetReport(It.IsAny<Guid>());
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}



//		[Fact]
//		public async void GetAllRollesTest()
//		{
//			// Arrange
//			_roleService.Setup(x => x.Roles());

//			// Act
//			var response = await _controller.GetAllRolles();
//			var okResult = response as OkObjectResult;

//			//assert
//			Assert.NotNull(okResult);
//			Assert.Equal(200, okResult.StatusCode);
//		}
//	}
//}
