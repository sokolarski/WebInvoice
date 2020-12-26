using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnAllDocumentDataToDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AllDocumentData",
                table: "VatDocuments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AllDocumentData",
                table: "NonVatDocuments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Companies",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllDocumentData",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "AllDocumentData",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");
        }
    }
}
