using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _2023073 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdImage4",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdImage4",
                table: "Ads");
        }
    }
}
