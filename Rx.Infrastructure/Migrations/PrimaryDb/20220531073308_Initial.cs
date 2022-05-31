using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.PrimaryDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationId", "Description", "LogoURL", "Name", "TenantId" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Mobile Solutions provider", "www.apple.com/logo", "Apple", new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
