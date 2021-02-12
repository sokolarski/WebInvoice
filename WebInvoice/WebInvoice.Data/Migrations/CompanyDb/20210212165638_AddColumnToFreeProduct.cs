using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnToFreeProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonVatDocuments_VatTypes_VatTypeId",
                table: "NonVatDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_VatDocuments_VatTypes_VatTypeId",
                table: "VatDocuments");

            migrationBuilder.DropIndex(
                name: "IX_VatDocuments_VatTypeId",
                table: "VatDocuments");

            migrationBuilder.DropIndex(
                name: "IX_NonVatDocuments_VatTypeId",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "VatTypeId",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "VatTypeId",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "TottalPrice",
                table: "FreeProducts");

            migrationBuilder.AddColumn<string>(
                name: "QuantityType",
                table: "FreeProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VatTypeId",
                table: "FreeProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FreeProducts_VatTypeId",
                table: "FreeProducts",
                column: "VatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FreeProducts_VatTypes_VatTypeId",
                table: "FreeProducts",
                column: "VatTypeId",
                principalTable: "VatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreeProducts_VatTypes_VatTypeId",
                table: "FreeProducts");

            migrationBuilder.DropIndex(
                name: "IX_FreeProducts_VatTypeId",
                table: "FreeProducts");

            migrationBuilder.DropColumn(
                name: "QuantityType",
                table: "FreeProducts");

            migrationBuilder.DropColumn(
                name: "VatTypeId",
                table: "FreeProducts");

            migrationBuilder.AddColumn<int>(
                name: "VatTypeId",
                table: "VatDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VatTypeId",
                table: "NonVatDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TottalPrice",
                table: "FreeProducts",
                type: "decimal(15,5)",
                precision: 15,
                scale: 5,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_VatDocuments_VatTypeId",
                table: "VatDocuments",
                column: "VatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NonVatDocuments_VatTypeId",
                table: "NonVatDocuments",
                column: "VatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonVatDocuments_VatTypes_VatTypeId",
                table: "NonVatDocuments",
                column: "VatTypeId",
                principalTable: "VatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VatDocuments_VatTypes_VatTypeId",
                table: "VatDocuments",
                column: "VatTypeId",
                principalTable: "VatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
