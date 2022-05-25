using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class planchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedirectURL",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Trial",
                table: "ProductPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectURL",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Trial",
                table: "ProductPlans");
        }
    }
}
