using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _2023072601 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaidStatus",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidStatus",
                table: "Bookings");
        }
    }
}
