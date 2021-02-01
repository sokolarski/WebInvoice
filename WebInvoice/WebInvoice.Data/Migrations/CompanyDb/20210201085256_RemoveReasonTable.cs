using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class RemoveReasonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonVatDocuments_Reasons_ReasonId",
                table: "NonVatDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_VatDocuments_Reasons_ReasonId",
                table: "VatDocuments");

            migrationBuilder.DropTable(
                name: "Reasons");

            migrationBuilder.DropIndex(
                name: "IX_VatDocuments_ReasonId",
                table: "VatDocuments");

            migrationBuilder.DropIndex(
                name: "IX_NonVatDocuments_ReasonId",
                table: "NonVatDocuments");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "VatDocuments");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "NonVatDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "VatDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "NonVatDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Reasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VatTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reasons_VatTypes_VatTypeId",
                        column: x => x.VatTypeId,
                        principalTable: "VatTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VatDocuments_ReasonId",
                table: "VatDocuments",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_NonVatDocuments_ReasonId",
                table: "NonVatDocuments",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_IsDeleted",
                table: "Reasons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_Name",
                table: "Reasons",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_VatTypeId",
                table: "Reasons",
                column: "VatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonVatDocuments_Reasons_ReasonId",
                table: "NonVatDocuments",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VatDocuments_Reasons_ReasonId",
                table: "VatDocuments",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
