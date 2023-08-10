using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _202310072 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Partners",
                type: "double",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Partners",
                type: "double",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_PartnerId",
                table: "Ads",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Partners_PartnerId",
                table: "Ads",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "PartnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Partners_PartnerId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_PartnerId",
                table: "Ads");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Partners",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Partners",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "PartnerId",
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
