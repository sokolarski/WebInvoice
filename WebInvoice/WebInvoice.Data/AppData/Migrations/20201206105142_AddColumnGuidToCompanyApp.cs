using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations
{
    public partial class AddColumnGuidToCompanyApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "CompanyApps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "CompanyApps");
        }
    }
}
