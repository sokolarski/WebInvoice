using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations
{
    public partial class addColumnIsVatRegistered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVatRegistered",
                table: "CompanyApps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVatRegistered",
                table: "CompanyApps");
        }
    }
}
