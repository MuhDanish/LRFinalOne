using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _202307073 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AdId",
                table: "Bookings",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Ads_AdId",
                table: "Bookings",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "AdId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Ads_AdId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AdId",
                table: "Bookings");
        }
    }
}
