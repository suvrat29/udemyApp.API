using Microsoft.EntityFrameworkCore.Migrations;

namespace udemyApp.API.Migrations
{
    public partial class ExtendedErrorLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Function",
                table: "Errorlogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "Errorlogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Errorlogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Function",
                table: "Errorlogs");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "Errorlogs");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Errorlogs");
        }
    }
}
