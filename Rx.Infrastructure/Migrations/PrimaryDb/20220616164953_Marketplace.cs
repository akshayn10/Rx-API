using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.PrimaryDb
{
    public partial class Marketplace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSubscriptions_SystemSubscriptionPlans_ProductPlanId",
                table: "SystemSubscriptions");

            migrationBuilder.RenameColumn(
                name: "ProductPlanId",
                table: "SystemSubscriptions",
                newName: "SystemSubscriptionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemSubscriptions_ProductPlanId",
                table: "SystemSubscriptions",
                newName: "IX_SystemSubscriptions_SystemSubscriptionPlanId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "SystemSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MarketplaceProducts",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HaveTrial = table.Column<bool>(type: "bit", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceProducts", x => x.ProductId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSubscriptions_OrganizationId",
                table: "SystemSubscriptions",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSubscriptions_Organizations_OrganizationId",
                table: "SystemSubscriptions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSubscriptions_SystemSubscriptionPlans_SystemSubscriptionPlanId",
                table: "SystemSubscriptions",
                column: "SystemSubscriptionPlanId",
                principalTable: "SystemSubscriptionPlans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSubscriptions_Organizations_OrganizationId",
                table: "SystemSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSubscriptions_SystemSubscriptionPlans_SystemSubscriptionPlanId",
                table: "SystemSubscriptions");

            migrationBuilder.DropTable(
                name: "MarketplaceProducts");

            migrationBuilder.DropIndex(
                name: "IX_SystemSubscriptions_OrganizationId",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "SystemSubscriptionPlanId",
                table: "SystemSubscriptions",
                newName: "ProductPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemSubscriptions_SystemSubscriptionPlanId",
                table: "SystemSubscriptions",
                newName: "IX_SystemSubscriptions_ProductPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSubscriptions_SystemSubscriptionPlans_ProductPlanId",
                table: "SystemSubscriptions",
                column: "ProductPlanId",
                principalTable: "SystemSubscriptionPlans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
