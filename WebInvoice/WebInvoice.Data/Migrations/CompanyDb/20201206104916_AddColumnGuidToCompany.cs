using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnGuidToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Companies");
        }
    }
}
