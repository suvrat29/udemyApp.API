using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace udemyApp.API.Migrations
{
    public partial class AddedErrorlogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Errorlogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Stacktrace = table.Column<string>(nullable: true),
                    Errortime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errorlogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errorlogs");
        }
    }
}