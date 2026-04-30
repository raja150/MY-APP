using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranSmart.API.Migrations
{
    public partial class StarEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SS_RaiseTicket");

            migrationBuilder.DropIndex(
                name: "IX_PS_EmpStatutory_EmpId",
                table: "PS_EmpStatutory");

            migrationBuilder.DropIndex(
                name: "IX_PS_EmployeePayInfo_EmployeeId",
                table: "PS_EmployeePayInfo");

            migrationBuilder.DropIndex(
                name: "IX_LM_LeaveRequestDetails_ApplyLeaveId",
                table: "LM_LeaveRequestDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SS_Ticket");

            migrationBuilder.DropColumn(
                name: "ChequePay",
                table: "PS_PaySheet");

            migrationBuilder.DropColumn(
                name: "AccountNoVerify",
                table: "PS_EmployeePayInfo");

            migrationBuilder.DropColumn(
                name: "ChequeNo",
                table: "PS_EmployeePayInfo");

            migrationBuilder.DropColumn(
                name: "ChequeNo",
                table: "EmployeePayInfo_Audit");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Users",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketStatusId",
                table: "SS_Ticket",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitBankId",
                table: "PS_PaySheet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayMode",
                table: "PS_PaySheet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CentumClub",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoublePay",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalQualityFeedbackDed",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaxFilesAndArrears",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstMinuteInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalQualityFeedbackDed",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LateComingDed",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NightShift",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OtherDed",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OtherInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductionInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PunctualityInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpotInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SundayInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TTeamInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnauthorizedLeaveDed",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyStarInc",
                table: "PS_IncentivesPayCut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "BankId",
                table: "PS_EmployeePayInfo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PS_Bank",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                table: "PS_Bank",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CalculateAtt",
                table: "Org_WorkType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "FunctionalAreaId",
                table: "Org_Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LOBId",
                table: "Org_Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "LM_LeaveBalance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "LM_LeaveBalance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "LM_BiometricAttLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "LM_BiometricAttLogs",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<decimal>(
                name: "NoOfLeaves",
                table: "LM_AttendanceSum",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OffDays",
                table: "LM_AttendanceSum",
                type: "decimal(4,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "LoginType",
                table: "LM_Attendance",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "LM_AdjustLeave",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "LM_AdjustLeave",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "_AppForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "JSON",
                table: "_AppForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                table: "_AppForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "_AppForms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddColumn<string>(
                name: "SearchJSON",
                table: "_AppForms",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.CreateTable(
                name: "_Tokens",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HD_DepartmentEmployee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeskDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_DepartmentEmployee", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_EmployeeDevice",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComputerType = table.Column<int>(type: "int", nullable: false),
                    HostName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActZeroInstalled = table.Column<bool>(type: "bit", nullable: false),
                    IsK7Installed = table.Column<bool>(type: "bit", nullable: false),
                    IsUninstalled = table.Column<bool>(type: "bit", nullable: false),
                    InstalledById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UninstalledById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstalledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UninstalledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_EmployeeDevice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeDevice_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeDevice_Org_Employee_InstalledById",
                        column: x => x.InstalledById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Org_EmployeeDevice_Org_Employee_UninstalledById",
                        column: x => x.UninstalledById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Org_FunctionalArea",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_FunctionalArea", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Org_LOB",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Org_LOB", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OT_Paper",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganiserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<short>(type: "smallint", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsJumbled = table.Column<bool>(type: "bit", nullable: false),
                    MoveToLive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ShowResult = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_Paper", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_Paper_Org_Employee_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Performance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformanceType = table.Column<byte>(type: "tinyint", nullable: false),
                    PerformedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Performance_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SS_ComplianceDoc",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SS_ComplianceDoc", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SS_ComplianceDoc_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Web_Attendance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Web_Attendance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Web_Attendance_Org_Employee_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Web_Attendance_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_Question",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SNo = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_Question", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_Question_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_Result",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    TotalTime = table.Column<int>(type: "int", nullable: false),
                    TotalMarks = table.Column<int>(type: "int", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Wrong = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_Result", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_Result_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_Result_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_TestDepartment",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_TestDepartment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_TestDepartment_Org_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Org_Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_TestDepartment_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_TestDesignation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_TestDesignation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_TestDesignation_Org_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Org_Designation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_TestDesignation_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_TestEmployee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_TestEmployee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_TestEmployee_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_TestEmployee_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_Choice",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SNo = table.Column<byte>(type: "tinyint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_Choice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_Choice_OT_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "OT_Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_ResultQuestion",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeSpent = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManuallyCorrected = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TestDesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TestDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReTake = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_ResultQuestion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_Org_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Org_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_OT_Paper_PaperId",
                        column: x => x.PaperId,
                        principalTable: "OT_Paper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_OT_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "OT_Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_OT_TestDepartment_TestDepartmentId",
                        column: x => x.TestDepartmentId,
                        principalTable: "OT_TestDepartment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_OT_TestDesignation_TestDesignationId",
                        column: x => x.TestDesignationId,
                        principalTable: "OT_TestDesignation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_ResultQuestion_OT_TestEmployee_TestEmployeeId",
                        column: x => x.TestEmployeeId,
                        principalTable: "OT_TestEmployee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OT_QuestionAnswer",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerTxt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT_QuestionAnswer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OT_QuestionAnswer_OT_Choice_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "OT_Choice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OT_QuestionAnswer_OT_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "OT_Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SS_Ticket_TicketStatusId",
                table: "SS_Ticket",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_PaySheet_DebitBankId",
                table: "PS_PaySheet",
                column: "DebitBankId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmpStatutory_EmpId",
                table: "PS_EmpStatutory",
                column: "EmpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmployeePayInfo_EmployeeId",
                table: "PS_EmployeePayInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_FunctionalAreaId",
                table: "Org_Employee",
                column: "FunctionalAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Employee_LOBId",
                table: "Org_Employee",
                column: "LOBId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestDetails_ApplyLeaveId_LeaveTypeId_LeaveDate",
                table: "LM_LeaveRequestDetails",
                columns: new[] { "ApplyLeaveId", "LeaveTypeId", "LeaveDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestDetails_LeaveTypeId",
                table: "LM_LeaveRequestDetails",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX__Tokens_UserId",
                table: "_Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeDevice_EmployeeId",
                table: "Org_EmployeeDevice",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeDevice_InstalledById",
                table: "Org_EmployeeDevice",
                column: "InstalledById");

            migrationBuilder.CreateIndex(
                name: "IX_Org_EmployeeDevice_UninstalledById",
                table: "Org_EmployeeDevice",
                column: "UninstalledById");

            migrationBuilder.CreateIndex(
                name: "IX_OT_Choice_QuestionId",
                table: "OT_Choice",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_Paper_OrganiserId",
                table: "OT_Paper",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_Question_PaperId",
                table: "OT_Question",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_QuestionAnswer_ChoiceId",
                table: "OT_QuestionAnswer",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_QuestionAnswer_QuestionId",
                table: "OT_QuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_Result_EmployeeId",
                table: "OT_Result",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_Result_PaperId",
                table: "OT_Result",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_EmployeeId",
                table: "OT_ResultQuestion",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_PaperId",
                table: "OT_ResultQuestion",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_QuestionId",
                table: "OT_ResultQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_TestDepartmentId",
                table: "OT_ResultQuestion",
                column: "TestDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_TestDesignationId",
                table: "OT_ResultQuestion",
                column: "TestDesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_ResultQuestion_TestEmployeeId",
                table: "OT_ResultQuestion",
                column: "TestEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestDepartment_DepartmentId",
                table: "OT_TestDepartment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestDepartment_PaperId",
                table: "OT_TestDepartment",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestDesignation_DesignationId",
                table: "OT_TestDesignation",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestDesignation_PaperId",
                table: "OT_TestDesignation",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestEmployee_EmployeeId",
                table: "OT_TestEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OT_TestEmployee_PaperId",
                table: "OT_TestEmployee",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_Performance_EmployeeId",
                table: "Performance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_ComplianceDoc_EmployeeId",
                table: "SS_ComplianceDoc",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Web_Attendance_ApprovedById",
                table: "Web_Attendance",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Web_Attendance_EmployeeId",
                table: "Web_Attendance",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LM_LeaveRequestDetails_LM_LeaveType_LeaveTypeId",
                table: "LM_LeaveRequestDetails",
                column: "LeaveTypeId",
                principalTable: "LM_LeaveType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Org_Employee_Org_FunctionalArea_FunctionalAreaId",
                table: "Org_Employee",
                column: "FunctionalAreaId",
                principalTable: "Org_FunctionalArea",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Org_Employee_Org_LOB_LOBId",
                table: "Org_Employee",
                column: "LOBId",
                principalTable: "Org_LOB",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PS_PaySheet_PS_Bank_DebitBankId",
                table: "PS_PaySheet",
                column: "DebitBankId",
                principalTable: "PS_Bank",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SS_Ticket_HD_TicketStatus_TicketStatusId",
                table: "SS_Ticket",
                column: "TicketStatusId",
                principalTable: "HD_TicketStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LM_LeaveRequestDetails_LM_LeaveType_LeaveTypeId",
                table: "LM_LeaveRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Org_Employee_Org_FunctionalArea_FunctionalAreaId",
                table: "Org_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Org_Employee_Org_LOB_LOBId",
                table: "Org_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_PS_PaySheet_PS_Bank_DebitBankId",
                table: "PS_PaySheet");

            migrationBuilder.DropForeignKey(
                name: "FK_SS_Ticket_HD_TicketStatus_TicketStatusId",
                table: "SS_Ticket");

            migrationBuilder.DropTable(
                name: "_Tokens");

            migrationBuilder.DropTable(
                name: "HD_DepartmentEmployee");

            migrationBuilder.DropTable(
                name: "Org_EmployeeDevice");

            migrationBuilder.DropTable(
                name: "Org_FunctionalArea");

            migrationBuilder.DropTable(
                name: "Org_LOB");

            migrationBuilder.DropTable(
                name: "OT_QuestionAnswer");

            migrationBuilder.DropTable(
                name: "OT_Result");

            migrationBuilder.DropTable(
                name: "OT_ResultQuestion");

            migrationBuilder.DropTable(
                name: "Performance");

            migrationBuilder.DropTable(
                name: "SS_ComplianceDoc");

            migrationBuilder.DropTable(
                name: "Web_Attendance");

            migrationBuilder.DropTable(
                name: "OT_Choice");

            migrationBuilder.DropTable(
                name: "OT_TestDepartment");

            migrationBuilder.DropTable(
                name: "OT_TestDesignation");

            migrationBuilder.DropTable(
                name: "OT_TestEmployee");

            migrationBuilder.DropTable(
                name: "OT_Question");

            migrationBuilder.DropTable(
                name: "OT_Paper");

            migrationBuilder.DropIndex(
                name: "IX_SS_Ticket_TicketStatusId",
                table: "SS_Ticket");

            migrationBuilder.DropIndex(
                name: "IX_PS_PaySheet_DebitBankId",
                table: "PS_PaySheet");

            migrationBuilder.DropIndex(
                name: "IX_PS_EmpStatutory_EmpId",
                table: "PS_EmpStatutory");

            migrationBuilder.DropIndex(
                name: "IX_PS_EmployeePayInfo_EmployeeId",
                table: "PS_EmployeePayInfo");

            migrationBuilder.DropIndex(
                name: "IX_Org_Employee_FunctionalAreaId",
                table: "Org_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Org_Employee_LOBId",
                table: "Org_Employee");

            migrationBuilder.DropIndex(
                name: "IX_LM_LeaveRequestDetails_ApplyLeaveId_LeaveTypeId_LeaveDate",
                table: "LM_LeaveRequestDetails");

            migrationBuilder.DropIndex(
                name: "IX_LM_LeaveRequestDetails_LeaveTypeId",
                table: "LM_LeaveRequestDetails");

            migrationBuilder.DropColumn(
                name: "TicketStatusId",
                table: "SS_Ticket");

            migrationBuilder.DropColumn(
                name: "DebitBankId",
                table: "PS_PaySheet");

            migrationBuilder.DropColumn(
                name: "PayMode",
                table: "PS_PaySheet");

            migrationBuilder.DropColumn(
                name: "CentumClub",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "DoublePay",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "ExternalQualityFeedbackDed",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "FaxFilesAndArrears",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "FirstMinuteInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "InternalQualityFeedbackDed",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "LateComingDed",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "NightShift",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "OtherDed",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "OtherInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "ProductionInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "PunctualityInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "SpotInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "SundayInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "TTeamInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "UnauthorizedLeaveDed",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "WeeklyStarInc",
                table: "PS_IncentivesPayCut");

            migrationBuilder.DropColumn(
                name: "AccountNo",
                table: "PS_Bank");

            migrationBuilder.DropColumn(
                name: "CalculateAtt",
                table: "Org_WorkType");

            migrationBuilder.DropColumn(
                name: "FunctionalAreaId",
                table: "Org_Employee");

            migrationBuilder.DropColumn(
                name: "LOBId",
                table: "Org_Employee");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "LM_LeaveBalance");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "LM_LeaveBalance");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "LM_BiometricAttLogs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LM_BiometricAttLogs");

            migrationBuilder.DropColumn(
                name: "NoOfLeaves",
                table: "LM_AttendanceSum");

            migrationBuilder.DropColumn(
                name: "OffDays",
                table: "LM_AttendanceSum");

            migrationBuilder.DropColumn(
                name: "LoginType",
                table: "LM_Attendance");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "LM_AdjustLeave");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "LM_AdjustLeave");

            migrationBuilder.DropColumn(
                name: "SearchJSON",
                table: "_AppForms");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "SS_Ticket",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "ChequePay",
                table: "PS_PaySheet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "BankId",
                table: "PS_EmployeePayInfo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "AccountNoVerify",
                table: "PS_EmployeePayInfo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChequeNo",
                table: "PS_EmployeePayInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PS_Bank",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "ChequeNo",
                table: "EmployeePayInfo_Audit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "_AppForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "JSON",
                table: "_AppForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                table: "_AppForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "_AppForms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.CreateTable(
                name: "SS_RaiseTicket",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TicketTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upload = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmpStatutory_EmpId",
                table: "PS_EmpStatutory",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_PS_EmployeePayInfo_EmployeeId",
                table: "PS_EmployeePayInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LM_LeaveRequestDetails_ApplyLeaveId",
                table: "LM_LeaveRequestDetails",
                column: "ApplyLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_SS_RaiseTicket_EmployeeId",
                table: "SS_RaiseTicket",
                column: "EmployeeId");
        }
    }
}
