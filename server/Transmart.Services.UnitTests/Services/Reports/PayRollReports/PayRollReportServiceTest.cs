using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.Reports;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Service.Reports;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Reports.PayRollReports
{
    public class PayRollReportServiceTest
    {
        private readonly Mock<TranSmartContext> _dbContext;
        private readonly Mock<IUnitOfWork<TranSmartContext>> uow;
        private readonly EmployeeDataGenerator _employeeData;
        private readonly PayRollReportService _reportService;
        private readonly EmployeeStatutoryRepository _empStatutoryReportService;
		private readonly Mock<DbContext> _context;
        public PayRollReportServiceTest()
        {
            var builder = new DbContextOptionsBuilder<TranSmartContext>();
            var app = new Mock<IApplicationUser>();
            var dbContextOptions = builder.Options;
            _dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
            uow = new Mock<IUnitOfWork<TranSmartContext>>();
			_empStatutoryReportService = new EmployeeStatutoryRepository(_dbContext.Object);
            _reportService = new PayRollReportService(uow.Object, _empStatutoryReportService);
			_employeeData = new EmployeeDataGenerator();
            _context = new Mock<DbContext>();
        }
        [Fact]
        public void EmployeeESI_Report()
        {
            #region Arrenge
            var employee = _employeeData.GetEmployeeData();
            IEnumerable<EmpStatutory> statutories = new List<EmpStatutory>
            {
                new EmpStatutory
                {
                    EmpId = employee.ID,
                    Emp = employee,
                    EnableESI=1,
                    EnablePF = 0,
                    ESINo="22"
                }
            };
            IEnumerable<PaySheet> paySheets = new List<PaySheet>
            {
                new PaySheet
                {
                    ESI = 10000,
                    Gross = 40000,
                    LOPDays = 0,
                    PayMonthId = Guid.NewGuid(),
                    PayMonth = new PayMonth{Month= 2,Year=2022}
                }
            };
            #endregion
            // Mock
            var mockEmpStatutories = statutories.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<EmpStatutory>()).Returns(mockEmpStatutories.Object);
            var repository = new RepositoryAsync<EmpStatutory>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<EmpStatutory>()).Returns(repository);

            var mockPaySheet = paySheets.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<PaySheet>()).Returns(mockPaySheet.Object);
            var paySheetRepository = new RepositoryAsync<PaySheet>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(paySheetRepository);

            // Act
            var dd = _reportService.EmployeeESI(employee.DepartmentId, employee.DesignationId, employee.TeamId,employee.ID, 2022, 2);
            // Assert
            Assert.True(dd.Result.Count() == 1);
        }

        [Fact]
        public async Task EmployeeEPF_Report()
        {
            // Arrenge
            var employee = _employeeData.GetEmployeeData();
            IEnumerable<EmpStatutory> statutories = new List<EmpStatutory>
            {
                new EmpStatutory
                {
                    EmpId = employee.ID,
                    Emp = employee,
                    EnableESI=0,
                    EnablePF = 1,
                    ESINo="22",
                    EmployeeContrib = 10000,
                    UAN = "UAN-22"
                }
            };
            IEnumerable<PaySheet> paySheets = new List<PaySheet>
            {
                new PaySheet
                {
                    ESI = 10000,
                    Gross = 40000,
                    LOPDays = 0,
                    PayMonthId = Guid.NewGuid(),
                    PayMonth = new PayMonth{Month= 2,Year=2022}
                }
            };
            // Mock
            var mockEmpStatutories = statutories.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<EmpStatutory>()).Returns(mockEmpStatutories.Object);
            var repository = new RepositoryAsync<EmpStatutory>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<EmpStatutory>()).Returns(repository);

            var mockPaySheet = paySheets.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<PaySheet>()).Returns(mockPaySheet.Object);
            var paySheetRepository = new RepositoryAsync<PaySheet>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(paySheetRepository);

            // Act
            var dd = await _reportService.EmployeeEPF(employee.DepartmentId, employee.DesignationId, employee.TeamId, employee.ID, 2022, 2);
			//Assert
			Assert.True(dd.Count() == 1);
        }

        [Fact]
        public async Task IncentivesPayCuts_Report()
        {
			// Arrenge
			int month = 2; int year = 2022;
            var employee = _employeeData.GetEmployeeData();
            IEnumerable<IncentivesPayCut> IncentivesPayCuts = new List<IncentivesPayCut>
            {
                new IncentivesPayCut
                {
                    EmployeeId = employee.ID,
                    Employee = employee,
                    Month = 2,
                    Year = 2022,
                }
            };
            // Mock
            var mockIncentivesPayCut = IncentivesPayCuts.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<IncentivesPayCut>()).Returns(mockIncentivesPayCut.Object);
            var repository = new RepositoryAsync<IncentivesPayCut>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<IncentivesPayCut>()).Returns(repository);
            
            // Act
            var dd = await _reportService.IncentivesPayCuts(employee.DepartmentId, employee.DesignationId, employee.TeamId, employee.ID, year, month);
			//Assert
			Assert.True(dd.Count() == 1);
        }

        [Fact]
        public async Task Arrears_Report()
        {
			// Arrenge
			int month = 2; int year = 2022;
			var employee = _employeeData.GetEmployeeData();
            IEnumerable<Arrear> arrears = new List<Arrear>
            {
                new Arrear
                {
                    EmployeeID = employee.ID,
                    Employee = employee,
                    Month = 2,
                    Year = 2022,
                    Pay = 1000
                }
            };
            // Mock
            var mockArrears = arrears.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<Arrear>()).Returns(mockArrears.Object);
            var repository = new RepositoryAsync<Arrear>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<Arrear>()).Returns(repository);

            // Act
            var dd = await _reportService.Arrears(employee.DepartmentId, employee.DesignationId, employee.TeamId, employee.ID, year, month);
			// Assert
			Assert.True(dd.Count() == 1);
        }

        [Fact]
        public async Task GetProfessionalTax_Report()
        {
			// Arrenge
			int month = 2; int year = 2022;
			var employee = _employeeData.GetEmployeeData();
            IEnumerable<PaySheet> paySheets = new List<PaySheet>
            {
                new PaySheet
                {
                    ESI = 10000,
                    Gross = 40000,
                    LOPDays = 0,
                    PayMonthId = Guid.NewGuid(),
                    PayMonth = new PayMonth{Month= 2,Year=2022},
                    Tax= 1000,
                    EmployeeID= employee.ID,
                    Employee = employee,
                    PTax =100
                }
            };
            // Mock
            var mockPaySheets = paySheets.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<PaySheet>()).Returns(mockPaySheets.Object);
            var repository = new RepositoryAsync<PaySheet>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repository);
            
            // Act
            var dd = await _reportService.GetProfessionalTax(employee.DepartmentId, employee.DesignationId, employee.TeamId, employee.ID, year, month);
            // Assert
            Assert.True(dd.Count() == 1);
        }

        [Fact]
        public async Task GetIncomeTax_Report()
        {
			// Arrenge
			int month = 2; int year = 2022;
			var employee = _employeeData.GetEmployeeData();
            IEnumerable<PaySheet> paySheets = new List<PaySheet>
            {
                new PaySheet
                {
                    ESI = 10000,
                    Gross = 40000,
                    LOPDays = 0,
                    PayMonthId = Guid.NewGuid(),
                    PayMonth = new PayMonth{Month= 2,Year=2022},
                    Tax= 1000,
                    EmployeeID= employee.ID,
                    Employee = employee,
                    PTax =100
                }
            };
            // Mock
            var mockPaySheets = paySheets.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<PaySheet>()).Returns(mockPaySheets.Object);
            var repository = new RepositoryAsync<PaySheet>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repository);
            
            // Act
            var dd = await _reportService.GetIncomeTax(employee.DepartmentId, employee.DesignationId, employee.TeamId, employee.ID, year, month);
            // Assert
            Assert.True(dd.Count() == 1);
        }

        [Fact]
        public async Task GetPaymentInfo_Report()
        {
			// Arrenge
			var employee = _employeeData.GetEmployeeData();
            IEnumerable<EmployeePayInfoAudit> payInfoAudits = new List<EmployeePayInfoAudit>
            {
                new EmployeePayInfoAudit
                {
                    EmployeeId= employee.ID,
                    Employee = employee,
                    Bank = "SBI",
                    AccountNo = "123412341234",
                    PayType="Cash"
                }
            };
           // Mock
            var mockPaySheets = payInfoAudits.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Set<EmployeePayInfoAudit>()).Returns(mockPaySheets.Object);
            var repository = new RepositoryAsync<EmployeePayInfoAudit>(_context.Object);
            uow.Setup(m => m.GetRepositoryAsync<EmployeePayInfoAudit>()).Returns(repository);

			// Act
            var dd = await _reportService.GetPaymentInfo(null, null, null, null);
            //Assert
            Assert.True(dd.Count() == 1);
        }
    }
}
