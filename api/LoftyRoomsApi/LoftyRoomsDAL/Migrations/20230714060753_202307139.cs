using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _202307139 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_RoomId",
                table: "Ads",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Rooms_RoomId",
                table: "Ads",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Rooms_RoomId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_RoomId",
                table: "Ads");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
