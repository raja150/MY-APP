using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.LM_Attendance
{
	public class CalculateAttendanceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly CalculateAttendance calculateAttendance;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<IScheduleService> _schService;
		private readonly Guid lTypeId = Guid.NewGuid();
		private readonly Guid empId = Guid.NewGuid();
		private readonly Guid unPaidLvTypeId = Guid.NewGuid();
		public CalculateAttendanceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);

			_employeeData = new EmployeeDataGenerator();
			_schService = new Mock<IScheduleService>();
			calculateAttendance = new CalculateAttendance(uow.Object, _schService.Object)
			{
				Attendances = new List<Attendance>
				{
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date.AddDays(-1), AttendanceStatus = (int)AttendanceStatus.Present },
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date, AttendanceStatus = (int)AttendanceStatus.Absent },

				},
				Exemption = new Exemptions
				{
					ID = Guid.NewGuid(),
					Employees = "80ccbc50-ebf9-4654-9160-c36201d1783c",
					Teams = "1524b506-a8c0-4bda-a085-ea2811d82b50",
					Department = "a9cc5e1b-e24f-4939-b47d-1b86e583afc7",
					Designations = "05adb896-81cf-4323-9898-a8db16ca0a20",
					Location = "8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"
				},
				Isholiday = new Holidays { ID = Guid.NewGuid(), Name = "Ganesh chathurthi", ReprApplication = "1" },
				WFH = new List<ApplyWfh>
				{
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Today.Date,
						ToDateC = DateTime.Today.Date,
						ReasonForWFH = "dfgf",
						Status = 2,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Parse("2022-07-15"),
						ToDateC = DateTime.Parse("2022-07-15"),
						ReasonForWFH = "dfgf",
						Status = 1,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						ToDateC = DateTime.Parse("2022-07-30"),
						FromDateC = DateTime.Parse("2022-08-04"),
						Status=2,
						EmployeeId = empId
					}
				},
				Allocations = new List<Allocation>
				{
					new Allocation
					{
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						ID = Guid.NewGuid(),
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
						WorkHoursSetting = new WorkHoursSetting{ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3") , Name = "WorkHour"}
					},
					new Allocation
					{
						EmployeeId = Guid.NewGuid(),
						ID = Guid.NewGuid(),
					}

				},
				WeekOffDays = new List<WeekOffDays>
				{
					new WeekOffDays
					{
						WeekOffSetupId=Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekDate=DateTime.Now,
						Type=3,
						WeekDay=5,
						WeekNoInMonth="4",
						WeekInYear=1,
						Status=2
					}
				},
				LeavesSettings = new LeaveSettings()
				{
					HourCalculation = 1,
					FullDayHours = 7,
					HalfDayHours = 4
				},
				Leaves = new List<ApplyLeaveDetails>
				{
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date,
						   ToDate = DateTime.Now.Date,
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Applied,
						},
						LeaveTypeId = lTypeId,
						LeaveCount = 1,
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Applied,
						},
						LeaveTypeId = lTypeId,
						LeaveCount = 0.5m
					}
			   },
				LeaveTypes = new List<LeaveType>
				{
					new LeaveType
					{
						ID= lTypeId,
						PayType = (int)PayTypeStatus.Paid
					}
				}

			};
		}
		private void EmployeesData()
		{
			var employees = new List<Employee>
			{
				new Employee
				{
					ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					No = "AVONTIX1822",
					Name = "Anudeep",
					Gender = 1,
					MaritalStatus = 1,
					Status = 1,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",

					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
						Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer",
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" },
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift
						{
							ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift"
						},
					},
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
					ReportingTo = new Employee { ID = Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"), Name = "Shiva" },
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType" },
					EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
					EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory" },
					Allocation = new Allocation { WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"), },
					AllowWebPunch = true
				},
				new Employee
				{
					ID = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					No = "AVONTIX1823",
					Name = "Vamshi",
					Gender = 1,
					MaritalStatus = 2,
					Status = 1,
					MobileNumber = "9639639633",
					DateOfBirth = new DateTime(1990, 02, 03),
					DateOfJoining = new DateTime(2021, 08, 13),
					AadhaarNumber = "561250752383",
					PanNumber = "BLMPJ2797M",
					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"), Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer" },
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					AllowWebPunch = false
				},
				 new Employee
				{
					ID= Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					No = "AVONTIX1824",
					Name = "Dharmendhar",
					Gender = 1,
					MaritalStatus = 3,
					Status=3,
					MobileNumber = "9639639634",
					DateOfBirth = new DateTime(1991 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752384",
					PanNumber = "BLMPJ2797N",
					DepartmentId = Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),
					Department = new Department{ID=Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),Name = "Transcription"},
					DesignationId = Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"),
					Designation = new Designation{ID=Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"), Name ="Healthcare Documentation"},
					WorkLocationId = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),
					WorkLocation = new Location{ID = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),Name="Tharnaka" },
					TeamId = Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"),
					Team = new Team{ID =Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"), Name="Quicklooks"},
					AllowWebPunch=true
				},
			};
			calculateAttendance.Employees = employees;
		}
		private void EmpData()
		{
			var employee = new List<Employee>() { new Employee {
			} };
		}
		private void AllocationData()
		{
			List<Allocation> allocations = new()
			{
						new Allocation
						{
							EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
							ID = Guid.NewGuid(),
						},
						new Allocation
						{
							EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
							ID = Guid.NewGuid(),
							ShiftId=Guid.NewGuid()
						}

					};

			calculateAttendance.Allocations = allocations;
		}
		private void WeekOffDaysData(int weekInYear, byte status, int type)
		{
			List<WeekOffDays> weekOffDays = new()
			{
				new WeekOffDays
				{
					WeekOffSetupId=Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"),
					WeekDate=DateTime.Now.Date,
					Type=type,
					WeekDay=(int)DateTime.Parse("2022-09-26").DayOfWeek,
					WeekNoInMonth="4",
					WeekInYear=weekInYear,
					Status=status
				},
				new WeekOffDays
				{
					WeekOffSetupId=Guid.NewGuid(),
					WeekDate=DateTime.Parse("2022-08-26"),
					Type=2,
					WeekDay=5,
					WeekInYear=2,
					WeekNoInMonth="4",
					Status=1
				}
			};
			calculateAttendance.WeekOffDays = weekOffDays;
		}

		private static List<BiometricAttLogs> BiometricAttData(Guid employeeId,string employeeNo, DateTime date, TimeSpan inTime, TimeSpan outTime)
		{
			return new List<BiometricAttLogs>
			{
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmployeeId=employeeId,
					EmpCode = employeeNo,
					MovementTime = date.AddMinutes(inTime.TotalMinutes)
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmployeeId=employeeId,
					EmpCode = employeeNo,
					MovementTime = date.AddMinutes(outTime.TotalMinutes)
				}
			};
		}

		private static List<ManualAttLogs> ManualAttLogsData(DateTime date, TimeSpan inTime, TimeSpan outTime)
		{
			return new List<ManualAttLogs>
			{
				 new ManualAttLogs
				 {
					 EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					 AttendanceDate = DateTime.Today,
					 InTime=DateTime.Today.AddHours(9),
					 OutTime=DateTime.Today.AddHours(18)},
				new ManualAttLogs
				{
					EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					AttendanceDate = DateTime.Today,
					InTime= date.AddMinutes(inTime.TotalMinutes),
					OutTime=date.AddMinutes(outTime.TotalMinutes),
				}
			};
		}

		private void LeavesData(string employeeId)
		{
			var applyLeave = new ApplyLeave()
			{
				EmployeeId = Guid.Parse(employeeId),
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied
			};
			List<ApplyLeaveDetails> leaves = new()
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeave=applyLeave,
				},
			};
			calculateAttendance.Leaves = leaves;
		}

		[Theory]
		[InlineData("2022-08-12", 33)]
		[InlineData("2022-01-01", 1)]
		[InlineData("2022-12-31", 53)]
		public void GetWeekNumber_Test(DateTime curentDate, int noOfWeeksUpToCurrentDate)
		{
			var result = CalculateAttendance.GetWeekNumber(curentDate);
			Assert.Equal(noOfWeeksUpToCurrentDate, result);
		}

		[Theory]
		[InlineData("2022-07-15", DayOfWeek.Friday)]
		public void DayIsWeekOff(DateTime curentDate, DayOfWeek dayOfWeek)
		{
			var result = CalculateAttendance.DayIsWeekOff(curentDate, 2, dayOfWeek);
			Assert.Equal(curentDate.AddDays(-7), result.Date);
		}
		[Fact]
		public void DayIsWeekOff_OccuranceMonth_LessThen_AttendanceMonth()
		{
			var currentDate = DateTime.Parse("2022-08-14");
			var result = CalculateAttendance.DayIsWeekOff(currentDate, 2, DayOfWeek.Sunday);
			Assert.Equal(currentDate, result.Date);
		}

		[Fact]
		public void GetAttInAndOutTime_BiometricLogs_EmployeeHasSchedule_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				AttendanceDate = DateTime.Today.Date,
				SchIntime = 9
			};
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 10,
				LoginGraceTime = 8,
				StartAt = int.Parse(new TimeSpan(9, 30, 0).TotalMinutes.ToString()),
				EndsAt = int.Parse(new TimeSpan(18, 30, 0).TotalMinutes.ToString())
			};
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employee.No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(employee.ID,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(employee.ID, empNo, outTime.Date, new TimeSpan(12, 30, 0), new TimeSpan(13, 30, 0)));
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(Guid.NewGuid(), "1234", inTime.Date, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)));

			var response = calculateAttendance.UpdateInAndOutTime(attendance, employee.ID);
			Assert.Equal(inTime, response.InTime);
			Assert.Equal(outTime, response.OutTime);
		}

		[Fact]
		public void GetAttInAndOutTime_BiometricLogs_EmployeeWithOutSchedule_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				AttendanceDate = DateTime.Today.Date,
			};
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 10,
				LoginGraceTime = 8,
				StartAt = int.Parse(new TimeSpan(9, 30, 0).TotalMinutes.ToString()),
				EndsAt = int.Parse(new TimeSpan(18, 30, 0).TotalMinutes.ToString())
			};
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employee.No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(employee.ID,empNo,  inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(employee.ID, empNo, outTime.Date, new TimeSpan(12, 30, 0), new TimeSpan(13, 30, 0)));
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(Guid.NewGuid(),"1234", inTime.Date, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)));

			var response = calculateAttendance.UpdateInAndOutTime(attendance, employee.ID);
			Assert.Equal(inTime, response.InTime);
			Assert.Equal(outTime, response.OutTime);
		}

		[Fact]
		public void GetAttInAndOutTime_ManualAttLogs_Test()
		{

			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				AttendanceDate = DateTime.Today.Date,
				SchIntime = 9
			};
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 10,
				LoginGraceTime = 8,
				StartAt = int.Parse(new TimeSpan(9, 30, 0).TotalMinutes.ToString()),
				EndsAt = int.Parse(new TimeSpan(18, 30, 0).TotalMinutes.ToString())
			};
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);

			calculateAttendance.ManualAttLogs = new List<ManualAttLogs>();
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(Guid.NewGuid(),"1234", inTime.Date, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(outTime.Date, new TimeSpan(12, 30, 0), new TimeSpan(13, 30, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)));

			var response = calculateAttendance.UpdateInAndOutTime(attendance, employee.ID);
			Assert.Equal(inTime, response.InTime);
			Assert.Equal(outTime, response.OutTime);

		}

		[Fact]
		public void GetWFHStatus_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();

			var result = calculateAttendance.GetWFHStatus(employee);
			Assert.True(result!=null);
		}
		[Fact]
		public void GetWorkHours_Allocation_WorkSettingId_NotNull()
		{
			var worksettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3");
			var employee = _employeeData.GetEmployeeData();
			calculateAttendance.Allocations = new List<Allocation> { new Allocation { WorkHoursSettingId = worksettingId,
			EmployeeId = employee.ID } };
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> { new WorkHoursSetting
			{
				ID = worksettingId,Name = "WorkHour",FullDayMinutes = 500
			},
			new WorkHoursSetting{ID = Guid.NewGuid() , Name = "New WorkHour" ,FullDayMinutes = 500 } };
			var result = calculateAttendance.GetWorkHours(employee);
			Assert.True(result.ID == worksettingId);
		}
		[Fact]
		public void GetWorkHours_Team_WorkSettingId_NotNull()
		{
			var worksettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3");
			var employee = _employeeData.GetEmployeeData();
			calculateAttendance.Allocations = new List<Allocation> { new Allocation { ID = Guid.NewGuid(), EmployeeId = Guid.NewGuid() } };
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> { new WorkHoursSetting
			{
				ID = worksettingId,Name = "WorkHour",FullDayMinutes = 500
			} };
			var result = calculateAttendance.GetWorkHours(employee);
			Assert.True(result.ID == employee.Team.WorkHoursSettingId);
		}
		[Fact]
		public void GetWorkHours_Department_WorkSettingId_NotNull()
		{
			var worksettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3");
			var employee = _employeeData.GetEmployeeData();
			employee.Team.WorkHoursSettingId = null;
			calculateAttendance.Allocations = new List<Allocation> { new Allocation { ID = Guid.NewGuid(), EmployeeId = Guid.NewGuid() } };
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> { new WorkHoursSetting
			{
				ID = worksettingId,Name = "WorkHour",FullDayMinutes = 500
			} };
			var result = calculateAttendance.GetWorkHours(employee);
			Assert.True(result.ID == worksettingId);
		}
		[Fact]
		public void GetEmpAttBasedOnWorkHours_FullDay_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
				WorkTime = 540
			};
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> {
				new WorkHoursSetting { ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),Name = "WorkHour",FullDayMinutes = 500 } ,
			new WorkHoursSetting{ ID = Guid.NewGuid() , Name = "New WorkHour" ,FullDayMinutes = 500 } };
			var response = calculateAttendance.AttBasedOnWorkHours(attendance, employee);
			Assert.True(response.Present == 1);
			Assert.True(response.AttendanceStatus == (int)AttendanceStatus.Present);
		}

		[Fact]
		public void GetEmpAttBasedOnWorkHours_HalfDay_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
				WorkTime = 400
			};
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> {
				new WorkHoursSetting { ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),Name = "WorkHour",FullDayMinutes = 500 , HalfDayMinutes = 250 } ,
			new WorkHoursSetting{ ID = Guid.NewGuid() , Name = "New WorkHour" ,FullDayMinutes = 500 , HalfDayMinutes = 250 } };

			var response = calculateAttendance.AttBasedOnWorkHours(attendance, employee);
			Assert.True(response.Present == 0.5m);
			Assert.True(response.AttendanceStatus == (int)AttendanceStatus.HalfDayPresent);
			Assert.True(response.Absent == 0.5m);
			Assert.True(response.HalfDayType == (int)AttendanceStatus.HalfDayAbsent);
		}

		[Fact]
		public void GetEmpAttBasedOnWorkHours_VeryLessHours_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
				WorkTime = 60
			};
			calculateAttendance.WorkHoursSettings = new List<WorkHoursSetting> {
				new WorkHoursSetting { ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),Name = "WorkHour",FullDayMinutes = 500 , HalfDayMinutes = 250 } ,
			new WorkHoursSetting{ ID = Guid.NewGuid() , Name = "New WorkHour" ,FullDayMinutes = 500 , HalfDayMinutes = 250 } };
			var response = calculateAttendance.AttBasedOnWorkHours(attendance, employee);
			Assert.True(response.Absent == 1);
			Assert.True(response.AttendanceStatus == (int)AttendanceStatus.Absent);
		}

		[Fact]
		public void GetEmpAttWithSchedules_NextDayOut_Is_Zero_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
			};
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 10,
				NextDayOut = 0,
				StartAt = 9,
				EndsAt = 18,
			};
			var response = CalculateAttendance.UpdateSchedule(attendance, scheduleDetails);
			Assert.True(response.SchIntime == 9);
			Assert.True(response.SchOutTime == 18);
			Assert.True(response.SchWorkTime == 9);
		}

		[Fact]
		public void GetEmpAttWithSchedules_NextDayOut_Is_NonZero_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
			};
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				NextDayOut = 1,
				StartAt = 22 * 60,
				EndsAt = 6 * 60
			};
			var response = CalculateAttendance.UpdateSchedule(attendance, scheduleDetails);
			Assert.True(response.SchWorkTime == 8 * 60);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_EmpId_Matched_Test()
		{
			var employee = _employeeData.GetEmployeeData();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_TeamId_Matched_Test()
		{
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_DesignationId_Matched_Test()
		{
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid();
			employee.TeamId = Guid.NewGuid();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_DepartmentId_Matched_Test()
		{
			var employee = _employeeData.GetEmployeeData();
			employee.TeamId = Guid.NewGuid();
			employee.DesignationId = Guid.NewGuid();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_LocationId_Matched_Test()
		{
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid(); employee.TeamId = Guid.NewGuid(); employee.DesignationId = Guid.NewGuid(); employee.DepartmentId = Guid.NewGuid();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpHaveExemptionFromHoliday_False_Case_Test()
		{
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid(); employee.TeamId = Guid.NewGuid(); employee.DesignationId = Guid.NewGuid();
			employee.DepartmentId = Guid.NewGuid(); employee.WorkLocationId = Guid.NewGuid();

			var result = calculateAttendance.IsEmpHaveExemptionFromHoliday(employee);
			Assert.False(result);
		}

		[Fact]
		public void GetWeekOffDetails_AllocationAndWeekOffSetupIdNotNull()
		{
			//Arrange
			var employee = new Employee
			{
				ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
				Designation = new Designation(),
				Department = new Department(),
				WorkLocation = new Location()
			};
			List<Allocation> allocations = new()
			{
						new Allocation
						{
							EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
							ID = Guid.NewGuid(),
							WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						},
						new Allocation
						{
							EmployeeId = Guid.NewGuid(),
							ID = Guid.NewGuid(),
						}

					};
			calculateAttendance.Allocations = allocations;
			//Act
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.True(result);
		}

		[Fact]
		public void GetWeekOffDetails_TeamWeekOffSetupIdNotNull()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Department.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = null;
			AllocationData();
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.True(result);
		}

		[Fact]
		public void GetWeekOffDetails_DesignationWeekOffSetupIdNotNull()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.WeekOffSetupId = null;
			employee.Department.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = null;
			AllocationData();
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.True(result);
		}

		[Fact]
		public void GetWeekOffDetails_DepartmentWeekOffSetupIdNotNull()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = null;
			AllocationData();
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.True(result);
		}

		[Fact]
		public void GetWeekOffDetails_WorkLocationWeekOffSetupIdNotNull()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.Department.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254");
			AllocationData();
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.True(result);
		}

		[Fact]
		public void GetWeekOffDetails_False_Case_Test()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.Team.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.Department.WeekOffSetupId = null;
			AllocationData();
			var result = calculateAttendance.GetWeekOffDetails(employee, DateTime.Now);
			//Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData(true, 1, 2)]//Yearly WeekOff(weekDay.Type == 2) And Attendance DayOfWeek Matches WeekDays.WeekDay and considered Even weekoff's
		[InlineData(true, 0, 2)]//Yearly WeekOff(weekDay.Type == 2) And Attendance DayOfWeek Matches WeekDays.WeekDay and considered All Weeks
		public void EmpHaveWeekOff_Yearly_WeekOff_Test(bool verify, int weekInYear, int type)
		{
			//Arrange
			WeekOffDaysData(weekInYear, 1, type);

			var result = calculateAttendance.EmpHaveWeekOff(DateTime.Parse("2022-09-26"), Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"));
			//Assert
			Assert.Equal(verify, result);
		}

		[Fact]//Yearly WeekOff(weekDay.Type == 2) And Attendance DayOfWeek Matches WeekDays.WeekDay and considered Odd weekoff's
		public void EmpHaveWeekOff_Yearly_WeekOff_Test_Considered_OddWeeks()
		{
			//Arrange
			List<WeekOffDays> weekOffDays = new()
			{
				new WeekOffDays
				{
					WeekOffSetupId=Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"),
					WeekDate=DateTime.Now.Date,
					Type=2,
					WeekDay=(int)DateTime.Parse("2022-10-04").DayOfWeek,
					WeekNoInMonth="4",
					WeekInYear=2,
					Status=1
				},
				new WeekOffDays
				{
					WeekOffSetupId=Guid.NewGuid(),
					WeekDate=DateTime.Parse("2022-08-26"),
					Type=2,
					WeekDay=5,
					WeekInYear=2,
					WeekNoInMonth="4",
					Status=1
				}
			};
			calculateAttendance.WeekOffDays = weekOffDays;
			//Act
			var result = calculateAttendance.EmpHaveWeekOff(DateTime.Parse("2022-10-04"), Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"));
			//Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData(true, 1, 1)]//Monthly WeekOff(weekDay.Type == 1) And result True Case
		public void EmpHaveWeekOff_Monthly_WeekOff_Test(bool verify, byte status, int weekInYear)
		{
			//Arrange
			WeekOffDaysData(weekInYear, status, weekInYear);

			var result = calculateAttendance.EmpHaveWeekOff(DateTime.Parse("2022-09-26"), Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"));
			//Assert
			Assert.Equal(verify, result);
		}

		[Theory]
		[InlineData(true, 1, 2, 3)]//WeekDate Wise (weekDay.Type == 3) And result True Case
		[InlineData(false, 1, 1, 3)]//WeekDate Wise (weekDay.Type == 3) And result False Case
		public void EmpHaveWeekOff_WeekDate_Wise_Test(bool verify, int weekInYear, byte status, int type)
		{
			//Arrange
			WeekOffDaysData(weekInYear, status, type);

			var result = calculateAttendance.EmpHaveWeekOff(DateTime.Today, Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083"));
			//Assert
			Assert.Equal(verify, result);
		}
		[Fact]
		public void MissedAttendanceCalculation_Test_When_IsEmpHaveExemptionFromHoliday_True_OneDay_Leave()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce");
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};

			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
			};
			#endregion
			var MissedAttCalculation = calculateAttendance.MissedAttendanceCalculation(attendance, employee, true, scheduleDetails);
			Assert.True(MissedAttCalculation.Leave == 1);
			Assert.True(MissedAttCalculation.LeaveTypeID == lTypeId);
			Assert.Equal((int)AttendanceStatus.Leave, MissedAttCalculation.AttendanceStatus);

		}
		[Fact]
		public void MissedAttendanceCalculation_Test_When_IsEmpHaveExemptionFromHoliday_True_HalfDayLeave_HalfDayAbsent()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce");
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};

			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
			};

			#endregion
			var calculateAtt = new CalculateAttendance(uow.Object, _schService.Object)
			{
				Leaves = new List<ApplyLeaveDetails>
				{
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Applied,
						},
						LeaveTypeId = lTypeId,
						LeaveCount = 0.5m
					}
			   },
			};
			var MissedAttCalculation = calculateAtt.MissedAttendanceCalculation(attendance, employee, true, scheduleDetails);
			Assert.True(MissedAttCalculation.Leave == 0.5m);
			Assert.True(MissedAttCalculation.Absent == 0.5m);
			Assert.True(MissedAttCalculation.LeaveTypeID == lTypeId);
			Assert.Equal((int)AttendanceStatus.HalfDayLeave, MissedAttCalculation.AttendanceStatus);

		}

		[Fact]
		public void MissedAttendanceCalculation_Test_When_IsEmpHaveExemptionFromHoliday_False()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			LeavesData("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};

			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
			};

			#endregion
			_ = calculateAttendance.MissedAttendanceCalculation(attendance, employee, false, scheduleDetails);
			Assert.Equal((int)AttendanceStatus.Holiday, attendance.AttendanceStatus);
		}

		[Fact]
		public void MissedAttendanceCalculation_Test_When_GetWeekOffDetails_True()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid();
			employee.TeamId = Guid.NewGuid(); employee.DepartmentId = Guid.NewGuid(); employee.DesignationId = Guid.NewGuid();
			employee.WorkLocationId = Guid.NewGuid();
			LeavesData(Guid.NewGuid().ToString());
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};
			var attendance = new Attendance
			{
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today,
			};

			#endregion

			var MissedAttCalculation = calculateAttendance.MissedAttendanceCalculation(attendance, employee, true, scheduleDetails);
			Assert.Equal((int)AttendanceStatus.WeekOff, MissedAttCalculation.AttendanceStatus);
		}

		[Fact]
		public void MissedAttendanceCalculation_Test_When_GetWFHStatus_True()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.ID = empId;
			employee.Team.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.Department.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = null;

			LeavesData(Guid.NewGuid().ToString());

			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};
			var attendance = new Attendance
			{
				ID = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c"),
				EmployeeId = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c"),
				Employee = new Employee { ID = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c") },
				AttendanceDate = DateTime.Parse("2022-09-19"),
			};
			#endregion

			var MissedAttCalculation = calculateAttendance.MissedAttendanceCalculation(attendance, employee, true, scheduleDetails);
			Assert.Equal((int)AttendanceStatus.WFH, MissedAttCalculation.AttendanceStatus);
		}

		[Fact]
		public void MissedAttendanceCalculation_Test_When_AttendanceStatus_Is_Absent()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			employee.ID = Guid.NewGuid();
			employee.Team.WeekOffSetupId = null;
			employee.Designation.WeekOffSetupId = null;
			employee.Department.WeekOffSetupId = null;
			employee.WorkLocation.WeekOffSetupId = null;

			LeavesData(Guid.NewGuid().ToString());
			var scheduleDetails = new ScheduleDetails
			{
				NoOfBreaks = 2,
				BreakTime = 1,
				LoginGraceTime = 8,
				EndsAt = 18,
				StartAt = 9
			};
			var attendance = new Attendance
			{
				ID = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c"),
				EmployeeId = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c"),
				Employee = new Employee { ID = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c") },
				AttendanceDate = DateTime.Parse("2022-08-01"),
			};
			#endregion

			var MissedAttCalculation = calculateAttendance.MissedAttendanceCalculation(attendance, employee, true, scheduleDetails);
			Assert.Equal((int)AttendanceStatus.Absent, MissedAttCalculation.AttendanceStatus);
		}

		[Fact]
		public async Task Process_Test_When_AttendanceStatus_Is_Leave()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			EmployeesData();
			AllocationData();

			calculateAttendance.ManualAttLogs = new List<ManualAttLogs>();
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employee.No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(employee.ID,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			#endregion

			var result = await calculateAttendance.Process(DateTime.Today);
			Assert.Equal((int)AttendanceStatus.Leave, result.FirstOrDefault().AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.Holiday, result.LastOrDefault().AttendanceStatus);
		}

		[Fact]
		public async Task Process_Test_When_AttendanceStatus_Is_Leave_Half_Days()
		{
			#region Arrange

			var employees = new List<Employee>
			{
				new Employee
				{
					ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					No = "AVONTIX1822",
					Name = "Anudeep",
					Gender = 1,
					MaritalStatus = 1,
					Status = 1,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",

					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
						Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer",
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" },
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift
						{
							ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift"
						},
					},
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
					ReportingTo = new Employee { ID = Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"), Name = "Shiva" },
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType" },
					EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
					EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory" },
					Allocation = new Allocation { WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"), },
					AllowWebPunch = true
				},
				new Employee
				{
					ID = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					No = "AVONTIX1823",
					Name = "Vamshi",
					Gender = 1,
					MaritalStatus = 2,
					Status = 1,
					MobileNumber = "9639639633",
					DateOfBirth = new DateTime(1990, 02, 03),
					DateOfJoining = new DateTime(2021, 08, 13),
					AadhaarNumber = "561250752383",
					PanNumber = "BLMPJ2797M",
					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"), Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer" },
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					AllowWebPunch = false
				},
				 new Employee
				{
					ID= Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					No = "AVONTIX1824",
					Name = "Dharmendhar",
					Gender = 1,
					MaritalStatus = 3,
					Status=3,
					MobileNumber = "9639639634",
					DateOfBirth = new DateTime(1991 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752384",
					PanNumber = "BLMPJ2797N",
					DepartmentId = Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),
					Department = new Department{ID=Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),Name = "Transcription"},
					DesignationId = Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"),
					Designation = new Designation{ID=Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"), Name ="Healthcare Documentation"},
					WorkLocationId = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),
					WorkLocation = new Location{ID = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),Name="Tharnaka" },
					TeamId = Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"),
					Team = new Team{ID =Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"), Name="Quicklooks"},
					AllowWebPunch=true
				},
			};
			AllocationData();
			var calcAttendance = new CalculateAttendance(uow.Object, _schService.Object)
			{
				Leaves = new List<ApplyLeaveDetails>
				{
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.NewGuid(),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 0.5m
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						   FromHalf = false, ToHalf = true
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 0.5m,
						IsFirstHalf = false,
						IsHalfDay = true
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date,
						   ToDate = DateTime.Now.Date,
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						   FromHalf = false, ToHalf = false
						},
						LeaveTypeId = lTypeId,
						LeaveCount = 1,
						IsFirstHalf = false,
						IsHalfDay = false
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 0.5m
					}
			   },
				Attendances = new List<Attendance>
				{
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date.AddDays(-1), AttendanceStatus = (int)AttendanceStatus.Present },
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date, AttendanceStatus = (int)AttendanceStatus.Absent },

				},
				Allocations = new List<Allocation>
				{
					new Allocation
					{
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						ID = Guid.NewGuid(),
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
						WorkHoursSetting = new WorkHoursSetting{ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3") , Name = "WorkHour"}
					},
					new Allocation
					{
						EmployeeId = Guid.NewGuid(),
						ID = Guid.NewGuid(),
					}

				},
				WFH = new List<ApplyWfh>
				{
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Today.Date,
						ToDateC = DateTime.Today.Date,
						ReasonForWFH = "dfgf",
						Status = 2,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Parse("2022-07-15"),
						ToDateC = DateTime.Parse("2022-07-15"),
						ReasonForWFH = "dfgf",
						Status = 1,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						ToDateC = DateTime.Parse("2022-07-30"),
						FromDateC = DateTime.Parse("2022-08-04"),
						Status=2,
						EmployeeId = empId
					}
				},
			};

			calcAttendance.Employees = employees.ToList();
			calcAttendance.ManualAttLogs = new List<ManualAttLogs>();
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo =employees.LastOrDefault().No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calcAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calcAttendance.BiometricLogs.AddRange(BiometricAttData(empId,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calcAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			#endregion

			var result = await calcAttendance.Process(DateTime.Parse("2022-12-22"));
			Assert.Equal((int)AttendanceStatus.HalfDayPresent, result.FirstOrDefault().AttendanceStatus);
			Assert.True(result.FirstOrDefault().Leave== 0.5m);
			Assert.Equal((int)AttendanceStatus.HalfDayLeave ,result.FirstOrDefault().HalfDayType);
			Assert.Equal((int)AttendanceStatus.HalfDayAbsent, result.LastOrDefault().HalfDayType);
		}
		[Fact]
		public async Task Process_Test_When_AttendanceStatus_Is_Leave_FullDay()
		{
			#region Arrange

			var employees = new List<Employee>
			{
				new Employee
				{
					ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					No = "AVONTIX1822",
					Name = "Anudeep",
					Gender = 1,
					MaritalStatus = 1,
					Status = 1,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",

					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
						Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer",
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" },
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift
						{
							ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift"
						},
					},
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" },
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup { ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
							Name = "WeekOff" }
					},
					ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
					ReportingTo = new Employee { ID = Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"), Name = "Shiva" },
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType" },
					EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
					EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory" },
					Allocation = new Allocation { WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"), },
					AllowWebPunch = true
				},
				new Employee
				{
					ID = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					No = "AVONTIX1823",
					Name = "Vamshi",
					Gender = 1,
					MaritalStatus = 2,
					Status = 1,
					MobileNumber = "9639639633",
					DateOfBirth = new DateTime(1990, 02, 03),
					DateOfJoining = new DateTime(2021, 08, 13),
					AadhaarNumber = "561250752383",
					PanNumber = "BLMPJ2797M",
					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department { ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"), Name = "IT Development",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer" },
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular",
						ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
						Shift = new TranSmart.Domain.Entities.Leave.Shift { ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
							Name = "DayShift" } },
					AllowWebPunch = false
				},
				new Employee
				{
					ID= Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					No = "AVONTIX1824",
					Name = "Dharmendhar",
					Gender = 1,
					MaritalStatus = 3,
					Status=3,
					MobileNumber = "9639639634",
					DateOfBirth = new DateTime(1991 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752384",
					PanNumber = "BLMPJ2797N",
					DepartmentId = Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),
					Department = new Department{ID=Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),Name = "Transcription"},
					DesignationId = Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"),
					Designation = new Designation{ID=Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"), Name ="Healthcare Documentation"},
					WorkLocationId = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),
					WorkLocation = new Location{ID = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),Name="Tharnaka" },
					TeamId = Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"),
					Team = new Team{ID =Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"), Name="Quicklooks"},
					AllowWebPunch=true
				},
			};
			AllocationData();
			var calcAttendance = new CalculateAttendance(uow.Object, _schService.Object)
			{
				Leaves = new List<ApplyLeaveDetails>
				{
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.NewGuid(),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 0.5m
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   FromHalf = false,ToHalf = false,
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 1
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
						   FromDate = DateTime.Now.Date,
						   ToDate = DateTime.Now.Date,
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = lTypeId,
						LeaveCount = 1,
					},
					new ApplyLeaveDetails
					{
						ID = Guid.NewGuid(),
						ApplyLeave = new ApplyLeave
						{
						   ID = Guid.NewGuid(),
						   EmployeeId = Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
						   FromDate = DateTime.Now.Date.AddDays(1),
						   ToDate = DateTime.Now.Date.AddDays(1),
						   Reason = "Corona",
						   EmergencyContNo = "9381742192",
						   Status = (int)ApplyLeaveSts.Approved,
						},
						LeaveTypeId = unPaidLvTypeId,
						LeaveCount = 0.5m
					}
			   },
				Attendances = new List<Attendance>
				{
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date.AddDays(-1), AttendanceStatus = (int)AttendanceStatus.Present },
					new Attendance{ EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), AttendanceDate = DateTime.Now.Date, AttendanceStatus = (int)AttendanceStatus.Absent },

				},
				Allocations = new List<Allocation>
				{
					new Allocation
					{
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						ID = Guid.NewGuid(),
						WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
						WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
						WorkHoursSetting = new WorkHoursSetting{ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3") , Name = "WorkHour"}
					},
					new Allocation
					{
						EmployeeId = Guid.NewGuid(),
						ID = Guid.NewGuid(),
					}

				},
				WFH = new List<ApplyWfh>
				{
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Today.Date,
						ToDateC = DateTime.Today.Date,
						ReasonForWFH = "dfgf",
						Status = 2,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
						AdminReason = "hfgh",
						FromDateC = DateTime.Parse("2022-07-15"),
						ToDateC = DateTime.Parse("2022-07-15"),
						ReasonForWFH = "dfgf",
						Status = 1,
					},
					new ApplyWfh
					{
						ID = Guid.NewGuid(),
						ToDateC = DateTime.Parse("2022-07-30"),
						FromDateC = DateTime.Parse("2022-08-04"),
						Status=2,
						EmployeeId = empId
					}
				},
			};

			calcAttendance.Employees = employees.ToList();
			calcAttendance.ManualAttLogs = new List<ManualAttLogs>();
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employees.LastOrDefault().No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calcAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calcAttendance.BiometricLogs.AddRange(BiometricAttData(empId,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calcAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			#endregion

			var result = await calcAttendance.Process(DateTime.Parse("2022-12-21"));
			Assert.Equal((int)AttendanceStatus.Leave, result.FirstOrDefault().AttendanceStatus);
			Assert.True(result.FirstOrDefault().Leave == 1);
		}

		[Fact]
		public async Task Process_Test_When_AttendanceStatus_Is_Present()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			EmployeesData();
			AllocationData();
			LeavesData(Guid.NewGuid().ToString());

			calculateAttendance.ManualAttLogs = new List<ManualAttLogs>();
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employee.No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(empId,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			#endregion

			var result = await calculateAttendance.Process(DateTime.Today);
			Assert.Equal((int)AttendanceStatus.Present, result.FirstOrDefault().AttendanceStatus);
		}

		[Fact]
		public async Task Process_Test_When_MissedAttendanceCalculation()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			EmployeesData();
			AllocationData();
			LeavesData(Guid.NewGuid().ToString());

			calculateAttendance.ManualAttLogs = new List<ManualAttLogs>();
			var inTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
			var outTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
			var empNo = employee.No.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(BiometricAttData(empId,empNo, inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			calculateAttendance.ManualAttLogs.AddRange(ManualAttLogsData(inTime.Date, new TimeSpan(inTime.Hour, 0, 0), new TimeSpan(outTime.Hour, 0, 0)));
			//Mock GetEmployeeSchedule
			_schService.Setup(x => x.GetEmployeeSchedule(It.IsAny<Employee>()))
							.Returns(Task.FromResult(new ScheduleDetails { BreakTime = 1, NoOfBreaks = 1, StartAt = 9, EndsAt = 18, LoginGraceTime = 9 }));

			#endregion

			var result = await calculateAttendance.Process(DateTime.Today);
			Assert.Equal((int)AttendanceStatus.Holiday, result.LastOrDefault().AttendanceStatus);
		}

		[Theory]
		[InlineData(55, 0, 1, 0, 1, 0)]// (in, out, in, out, in)
		[InlineData(75, 0, 1, 1, 0, 1)]// (in, out, out, in, out)
		[InlineData(75, 0, 1, 0, 0, 1)]// (in, out, in, in, out)
		[InlineData(115, 0, 1, 1, 0, 0)]// (in, out, out, in, in)
		public async Task Process_Test_CalculateBreakTime(int breakTime, int movementType1, int movementType2, int movementType3, int movementType4, int movementType5)
		{
			#region Arrange
			var employeeId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce");
			EmployeesData();
			AllocationData();
			LeavesData(Guid.NewGuid().ToString());
			var biometricData = new List<BiometricAttLogs>
			{
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmpCode = "1823",
					EmployeeId = employeeId,
					MovementType = movementType1,
					MovementTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 15, 0)
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmployeeId = employeeId,
					EmpCode = "1823",
					MovementType = movementType2,
					MovementTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 15, 0)
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmpCode = "1823",
					EmployeeId = employeeId,
					MovementType = movementType3,
					MovementTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0)
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmpCode = "1823",
					EmployeeId = employeeId,
					MovementType = movementType4,
					MovementTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 30, 0)
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					EmpCode = "1823",
					EmployeeId = employeeId,
					MovementType = movementType5,
					MovementTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 10, 0)
				}
			};

			calculateAttendance.ManualAttLogs = new List<ManualAttLogs>();
			calculateAttendance.BiometricLogs = new List<BiometricAttLogs>();
			calculateAttendance.BiometricLogs.AddRange(biometricData);
			#endregion

			var result = await calculateAttendance.Process(DateTime.Today);
			Assert.Equal(breakTime, result.FirstOrDefault().BreakTime);
		}

	}
}
