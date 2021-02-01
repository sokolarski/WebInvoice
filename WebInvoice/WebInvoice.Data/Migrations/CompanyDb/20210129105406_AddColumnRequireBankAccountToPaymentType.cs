using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations.CompanyDb
{
    public partial class AddColumnRequireBankAccountToPaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireBankAccount",
                table: "Reasons");

            migrationBuilder.AddColumn<bool>(
                name: "RequireBankAccount",
                table: "PaymentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireBankAccount",
                table: "PaymentTypes");

            migrationBuilder.AddColumn<bool>(
                name: "RequireBankAccount",
                table: "Reasons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
