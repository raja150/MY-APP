using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranSmart.API.Migrations
{
    public partial class yeardate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Performance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekNumber",
                table: "Performance",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Performance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Performance");

            migrationBuilder.DropColumn(
                name: "WeekNumber",
                table: "Performance");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Performance");
        }
    }
}
