using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;

namespace Transmart.Services.UnitTests.Services.Leave.EmployeeData
{
	public class EmployeeDataGenerator
	{
		private Employee employee = new Employee
		{
			ID = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
			No = "AVONTIX1822",
			Name = "Anudeep",
			Gender = 1,
			Status = 1,
			MobileNumber = "9639639632",
			DateOfBirth = new DateTime(1989, 02, 02),
			DateOfJoining = new DateTime(2020, 08, 12),
			AadhaarNumber = "561250752388",
			PanNumber = "BLMPJ2797L",
			DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
			Department = new Department
			{
				ID = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
				Name = "IT Development",
				ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
				Shift = new TranSmart.Domain.Entities.Leave.Shift
				{
					ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
					Name = "DayShift"
				},
				WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
				WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup
				{
					ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
					Name = "WeekOff"
				},
				WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
				WorkHoursSetting = new TranSmart.Domain.Entities.Leave.WorkHoursSetting
				{
					ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
					Name = "WorkHour"
				}
			},
			DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
			Designation = new Designation
			{
				ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
				Name = "Jr Software Developer",
				WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
				WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup
				{
					ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
					Name = "WeekOff"
				},
				ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
				Shift = new TranSmart.Domain.Entities.Leave.Shift
				{
					ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
					Name = "DayShift"
				},
				WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
				WorkHoursSetting = new TranSmart.Domain.Entities.Leave.WorkHoursSetting
				{
					ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
					Name = "WorkHour"
				}
			},
			WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
			WorkLocation = new Location
			{
				ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
				Name = "Jubilee Hills",
				WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
				WorkHoursSetting = new TranSmart.Domain.Entities.Leave.WorkHoursSetting
				{
					ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
					Name = "WorkHour"
				}
			},
			TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
			Team = new Team
			{
				ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
				Name = "Regular",
				ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
				Shift = new TranSmart.Domain.Entities.Leave.Shift
				{
					ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
					Name = "DayShift"
				},
				WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
				WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup
				{
					ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
					Name = "WeekOff"
				},
				WorkHoursSettingId = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
				WorkHoursSetting = new TranSmart.Domain.Entities.Leave.WorkHoursSetting
				{
					ID = Guid.Parse("77793f07-51f1-4a98-81de-4d82b527a3d3"),
					Name = "WorkHour"
				}
			},
			ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
			ReportingTo = new Employee
			{
				ID = Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"),
				Name = "Shiva",
				ReportingToId = Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
				ReportingTo = new Employee
				{
					Name = "Vamshi",
				}
			},
			WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
			WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType" },
			EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
			EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory" },
			Allocation = new Allocation { WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"), }
			//DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
			//Designation = new Designation { ID = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name = "Jr Software Developer" },
			//WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
			//WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"), Name = "Jubilee Hills" },
			//TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
			//Team = new Team { ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name = "Regular" },
			//ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
			//ReportingTo=new Employee { ID= Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"), Name="Shiva"},
			//WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
			//WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType"},
			//EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
			//EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory"}
		};
		private IEnumerable<Employee> employees = new List<Employee>
			{
				new Employee
				{
					ID= Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					No = "AVONTIX1822",
					Name = "Anudeep",
					Gender = 1,
					MaritalStatus = 1,
					Status=1,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989 , 02 , 02),
					DateOfJoining = new DateTime(2020 , 08 , 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",

					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department{ID=Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
													Name = "IT Development",
													ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Shift = new TranSmart.Domain.Entities.Leave.Shift{ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Name = "DayShift"},
													WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup{ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													Name= "WeekOff"}
					},
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation{ID=Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name ="Jr Software Developer",
												  WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup{ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													Name= "WeekOff"},
													ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Shift = new TranSmart.Domain.Entities.Leave.Shift
													{
														ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
														Name = "DayShift"
													},
					},
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location{ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),Name="Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team{ID =Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name="Regular",
									ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Shift = new TranSmart.Domain.Entities.Leave.Shift{ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Name = "DayShift"},
									WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													WeekOffSetup = new TranSmart.Domain.Entities.Leave.WeekOffSetup{ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
													Name= "WeekOff"}
					},
					ReportingToId = Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"),
					ReportingTo=new Employee { ID= Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"), Name="Shiva"},
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
					EmpCategoryId = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"),
					EmpCategory = new EmployeeCategory { ID = Guid.Parse("1744dfa1-fae9-4f5d-a310-4d603a12d4cf"), Name = "Employee CAtegory"},
					Allocation = new Allocation{WeekOffSetupId = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"), },
					AllowWebPunch=true
				},
				new Employee
				{
					ID= Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					No = "AVONTIX1823",
					Name = "Vamshi",
					Gender = 1,
					MaritalStatus = 2,
					Status=1,
					MobileNumber = "9639639633",
					DateOfBirth = new DateTime(1990 , 02 , 03),
					DateOfJoining = new DateTime(2021 , 08 , 13),
					AadhaarNumber = "561250752383",
					PanNumber = "BLMPJ2797M",
					DepartmentId = Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),
					Department = new Department{ID=Guid.Parse("a9cc5e1b-e24f-4939-b47d-1b86e583afc7"),Name = "IT Development",
													ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Shift = new TranSmart.Domain.Entities.Leave.Shift{ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Name = "DayShift"}},
					DesignationId = Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"),
					Designation = new Designation{ID=Guid.Parse("05adb896-81cf-4323-9898-a8db16ca0a20"), Name ="Jr Software Developer"},
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),
					WorkLocation = new Location{ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbf"),Name="Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"),
					Team = new Team{ID =Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b50"), Name="Regular",
													ShiftId = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Shift = new TranSmart.Domain.Entities.Leave.Shift{ID = Guid.Parse("ed03407a-722c-4e3b-b82c-e896c30530f3"),
													Name = "DayShift"}},
					AllowWebPunch=false,
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
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
					AllowWebPunch=true,
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
				},
				new Employee
				{
					ID= Guid.Parse("9f4fde5e-cf68-460d-b034-39693cd05962"),
					No = "AVONTIX1825",
					Name = "Shiva",
					Gender = 1,
					MaritalStatus = 1,
					Status=2,
					MobileNumber = "9639639635",
					DateOfBirth = new DateTime(1993 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752385",
					PanNumber = "BLMPJ2797P",
					DepartmentId = Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),
					Department = new Department{ID=Guid.Parse("0006f9b7-2bf3-4d5d-bf2c-20f7cb7a0f7b"),Name = "Transcription"},
					DesignationId = Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"),
					Designation = new Designation{ID=Guid.Parse("5a7b5f72-4a69-4a87-9af4-5cdbe4a4b291"), Name ="Healthcare Documentation"},
					WorkLocationId = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),
					WorkLocation = new Location{ID = Guid.Parse("e36d3f94-f700-4843-98b9-85b77feeec04"),Name="Tharnaka" },
					TeamId = Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"),
					Team = new Team{ID =Guid.Parse("a811da90-ce8e-41b3-bc52-9fce9528f283"), Name="Quicklooks"},
					AllowWebPunch=false,
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
				},new Employee
				{
					ID= Guid.Parse("d75fe57f-20b8-4f19-b743-8d0f8bc817dd"),
					No = "AVONTIX1826",
					Name = "Mahesh",
					Gender = 1,
					MaritalStatus = 2,
					Status=1,
					MobileNumber = "9639639636",
					DateOfBirth = new DateTime(1996 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752386",
					PanNumber = "BLMPJ2797Q",
					DepartmentId = Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),
					Department = new Department{ID=Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),Name = "Coding", WeekOffSetupId=Guid.Parse("4fb6a65e-d523-406e-8e78-887f93de2083")},
					DesignationId = Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"),
					Designation = new Designation{ID=Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"), Name ="Sr.Manager",WeekOffSetupId=Guid.Parse("3fb6a65e-d523-406e-8e78-887f93de2083")},
					WorkLocationId = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),
					WorkLocation = new Location{ID = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),Name="Vijayawada" ,WeekOffSetupId=Guid.Parse("5fb6a65e-d523-406e-8e78-887f93de2083")},
					TeamId = Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"),
					Team = new Team{ID =Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"), Name="Stedmans", WeekOffSetupId=Guid.Parse("2fb6a65e-d523-406e-8e78-887f93de2083")},
					AllowWebPunch=false,
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
				},
				new Employee
				{
					ID= Guid.Parse("4c48a4cd-6933-4ba0-9e01-600c4124800c"),
					No = "AVONTIX2061",
					Name = "Vishnu",
					Gender = 1,
					MaritalStatus = 3,
					Status=2,
					MobileNumber = "9639639637",
					DateOfBirth = new DateTime(1994 , 02 , 02),
					DateOfJoining = new DateTime(2021 , 08 , 12),
					AadhaarNumber = "561250752387",
					PanNumber = "BLMPJ2797R",
					DepartmentId = Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),
					Department = new Department{ID=Guid.Parse("e3de3a49-01d0-46bc-9201-3cc1ffaf1459"),Name = "Coding"},
					DesignationId = Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"),
					Designation = new Designation{ID=Guid.Parse("cc231104-03c1-4483-b050-a155e4c1ba25"), Name ="Sr.Manager"},
					WorkLocationId = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),
					WorkLocation = new Location{ID = Guid.Parse("b517c31b-a748-4918-ad29-612b246cc866"),Name="Vijayawada" },
					TeamId = Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"),
					Team = new Team{ID =Guid.Parse("fd40b8a6-10f6-4448-bc34-e45532d3c0ca"), Name="Stedmans"},
					ReportingToId = Guid.Parse("77934662-9896-44d5-bc74-5cce64150fba"),
					AllowWebPunch=true,
					WorkTypeId = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"),
					WorkType = new WorkType { ID = Guid.Parse("467e3315-5151-4b6b-af54-08b35699632f"), Name = "WorkType", SalaryPaying=true},
				}
			}.AsQueryable();
		public IEnumerable<Employee> GetAllEmployeesData()
		{
			return employees;
		}
		public Employee GetEmployeeData()
		{
			return employee;
		}
	}
}
