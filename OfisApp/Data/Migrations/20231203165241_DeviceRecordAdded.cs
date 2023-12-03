using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfisApp.Data.Migrations
{
    public partial class DeviceRecordAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnterDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExitDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    CardOwner = table.Column<string>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceRecords");
        }
    }
}
