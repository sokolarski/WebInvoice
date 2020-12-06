using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnCityToCompanyObj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CompanyObjects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "CompanyObjects");
        }
    }
}
