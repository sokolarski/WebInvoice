using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class addColumnsToDocuents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VatDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeText",
                table: "VatDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NonVatDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeText",
                table: "NonVatDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "FreeText",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "FreeText",
                table: "NonVatDocuments");
        }
    }
}
