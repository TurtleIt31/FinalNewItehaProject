using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalNewItehaProject.Data.Migrations
{
    public partial class finalones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserModel");
        }
    }
}
