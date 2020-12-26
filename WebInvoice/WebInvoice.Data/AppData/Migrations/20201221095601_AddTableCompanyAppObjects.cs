using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoice.Data.Migrations
{
    public partial class AddTableCompanyAppObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanySlug",
                table: "CompanyApps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyAppObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectSlug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyAppId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAppObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAppObjects_CompanyApps_CompanyAppId",
                        column: x => x.CompanyAppId,
                        principalTable: "CompanyApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAppObjects_CompanyAppId",
                table: "CompanyAppObjects",
                column: "CompanyAppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAppObjects");

            migrationBuilder.DropColumn(
                name: "CompanySlug",
                table: "CompanyApps");
        }
    }
}
