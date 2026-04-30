using Microsoft.EntityFrameworkCore.Migrations;

namespace TranSmart.API.Migrations
{
    public partial class Users_SSO_Sync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Passwords_Users_UserId",
                table: "User_Passwords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Passwords",
                table: "User_Passwords");

            migrationBuilder.RenameTable(
                name: "User_Passwords",
                newName: "_UserPasswords");

            migrationBuilder.RenameIndex(
                name: "IX_User_Passwords_UserId",
                table: "_UserPasswords",
                newName: "IX__UserPasswords_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserPasswords",
                table: "_UserPasswords",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__UserPasswords_Users_UserId",
                table: "_UserPasswords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserPasswords_Users_UserId",
                table: "_UserPasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserPasswords",
                table: "_UserPasswords");

            migrationBuilder.RenameTable(
                name: "_UserPasswords",
                newName: "User_Passwords");

            migrationBuilder.RenameIndex(
                name: "IX__UserPasswords_UserId",
                table: "User_Passwords",
                newName: "IX_User_Passwords_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Passwords",
                table: "User_Passwords",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Passwords_Users_UserId",
                table: "User_Passwords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
