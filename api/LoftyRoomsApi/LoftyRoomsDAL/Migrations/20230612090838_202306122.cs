using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    public partial class _202306122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Acc_Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Acc_Users");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Acc_Users");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "Acc_Users");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Acc_Users",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Acc_Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Acc_Users",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Cnic",
                table: "Acc_Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Acc_Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cnic",
                table: "Acc_Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Acc_Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Acc_Users",
                newName: "RegionId");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Acc_Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Acc_Users",
                newName: "PostCode");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Acc_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Acc_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "Acc_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Option",
                table: "Acc_Users",
                type: "int",
                nullable: true);
        }
    }
}
