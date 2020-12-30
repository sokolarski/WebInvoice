using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations
{
    public partial class AddGuidCilumnToCompanyAppObjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "CompanyAppObjects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "CompanyAppObjects");
        }
    }
}
