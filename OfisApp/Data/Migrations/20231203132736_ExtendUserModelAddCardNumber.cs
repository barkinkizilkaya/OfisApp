using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfisApp.Data.Migrations
{
    public partial class ExtendUserModelAddCardNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CardNumber",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "AspNetUsers");
        }
    }
}
