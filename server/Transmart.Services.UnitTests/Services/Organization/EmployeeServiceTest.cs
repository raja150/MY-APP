using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class EmployeeServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IEmployeeService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employee;
		public EmployeeServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new EmployeeService(uow.Object);
			_context = new Mock<DbContext>();
			_employee = new EmployeeDataGenerator();
		}

		private void MockEmployeeData()
		{
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);
		}

		[Fact]
		public async Task GetEmp_SpecificEmployee_ReturnEmployeeData()
		{
			// Arrange && Mock
			MockEmployeeData();

			var employeeId = _employee.GetEmployeeData().ID;

			//Act
			var dd = await _service.GetEmp(employeeId);

			//Assert
			Assert.Equal(employeeId, dd.ID);
		}



		  
		[Fact]
		public void GetDetails_SpecificEmployee_ReturnEmployeeData()
		{
			// Arrange && Mock
			MockEmployeeData();

			var employee = _employee.GetEmployeeData();

			//Act
			var dd = _service.GetDetails(employee.ID).Result;

			//Assert
			Assert.Equal(employee.ID, dd.ID);
			Assert.Equal(employee.DepartmentId, dd.DepartmentId);
			Assert.Equal(employee.DesignationId, dd.DesignationId);
			Assert.Equal(employee.ReportingToId , dd.ReportingToId);
			Assert.Equal(employee.TeamId, dd.TeamId);
			Assert.Equal(employee.WorkLocationId , dd.WorkLocationId);
			Assert.Equal(employee.WorkTypeId , dd.WorkTypeId);
		}




		[Fact]
		public void GetResignationDetails_SpecificEmployee_ReturnResignationDetails()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<EmployeeResignation>
			{
				new EmployeeResignation
				{
					ID = Guid.NewGuid(),
					EmployeeId = employeeId
				},
				new EmployeeResignation
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid()
				}
			};

			var mockEmployeeResignation = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeResignation(uow, _context, mockEmployeeResignation);

			//Act
			var dd = _service.GetResignationDetails(employeeId).Result;

			//Assert
			Assert.Equal(employeeId, dd.EmployeeId);
		}



		[Fact]
		public async Task GetContactDetails_SpecificEmployee_ReturnContactDetailsList()
		{
			//Arrange  
			var employeeId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<EmployeeFamily>
			{
				new EmployeeFamily
				{
					EmployeeId = employeeId
				},
				new EmployeeFamily
				{
					EmployeeId = employeeId
				},
				new EmployeeFamily
				{
					EmployeeId = Guid.NewGuid()
				}
			};

			// Mock
			var mockEmployeeFamily = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeFamily(uow, _context, mockEmployeeFamily);

			//Act
			var dd = await _service.GetContactDetails(employeeId);

			//Assert
			Assert.Equal(2, dd.Count());
		}

		
		[Fact]
		public void GetPresentAddress_SpecificEmployee_ReturnPresentAddressData()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<EmployeePresentAd>
			{
				new EmployeePresentAd
				{
					EmployeeId = employeeId
				},
				new EmployeePresentAd
				{
					EmployeeId = Guid.NewGuid()
				}
			};

			var mockEmployeePresentAd = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePresentAd(uow, _context, mockEmployeePresentAd);

			//Act
			var dd = _service.GetPresentAddress(employeeId).Result;

			//Assert
			Assert.Equal(employeeId, dd.EmployeeId);
		}


		[Fact]
		public void GetPermanentAd_SpecificEmployee_ReturnPermanentAddress()
		{
			//Arrange & Mock
			var employeeId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<EmployeePermanentAd>
			{
				new EmployeePermanentAd
				{
				   EmployeeId = employeeId
				},
				new EmployeePermanentAd
				{
				   EmployeeId = Guid.NewGuid()
				}
			};

			var mockEmployeePermanentAd = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePermanentAd(uow, _context, mockEmployeePermanentAd);


			//Act
			var dd = _service.GetPermanentAddress(employeeId).Result;

			//Assert
			Assert.Equal(employeeId, dd.EmployeeId);
		}




		[Fact]
		public void GetEmergencyAddress_SpecificEmployee_ReturnEmergencyAddress()
		{
			// Arrange &  Mock
			var employeeId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var data = new List<EmployeeEmergencyAd>
			{
				new EmployeeEmergencyAd
				{
					EmployeeId = employeeId
				},
				new EmployeeEmergencyAd
				{
					EmployeeId = Guid.NewGuid()
				}
			};
			var mockEmployeeEmergencyAd = data.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeEmergencyAd(uow, _context, mockEmployeeEmergencyAd);

			//Act
			var dd = _service.GetEmergencyAddress(employeeId).Result;

			//Assert
			Assert.Equal(employeeId, dd.EmployeeId);
		}



		[Fact]
		public async Task SearchEmp_ExistName_ReturnEmployee()
		{
			// Arrange & Mock
			var employees = _employee.GetAllEmployeesData();
			
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var result = await _service.SearchEmp("Shiva", Guid.NewGuid(),Guid.NewGuid());

			//Assert
			Assert.Equal(employees.Count(x=>x.Name=="Shiva"),result.Count());
		}




		[Fact]
		public async Task SearchEmp_SpecificNoNameMbl_ReturnEmployee()
		{
			// Arrange & Mock
			var employees = _employee.GetAllEmployeesData();
		
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var result = await _service.SearchEmp("AVONTIX1822");

			//Assert
			Assert.Equal(employees.Count(x=>x.No== "AVONTIX1822"),result.Count());
		}


		

		[Fact]
		public void  CustomValidation_ValidData_NoException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682354",
				AadhaarNumber = "307630763076",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasNoError);
		}




		[Fact]  
		public void CustomValidationTest_InvalidPANFormat_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "2gfyu",
				MobileNumber = "9866682334",
				AadhaarNumber = "343450752386",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}





		

		[Fact]  
		public void  CustomValidation_ExistPAN_ThrowException()
		{
			//Arrange & Mock
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			Employee item = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "BLMPJ2797R",
				MobileNumber = "9866682334",
				AadhaarNumber = "461250752386",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(item, result);

			//Assert
			Assert.True(result.HasError);
		}






		[Fact] 
		public void  CustomValidation_NonNumericMobileFormat_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "fjfgj",
				AadhaarNumber = "307630763076",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}






		[Fact]  
		public void  CustomValidation_ExistMobileNumber_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9639639632",
				AadhaarNumber = "307630763076",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}




		[Fact]   
		public void  CustomValidation_ExistAadharNumber_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682334",
				AadhaarNumber = "561250752387",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}




		[Fact]   
		public void  CustomValidation_InvalidWorkMailFormat_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682334",
				AadhaarNumber = "661250752387",
				WorkEmail = "shivagfffmail.com",
				PersonalEmail = "arjun@gmail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}



		[Fact]  
		public void  CustomValidation_InvalidPersonalMailFormat_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682334",
				AadhaarNumber = "661250752387",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "arjail.com",
				PassportNumber = "P9703964",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}





		[Fact]  
		public  void CustomValidation_InvalidPassportFormat_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682334",
				AadhaarNumber = "661250752387",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "anjali@gmail.com",
				PassportNumber = "55",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2022-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}




		[Fact] 
		public void  CustomValidation_NoSpecificGapBtwDOBAndDOJ_ThrowException()
		{
			//Arrange
			Employee test = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				PanNumber = "PEVFV4506E",
				MobileNumber = "9866682334",
				AadhaarNumber = "661250752387",
				WorkEmail = "shiva@gmail.com",
				PersonalEmail = "anjali@gmail.com",
				PassportNumber = "55",
				DateOfBirth = DateTime.Parse("2000-08-08"),
				DateOfJoining = DateTime.Parse("2010-08-01")
			};

			var employees = _employee.GetAllEmployeesData();

			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var src = new EmployeeService(uow.Object);
			var result = new Result<Employee>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}



		[Fact]
		public void GetEmps_SpecificStartDate_ReturnEmployeesList()
		{
			// Arrange &  Mock
			var employees = new List<Employee>
			{
				new Employee
				{
					LastWorkingDate = DateTime.Parse("2022-05-05")
				},
				new Employee
				{
					LastWorkingDate = DateTime.Parse("2022-06-06")
				},
				new Employee
				{
					LastWorkingDate = DateTime.Parse("2022-01-01")
				}
			};

			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var dd = _service.GetEmps(DateTime.Parse("2022-04-04")).Result;

			//Assert
			Assert.Equal(2, dd.ToList().Count);
		}

		


		[Fact]
		public async Task GetBirthdays()
		{
			// Arrange 
			var departmentId = Guid.Parse("96364543-013f-45ab-96a8-e8b273cf11ce");
			var employees = new List<Employee>
			{
				new Employee
				{
					ID = Guid.NewGuid(),
					DateOfBirth = DateTime.Parse("2000-04-04"),
					DepartmentId = departmentId
				},
				new Employee
				{
					ID = Guid.NewGuid(),
					DateOfBirth = DateTime.Parse("2000-06-06"),
					DepartmentId = departmentId
				}
			};
			//Mock
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var dd = await _service.GetBirthdays(departmentId);

			//Assert
			Assert.True(dd.Count() == 0);
		}



		[Fact]   
		public async Task LeavesApprovedEmployees_ApprovedStatus_ReturnApprovedEmployeesList()
		{
			// Arrange 
			var departmentId = Guid.NewGuid();
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					Employee = new Employee
					{
						DepartmentId = departmentId
					},
					FromDate = DateTime.Now.AddDays(-2),
					ToDate = DateTime.Now.AddDays(4),
					Status = 2
				},
				new ApplyLeave
				{
					Employee = new Employee
					{
						DepartmentId = departmentId
					},
					FromDate = DateTime.Now.AddDays(-1),
					ToDate = DateTime.Now.AddDays(2),
					Status = 2
				},
				new ApplyLeave
				{
					Employee = new Employee
					{
						DepartmentId = departmentId
					},
					FromDate =DateTime.Now.AddDays(2),
					ToDate =DateTime.Now.AddDays(4),
					Status = 1
				}
			};

			// Mock
			var mockApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);
			
			//Act
			var dd = await _service.LeavesApprovedEmployees(departmentId);

			//Assert
			Assert.Equal(2, dd.ToList().Count);
		}


		

		[Fact] 
		public async Task ApprovedPendingEmployees_StatusPending_ReturnPendingEmployeesList()
		{
			//Arrange  &  Mock
			var id = Guid.Parse("6e39634a-ac45-45ce-81fa-58484ff39f57");
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					Employee = new Employee
					{
						ReportingToId = id
					},
					Status = 1
				},
				new ApplyLeave
				{
					Employee = new Employee
					{
						ReportingToId = id
					},
					Status = 1
				},
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					EmployeeId = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						ReportingToId = id
					},
					Status = 2
				}
			};

			var mockApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);

			//Act
			var dd = await _service.ApprovedPendingEmployees(id);

			//Assert
			Assert.Equal(2, dd.ToList().Count);
		}




		[Fact]  
		public async Task ApprovedPendingWFHEmployees_StatusPending_ReturnPendingWFHEmployeesList()
		{
			//Arrange  &  Mock
			var id = Guid.Parse("6e39634a-ac45-45ce-81fa-58484ff39f57");
			var applyWFH = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					Employee = new Employee
					{
						ReportingToId = id
					},
					Status = 1
				},
				new ApplyWfh
				{
					Employee = new Employee
					{
						ReportingToId = id
					},
					Status = 1
				},
				new ApplyWfh
				{
					Employee = new Employee
					{
						ReportingToId = Guid.NewGuid()
					},
					Status = 1
				},
				new ApplyWfh
				{
					Employee = new Employee
					{
						ReportingToId = id
					},
					Status = 2
				}
			};

			var mockApplyWFH = applyWFH.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyWfh>()).Returns(mockApplyWFH.Object);
			var repository = new RepositoryAsync<ApplyWfh>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyWfh>()).Returns(repository);

			//Act
			var dd = await _service.ApprovedPendingWFHEmployees(id);

			//Assert
			Assert.Equal(2, dd.ToList().Count);
		}



		

		[Fact]
		public void GetLoginEmpMail_SpecificEmployee_ReturnLoginEmployeeMail()
		{
			// Arrange && Mock
			MockEmployeeData();
			var employeeId = _employee.GetEmployeeData().ID;

			//Act
			var dd = _service.GetLoginEmpMail(employeeId).Result;

			//Assert
			Assert.Equal(employeeId, dd.ID);
		}




		[Fact]
		public async Task AddEmployee_NewEmployee_NoException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<Employee>
			{
				new Employee
				{
					No = "Avontix2065"
				}
			};
			var replication = new List<Replication> { new Replication { DepartmentId = Guid.NewGuid(),RefId = Guid.NewGuid()} };
			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var mockReplicatiom = replication.AsQueryable().BuildMockDbSet();
			SetData.MockReplication(uow, _context, mockReplicatiom);

			var test = new List<Employee>
			{
				new Employee
				{
					No = "Avontix2069"
				}
			};

			//Act
			var result = await _service.AddEmployee(test);

			//Assert
			Assert.True(result.HasNoError);
		}


		[Fact]
		public async Task AddEmployee_ThrowException()
		{
			// Arrange & Mock
		    var mockToDatabase = new List<Employee>{};
			
			var mockEmployee = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var employee = new List<Employee> { };

			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

			//Act
			var result = await _service.AddEmployee(employee);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
