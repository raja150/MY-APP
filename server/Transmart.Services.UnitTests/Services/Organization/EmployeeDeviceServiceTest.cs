using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class EmployeeDeviceServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IEmployeeDeviceService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employee;

		public EmployeeDeviceServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new EmployeeDeviceService(uow.Object);
			_context = new Mock<DbContext>();
			_employee = new EmployeeDataGenerator();

		}
		[Fact]
		public async Task Get_GetEmployeeDeviceWithId_GetValidRecords()
		{
			//Arrange
			var id = Guid.NewGuid();
			var employee = _employee.GetEmployeeData();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = id,
					EmployeeId = employee.ID,
					MobileNumber ="9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber ="8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			//Act
			var employeeDevice = await _service.GetById(id);

			//Assert
			Assert.Equal(id, employeeDevice.ID);
			Assert.Equal(employee.ID, employeeDevice.EmployeeId);
		}

		[Fact]
		public async Task GetPaginate_PaginateList_GetData()
		{
			//Arrange
			var employee = _employee.GetEmployeeData();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					MobileNumber ="9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber ="8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName ="Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn =DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			//Act
			var employeeDevice = await _service.GetPaginate(new BaseSearch());

			//Assert
			Assert.Equal(2, employeeDevice.Count);
		}

		[Fact]
		public void CustomValidation_WithoutException_Test()
		{
			// Arrange
			var result = new Result<EmployeeDevice>();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			var item = new EmployeeDevice()
			{
				ID = Guid.NewGuid(),
				EmployeeId = Guid.NewGuid(),
				MobileNumber = "8796543212",
				ComputerType = (int)ComputerType.OfficeDesktop,
				HostName = "Host",
				IsK7Installed = false,
				IsActZeroInstalled = false,
				IsUninstalled = true,
				UninstalledOn = DateTime.Today,
			};

			//Act
			_ = _service.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public void CustomValidation_InstalledOnIsRequired_Exception()
		{
			// Arrange
			var result = new Result<EmployeeDevice>();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			var item = new EmployeeDevice()
			{
				ID = Guid.NewGuid(),
				EmployeeId = Guid.NewGuid(),
				MobileNumber = "8796543212",
				ComputerType = (int)ComputerType.OfficeDesktop,
				HostName = "Host",
				IsK7Installed = true,
			};

			//Act
			_ = _service.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void CustomValidation_UninstalledOnIsRequired_Exception()
		{
			// Arrange
			var result = new Result<EmployeeDevice>();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			var item = new EmployeeDevice()
			{
				ID = Guid.NewGuid(),
				EmployeeId = Guid.NewGuid(),
				MobileNumber = "8796543212",
				ComputerType = (int)ComputerType.OfficeDesktop,
				HostName = "Host",
				IsUninstalled = true,
			};

			//Act
			_ = _service.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public void CustomValidation_EmployeeAlreadyExists_Warning()
		{
			// Arrange
			var result = new Result<EmployeeDevice>();
			var employee = _employee.GetEmployeeData();
			var employeeDevices = new List<EmployeeDevice>()
			{
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					MobileNumber = "9876543212",
					ComputerType = (int)ComputerType.OfficeLaptop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				},
				new EmployeeDevice
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					MobileNumber = "8796543212",
					ComputerType = (int)ComputerType.OfficeDesktop,
					HostName = "Host",
					IsK7Installed = true,
					IsActZeroInstalled = true,
					InstalledOn = DateTime.Today,
				}
			};

			//Mock
			var mockSet = employeeDevices.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDevice(uow, _context, mockSet);

			var item = new EmployeeDevice()
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				MobileNumber = "9876543212",
				ComputerType = (int)ComputerType.OfficeLaptop,
				HostName = "Host",
			};

			//Act
			_ = _service.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}

	}
}
