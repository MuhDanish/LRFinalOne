using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _2023073108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Inquiries_Bookings_BookingId",
            //    table: "Inquiries");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Inquiries",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerWalletAmountsId",
                table: "Inquiries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_CustomerWalletAmountsId",
                table: "Inquiries",
                column: "CustomerWalletAmountsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_Bookings_BookingId",
                table: "Inquiries",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_CustomerWalletAmounts_CustomerWalletAmountsId",
                table: "Inquiries",
                column: "CustomerWalletAmountsId",
                principalTable: "CustomerWalletAmounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Inquiries_Bookings_BookingId",
            //    table: "Inquiries");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Inquiries_CustomerWalletAmounts_CustomerWalletAmountsId",
            //    table: "Inquiries");

            //migrationBuilder.DropIndex(
            //    name: "IX_Inquiries_CustomerWalletAmountsId",
            //    table: "Inquiries");

            //migrationBuilder.DropColumn(
            //    name: "CustomerWalletAmountsId",
            //    table: "Inquiries");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Inquiries",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_Bookings_BookingId",
                table: "Inquiries",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
