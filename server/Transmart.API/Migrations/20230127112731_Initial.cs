using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TranSmart.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_AppForms",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Header = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AppForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_Groups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_LookUpMaster",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LookUpMaster", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_LookUpValues",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LookUpValues", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_Replication",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<byte>(type: "tinyint", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Replication", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_ReportModule",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ReportModule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_Role",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Cards = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Menu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_SequenceNo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Attribute = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NextNo = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NextDisplayNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SequenceNo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_UserSettings",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordExpiry = table.Column<int>(type: "int", nullable: false),
                    MinimumPassword = table.Column<int>(type: "int", nullable: false),
                    MaximumPassword = table.Column<int>(type: "int", nullable: false),
                    AllowedSpecial = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TrackLoginInfo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AllowNumber = table.Column<int>(type: "int", nullable: false),
                    DisableUserAc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccesedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccesedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Daily_EventLog",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrMSG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Daily_EventLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaves",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Leaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLeaves", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportingTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HD_DeskGroup",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CanEditTicket = table.Column<int>(type: "int", nullable: false),
                    CanPostReply = table.Column<int>(type: "int", nullable: false),
                    CanCloseTicket = table.Column<int>(type: "int", nullable: false),
                    CanAssignTicket = table.Column<int>(type: "int", nullable: false),
                    CanTransferTicket = table.Column<int>(type: "int", nullable: false),
                    CanDeleteTicket = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_DeskGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HD_TicketStatus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_TicketStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Leaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_AttendanceModifyLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromLeaveTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ToLeaveTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PresentAttStatus = table.Column<int>(type: "int", nullable: false),
                    ModifyStatus = table.Column<int>(type: "int", nullable: false),
                    IsHalfDay = table.Column<bool>(type: "bit", nullable: false),
                    IsFirstOff = table.Column<bool>(type: "bit", nullable: false),
                    HalfDayType = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_AttendanceModifyLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_BiometricAttLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovementTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_BiometricAttLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_Holidays",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ReprApplication = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_Holidays", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PayType = table.Column<int>(type: "int", nullable: false),
                    DefaultPayoff = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveAfter = table.Column<int>(type: "int", nullable: false),
                    EffectiveType = table.Column<int>(type: "int", nullable: false),
                    EffectiveBy = table.Column<int>(type: "int", nullable: false),
                    ProrateByT = table.Column<int>(type: "int", nullable: false),
                    RoundOff = table.Column<int>(type: "int", nullable: false),
                    RoundOffTo = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastDate = table.Column<int>(type: "int", nullable: false),
                    FutureDate = table.Column<int>(type: "int", nullable: false),
                    MinLeaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxLeaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxApplications = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    specifiedperio = table.Column<int>(type: "int", nullable: true),
                    ProofNeeded = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_ManualAttLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_ManualAttLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_Shift",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    StartFrom = table.Column<int>(type: "int", nullable: false),
                    EndsOn = table.Column<int>(type: "int", nullable: false),
                    loginGraceTime = table.Column<int>(type: "int", nullable: true),
                    logoutGraceTime = table.Column<int>(type: "int", nullable: true),
                    Allowance = table.Column<int>(type: "int", nullable: true),
                    BreakTime = table.Column<int>(type: "int", nullable: false),
                    Desciption = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    NoOfBreaks = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_Shift", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_WeekOffSetup",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_WeekOffSetup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LM_WorkHoursSetting",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FullDayMinutes = table.Column<int>(type: "int", nullable: false),
                    HalfDayMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_WorkHoursSetting", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeCategory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_Organizations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    logo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    DateShowFormat = table.Column<int>(type: "int", nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    TAN = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ProbationPeriodType = table.Column<int>(type: "int", nullable: false),
                    MonthStartDay = table.Column<int>(type: "int", nullable: false),
                    AddressOne = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddressSecond = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    City = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    State = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Organizations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_ProbationPeriod",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProbationPeriodType = table.Column<int>(type: "int", nullable: false),
                    ProbationPeriodTime = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_ProbationPeriod", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_State",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_State", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_WorkType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_WorkType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_Bank",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    BankNoLength = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Bank", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_EarningComponent",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EarningType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PartOfSalary = table.Column<bool>(type: "bit", nullable: false),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    EPFContribution = table.Column<int>(type: "int", nullable: false),
                    ESIContribution = table.Column<bool>(type: "bit", nullable: false),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: false),
                    PartEmployerCTC = table.Column<bool>(type: "bit", nullable: false),
                    HideWhenZero = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_EarningComponent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_OtherSections",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_OtherSections", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section80C",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section80C", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section80D",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section80D", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PS_Template",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Template", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginFail",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginFail", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ValueText",
                columns: table => new
                {
                    Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueText", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "_Pages",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SNo = table.Column<int>(type: "int", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Privilege = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pages", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Pages__Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "_Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "_Report",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Report", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Report__Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "_Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Report__ReportModule_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "_ReportModule",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_Exemptions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolidaysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Employees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Teams = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_Exemptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_Exemptions_LM_Holidays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "LM_Holidays",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveSettings",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HourCalculation = table.Column<int>(type: "int", nullable: false),
                    FullDayHours = table.Column<int>(type: "int", nullable: true),
                    HalfDayHours = table.Column<int>(type: "int", nullable: true),
                    IncludeWeekend = table.Column<int>(type: "int", nullable: true),
                    IncludeHoliday = table.Column<int>(type: "int", nullable: true),
                    IncludeLeave = table.Column<int>(type: "int", nullable: true),
                    CompCreditToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompoLeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpireInMonths = table.Column<int>(type: "int", nullable: false),
                    WeekendPeriod = table.Column<int>(type: "int", nullable: true),
                    WeekendPeriodDn = table.Column<bool>(type: "bit", nullable: false),
                    HolidayPeriod = table.Column<int>(type: "int", nullable: true),
                    HolidayPeriodDn = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveSettings_LM_LeaveType_CompCreditToId",
                        column: x => x.CompCreditToId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveSettings_LM_LeaveType_CompoLeaveTypeId",
                        column: x => x.CompoLeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveTypeSchedule",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccType = table.Column<int>(type: "int", nullable: false),
                    AccOnDay = table.Column<int>(type: "int", nullable: false),
                    AccOnYearly = table.Column<int>(type: "int", nullable: true),
                    AccOnHalfYearly = table.Column<int>(type: "int", nullable: true),
                    AccOnQuarterly = table.Column<int>(type: "int", nullable: true),
                    NoOfDays = table.Column<int>(type: "int", nullable: false),
                    ResetType = table.Column<int>(type: "int", nullable: false),
                    ResOnDay = table.Column<int>(type: "int", nullable: false),
                    ResOnYearly = table.Column<int>(type: "int", nullable: true),
                    ResOnHalfYearly = table.Column<int>(type: "int", nullable: true),
                    ResOnQuarterly = table.Column<int>(type: "int", nullable: true),
                    ResetNoOfDays = table.Column<int>(type: "int", nullable: false),
                    FwdType = table.Column<int>(type: "int", nullable: false),
                    FwdPercentage = table.Column<int>(type: "int", nullable: true),
                    FwdDays = table.Column<int>(type: "int", nullable: true),
                    FwdLimit = table.Column<int>(type: "int", nullable: false),
                    FwdOverallLimit = table.Column<int>(type: "int", nullable: true),
                    OpeningBalance = table.Column<int>(type: "int", nullable: true),
                    MaxBalance = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveTypeSchedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveTypeSchedule_LM_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_WeekOffDays",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<int>(type: "int", nullable: true),
                    WeekNoInMonth = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    WeekInYear = table.Column<int>(type: "int", nullable: true),
                    WeekDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_WeekOffDays", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_WeekOffDays_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Department",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkHoursSettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Department", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Department_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Department_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Department_LM_WorkHoursSetting_WorkHoursSettingId",
                        column: x => x.WorkHoursSettingId,
                        principalTable: "LM_WorkHoursSetting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Team",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkHoursSettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Team", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Team_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Team_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Team_LM_WorkHoursSetting_WorkHoursSettingId",
                        column: x => x.WorkHoursSettingId,
                        principalTable: "LM_WorkHoursSetting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PaySettings",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxDeductor = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    TaxDeductorFNam = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FYFromMonth = table.Column<int>(type: "int", nullable: false),
                    FYFromDay = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PaySettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PaySettings_Org_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Org_Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Location",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkHoursSettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Location", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Location_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Location_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Location_LM_WorkHoursSetting_WorkHoursSettingId",
                        column: x => x.WorkHoursSettingId,
                        principalTable: "LM_WorkHoursSetting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Location_Org_State_StateId",
                        column: x => x.StateId,
                        principalTable: "Org_State",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_ProfessionalTax",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_ProfessionalTax", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_ProfessionalTax_Org_State_StateId",
                        column: x => x.StateId,
                        principalTable: "Org_State",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_DeductionComponent",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deduct = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DeductionPlan = table.Column<int>(type: "int", nullable: true),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    EarningId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_DeductionComponent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_DeductionComponent_PS_EarningComponent_EarningId",
                        column: x => x.EarningId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_TemplateEarning",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    PercentOn = table.Column<int>(type: "int", nullable: true),
                    PercentOnCompId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_TemplateEarning", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_TemplateEarning_PS_EarningComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_TemplateEarning_PS_EarningComponent_PercentOnCompId",
                        column: x => x.PercentOnCompId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_TemplateEarning_PS_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "PS_Template",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Privilege = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RolePrivileges__Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "_Pages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePrivileges__Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "_Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleReportPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Privilege = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleReportPrivileges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoleReportPrivileges__Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "_Report",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleReportPrivileges__Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "_Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Designation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkHoursSettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Designation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Designation_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Designation_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Designation_LM_WorkHoursSetting_WorkHoursSettingId",
                        column: x => x.WorkHoursSettingId,
                        principalTable: "LM_WorkHoursSetting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Designation_Org_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Org_Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_DeclarationSetting",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lock = table.Column<int>(type: "int", nullable: false),
                    MaxLimitEightyC = table.Column<int>(type: "int", nullable: false),
                    MaxLimitEightyD = table.Column<int>(type: "int", nullable: false),
                    EducationCess = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HouseLoanInt = table.Column<int>(type: "int", nullable: false),
                    TaxDedLastMonth = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_DeclarationSetting", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_DeclarationSetting_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_EPF",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnableEPF = table.Column<int>(type: "int", nullable: false),
                    EmployeeContrib = table.Column<int>(type: "int", nullable: true),
                    IncludeinCTC = table.Column<bool>(type: "bit", nullable: false),
                    PFConfiguration = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    RestrictedWage = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_EPF", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_EPF_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_ESI",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnableESI = table.Column<int>(type: "int", nullable: false),
                    ESISalaryLimit = table.Column<int>(type: "int", nullable: true),
                    ESINo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmployeesCont = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployerCont = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AddToCTC = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_ESI", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_ESI_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_FinancialYear",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromYear = table.Column<int>(type: "int", nullable: false),
                    ToYear = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_FinancialYear", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_FinancialYear_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_NewRegimeSlab",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncomeFrom = table.Column<int>(type: "int", nullable: false),
                    IncomeTo = table.Column<int>(type: "int", nullable: false),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_NewRegimeSlab", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_NewRegimeSlab_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_OldRegimeSlab",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncomeFrom = table.Column<int>(type: "int", nullable: false),
                    IncomeTo = table.Column<int>(type: "int", nullable: false),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_OldRegimeSlab", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_OldRegimeSlab_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PaySetup",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryDaysType = table.Column<int>(type: "int", nullable: false),
                    PayOn = table.Column<int>(type: "int", nullable: false),
                    MonthDay = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PaySetup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PaySetup_PS_PaySettings_PaySettingsId",
                        column: x => x.PaySettingsId,
                        principalTable: "PS_PaySettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_ProfessionalTaxSlab",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalTaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_ProfessionalTaxSlab", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_ProfessionalTaxSlab_PS_ProfessionalTax_ProfessionalTaxId",
                        column: x => x.ProfessionalTaxId,
                        principalTable: "PS_ProfessionalTax",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_TemplateDeduction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_TemplateDeduction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_TemplateDeduction_PS_DeductionComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_DeductionComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_TemplateDeduction_PS_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "PS_Template",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Employee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    No = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastWorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportingToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkEmail = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PanNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    MarriageDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProfileStatus = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmpCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AllowWebPunch = table.Column<bool>(type: "bit", nullable: false),
                    ProbationPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkFromHome = table.Column<int>(type: "int", nullable: false),
                    DOBC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Employee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Org_Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Org_Designation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_Employee_ReportingToId",
                        column: x => x.ReportingToId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_EmployeeCategory_EmpCategoryId",
                        column: x => x.EmpCategoryId,
                        principalTable: "Org_EmployeeCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_Location_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "Org_Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_ProbationPeriod_ProbationPeriodId",
                        column: x => x.ProbationPeriodId,
                        principalTable: "Org_ProbationPeriod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Org_Team",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Employee_Org_WorkType_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "Org_WorkType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PayMonth",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Net = table.Column<int>(type: "int", nullable: false),
                    Employees = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PayMonth", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PayMonth_PS_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "PS_FinancialYear",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayInfo_Audit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayInfo_Audit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmployeePayInfo_Audit_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayInfoStatus_Audit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayInfoStatus_Audit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmployeePayInfoStatus_Audit_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HD_DeskDepartment",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_DeskDepartment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_DeskDepartment_Org_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HD_DeskGroupEmployee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeskGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_DeskGroupEmployee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_DeskGroupEmployee_HD_DeskGroup_DeskGroupId",
                        column: x => x.DeskGroupId,
                        principalTable: "HD_DeskGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HD_DeskGroupEmployee_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_AdjustLeave",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Addedby = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_AdjustLeave", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_AdjustLeave_LM_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_AdjustLeave_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_ApprovedLeaves",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Allow = table.Column<bool>(type: "bit", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: true),
                    NoOfLeaves = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_ApprovedLeaves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_ApprovedLeaves_LM_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_ApprovedLeaves_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_Attendance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchIntime = table.Column<int>(type: "int", nullable: true),
                    SchOutTime = table.Column<int>(type: "int", nullable: true),
                    SchWorkTime = table.Column<int>(type: "int", nullable: true),
                    SchBreakTime = table.Column<int>(type: "int", nullable: true),
                    SchInTimeGrace = table.Column<int>(type: "int", nullable: true),
                    SchBreaks = table.Column<int>(type: "int", nullable: true),
                    InTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkTime = table.Column<int>(type: "int", nullable: true),
                    BreakTime = table.Column<int>(type: "int", nullable: true),
                    Breaks = table.Column<int>(type: "int", nullable: true),
                    AttendanceStatus = table.Column<int>(type: "int", nullable: true),
                    UADays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsHalfDay = table.Column<bool>(type: "bit", nullable: true),
                    HalfDayType = table.Column<int>(type: "int", nullable: true),
                    IsFirstHalf = table.Column<bool>(type: "bit", nullable: true),
                    Present = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    Absent = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    Leave = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    WFH = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_Attendance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_Attendance_LM_LeaveType_LeaveTypeID",
                        column: x => x.LeaveTypeID,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_Attendance_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_AttendanceSum",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Month = table.Column<byte>(type: "tinyint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Present = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LOP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unauthorized = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_AttendanceSum", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_AttendanceSum_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_CompOffRequest",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmailID = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReasonForApply = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AdminReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_CompOffRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_CompOffRequest_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_CompOffRequest_Org_Employee_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_CompOffRequest_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveRequest",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NoOfLeaves = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LeaveTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContNo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromHalf = table.Column<bool>(type: "bit", nullable: false),
                    ToHalf = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveRequest_Org_Employee_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveRequest_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_UnAuthorizedLeaves",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveStatus = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_UnAuthorizedLeaves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_UnAuthorizedLeaves_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_WFHRequest",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromDateC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDateC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromHalf = table.Column<bool>(type: "bit", nullable: false),
                    ToHalf = table.Column<bool>(type: "bit", nullable: false),
                    ReasonForWFH = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AdminReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_WFHRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_WFHRequest_LM_Shift_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_WFHRequest_Org_Employee_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_WFHRequest_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_Allocation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeekOffSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkHoursSettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_Allocation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_Allocation_LM_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "LM_Shift",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Allocation_LM_WeekOffSetup_WeekOffSetupId",
                        column: x => x.WeekOffSetupId,
                        principalTable: "LM_WeekOffSetup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Allocation_LM_WorkHoursSetting_WorkHoursSettingId",
                        column: x => x.WorkHoursSettingId,
                        principalTable: "LM_WorkHoursSetting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_Allocation_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeEducation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Medium = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Institute = table.Column<string>(type: "nvarchar(1264)", maxLength: 1264, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearOfPassing = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeEducation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeEducation_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeEmergencyAd",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressLineOne = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    AddressLineTwo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CityOrTown = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    EmergencyConNo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    State = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeEmergencyAd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeEmergencyAd_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeFamily",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    HumanRelation = table.Column<int>(type: "int", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeFamily", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeFamily_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeePermanentAd",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressLineOne = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SameAsPresent = table.Column<int>(type: "int", nullable: false),
                    AddressLineTwo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CityOrTown = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    State = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeePermanentAd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeePermanentAd_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeePresentAd",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressLineOne = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    AddressLineTwo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CityOrTown = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    State = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeePresentAd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeePresentAd_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeResignation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResignationType = table.Column<int>(type: "int", nullable: false),
                    ResignationOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeavingReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    AllowRehiring = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeResignation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeResignation_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeWorkExp",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    ResignedReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeWorkExp", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeWorkExp_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Arrear",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Pay = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Arrear", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Arrear_Org_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Declaration",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsNewRegime = table.Column<bool>(type: "bit", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Perquisites = table.Column<int>(type: "int", nullable: false),
                    PreviousEmployment = table.Column<int>(type: "int", nullable: false),
                    TotalSalary = table.Column<int>(type: "int", nullable: false),
                    Allowance = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    StandardDeduction = table.Column<int>(type: "int", nullable: false),
                    TaxOnEmployment = table.Column<int>(type: "int", nullable: false),
                    Deductions = table.Column<int>(type: "int", nullable: false),
                    IncomeChargeable = table.Column<int>(type: "int", nullable: false),
                    HouseIncome = table.Column<int>(type: "int", nullable: false),
                    OtherIncome = table.Column<int>(type: "int", nullable: false),
                    GrossTotal = table.Column<int>(type: "int", nullable: false),
                    EightyC = table.Column<int>(type: "int", nullable: false),
                    EPF = table.Column<int>(type: "int", nullable: false),
                    HomeLoanPrincipal = table.Column<int>(type: "int", nullable: false),
                    EightyD = table.Column<int>(type: "int", nullable: false),
                    OtherSections = table.Column<int>(type: "int", nullable: false),
                    Taxable = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    Relief = table.Column<int>(type: "int", nullable: false),
                    Cess = table.Column<int>(type: "int", nullable: false),
                    TaxPayable = table.Column<int>(type: "int", nullable: false),
                    TaxPaid = table.Column<int>(type: "int", nullable: false),
                    Due = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Declaration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Declaration_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Declaration_PS_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "PS_FinancialYear",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_EmpBonus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ReleasedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_EmpBonus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_EmpBonus_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_EmployeePayInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayMode = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNoVerify = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_EmployeePayInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_EmployeePayInfo_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_EmployeePayInfo_PS_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "PS_Bank",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_EmpStatutory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnablePF = table.Column<int>(type: "int", nullable: false),
                    EmployeesProvid = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UAN = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmployeeContrib = table.Column<int>(type: "int", nullable: true),
                    EnableESI = table.Column<int>(type: "int", nullable: false),
                    ESINo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_EmpStatutory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_EmpStatutory_Org_Employee_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_IncentivesPayCut",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Incentives = table.Column<int>(type: "int", nullable: false),
                    PayCut = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_IncentivesPayCut", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_IncentivesPayCut_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_IncomeTaxLimit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_IncomeTaxLimit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_IncomeTaxLimit_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_LateComers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_LateComers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_LateComers_Org_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Loan",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanNo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanReleasedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanAmount = table.Column<int>(type: "int", nullable: false),
                    DeductFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyAmount = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Due = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Loan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Loan_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Salary",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Annually = table.Column<int>(type: "int", nullable: false),
                    CTC = table.Column<int>(type: "int", nullable: false),
                    Monthly = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Salary", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Salary_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Salary_PS_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "PS_Template",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_ClientVisitRequest",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaceOfVisit = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurposeOfVisit = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminReason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_ClientVisitRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_ClientVisitRequest_Org_Employee_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_ClientVisitRequest_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_Images",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFlag = table.Column<byte>(type: "tinyint", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ResizeImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_Images_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_RaiseTicket",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TicketTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_RaiseTicket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_RaiseTicket_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoOfWrongs = table.Column<int>(type: "int", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users__Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "_Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Org_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PaySheet",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayMonthId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PresentDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LOPDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UADays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LCDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Gross = table.Column<int>(type: "int", nullable: false),
                    Deduction = table.Column<int>(type: "int", nullable: false),
                    Net = table.Column<int>(type: "int", nullable: false),
                    Incentive = table.Column<int>(type: "int", nullable: false),
                    Arrears = table.Column<int>(type: "int", nullable: false),
                    LOP = table.Column<int>(type: "int", nullable: false),
                    UA = table.Column<int>(type: "int", nullable: false),
                    LC = table.Column<int>(type: "int", nullable: false),
                    PayCut = table.Column<int>(type: "int", nullable: false),
                    EPFGross = table.Column<int>(type: "int", nullable: false),
                    EPF = table.Column<int>(type: "int", nullable: false),
                    ESIGross = table.Column<int>(type: "int", nullable: false),
                    ESI = table.Column<int>(type: "int", nullable: false),
                    ESIApplied = table.Column<bool>(type: "bit", nullable: false),
                    PTax = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    Loan = table.Column<int>(type: "int", nullable: false),
                    Hold = table.Column<bool>(type: "bit", nullable: false),
                    EPFNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ESINo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankIFSC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankACNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequePay = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PayslipMailedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PaySheet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PaySheet_Org_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_PaySheet_PS_PayMonth_PayMonthId",
                        column: x => x.PayMonthId,
                        principalTable: "PS_PayMonth",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HD_DepartmentGroup",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeskDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_DepartmentGroup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_DepartmentGroup_HD_DeskDepartment_DeskDepartmentId",
                        column: x => x.DeskDepartmentId,
                        principalTable: "HD_DeskDepartment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HD_DepartmentGroup_HD_DeskGroup_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "HD_DeskGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HD_HelpTopic",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    DueHours = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_HelpTopic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_HelpTopic_HD_DeskDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HD_DeskDepartment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HD_HelpTopic_HD_TicketStatus_TicketStatusId",
                        column: x => x.TicketStatusId,
                        principalTable: "HD_TicketStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveBalance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LeavesAddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreconsumableLeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplyLeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplyCompensatoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomizedBalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveBalance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_LM_AdjustLeave_CustomizedBalId",
                        column: x => x.CustomizedBalId,
                        principalTable: "LM_AdjustLeave",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_LM_ApprovedLeaves_PreconsumableLeaveId",
                        column: x => x.PreconsumableLeaveId,
                        principalTable: "LM_ApprovedLeaves",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_LM_CompOffRequest_ApplyCompensatoryId",
                        column: x => x.ApplyCompensatoryId,
                        principalTable: "LM_CompOffRequest",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_LM_LeaveRequest_ApplyLeaveId",
                        column: x => x.ApplyLeaveId,
                        principalTable: "LM_LeaveRequest",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_LM_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveBalance_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveRequestDetails",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplyLeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsHalfDay = table.Column<bool>(type: "bit", nullable: false),
                    IsFirstHalf = table.Column<bool>(type: "bit", nullable: false),
                    LeaveCount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveRequestDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveRequestDetails_LM_LeaveRequest_ApplyLeaveId",
                        column: x => x.ApplyLeaveId,
                        principalTable: "LM_LeaveRequest",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LM_LeaveRequestLeaveType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplyLeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoOfLeaves = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LM_LeaveRequestLeaveType", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LM_LeaveRequestLeaveType_LM_LeaveRequest_ApplyLeaveId",
                        column: x => x.ApplyLeaveId,
                        principalTable: "LM_LeaveRequest",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LM_LeaveRequestLeaveType_LM_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LM_LeaveType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_DeclarationAllowance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_DeclarationAllowance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_DeclarationAllowance_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_DeclarationAllowance_PS_EarningComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_HomeLoanPay",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestPaid = table.Column<int>(type: "int", nullable: false),
                    Principle = table.Column<int>(type: "int", nullable: false),
                    NameOfLender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LenderPAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_HomeLoanPay", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_HomeLoanPay_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_HRADeclaration",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentalFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentalTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandLord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_HRADeclaration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_HRADeclaration_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_LetOutProperty",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnualRentReceived = table.Column<int>(type: "int", nullable: false),
                    MunicipalTaxPaid = table.Column<int>(type: "int", nullable: false),
                    NetAnnualValue = table.Column<int>(type: "int", nullable: false),
                    StandardDeduction = table.Column<int>(type: "int", nullable: false),
                    RepayingHomeLoan = table.Column<bool>(type: "bit", nullable: false),
                    InterestPaid = table.Column<int>(type: "int", nullable: false),
                    Principle = table.Column<int>(type: "int", nullable: false),
                    NameOfLender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LenderPAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetIncome = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_LetOutProperty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_LetOutProperty_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_OtherIncomeSources",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherSources = table.Column<int>(type: "int", nullable: false),
                    InterestOnSaving = table.Column<int>(type: "int", nullable: false),
                    InterestOnFD = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_OtherIncomeSources", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_OtherIncomeSources_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PrevEmployment",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncomeAfterException = table.Column<int>(type: "int", nullable: false),
                    IncomeTax = table.Column<int>(type: "int", nullable: false),
                    ProfessionalTax = table.Column<int>(type: "int", nullable: false),
                    ProvisionalFund = table.Column<int>(type: "int", nullable: false),
                    EncashmentExceptions = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PrevEmployment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PrevEmployment_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section6A80C",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Section80CId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section6A80C", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80C_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80C_PS_Section80C_Section80CId",
                        column: x => x.Section80CId,
                        principalTable: "PS_Section80C",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section6A80CWages",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section6A80CWages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80CWages_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80CWages_PS_DeductionComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_DeductionComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section6A80D",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Section80DId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Qualified = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section6A80D", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80D_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Section6A80D_PS_Section80D_Section80DId",
                        column: x => x.Section80DId,
                        principalTable: "PS_Section80D",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_Section6AOther",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeclarationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherSectionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Qualified = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_Section6AOther", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_Section6AOther_PS_Declaration_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "PS_Declaration",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_Section6AOther_PS_OtherSections_OtherSectionsId",
                        column: x => x.OtherSectionsId,
                        principalTable: "PS_OtherSections",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_LoanDeduction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayMonthId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deducted = table.Column<int>(type: "int", nullable: false),
                    Left = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_LoanDeduction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_LoanDeduction_Org_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_LoanDeduction_PS_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "PS_Loan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_LoanDeduction_PS_PayMonth_PayMonthId",
                        column: x => x.PayMonthId,
                        principalTable: "PS_PayMonth",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_SalaryDeduction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Monthly = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_SalaryDeduction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_SalaryDeduction_PS_DeductionComponent_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "PS_DeductionComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryDeduction_PS_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "PS_Salary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_SalaryEarning",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PercentOn = table.Column<int>(type: "int", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentOnCompId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Monthly = table.Column<int>(type: "int", nullable: false),
                    Annually = table.Column<int>(type: "int", nullable: false),
                    FromTemplate = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_SalaryEarning", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarning_PS_EarningComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarning_PS_EarningComponent_PercentOnCompId",
                        column: x => x.PercentOnCompId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarning_PS_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "PS_Salary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_SalaryHistory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnnualCTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostToCompany = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyCTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_SalaryHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_SalaryHistory_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryHistory_PS_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "PS_Salary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryHistory_PS_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "PS_Template",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Audit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Audit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Audit_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLog",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserLoginLog_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_PaySheetDeduction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeaderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Deduction = table.Column<int>(type: "int", nullable: false),
                    DeductType = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PaySheetDeduction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PaySheetDeduction_PS_DeductionComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_DeductionComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_PaySheetDeduction_PS_PaySheet_PaySheetId",
                        column: x => x.PaySheetId,
                        principalTable: "PS_PaySheet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PS_PaySheetEarning",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaySheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeaderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Earning = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EarningType = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_PaySheetEarning", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_PaySheetEarning_PS_EarningComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_PaySheetEarning_PS_PaySheet_PaySheetId",
                        column: x => x.PaySheetId,
                        principalTable: "PS_PaySheet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HD_HelpTopicSub",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HelpTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTopic = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_HelpTopicSub", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_HelpTopicSub_HD_HelpTopic_HelpTopicId",
                        column: x => x.HelpTopicId,
                        principalTable: "HD_HelpTopic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_SalaryDeductionHistory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Monthly = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SalaryHistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_SalaryDeductionHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_SalaryDeductionHistory_PS_DeductionComponent_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "PS_DeductionComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryDeductionHistory_PS_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "PS_Salary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryDeductionHistory_PS_SalaryHistory_SalaryHistoryID",
                        column: x => x.SalaryHistoryID,
                        principalTable: "PS_SalaryHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PS_SalaryEarningHistory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PercentOn = table.Column<int>(type: "int", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentOnCompId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Monthly = table.Column<int>(type: "int", nullable: false),
                    Annually = table.Column<int>(type: "int", nullable: false),
                    FromTemplate = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SalaryHistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS_SalaryEarningHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarningHistory_PS_EarningComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarningHistory_PS_EarningComponent_PercentOnCompId",
                        column: x => x.PercentOnCompId,
                        principalTable: "PS_EarningComponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarningHistory_PS_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "PS_Salary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PS_SalaryEarningHistory_PS_SalaryHistory_SalaryHistoryID",
                        column: x => x.SalaryHistoryID,
                        principalTable: "PS_SalaryHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_Ticket",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RaiseById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaisedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HelpTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_Ticket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_Ticket_HD_DeskDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HD_DeskDepartment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_Ticket_HD_HelpTopic_HelpTopicId",
                        column: x => x.HelpTopicId,
                        principalTable: "HD_HelpTopic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_Ticket_HD_HelpTopicSub_SubTopicId",
                        column: x => x.SubTopicId,
                        principalTable: "HD_HelpTopicSub",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_Ticket_Org_Employee_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_Ticket_Org_Employee_RaiseById",
                        column: x => x.RaiseById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_TicketLog",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RepliedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepliedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfLog = table.Column<byte>(type: "tinyint", nullable: false),
                    TicketStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_TicketLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_TicketLog_HD_TicketStatus_TicketStatusId",
                        column: x => x.TicketStatusId,
                        principalTable: "HD_TicketStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_TicketLog_Org_Employee_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_TicketLog_Org_Employee_RepliedById",
                        column: x => x.RepliedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_TicketLog_SS_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "SS_Ticket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_TicketLogRecipients",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_TicketLogRecipients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_TicketLogRecipients_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SS_TicketLogRecipients_SS_TicketLog_TicketLogId",
                        column: x => x.TicketLogId,
                        principalTable: "SS_TicketLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX__Pages_GroupId",
                table: "_Pages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX__Report_GroupID",
                table: "_Report",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX__Report_ModuleId",
                table: "_Report",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayInfo_Audit_EmployeeId",
                table: "EmployeePayInfo_Audit",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayInfoStatus_Audit_EmployeeId",
                table: "EmployeePayInfoStatus_Audit",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_DepartmentGroup_DeskDepartmentId",
                table: "HD_DepartmentGroup",
                column: "DeskDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_DepartmentGroup_GroupsId",
                table: "HD_DepartmentGroup",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_DeskDepartment_ManagerId",
                table: "HD_DeskDepartment",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_DeskGroupEmployee_DeskGroupId",
                table: "HD_DeskGroupEmployee",
                column: "DeskGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_DeskGroupEmployee_EmployeeId",
                table: "HD_DeskGroupEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_HelpTopic_DepartmentId",
                table: "HD_HelpTopic",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_HelpTopic_TicketStatusId",
                table: "HD_HelpTopic",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_HelpTopicSub_HelpTopicId",
                table: "HD_HelpTopicSub",
                column: "HelpTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_AdjustLeave_EmployeeId",
                table: "LM_AdjustLeave",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_AdjustLeave_LeaveTypeId",
                table: "LM_AdjustLeave",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_ApprovedLeaves_EmployeeId",
                table: "LM_ApprovedLeaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_ApprovedLeaves_LeaveTypeId",
                table: "LM_ApprovedLeaves",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_Attendance_EmployeeId",
                table: "LM_Attendance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_Attendance_LeaveTypeID",
                table: "LM_Attendance",
                column: "LeaveTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LM_AttendanceSum_EmployeeId",
                table: "LM_AttendanceSum",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_CompOffRequest_ApprovedById",
                table: "LM_CompOffRequest",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LM_CompOffRequest_EmployeeId",
                table: "LM_CompOffRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_CompOffRequest_ShiftId",
                table: "LM_CompOffRequest",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_Exemptions_HolidaysId",
                table: "LM_Exemptions",
                column: "HolidaysId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_ApplyCompensatoryId",
                table: "LM_LeaveBalance",
                column: "ApplyCompensatoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_ApplyLeaveId",
                table: "LM_LeaveBalance",
                column: "ApplyLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_CustomizedBalId",
                table: "LM_LeaveBalance",
                column: "CustomizedBalId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_EmployeeId",
                table: "LM_LeaveBalance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_LeaveTypeId",
                table: "LM_LeaveBalance",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveBalance_PreconsumableLeaveId",
                table: "LM_LeaveBalance",
                column: "PreconsumableLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequest_ApprovedById",
                table: "LM_LeaveRequest",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequest_EmployeeId",
                table: "LM_LeaveRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestDetails_ApplyLeaveId",
                table: "LM_LeaveRequestDetails",
                column: "ApplyLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestLeaveType_ApplyLeaveId",
                table: "LM_LeaveRequestLeaveType",
                column: "ApplyLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestLeaveType_LeaveTypeId",
                table: "LM_LeaveRequestLeaveType",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveSettings_CompCreditToId",
                table: "LM_LeaveSettings",
                column: "CompCreditToId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveSettings_CompoLeaveTypeId",
                table: "LM_LeaveSettings",
                column: "CompoLeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveTypeSchedule_LeaveTypeId",
                table: "LM_LeaveTypeSchedule",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_UnAuthorizedLeaves_EmployeeId",
                table: "LM_UnAuthorizedLeaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_WeekOffDays_WeekOffSetupId",
                table: "LM_WeekOffDays",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_WFHRequest_ApprovedById",
                table: "LM_WFHRequest",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LM_WFHRequest_EmployeeId",
                table: "LM_WFHRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_WFHRequest_ShiftID",
                table: "LM_WFHRequest",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Allocation_EmployeeId",
                table: "Org_Allocation",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Org_Allocation_ShiftId",
                table: "Org_Allocation",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Allocation_WeekOffSetupId",
                table: "Org_Allocation",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Allocation_WorkHoursSettingId",
                table: "Org_Allocation",
                column: "WorkHoursSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Department_ShiftId",
                table: "Org_Department",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Department_WeekOffSetupId",
                table: "Org_Department",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Department_WorkHoursSettingId",
                table: "Org_Department",
                column: "WorkHoursSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Designation_DepartmentId",
                table: "Org_Designation",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Designation_ShiftId",
                table: "Org_Designation",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Designation_WeekOffSetupId",
                table: "Org_Designation",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Designation_WorkHoursSettingId",
                table: "Org_Designation",
                column: "WorkHoursSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_DepartmentId",
                table: "Org_Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_DesignationId",
                table: "Org_Employee",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_EmpCategoryId",
                table: "Org_Employee",
                column: "EmpCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_No",
                table: "Org_Employee",
                column: "No",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_ProbationPeriodId",
                table: "Org_Employee",
                column: "ProbationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_ReportingToId",
                table: "Org_Employee",
                column: "ReportingToId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_TeamId",
                table: "Org_Employee",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_WorkLocationId",
                table: "Org_Employee",
                column: "WorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_WorkTypeId",
                table: "Org_Employee",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeEducation_EmployeeId",
                table: "Org_EmployeeEducation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeEmergencyAd_EmployeeId",
                table: "Org_EmployeeEmergencyAd",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeFamily_EmployeeId",
                table: "Org_EmployeeFamily",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeePermanentAd_EmployeeId",
                table: "Org_EmployeePermanentAd",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeePresentAd_EmployeeId",
                table: "Org_EmployeePresentAd",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeResignation_EmployeeId",
                table: "Org_EmployeeResignation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeWorkExp_EmployeeId",
                table: "Org_EmployeeWorkExp",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Location_ShiftId",
                table: "Org_Location",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Location_StateId",
                table: "Org_Location",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Location_WeekOffSetupId",
                table: "Org_Location",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Location_WorkHoursSettingId",
                table: "Org_Location",
                column: "WorkHoursSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Team_ShiftId",
                table: "Org_Team",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Team_WeekOffSetupId",
                table: "Org_Team",
                column: "WeekOffSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Team_WorkHoursSettingId",
                table: "Org_Team",
                column: "WorkHoursSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Arrear_EmployeeID",
                table: "PS_Arrear",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Declaration_EmployeeId",
                table: "PS_Declaration",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Declaration_FinancialYearId",
                table: "PS_Declaration",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_DeclarationAllowance_ComponentId",
                table: "PS_DeclarationAllowance",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_DeclarationAllowance_DeclarationId",
                table: "PS_DeclarationAllowance",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_DeclarationSetting_PaySettingsId",
                table: "PS_DeclarationSetting",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_DeductionComponent_EarningId",
                table: "PS_DeductionComponent",
                column: "EarningId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmpBonus_EmployeeId",
                table: "PS_EmpBonus",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmployeePayInfo_BankId",
                table: "PS_EmployeePayInfo",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmployeePayInfo_EmployeeId",
                table: "PS_EmployeePayInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmpStatutory_EmpId",
                table: "PS_EmpStatutory",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EPF_PaySettingsId",
                table: "PS_EPF",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_ESI_PaySettingsId",
                table: "PS_ESI",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_FinancialYear_PaySettingsId",
                table: "PS_FinancialYear",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_HomeLoanPay_DeclarationId",
                table: "PS_HomeLoanPay",
                column: "DeclarationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PS_HRADeclaration_DeclarationId",
                table: "PS_HRADeclaration",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_IncentivesPayCut_EmployeeId",
                table: "PS_IncentivesPayCut",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_IncomeTaxLimit_EmployeeId",
                table: "PS_IncomeTaxLimit",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_LateComers_EmployeeID",
                table: "PS_LateComers",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_LetOutProperty_DeclarationId",
                table: "PS_LetOutProperty",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Loan_EmployeeId",
                table: "PS_Loan",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_LoanDeduction_EmployeeID",
                table: "PS_LoanDeduction",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_LoanDeduction_LoanID",
                table: "PS_LoanDeduction",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_LoanDeduction_PayMonthId",
                table: "PS_LoanDeduction",
                column: "PayMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_NewRegimeSlab_PaySettingsId",
                table: "PS_NewRegimeSlab",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_OldRegimeSlab_PaySettingsId",
                table: "PS_OldRegimeSlab",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_OtherIncomeSources_DeclarationId",
                table: "PS_OtherIncomeSources",
                column: "DeclarationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PS_PayMonth_FinancialYearId",
                table: "PS_PayMonth",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySettings_OrganizationId",
                table: "PS_PaySettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySetup_PaySettingsId",
                table: "PS_PaySetup",
                column: "PaySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheet_EmployeeID",
                table: "PS_PaySheet",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheet_PayMonthId_EmployeeID",
                table: "PS_PaySheet",
                columns: new[] { "PayMonthId", "EmployeeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheetDeduction_ComponentId",
                table: "PS_PaySheetDeduction",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheetDeduction_PaySheetId",
                table: "PS_PaySheetDeduction",
                column: "PaySheetId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheetEarning_ComponentId",
                table: "PS_PaySheetEarning",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheetEarning_PaySheetId",
                table: "PS_PaySheetEarning",
                column: "PaySheetId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PrevEmployment_DeclarationId",
                table: "PS_PrevEmployment",
                column: "DeclarationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PS_ProfessionalTax_StateId",
                table: "PS_ProfessionalTax",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_ProfessionalTaxSlab_ProfessionalTaxId",
                table: "PS_ProfessionalTaxSlab",
                column: "ProfessionalTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Salary_EmployeeId",
                table: "PS_Salary",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Salary_TemplateId",
                table: "PS_Salary",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryDeduction_DeductionId",
                table: "PS_SalaryDeduction",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryDeduction_SalaryId",
                table: "PS_SalaryDeduction",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryDeductionHistory_DeductionId",
                table: "PS_SalaryDeductionHistory",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryDeductionHistory_SalaryHistoryID",
                table: "PS_SalaryDeductionHistory",
                column: "SalaryHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryDeductionHistory_SalaryId",
                table: "PS_SalaryDeductionHistory",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarning_ComponentId",
                table: "PS_SalaryEarning",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarning_PercentOnCompId",
                table: "PS_SalaryEarning",
                column: "PercentOnCompId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarning_SalaryId",
                table: "PS_SalaryEarning",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarningHistory_ComponentId",
                table: "PS_SalaryEarningHistory",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarningHistory_PercentOnCompId",
                table: "PS_SalaryEarningHistory",
                column: "PercentOnCompId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarningHistory_SalaryHistoryID",
                table: "PS_SalaryEarningHistory",
                column: "SalaryHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryEarningHistory_SalaryId",
                table: "PS_SalaryEarningHistory",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryHistory_EmployeeId",
                table: "PS_SalaryHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryHistory_SalaryId",
                table: "PS_SalaryHistory",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_SalaryHistory_TemplateId",
                table: "PS_SalaryHistory",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80C_DeclarationId",
                table: "PS_Section6A80C",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80C_Section80CId",
                table: "PS_Section6A80C",
                column: "Section80CId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80CWages_ComponentId",
                table: "PS_Section6A80CWages",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80CWages_DeclarationId",
                table: "PS_Section6A80CWages",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80D_DeclarationId",
                table: "PS_Section6A80D",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6A80D_Section80DId",
                table: "PS_Section6A80D",
                column: "Section80DId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6AOther_DeclarationId",
                table: "PS_Section6AOther",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_Section6AOther_OtherSectionsId",
                table: "PS_Section6AOther",
                column: "OtherSectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_TemplateDeduction_ComponentId",
                table: "PS_TemplateDeduction",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_TemplateDeduction_TemplateId",
                table: "PS_TemplateDeduction",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_TemplateEarning_ComponentId",
                table: "PS_TemplateEarning",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_TemplateEarning_PercentOnCompId",
                table: "PS_TemplateEarning",
                column: "PercentOnCompId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_TemplateEarning_TemplateId",
                table: "PS_TemplateEarning",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_PageId",
                table: "RolePrivileges",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_RoleId",
                table: "RolePrivileges",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleReportPrivileges_ReportId",
                table: "RoleReportPrivileges",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleReportPrivileges_RoleId",
                table: "RoleReportPrivileges",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_ClientVisitRequest_ApprovedById",
                table: "SS_ClientVisitRequest",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_SS_ClientVisitRequest_EmployeeId",
                table: "SS_ClientVisitRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Images_EmployeeId",
                table: "SS_Images",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_RaiseTicket_EmployeeId",
                table: "SS_RaiseTicket",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_AssignedToId",
                table: "SS_Ticket",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_DepartmentId",
                table: "SS_Ticket",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_HelpTopicId",
                table: "SS_Ticket",
                column: "HelpTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_RaiseById",
                table: "SS_Ticket",
                column: "RaiseById");

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_SubTopicId",
                table: "SS_Ticket",
                column: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLog_AssignedToId",
                table: "SS_TicketLog",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLog_RepliedById",
                table: "SS_TicketLog",
                column: "RepliedById");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLog_TicketId",
                table: "SS_TicketLog",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLog_TicketStatusId",
                table: "SS_TicketLog",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLogRecipients_EmployeeId",
                table: "SS_TicketLogRecipients",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_TicketLogRecipients_TicketLogId",
                table: "SS_TicketLogRecipients",
                column: "TicketLogId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Audit_UserId",
                table: "User_Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLog_UserId",
                table: "UserLoginLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeID",
                table: "Users",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_AppForms");

            migrationBuilder.DropTable(
                name: "_LookUpMaster");

            migrationBuilder.DropTable(
                name: "_LookUpValues");

            migrationBuilder.DropTable(
                name: "_Replication");

            migrationBuilder.DropTable(
                name: "_SequenceNo");

            migrationBuilder.DropTable(
                name: "_UserSettings");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "Daily_EventLog");

            migrationBuilder.DropTable(
                name: "EmployeeLeaves");

            migrationBuilder.DropTable(
                name: "EmployeePayInfo_Audit");

            migrationBuilder.DropTable(
                name: "EmployeePayInfoStatus_Audit");

            migrationBuilder.DropTable(
                name: "EmployeeProfiles");

            migrationBuilder.DropTable(
                name: "HD_DepartmentGroup");

            migrationBuilder.DropTable(
                name: "HD_DeskGroupEmployee");

            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "LM_Attendance");

            migrationBuilder.DropTable(
                name: "LM_AttendanceModifyLogs");

            migrationBuilder.DropTable(
                name: "LM_AttendanceSum");

            migrationBuilder.DropTable(
                name: "LM_BiometricAttLogs");

            migrationBuilder.DropTable(
                name: "LM_Exemptions");

            migrationBuilder.DropTable(
                name: "LM_LeaveBalance");

            migrationBuilder.DropTable(
                name: "LM_LeaveRequestDetails");

            migrationBuilder.DropTable(
                name: "LM_LeaveRequestLeaveType");

            migrationBuilder.DropTable(
                name: "LM_LeaveSettings");

            migrationBuilder.DropTable(
                name: "LM_LeaveTypeSchedule");

            migrationBuilder.DropTable(
                name: "LM_ManualAttLogs");

            migrationBuilder.DropTable(
                name: "LM_UnAuthorizedLeaves");

            migrationBuilder.DropTable(
                name: "LM_WeekOffDays");

            migrationBuilder.DropTable(
                name: "LM_WFHRequest");

            migrationBuilder.DropTable(
                name: "Org_Allocation");

            migrationBuilder.DropTable(
                name: "Org_EmployeeEducation");

            migrationBuilder.DropTable(
                name: "Org_EmployeeEmergencyAd");

            migrationBuilder.DropTable(
                name: "Org_EmployeeFamily");

            migrationBuilder.DropTable(
                name: "Org_EmployeePermanentAd");

            migrationBuilder.DropTable(
                name: "Org_EmployeePresentAd");

            migrationBuilder.DropTable(
                name: "Org_EmployeeResignation");

            migrationBuilder.DropTable(
                name: "Org_EmployeeWorkExp");

            migrationBuilder.DropTable(
                name: "PS_Arrear");

            migrationBuilder.DropTable(
                name: "PS_DeclarationAllowance");

            migrationBuilder.DropTable(
                name: "PS_DeclarationSetting");

            migrationBuilder.DropTable(
                name: "PS_EmpBonus");

            migrationBuilder.DropTable(
                name: "PS_EmployeePayInfo");

            migrationBuilder.DropTable(
                name: "PS_EmpStatutory");

            migrationBuilder.DropTable(
                name: "PS_EPF");

            migrationBuilder.DropTable(
                name: "PS_ESI");

            migrationBuilder.DropTable(
                name: "PS_HomeLoanPay");

            migrationBuilder.DropTable(
                name: "PS_HRADeclaration");

            migrationBuilder.DropTable(
                name: "PS_IncentivesPayCut");

            migrationBuilder.DropTable(
                name: "PS_IncomeTaxLimit");

            migrationBuilder.DropTable(
                name: "PS_LateComers");

            migrationBuilder.DropTable(
                name: "PS_LetOutProperty");

            migrationBuilder.DropTable(
                name: "PS_LoanDeduction");

            migrationBuilder.DropTable(
                name: "PS_NewRegimeSlab");

            migrationBuilder.DropTable(
                name: "PS_OldRegimeSlab");

            migrationBuilder.DropTable(
                name: "PS_OtherIncomeSources");

            migrationBuilder.DropTable(
                name: "PS_PaySetup");

            migrationBuilder.DropTable(
                name: "PS_PaySheetDeduction");

            migrationBuilder.DropTable(
                name: "PS_PaySheetEarning");

            migrationBuilder.DropTable(
                name: "PS_PrevEmployment");

            migrationBuilder.DropTable(
                name: "PS_ProfessionalTaxSlab");

            migrationBuilder.DropTable(
                name: "PS_SalaryDeduction");

            migrationBuilder.DropTable(
                name: "PS_SalaryDeductionHistory");

            migrationBuilder.DropTable(
                name: "PS_SalaryEarning");

            migrationBuilder.DropTable(
                name: "PS_SalaryEarningHistory");

            migrationBuilder.DropTable(
                name: "PS_Section6A80C");

            migrationBuilder.DropTable(
                name: "PS_Section6A80CWages");

            migrationBuilder.DropTable(
                name: "PS_Section6A80D");

            migrationBuilder.DropTable(
                name: "PS_Section6AOther");

            migrationBuilder.DropTable(
                name: "PS_TemplateDeduction");

            migrationBuilder.DropTable(
                name: "PS_TemplateEarning");

            migrationBuilder.DropTable(
                name: "RolePrivileges");

            migrationBuilder.DropTable(
                name: "RoleReportPrivileges");

            migrationBuilder.DropTable(
                name: "SS_ClientVisitRequest");

            migrationBuilder.DropTable(
                name: "SS_Images");

            migrationBuilder.DropTable(
                name: "SS_RaiseTicket");

            migrationBuilder.DropTable(
                name: "SS_TicketLogRecipients");

            migrationBuilder.DropTable(
                name: "User_Audit");

            migrationBuilder.DropTable(
                name: "UserLoginFail");

            migrationBuilder.DropTable(
                name: "UserLoginLog");

            migrationBuilder.DropTable(
                name: "ValueText");

            migrationBuilder.DropTable(
                name: "HD_DeskGroup");

            migrationBuilder.DropTable(
                name: "LM_Holidays");

            migrationBuilder.DropTable(
                name: "LM_AdjustLeave");

            migrationBuilder.DropTable(
                name: "LM_ApprovedLeaves");

            migrationBuilder.DropTable(
                name: "LM_CompOffRequest");

            migrationBuilder.DropTable(
                name: "LM_LeaveRequest");

            migrationBuilder.DropTable(
                name: "PS_Bank");

            migrationBuilder.DropTable(
                name: "PS_Loan");

            migrationBuilder.DropTable(
                name: "PS_PaySheet");

            migrationBuilder.DropTable(
                name: "PS_ProfessionalTax");

            migrationBuilder.DropTable(
                name: "PS_SalaryHistory");

            migrationBuilder.DropTable(
                name: "PS_Section80C");

            migrationBuilder.DropTable(
                name: "PS_Section80D");

            migrationBuilder.DropTable(
                name: "PS_Declaration");

            migrationBuilder.DropTable(
                name: "PS_OtherSections");

            migrationBuilder.DropTable(
                name: "PS_DeductionComponent");

            migrationBuilder.DropTable(
                name: "_Pages");

            migrationBuilder.DropTable(
                name: "_Report");

            migrationBuilder.DropTable(
                name: "SS_TicketLog");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LM_LeaveType");

            migrationBuilder.DropTable(
                name: "PS_PayMonth");

            migrationBuilder.DropTable(
                name: "PS_Salary");

            migrationBuilder.DropTable(
                name: "PS_EarningComponent");

            migrationBuilder.DropTable(
                name: "_Groups");

            migrationBuilder.DropTable(
                name: "_ReportModule");

            migrationBuilder.DropTable(
                name: "SS_Ticket");

            migrationBuilder.DropTable(
                name: "_Role");

            migrationBuilder.DropTable(
                name: "PS_FinancialYear");

            migrationBuilder.DropTable(
                name: "PS_Template");

            migrationBuilder.DropTable(
                name: "HD_HelpTopicSub");

            migrationBuilder.DropTable(
                name: "PS_PaySettings");

            migrationBuilder.DropTable(
                name: "HD_HelpTopic");

            migrationBuilder.DropTable(
                name: "Org_Organizations");

            migrationBuilder.DropTable(
                name: "HD_DeskDepartment");

            migrationBuilder.DropTable(
                name: "HD_TicketStatus");

            migrationBuilder.DropTable(
                name: "Org_Employee");

            migrationBuilder.DropTable(
                name: "Org_Designation");

            migrationBuilder.DropTable(
                name: "Org_EmployeeCategory");

            migrationBuilder.DropTable(
                name: "Org_Location");

            migrationBuilder.DropTable(
                name: "Org_ProbationPeriod");

            migrationBuilder.DropTable(
                name: "Org_Team");

            migrationBuilder.DropTable(
                name: "Org_WorkType");

            migrationBuilder.DropTable(
                name: "Org_Department");

            migrationBuilder.DropTable(
                name: "Org_State");

            migrationBuilder.DropTable(
                name: "LM_Shift");

            migrationBuilder.DropTable(
                name: "LM_WeekOffSetup");

            migrationBuilder.DropTable(
                name: "LM_WorkHoursSetting");
        }
    }
}
