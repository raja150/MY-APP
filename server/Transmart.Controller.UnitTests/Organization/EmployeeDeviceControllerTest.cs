using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class EmployeeDeviceControllerTest : ControllerTestBase
	{
		private readonly Mock<IEmployeeDeviceService> _service;

		private readonly EmployeeDeviceController _controller;
		public EmployeeDeviceControllerTest() : base()
		{
			_service = new Mock<IEmployeeDeviceService>();
			_controller = new EmployeeDeviceController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Fact]
		public async Task EmployeeDevice_Paginate_Test()
		{
			// Arrange  
			var list = new List<EmployeeDevice>
			{
				new EmployeeDevice
				{
					EmployeeId = EmployeeId,
					MobileNumber ="9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				},
				new EmployeeDevice
				{
					EmployeeId = Guid.NewGuid(),
					MobileNumber ="8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				}
			}.AsQueryable();

			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			// Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Single(((TranSmart.API.Models.Paginate<EmployeeDeviceModel>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId));
		}

		[Fact]
		public async Task EmployeeDevice_GetById_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new EmployeeDevice { ID = id, EmployeeId = EmployeeId }));

			// Act
			var response = await _controller.Get(id);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(id, ((EmployeeDeviceModel)okResult.Value).ID);
			Assert.Equal(EmployeeId, ((EmployeeDeviceModel)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task EmployeeDevice_Post_AddDuplicate_False()
		{
			// Arrange  
			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.AddAsync(It.IsAny<EmployeeDevice>())).Callback((EmployeeDevice employeeDevice) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeeDevice { ID = Guid.NewGuid(), EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			// Act
			var response = await _controller.Post(new EmployeeDeviceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeeDeviceModel).EmployeeId);
		}

		[Fact]
		public async Task EmployeeDevice_Post_AddDuplicate_True()
		{
			// Arrange  
			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.AddOnlyAsync(It.IsAny<EmployeeDevice>())).Callback((EmployeeDevice employeeDevice) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeeDevice { ID = Guid.NewGuid(), EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			// Act
			var response = await _controller.Post(
				new EmployeeDeviceModel
				{
					ID = Guid.NewGuid(),
					EmployeeId = EmployeeId,
					addDuplicate = true
				});
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeeDeviceModel).EmployeeId);
		}

		[Fact]
		public async Task EmployeeDevice_Put_AddDuplicate_False()
		{
			// Arrange
			var id = Guid.NewGuid();
			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<EmployeeDevice>())).Callback((EmployeeDevice employeeDevice) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeeDevice { ID = id, EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			// Act
			var response = await _controller.Put(new EmployeeDeviceModel { ID = id, EmployeeId = EmployeeId });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(id, (okResult.Value as EmployeeDeviceModel).ID);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeeDeviceModel).EmployeeId);
		}

		[Fact]
		public async Task EmployeeDevice_Put_AddDuplicate_True()
		{
			// Arrange
			var id = Guid.NewGuid();
			var result = new Result<EmployeeDevice>();
			_service.Setup(x => x.UpdateOnlyAsync(It.IsAny<EmployeeDevice>())).Callback((EmployeeDevice employeeDevice) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeeDevice
				{
					ID = id,
					EmployeeId = EmployeeId,
					IsUninstalled = true,
					UninstalledOn = DateTime.Today,
				};

			}).ReturnsAsync(result);

			// Act
			var response = await _controller.Put(
				new EmployeeDeviceModel
				{
					ID = id,
					EmployeeId = EmployeeId,
					addDuplicate = true,
					IsUninstalled = true,
					UninstalledOn = DateTime.Now,
				});
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(id, (okResult.Value as EmployeeDeviceModel).ID);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeeDeviceModel).EmployeeId);
			Assert.Equal(DateTime.Today, (okResult.Value as EmployeeDeviceModel).UninstalledOn);
		}
	}
}
