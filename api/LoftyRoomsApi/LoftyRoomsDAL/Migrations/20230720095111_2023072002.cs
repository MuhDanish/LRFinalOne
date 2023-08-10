using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _2023072002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "CustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Debit",
                table: "CustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "CustomerWallets");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "CustomerWallets");
        }
    }
}
