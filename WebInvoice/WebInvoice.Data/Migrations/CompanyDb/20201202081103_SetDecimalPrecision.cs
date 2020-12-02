using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class SetDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percantage",
                table: "VatTypes",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                table: "VatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tottal",
                table: "VatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTottal",
                table: "VatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Sales",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Sales",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Sales",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Products",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                table: "NonVatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tottal",
                table: "NonVatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTottal",
                table: "NonVatDocuments",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percantage",
                table: "VatTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                table: "VatDocuments",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tottal",
                table: "VatDocuments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTottal",
                table: "VatDocuments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                table: "NonVatDocuments",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tottal",
                table: "NonVatDocuments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTottal",
                table: "NonVatDocuments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldPrecision: 15,
                oldScale: 5);
        }
    }
}
