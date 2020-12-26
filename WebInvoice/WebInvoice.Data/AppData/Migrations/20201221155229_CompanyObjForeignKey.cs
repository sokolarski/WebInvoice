using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations
{
    public partial class CompanyObjForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyAppObjects");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyAppId",
                table: "CompanyAppObjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CompanyAppId",
                table: "CompanyAppObjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyAppObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
