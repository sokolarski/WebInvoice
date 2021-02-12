using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnTottalToSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TottalWithVat",
                table: "Sales",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Vat",
                table: "Sales",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TottalWithVat",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Sales");
        }
    }
}
