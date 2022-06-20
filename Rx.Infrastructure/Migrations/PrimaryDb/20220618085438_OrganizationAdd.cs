using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.PrimaryDb
{
    public partial class OrganizationAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationAddresses_OrganizationId",
                table: "OrganizationAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationAddresses_OrganizationId",
                table: "OrganizationAddresses",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationAddresses_OrganizationId",
                table: "OrganizationAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationAddresses_OrganizationId",
                table: "OrganizationAddresses",
                column: "OrganizationId",
                unique: true);
        }
    }
}
