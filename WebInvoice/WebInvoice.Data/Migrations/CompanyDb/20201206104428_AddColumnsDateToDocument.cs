using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnsDateToDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VatDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VatReasonDate",
                table: "VatDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "NonVatDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VatReasonDate",
                table: "NonVatDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "VatReasonDate",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "VatReasonDate",
                table: "NonVatDocuments");
        }
    }
}
