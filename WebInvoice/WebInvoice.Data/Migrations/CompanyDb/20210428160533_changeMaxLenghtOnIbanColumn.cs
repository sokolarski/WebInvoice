using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class changeMaxLenghtOnIbanColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "BankAccounts",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "BankAccounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);
        }
    }
}
