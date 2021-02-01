using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddFreeProductEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FreeProductId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireBankAccount",
                table: "Reasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FreeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(15,5)", precision: 15, scale: 5, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,5)", precision: 15, scale: 5, nullable: false),
                    TottalPrice = table.Column<decimal>(type: "decimal(15,5)", precision: 15, scale: 5, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeProducts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_FreeProductId",
                table: "Sales",
                column: "FreeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FreeProducts_IsDeleted",
                table: "FreeProducts",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_FreeProducts_FreeProductId",
                table: "Sales",
                column: "FreeProductId",
                principalTable: "FreeProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_FreeProducts_FreeProductId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "FreeProducts");

            migrationBuilder.DropIndex(
                name: "IX_Sales_FreeProductId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FreeProductId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "RequireBankAccount",
                table: "Reasons");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
